using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Volt
{
    namespace Shop
    {
        public class EmoticonShopItem : UIBase
        {
            public GameObject purchaseButtonGO;
            public GameObject purchasedButtonGO;
            public UISprite emoticonSprite;
            //public UILabel emoticonNameLabel;
            public UISprite priceTypeICONSprite;
            public UILabel priceCountLabel;
            public UISprite purchaseButtonSprite;
            public UIButton purchaseButton;

            private int ID;
            private string emoticonName_KOR;
            private string emoticonName_EN;
            private string emoticonName_GER;
            private string emoticonName_Fren;
            private EAssetsType priceType;
            private int priceCount;
            private ShopRobotSelectType selectRobotType;
            private ObjectType objectType;
            private string emoticonModel;
            private int objectCount;
            private string priceICON;
            private string emoticonImageName;
            private string BuyButtonName_KR;
            private string BuyButtonName_EN;
            private string BuyButtonName_GER;
            private string BuyButtonName_Fren;
            private string getBuyButtonNormalSprite;
            private string getBuyButtonPushedSprite;

            public void Init(int ID, string emoticonName_KR, string emoticonName_EN, string emoticonName_GER,
                string emoticonName_Fren, EAssetsType priceType, int priceCount,
                ShopRobotSelectType selectRobotType, ObjectType objectType, string emoticonModel,
                int objectCount, string priceICON, string emoticonImageName,
                string BuyButtonName_KR, string BuyButtonName_EN, string BuyButtonName_GER,
                string BuyButtonName_Fren, string getBuyButtonNormalSprite, string getBuyButtonPushedSprite)
            {
                this.ID = ID;
                this.emoticonName_KOR = emoticonName_KR;
                this.emoticonName_EN = emoticonName_EN;
                this.emoticonName_GER = emoticonName_GER;
                this.emoticonName_Fren = emoticonName_Fren;
                this.priceType = priceType;
                this.priceCount = priceCount;
                this.selectRobotType = selectRobotType;
                this.objectType = objectType;
                this.emoticonModel = emoticonModel;
                this.objectCount = objectCount;
                this.priceICON = priceICON;
                this.emoticonImageName = emoticonImageName;
                this.BuyButtonName_KR = BuyButtonName_KR;
                this.BuyButtonName_EN = BuyButtonName_EN;
                this.BuyButtonName_GER = BuyButtonName_GER;
                this.BuyButtonName_Fren = BuyButtonName_Fren;
                this.getBuyButtonNormalSprite = getBuyButtonNormalSprite;
                this.getBuyButtonPushedSprite = getBuyButtonPushedSprite;

                if (Volt_PlayerData.instance.IsHaveEmoticon(this.ID))
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
                purchaseButton.GetComponent<PurchaseButton>().objectType = ObjectType.Emoticon;

                emoticonSprite.spriteName = emoticonImageName;
                //emoticonImage.mainTexture = Resources.Load<Texture>($"Images/EmoticonConceptShots/{this.emoticonImageName}");
                //emoticonNameLabel.trueTypeFont = nguiFont.dynamicFont;
                //emoticonNameLabel.fontSize = this.emoticonNameFontSize;
                //switch (Application.systemLanguage)
                //{
                //    case SystemLanguage.Korean:
                //        emoticonNameLabel.text = this.emoticonName_KOR;
                //        break;
                //    case SystemLanguage.German:
                //        emoticonNameLabel.text = this.emoticonName_GER;
                //        break;
                //    case SystemLanguage.French:
                //        emoticonNameLabel.text = this.emoticonName_Fren;
                //        break;
                //    default:
                //        emoticonNameLabel.text = this.emoticonName_EN;
                //        break;
                //}

                priceTypeICONSprite.spriteName = this.priceICON;

                priceCountLabel.text = this.priceCount.ToString();
            }

            public int GetID()
            {
                return ID;
            }
            public void OnPurchased()
            {
                if (Volt_PlayerData.instance.IsHaveEmoticon(this.ID))
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

            }
        }


    }
}