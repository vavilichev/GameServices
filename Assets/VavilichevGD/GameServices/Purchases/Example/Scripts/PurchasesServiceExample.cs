using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.GameServices.AD;

namespace VavilichevGD.GameServices.Purchasing.Example {
    public class PurchasesServiceExample : MonoBehaviour {
        
        [SerializeField] private WidgetProductExample widgetProductPrefab;
        [SerializeField] private Transform contentContainer;
        [SerializeField] private Text textLog;
        [SerializeField] private ProductInfo[] infos;
        [Space] 
        [SerializeField] private Text textSoftCurrencyValue;
        [SerializeField] private Button buttonAddSoftCurrency;
        [SerializeField] private Text textHardCurrencyValue;
        [SerializeField] private Button buttonAddHardCurrency;

        private BankService bankService;
        private List<Product> products;

        #region LIFECYCLE

        private void OnEnable() {
            this.buttonAddSoftCurrency.onClick.AddListener(this.OnAddSoftCurrencyButtonClick);
            this.buttonAddHardCurrency.onClick.AddListener(this.OnAddHardCurrencyButtonClick);
        }

        private void OnDisable() {
            this.buttonAddSoftCurrency.onClick.RemoveListener(this.OnAddSoftCurrencyButtonClick);
            this.buttonAddHardCurrency.onClick.RemoveListener(this.OnAddHardCurrencyButtonClick);
        }

        private void Start() {
            this.EraseLog();
            this.InitProducts();
            this.CreateServices();
            this.SetupScreen();
        }

        private void OnDestroy() {
            this.bankService.OnSoftCurrencyValueChangedEvent -= this.OnSoftCurrencyValueChanged;
            this.bankService.OnHardCurrencyValueChangedEvent -= this.OnHardCurrencyValueChanged;
        }

        private void InitProducts() {
            this.products = new List<Product>();
            foreach (var info in this.infos)
                this.products.Add(new Product(info, new ProductState(info.id)));
        }

        #region CREATE SERVICES

        private void CreateServices() {
            this.CreateADSService();
            this.CreateIAPService();
            this.CreateBankService();
            this.CreatePurchasesService();
        }

        private void CreateADSService() {
            var adsService = new ADSService();
            adsService.isLoggingEnabled = true;
            adsService.InitializeAsync();
        }

        private void CreateIAPService() {
            var iapService = new IAPService(this.products.ToArray());
            iapService.isLoggingEnabled = true;
            iapService.InitializeAsync();
        }

        private void CreateBankService() {
            this.bankService = new BankService();
            this.bankService.isLoggingEnabled = true;
            this.bankService.InitializeAsync();
            
            this.bankService.OnSoftCurrencyValueChangedEvent += this.OnSoftCurrencyValueChanged;
            this.bankService.OnHardCurrencyValueChangedEvent += this.OnHardCurrencyValueChanged;
        }

        private void CreatePurchasesService() {
            var purchasesService = new PurchasesService();
            purchasesService.isLoggingEnabled = true;
            purchasesService.InitializeAsync();
        }

        #endregion
        

        private void SetupScreen() {
            if (IAP.isInitialized)
                this.CreateProductWidgets(this.products.ToArray());
            else
                IAP.OnIAPServiceInitializedEvent += this.OnIAPServiceInitialized;

            if (Bank.isInitialized)
                this.UpdateCurrencyInfo();
            else
                Bank.OnBankServiceInitializedEvent += this.OnBankServiceInitialized;
        }

        #endregion
        
        private void UpdateCurrencyInfo() {
            this.textSoftCurrencyValue.text = Bank.softCurrency.ToString();
            this.textHardCurrencyValue.text = Bank.hardCurrency.ToString();
        }

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
            
            Purchases.Purchase(this, product, Callback);
        }
        
        private void OnBankServiceInitialized() {
            Bank.OnBankServiceInitializedEvent -= this.OnBankServiceInitialized;
            this.UpdateCurrencyInfo();
        }
        
        private void OnSoftCurrencyValueChanged(object sender, int newValue, int i) {
            this.UpdateCurrencyInfo();
        }
        
        private void OnHardCurrencyValueChanged(object sender, int newValue, int i) {
            this.UpdateCurrencyInfo();
        }

        private void OnAddSoftCurrencyButtonClick() {
            Bank.AddSoftCurrency(this, 100);
        }

        private void OnAddHardCurrencyButtonClick() {
            Bank.AddHardCurrency(this, 100);
        }

        #endregion
    }
}