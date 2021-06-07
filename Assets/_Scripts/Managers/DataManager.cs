using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static Define;

public class DataManager
{
    public Dictionary<RobotType, RobotSkinObject> SkinDatas = new Dictionary<RobotType, RobotSkinObject>();

    private AsyncOperationHandle skinDatasLoadHandle;
    public EBenefitType CurrentGetBenefitType { private set; get; }
    public int GetBenefitResult { private set; get; }
    public InfoShop CurrentProductInfoShop { private set; get; }
    public EShopPurchase CurrentProductType { private set; get; }
    public PurchaseProductResult PurchaseProductResult { private set; get; }

    public VFXPoolData VFXPoolData { private set; get; } = null;
    public ObjectPoolData ObjectData { private set; get; } = null;
    public void Init()
    {
        skinDatasLoadHandle = Addressables.LoadAssetsAsync<RobotSkinObject>("SkinDatas",
            (result) =>
            {
                SkinDatas.Add(result.robotType, result);
            });

        skinDatasLoadHandle.Completed += (result) =>
        {
            Volt_PlayerData.instance.LoadUserSkinData();
        };

        Addressables.LoadAssetAsync<VFXPoolData>("VFXPoolData").Completed +=
            (result) =>
            {
                VFXPoolData = result.Result;
                VFXPoolData.Init();
            };

        Addressables.LoadAssetAsync<ObjectPoolData>("ObjectPoolData").Completed +=
            (result) =>
            {
                ObjectData = result.Result;
                ObjectData.Init();
            };
    }

    public void SetBenefitInfo(EBenefitType benefitType, int result)
    {
        CurrentGetBenefitType = benefitType;
        GetBenefitResult = result;
    }

    public void SetPurchaseProductInfo(InfoShop info, EShopPurchase type)
    {
        this.CurrentProductInfoShop = info;
        this.CurrentProductType = type;
    }

    public void SetPurchaseProductResult(EShopPurchase type, bool isSuccess)
    {
        SetPurchaseProductResult(new PurchaseProductResult(type, isSuccess));
    }

    public void SetPurchaseProductResult(PurchaseProductResult result)
    {
        PurchaseProductResult = result;
    }
}
