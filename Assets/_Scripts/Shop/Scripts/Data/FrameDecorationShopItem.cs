using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Volt
{
    namespace Shop
    {
        public class FrameDecorationShopItem : MonoBehaviour
        {
            public  UISprite purchaseButtonSprite;
            public  UIButton purchaseButton;
            public  UISprite objectSprite;
            public  UISprite priceICONSprite;
            public  UILabel  priceCountLabel;
            private NGUIFont nguiFont;

            private int ID;
            private string edgeName;
            private EAssetsType priceType;
            private int priceCount;
            private ObjectType objectType;
            private int objectCount;
            private string priceICON;
            private string iconAtlas;
            private string edgeSprite;
            private string getBuyButtonNormalSprite;
            private string getBuyButtonPushedSprite;
            private string atlas;
            private string font;
            private int priceFontSize;

            public void Init(int ID, string edgeName, EAssetsType priceType, int priceCount,
                ObjectType objectType, int objectCount, string priceICON, string edgeSprite, string iconAtlas,
                string getBuyButtonNormalSprite, string getBuyButtonPushedSprite,
                string atlas, string font, int priceFontSize)
            {
                this.ID = ID;
                this.edgeName = edgeName;
                this.priceType = priceType;
                this.priceCount = priceCount;
                this.objectType = objectType;
                this.objectCount = objectCount;
                this.priceICON = priceICON;
                this.iconAtlas = iconAtlas;
                this.edgeSprite = edgeSprite;
                this.getBuyButtonNormalSprite = getBuyButtonNormalSprite;
                this.getBuyButtonPushedSprite = getBuyButtonPushedSprite;
                this.atlas = atlas;
                this.font = font;
                this.priceFontSize = priceFontSize;

                nguiFont = Resources.Load<NGUIFont>("Font/" + this.font);

                purchaseButtonSprite.atlas = AtlasManager.instance.GetAtlas(this.atlas);
                purchaseButtonSprite.material = AtlasManager.instance.GetMaterial(this.atlas);
                purchaseButtonSprite.spriteName = this.getBuyButtonNormalSprite;
                purchaseButton.normalSprite = this.getBuyButtonNormalSprite;
                purchaseButton.pressedSprite = this.getBuyButtonPushedSprite;

                objectSprite.atlas = AtlasManager.instance.GetAtlas(this.atlas);
                objectSprite.material = AtlasManager.instance.GetMaterial(this.atlas);
                objectSprite.spriteName = this.edgeSprite;

                /*if (Volt_PlayerData.instance.IsHaveFrameDeco(this.ID))
                {
                    purchaseButton.enabled = false;
                    purchaseButton.GetComponent<Collider>().enabled = false;

                    priceCountLabel.text = "Already \nPurchased";
                }
                else
                {
                    priceICONSprite.atlas = AtlasManager.instance.GetAtlas(this.atlas);
                    priceICONSprite.material = AtlasManager.instance.GetMaterial(this.atlas);
                    priceICONSprite.spriteName = priceICON.ToString();

                    priceCountLabel.trueTypeFont = nguiFont.dynamicFont;
                    priceCountLabel.fontSize = priceFontSize;
                    priceCountLabel.text = priceCount.ToString();
                }*/

                priceICONSprite.atlas = AtlasManager.instance.GetAtlas(this.iconAtlas);
                priceICONSprite.material = AtlasManager.instance.GetMaterial(this.iconAtlas);
                priceICONSprite.spriteName = this.priceICON.ToString();

                priceCountLabel.trueTypeFont = nguiFont.dynamicFont;
                priceCountLabel.fontSize = priceFontSize;
                priceCountLabel.text = priceCount.ToString();
            }
        }
    }
}