using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Volt
{
    namespace Shop
    {
        public class BatteryShopItem : UIBase
        {
            public UISprite purchaseButtonSprite;
            public UIButton purchaseButton;
            public UISprite objectSprite;
            public UILabel objectCountLabel;
            public UISprite priceTypeSprite;
            public UILabel priceCountLabel;

            private int ID;
            private string objectName;
            private EAssetsType priceType;
            private int priceCount;
            private ObjectType objectType;
            private int objectCount;
            private string objectICON;
            private string priceICON;
            private string getBuyButtonNormalSprite;
            private string getBuyButtonPushedSprite;

            public void Init(int ID, string objectName, EAssetsType priceType, int priceCount,
                ObjectType objectType, int objectCount, string objectICON, string priceICON,
                string getBuyButtonNormalSprite, string getBuyButtonPushedSprite)
            {
                this.ID = ID;
                this.objectName = objectName;
                this.priceType = priceType;
                this.priceCount = priceCount;
                this.objectType = objectType;
                this.objectCount = objectCount;
                this.objectICON = objectICON;
                this.priceICON = priceICON;
                this.getBuyButtonNormalSprite = getBuyButtonNormalSprite;
                this.getBuyButtonPushedSprite = getBuyButtonPushedSprite;

                purchaseButtonSprite.spriteName = this.getBuyButtonNormalSprite;
                purchaseButton.normalSprite = this.getBuyButtonNormalSprite;
                purchaseButton.pressedSprite = this.getBuyButtonPushedSprite;

                purchaseButton.GetComponent<PurchaseButton>().Init(this.ID);
                purchaseButton.GetComponent<PurchaseButton>().objectType = ObjectType.Battery;
                
                objectSprite.spriteName = this.objectICON;
                if (objectCount > 0)
                {
                    objectCountLabel.text = "X " + this.objectCount.ToString();
                }
                else
                    objectCountLabel.text = "";

                priceTypeSprite.spriteName = this.priceICON;
                priceCountLabel.text = this.priceCount.ToString();

                if (this.ID != 3000005)
                {
                    if (!GetComponent<Volt_RewardedAds>())
                    {
                        gameObject.AddComponent<Volt_RewardedAds>();
                        GetComponent<Volt_RewardedAds>().CreateAd();
                    }
                }
            }

            public int GetID()
            {
                return ID;
            }

            public override void Init()
            {

            }
            private void OnEnable()
            {
                if (GetComponent<Volt_RewardedAds>() && GetComponent<Volt_RewardedAds>().rewardedVideoAd == null)
                {
                    GetComponent<Volt_RewardedAds>().CreateAd();
                }
            }

            private void Update()
            {
                if (this.ID != 3000005) return;

                if (Volt_PlayerData.instance.RemainAdCnt == 0 ||
                    Volt_PlayerData.instance.IsHavePackage(8000001))
                {
                    Debug.Log(Volt_PlayerData.instance.IsGetBenefit(8000001));

                    purchaseButton.GetComponent<UIButton>().SetState(UIButtonColor.State.Disabled,true);
                    purchaseButton.GetComponent<UIButton>().isEnabled = false;
                    return;
                }

                if (AdCharge.instance.remainSecond <= 0f)
                {
                    purchaseButton.GetComponent<UIButton>().SetState(UIButtonColor.State.Normal, true);
                    purchaseButton.GetComponent<UIButton>().isEnabled = true;
                    priceCountLabel.text = "FREE";
                }
                else
                {
                    purchaseButton.GetComponent<UIButton>().SetState(UIButtonColor.State.Disabled, true);
                    purchaseButton.GetComponent<UIButton>().isEnabled = false;
                    priceCountLabel.text = AdCharge.instance.ToString();
                }
            }

        }
    }
}