using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Volt
{
    namespace Shop
    {
        public class GoldShopItem : UIBase
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

                purchaseButtonSprite.spriteName = this.getBuyButtonNormalSprite;
                purchaseButton.normalSprite = this.getBuyButtonNormalSprite;
                purchaseButton.pressedSprite = this.getBuyButtonPushedSprite;

                purchaseButton.GetComponent<PurchaseButton>().Init(this.ID);
                purchaseButton.GetComponent<PurchaseButton>().objectType = ObjectType.Gold;

                objectSprite.spriteName = this.objectICON;
                if (objectCount > 0)
                {
                    objectCountLabel.text = "X " + this.objectCount.ToString();
                }
                else
                    objectCountLabel.text = "";

                priceTypeSprite.spriteName = this.priceICON;

                priceCountLabel.text = this.priceCount.ToString();
            }

            public int GetID()
            {
                return ID;
            }

            public override void Init()
            {
            }
        }
    }
}