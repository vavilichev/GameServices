using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VavilichevGD.GameServices.Purchasing.Example {
    public class IAPServiceExample : MonoBehaviour {

        [SerializeField] private WidgetProductExample widgetProductPrefab;
        [SerializeField] private Transform contentContainer;
        [SerializeField] private Text textLog;
        [SerializeField] private ProductInfo[] infos;

        private List<Product> products;

        #region LIFECYCLE

        private void Start() {
            this.EraseLog();
            this.InitProducts();
            this.CreateService();
            this.SetupScreen();
        }

        private void InitProducts() {
            this.products = new List<Product>();
            foreach (var info in this.infos) 
                this.products.Add(new Product(info, new ProductState(info.id)));
        }

        private void CreateService() {
            var iapService = new IAPService(this.products.ToArray());
            iapService.isLoggingEnabled = true;
            iapService.InitializeAsync();
        }

        private void SetupScreen() {
            if (IAP.isInitialized)
                this.CreateProductWidgets(this.products.ToArray());
            else
                IAP.OnIAPServiceInitializedEvent += this.OnIAPServiceInitialized;
        }

        #endregion

        private void EraseLog() {
            this.textLog.text = "";
        }

        private void Log(string text) {
            this.textLog.text = text;
        }
        
        
        
        private void CreateProductWidgets(Product[] products) {
            foreach (var product in products) 
                this.CreateProductWidget(product);
        }

        private void CreateProductWidget(Product product) {
            var createdWidget = Instantiate(this.widgetProductPrefab, this.contentContainer);
            createdWidget.Setup(product);
            createdWidget.OnClickedEvent += this.OnPurchaseClicked;
        }

        
        #region EVENTS

        private void OnIAPServiceInitialized() {
            IAP.OnIAPServiceInitializedEvent -= this.OnIAPServiceInitialized;
            this.CreateProductWidgets(this.products.ToArray());
        }
        
        private void OnPurchaseClicked(WidgetProductExample widget) {
            this.EraseLog();
            var product = widget.product;

            void Callback(Product purchasedProduct, bool success) {
                this.Log($"Product ({purchasedProduct.info.id}) purchased. Success = {success}");
            }
            
            IAP.Purchase(product, Callback);
        }

        #endregion
        
    }
}