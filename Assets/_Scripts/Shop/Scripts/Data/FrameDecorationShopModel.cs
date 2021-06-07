using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Volt
{
    namespace Shop
    {
        public class FrameDecorationShopModel
        {
            public int ID;
            public string edgeName;
            public EAssetsType priceType;
            public int priceCount;
            public ObjectType objectType;
            public int objectCount;
            public string priceICON;
            public string iconAtlas;
            public string edgeSprite;
            public string getBuyButtonNormalSprite;
            public string getBuyButtonPushedSprite;
            public string atlas;
            public string font;
            public int priceFontSize;

            public FrameDecorationShopModel(int id, string edgeName, int priceType, int priceCount,
                string objectType, int objectCount, string priceICON, string edgeSprite, string iconAtlas,
                string getBuyButtonNormalSprite, string getBuyButtonPushedSprite,
                string atlas, string font, int priceFontSize)
            {
                this.ID = id;
                this.edgeName = edgeName;
                this.priceType = (EAssetsType)priceType;
                this.priceCount = priceCount;
                this.objectType = (ObjectType)System.Enum.Parse(typeof(ObjectType), objectType);
                this.objectCount = objectCount;
                this.priceICON = priceICON;
                this.iconAtlas = iconAtlas;
                this.edgeSprite = edgeSprite;
                this.getBuyButtonNormalSprite = getBuyButtonNormalSprite;
                this.getBuyButtonPushedSprite = getBuyButtonPushedSprite;
                this.atlas = atlas;
                this.font = font;
                this.priceFontSize = priceFontSize;
            }

            public override string ToString()
            {
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                builder.Append("Param ID: " + this.ID);
                builder.Append("/EdgeName: " + this.edgeName);
                builder.Append("/PriceType: " + this.priceType.ToString());
                builder.Append("/PriceCount: " + this.priceCount);
                builder.Append("/ObjectType: " + this.objectType.ToString());
                builder.Append("/ObjectCount: " + this.objectCount);
                builder.Append("/PriceICON: " + this.priceICON);
                builder.Append("/ICONAtlas: " + this.iconAtlas);
                builder.Append("/EdgeSprite: " + this.edgeSprite);
                builder.Append("/GetBuyButtonNormalSprite: " + this.getBuyButtonNormalSprite);
                builder.Append("/GetBuyButtonPushedSprite: " + this.getBuyButtonPushedSprite);
                builder.Append("/Atlas: " + this.atlas);
                builder.Append("/Font: " + this.font);
                builder.Append("/PriceFontSize: " + this.priceFontSize);

                return builder.ToString();
            }
        }
    }
}
