using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Volt
{
    namespace Shop
    {
        public class PackageShopModel
        {
            public int ID;
            public string objectName;
            public EAssetsType priceType;
            public int priceCount;
            public ObjectType objectType;
            public int objectCount;
            public string objectICON;
            public string priceICON;
            public string iconAtlas;
            public string getBuyButtonNormalSprite;
            public string getBuyButtonPushedSprite;
            public string atlas;
            public string font;
            public int priceFontSize;
            public int objectFontSize;

            public PackageShopModel(int id, string objectName, int priceType, int priceCount,
                string objectType, int objectCount, string objectICON, string priceICON, string iconAtlas,
                string getBuyButtonNormalSprite, string getBuyButtonPushedSprite,
                string atlas, string font, int priceFontSize, int objectFontSize)
            {
                this.ID = id;
                this.objectName = objectName;
                this.priceType = (EAssetsType)priceType;
                this.priceCount = priceCount;
                this.objectType = (ObjectType)System.Enum.Parse(typeof(ObjectType), objectType);
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
            }

            public override string ToString()
            {
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                builder.Append("Param ID: " + this.ID);
                builder.Append("/ObjectName: " + this.objectName);
                builder.Append("/PriceType: " + this.priceType.ToString());
                builder.Append("/PriceCount: " + this.priceCount);
                builder.Append("/ObjectType: " + this.objectType.ToString());
                builder.Append("/ObjectCount: " + this.objectCount);
                builder.Append("/ObjectICON: " + this.objectICON);
                builder.Append("/PriceICON: " + this.priceICON);
                builder.Append("/ICONAtlas: " + this.iconAtlas);
                builder.Append("/GetBuyButtonNormalSprite: " + this.getBuyButtonNormalSprite);
                builder.Append("/GetBuyButtonPushedSprite: " + this.getBuyButtonPushedSprite);
                builder.Append("/Atlas: " + this.atlas);
                builder.Append("/Font: " + this.font);
                builder.Append("/PriceFontSize: " + this.priceFontSize);
                builder.Append("/ObjectFontSize: " + this.objectFontSize);

                return builder.ToString();
            }
        }
    }
}
