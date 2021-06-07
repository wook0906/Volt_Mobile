using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Volt
{
    namespace Shop
    {
        public class PackageShopItem : UIBase
        {
            public UISprite purchaseButtonSprite;
            public UIButton purchaseButton;
            public GameObject purchasedButtonGO;
            public UISprite objectSprite;
            public UILabel objectNameLabel;
            public UISprite priceTypeSprite;
            public UILabel priceCountLabel;
            private NGUIFont nguiFont;

            private int ID;
            private string objectName;
            private EAssetsType priceType;
            private int priceCount;
            private ObjectType objectType;
            private int objectCount;
            private string objectICON;
            private string priceICON;
            private string iconAtlas;
            private string getBuyButtonNormalSprite;
            private string getBuyButtonPushedSprite;
            private string atlas;
            private string font;
            private int priceFontSize;
            private int objectFontSize;

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
                this.iconAtlas = iconAtlas;
                this.getBuyButtonNormalSprite = getBuyButtonNormalSprite;
                this.getBuyButtonPushedSprite = getBuyButtonPushedSprite;
                this.atlas = atlas;
                this.font = font;
                this.priceFontSize = priceFontSize;
                this.objectFontSize = objectFontSize;

                purchaseButtonSprite.spriteName = this.getBuyButtonNormalSprite;
                purchaseButton.normalSprite = this.getBuyButtonNormalSprite;
                purchaseButton.pressedSprite = this.getBuyButtonPushedSprite;

                purchaseButton.GetComponent<PurchaseButton>().Init(this.ID);
                purchaseButton.GetComponent<PurchaseButton>().objectType = ObjectType.Package;

                objectSprite.spriteName = this.objectICON;
                objectNameLabel.text = this.objectName;

                priceTypeSprite.spriteName = this.priceICON;

                priceCountLabel.fontSize = this.priceFontSize;
                priceCountLabel.text = this.priceCount.ToString();

                if (Volt_PlayerData.instance.IsHavePackage(this.ID))
                {
                    purchaseButton.gameObject.SetActive(false);
                    purchasedButtonGO.SetActive(true);
                }
                else
                {
                    purchaseButton.gameObject.SetActive(true);
                    purchasedButtonGO.SetActive(false);

                    //purchaseButtonSprite.atlas = AtlasManager.instance.GetAtlas(this.atlas);
                    //purchaseButtonSprite.material = AtlasManager.instance.GetMaterial(this.atlas);
                    purchaseButtonSprite.spriteName = this.getBuyButtonNormalSprite;
                    purchaseButton.normalSprite = this.getBuyButtonNormalSprite;
                    purchaseButton.pressedSprite = this.getBuyButtonPushedSprite;
                }
            }

            public int GetID()
            {
                return ID;
            }
            public void OnPurchased()
            {
                if (Volt_PlayerData.instance.IsHavePackage(this.ID))
                {
                    purchaseButton.gameObject.SetActive(false);
                    purchasedButtonGO.SetActive(true);
                }
                else
                {
                    purchaseButton.gameObject.SetActive(true);
                    purchasedButtonGO.SetActive(false);

                    //purchaseButtonSprite.atlas = AtlasManager.instance.GetAtlas(this.atlas);
                    //purchaseButtonSprite.material = AtlasManager.instance.GetMaterial(this.atlas);
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
