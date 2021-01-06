using UnityEngine;
using UnityEditor;

namespace VavilichevGD.Utils.Editor {
	public static class EditorExtensions {
		public static void LoadOrCreateScriptableObject<T>(string path) where T : ScriptableObject{
			var loadedAsset = AssetDatabase.LoadAssetAtPath<T>(path);
			if (loadedAsset) {
				EditorUtility.FocusProjectWindow();
				Selection.activeObject = loadedAsset;
				return;
			}
            
			var createdAsset = ScriptableObject.CreateInstance<T>();

			AssetDatabase.CreateAsset(createdAsset, path);
			AssetDatabase.SaveAssets();
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = createdAsset;
		}
		
	}
}