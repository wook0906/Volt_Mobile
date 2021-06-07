using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public class IAPManager : MonoBehaviour, IStoreListener
{
    
    //모든 아이템 등록
    //public const string productBattery = "battery"; //소모성
    //public static readonly string[] productBatterys = { "battery1", "battery3", "battery5", "battery10" };
    //public const string productDiamond = "diamond"; //소모성
    public static readonly string[] productDiamonds = { "diamond25", "diamond50", "diamond125", "diamond250" };
    public static readonly string[] productPackages = { "package8000001" };

    //public const string productCharacterSkin = "character_skin";//비소모성
    //public static readonly string[] productCharacterSkins = { "skin1", "skin2", "skin3", "skin4", "skin5", "skin6" };
    //아이템 등록 끝

    //private const string _IOS_BatteryId = "com.GFS.Mobile_Volt.battery";
    //private const string _android_BatteryId = "com.GFS.Mobile_Volt.battery";

    //private const string _IOS_SkinId = "com.GFS.Mobile_Volt.skin";
    //private const string _android_SkinId = "com.GFS.Mobile_Volt.skin";

    private readonly string[] _IOS_Packages = { "package8000001" };
    private readonly string[] _android_Packages = { "package8000001" };


    private readonly string[] _IOS_Diamonds = { "diamond25", "diamond50", "diamond125", "diamond250" };
    private readonly string[] _android_Diamonds = { "diamond25", "diamond50", "diamond125", "diamond250" };

    private static IAPManager instance;
    public static IAPManager Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<IAPManager>();
            if (instance == null) instance = new GameObject("IAP Manager").AddComponent<IAPManager>();
            return instance;
        }
    }

    private IStoreController storeController;
    private IExtensionProvider storeExtensionProvider;

    public bool IsInitialized => storeController != null && storeExtensionProvider != null;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        InitUnityIAP();
    }
    void InitUnityIAP()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        
        for (int i = 0; i < productDiamonds.Length; i++)
        {
            builder.AddProduct(productDiamonds[i], ProductType.Consumable,
                new IDs()
                {
                    {_IOS_Diamonds[i],AppleAppStore.Name },
                    {_android_Diamonds[i],GooglePlay.Name }
                }
                );
            
        }
        for(int i = 0; i< productPackages.Length; i++)
        {
            builder.AddProduct(productPackages[i], ProductType.Consumable,
                new IDs()
                {
                    {_IOS_Packages[i],AppleAppStore.Name },
                    {_android_Packages[i],GooglePlay.Name }
                }
                );
        }
        //foreach (var item in productDiamonds)
        //{
        //    builder.AddProduct(
        //    item, ProductType.Subscription,
        //    new IDs()
        //    {
        //        {_IOS_Diamond,AppleAppStore.Name },
        //        {_android_Diamond,GooglePlay.Name }
        //    }
        //    );
        //}
       
        UnityPurchasing.Initialize(this, builder);
    }
   

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("유니티 IAP 초기화 성공");
        storeController = controller;
        storeExtensionProvider = extensions;
        //foreach (var item in controller.products.all)
        //{
        //    if (item.availableToPurchase)
        //    {
        //        Debug.Log(item.metadata.localizedTitle + " 사용가능");
        //    }
        //    else
        //    {
        //        Debug.LogError(item.metadata.localizedTitle + " 사용불가");
        //    }
        //}
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"유니티 IAP 초기화 실패{error}");
    }

    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        Debug.LogWarning($"구매 실패 ID - {i.definition.id},{p}");
        Volt_ShopUIManager.S.BoughtItemConfirmPopup(EShopPurchase.Diamond, false);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {

        Debug.Log($"구매 성공 - ID : {e.purchasedProduct.definition.id}");
        //여기해야한다....!!!!
        //if(IsContainspurchasedProduct(productBatterys,e.purchasedProduct.definition.id))
        //{
        //    Debug.Log("해당 상품만큼 배터리 상승 처리...");
        //}
        //else if (IsContainspurchasedProduct(productCharacterSkins,e.purchasedProduct.definition.id))
        //{
        //    Debug.Log("스킨 등록...");
        //}

   

        if (IsContainspurchasedProduct(productDiamonds, e.purchasedProduct.definition.id)||
            IsContainspurchasedProduct(productPackages, e.purchasedProduct.definition.id))
        {
            try
            {
                bool googlePlatForm = false;

                if (Application.platform == RuntimePlatform.Android)
                    googlePlatForm = true;

                //Debug.Log("googlePlatForm : " + googlePlatForm);

                //Debug.Log("e.purchasedProduct.definition.id : " + e.purchasedProduct.definition.id);

                string productId = e.purchasedProduct.definition.id;
                string purchaseToken = "NotToken";

                JObject jsonObject = JObject.Parse(e.purchasedProduct.receipt);
                string payload = (string)jsonObject["Payload"];

                //Debug.Log("Receipt.Payload : " + payload);
                //Debug.Log("Payload.Length : " + payload.Length); // 1132

                if(googlePlatForm)
                {
                    JObject payloadObject = JObject.Parse(payload);
                    string payloadJson = (string)payloadObject["json"];

                    //Debug.Log("Receipt.PayloadJaon : " + payloadJson);

                    JObject payloadJsonObject = JObject.Parse(payloadJson);

                    productId = (string)payloadJsonObject["productId"];
                    purchaseToken = (string)payloadJsonObject["purchaseToken"];
                }

                //Debug.Log("Receipt.Payload.ProductId : " + productId);
                //Debug.Log("Receipt.Payload.PurchaseToken : " + purchaseToken);

                int projectItemID = ChangeStoreIDToProjectItemID(e.purchasedProduct.definition.id);
                //Debug.Log($"Project item ID : {projectItemID}");

                if (googlePlatForm)
                    PacketTransmission.SendIAPPacket(1, 1, projectItemID, googlePlatForm, productId, purchaseToken, payload);
                else
                {
                    int endSerial = ((payload.Length / 900) + 1);

                    for (int serial = 1; serial <= endSerial; serial++)
                    {
                        System.Threading.Thread.Sleep(100);

                        if (serial == endSerial)
                        {
                            PacketTransmission.SendIAPPacket(serial, endSerial, projectItemID, googlePlatForm, productId, purchaseToken,
                            payload.Substring((serial - 1) * 900, payload.Length % 900));
                            break;
                        }

                        PacketTransmission.SendIAPPacket(serial, endSerial, projectItemID, googlePlatForm, productId, purchaseToken,
                            payload.Substring((serial - 1) * 900, 900));
                    }
                        
                }
                
                //Volt_ShopUIManager.S.BoughtItemConfirmPopup(EShopPurchase.Diamond, true);
                //Debug.Log(int.Parse(e.purchasedProduct.definition.id.Substring(7)) + " 다이아몬드 구매 성공!...");
            }
            catch (Exception ex)
            {
                return PurchaseProcessingResult.Complete;
            }
            
        }
        return PurchaseProcessingResult.Complete;
    }
    bool IsContainspurchasedProduct(string[] products, string purchasedProduct)
    {
        foreach (var item in products)
        {
            if (item == purchasedProduct)
                return true;
        }
        return false;
    }
    public string GetProductIDByPurchasedProductID(string[] products, string productID)
    {
        foreach (var item in products)
        {
            if (item == productID)
                return item;
        }
        return null;
    }
    public void Purchase(string productId)
    {
        if (!IsInitialized) return;
        var product = storeController.products.WithID(productId);
        if(product != null && product.availableToPurchase)
        {
            Debug.Log($"구매 시도 - {product.definition.id}");
            storeController.InitiatePurchase(product);
        }
        else
        {
            Debug.Log($"구매 시도 불가- {productId}");
        }
    }
    public void RestorePurchase()
    {
        
        if (!IsInitialized) return;
        if(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            //Debug.Log("구매 복구 시도");
            var appleExt = storeExtensionProvider.GetExtension<IAppleExtensions>();
            appleExt.RestoreTransactions(
                result => Debug.Log($"구매 복구 시도 결과 - {result}"));
        }
    }
    public bool HadPurchased(string productId)
    {
        if (!IsInitialized) return false;
        var product = storeController.products.WithID(productId);
        if (product != null)
        {
            return product.hasReceipt;
        }
        return false;
    }

    public string ChangeProjectItemIDToStoreID(string itemID)
    {
        switch (itemID)
        {
            case "4000001":
                return "diamond25";
            case "4000002":
                return "diamond50";
            case "4000003":
                return "diamond125";
            case "4000004":
                return "diamond250";
            case "8000001":
                return "package8000001";
            default:
                //Debug.LogError("ChangeItemIDToStoreID Error");
                return "";
        }
    }
    public int ChangeStoreIDToProjectItemID(string storeID)
    {
        switch (storeID)
        {
            case "diamond25":
                return 4000001;
            case "diamond50":
                return 4000002;
            case "diamond125":
                return 4000003;
            case "diamond250":
                return 4000004;
            case "package8000001":
                return 8000001;
            default:
                //Debug.LogError("ChangeStoreIDToProjectItemID Error");
                return 0;
        }
    }

    public void RestorePurchases()
    {
        // If Purchasing has not yet been set up ...
        if (!IsInitialized)
        {
            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = storeExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) => {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then no purchases are available to be restored.
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                
            });
        }
        // Otherwise ...
        else
        {
            // We are not running on an Apple device. No work is necessary to restore purchases.
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }
}
