using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Volt
{
    namespace Shop
    {
        public class DiamondShopItem : UIBase
        {
            public UISprite purchaseButtonSprite;
            public UIButton purchaseButton;
            public UISprite objectSprite;
            public UILabel objectCountLabel;
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

            public override void Init()
            {

            }

            public void Init(int ID, string objectName, EAssetsType priceType, int priceCount,
                ObjectType objectType, int objectCount, string objectICON, string priceICON, string iconAtlas,
                string getBuyButtonNormalSprite, string getBuyButtonPushedSprite,
                string atlas, string font, int priceFontSize, int objectFontSize)
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
                
                purchaseButtonSprite.spriteName = getBuyButtonNormalSprite;
                purchaseButton.normalSprite = getBuyButtonNormalSprite;
                purchaseButton.pressedSprite = getBuyButtonPushedSprite;

                purchaseButton.GetComponent<PurchaseButton>().Init(this.ID);
                purchaseButton.GetComponent<PurchaseButton>().objectType = ObjectType.Diamond;
                //purchaseButton.enabled = false;

                objectSprite.spriteName = this.objectICON.ToString();
                objectCountLabel.text = this.objectCount.ToString();
                priceCountLabel.text = this.priceCount.ToString();

                //switch (Application.systemLanguage)
                //{
                //    case SystemLanguage.Korean:
                //        priceCountLabel.text = "준비중";
                //        break;
                //    default:
                //        priceCountLabel.text = "Preparing";
                //        break;
                //}
                
            }


            public int GetID()
            {
                return ID;
            }
        }
    }
}