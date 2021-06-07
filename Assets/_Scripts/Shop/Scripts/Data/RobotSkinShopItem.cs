using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Volt
{
    namespace Shop
    {
        public class RobotSkinShopItem : UIBase
        {
            public GameObject purchaseButtonGO;
            public GameObject purchasedButtonGO;
            public UISprite robotSkinSprite;
            public UILabel robotSkinNameLabel;
            public UISprite priceTypeICONSprite;
            public UILabel priceCountLabel;
            public UISprite purchaseButtonSprite;
            public UIButton purchaseButton;
            
            private int ID;
            private string skinName_KOR;
            private string skinName_EN;
            private string skinName_GER;
            private string skinName_Fren;
            private EAssetsType priceType;
            private int priceCount;
            private ShopRobotSelectType skinType;
            private ObjectType objectType;
            private string skinModel;
            private int objectCount;
            private string priceICON;
            private string skinImageName;
            private string BuyButtonName_KR;
            private string BuyButtonName_EN;
            private string BuyButtonName_GER;
            private string BuyButtonName_Fren;
            private string getBuyButtonNormalSprite;
            private string getBuyButtonPushedSprite;

            public void Init(int ID, string skinName_KR, string skinName_EN, string skinName_GER,
                string skinName_Fren, EAssetsType priceType, int priceCount,
                ShopRobotSelectType skinType, ObjectType objectType, string skinModel,
                int objectCount, string priceICON, string skinImageName, string BuyButtonName_KR,
                string BuyButtonName_EN, string BuyButtonName_GER, string BuyButtonName_Fren,
                string getBuyButtonNormalSprite, string getBuyButtonPushedSprite)
            {
                this.ID = ID;
                this.skinName_KOR = skinName_KR;
                this.skinName_EN = skinName_EN;
                this.skinName_GER = skinName_GER;
                this.skinName_Fren = skinName_Fren;
                this.priceType = priceType;
                this.priceCount = priceCount;
                this.skinType = skinType;
                this.objectType = objectType;
                this.skinModel = skinModel;
                this.objectCount = objectCount;
                this.priceICON = priceICON;
                this.skinImageName = skinImageName;
                this.BuyButtonName_KR = BuyButtonName_KR;
                this.BuyButtonName_EN = BuyButtonName_EN;
                this.BuyButtonName_GER = BuyButtonName_GER;
                this.BuyButtonName_Fren = BuyButtonName_Fren;
                this.getBuyButtonNormalSprite = getBuyButtonNormalSprite;
                this.getBuyButtonPushedSprite = getBuyButtonPushedSprite;

                if (Volt_PlayerData.instance.IsHaveSkin(this.ID))
                {
                    purchaseButtonGO.SetActive(false);
                    purchasedButtonGO.SetActive(true);
                }
                else
                {
                    purchaseButtonGO.SetActive(true);
                    purchasedButtonGO.SetActive(false);

                    purchaseButtonSprite.spriteName = this.getBuyButtonNormalSprite;
                    purchaseButton.normalSprite = this.getBuyButtonNormalSprite;
                    purchaseButton.pressedSprite = this.getBuyButtonPushedSprite;
                }


                purchaseButtonSprite.spriteName = this.getBuyButtonNormalSprite;
                purchaseButton.normalSprite = this.getBuyButtonNormalSprite;
                purchaseButton.pressedSprite = this.getBuyButtonPushedSprite;

                purchaseButton.GetComponent<PurchaseButton>().Init(this.ID);
                purchaseButton.GetComponent<PurchaseButton>().objectType = ObjectType.Skin;

                //robotSkinSprite.mainTexture = Resources.Load<Texture>($"Images/SkinConceptShots/{this.skinImageName}");
                robotSkinSprite.spriteName = skinImageName;
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.Korean:
                        robotSkinNameLabel.text = this.skinName_KOR;
                        break;
                    case SystemLanguage.German:
                        robotSkinNameLabel.text = this.skinName_GER;
                        break;
                    case SystemLanguage.French:
                        robotSkinNameLabel.text = this.skinName_Fren;
                        break;
                    default:
                        robotSkinNameLabel.text = this.skinName_EN;
                        break;
                }

                priceTypeICONSprite.spriteName = this.priceICON;

                priceCountLabel.text = this.priceCount.ToString();
            }

            public int GetID()
            {
                return ID;
            }
            public void OnPurchased()
            {
                if (Volt_PlayerData.instance.IsHaveSkin(this.ID))
                {
                    purchaseButtonGO.SetActive(false);
                    purchasedButtonGO.SetActive(true);
                }
                else
                {
                    purchaseButtonGO.SetActive(true);
                    purchasedButtonGO.SetActive(false);

                    purchaseButtonSprite.spriteName = this.getBuyButtonNormalSprite;
                    purchaseButton.normalSprite = this.getBuyButtonNormalSprite;
                    purchaseButton.pressedSprite = this.getBuyButtonPushedSprite;
                }
            }

            public override void Init()
            {
                //throw new System.NotImplementedException();
            }
        }
       
        
    }
}
