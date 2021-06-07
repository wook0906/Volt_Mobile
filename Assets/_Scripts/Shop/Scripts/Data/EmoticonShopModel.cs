using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Volt
{
    namespace Shop
    {
        public class EmoticonShopModel
        {
            public int ID;
            public string emoticonName_KOR;
            public string emoticonName_EN;
            public string emoticonName_GER;
            public string emoticonName_Fren;
            public EAssetsType priceType;
            public int priceCount;
            public ShopRobotSelectType shopRobotSelectType;
            public ObjectType objectType;
            public string emoticonModel;
            public int objectCount;
            public string priceICON;
            public string iconAtlas;
            public string emoticonSprite;
            public string emoticonAtlas;
            public string BuyButtonName_KR;
            public string BuyButtonName_EN;
            public string BuyButtonName_GER;
            public string BuyButtonName_Fren;
            public string getBuyButtonNormalSprite;
            public string getBuyButtonPushedSprite;
            public string atlas;
            public string font;
            public int priceFontSize;
            public int emoticonNameFontSize;

            public EmoticonShopModel(int id, string emoticonName_KR, string emoticonName_EN, string emoticonName_GER,
                string emoticonName_Fren, int priceType, int priceCount,
                string emoticonType, string objectType, string emoticonModel, int objectCount,
                string priceICON, string iconAtlas, string emoticonSprite, string emoticonAtlas,
                string BuyButtonName_KR, string BuyButtonName_EN, string BuyButtonName_GER,
                string BuyButtonName_Fren, string getBuyButtonNormalSprite, string getBuyButtonPushedSprite,
                string atlas, string font, int priceFontSize, int emoticonNameFontSize)
            {
                this.ID = id;
                this.emoticonName_KOR = emoticonName_KR;
                this.emoticonName_EN = emoticonName_EN;
                this.emoticonName_GER = emoticonName_GER;
                this.emoticonName_Fren = emoticonName_Fren;
                this.priceType = (EAssetsType)priceType;
                this.priceCount = priceCount;
                this.shopRobotSelectType = (ShopRobotSelectType)System.Enum.Parse(typeof(ShopRobotSelectType), emoticonType);
                this.objectType = (ObjectType)System.Enum.Parse(typeof(ObjectType), objectType);
                this.objectCount = objectCount;
                this.emoticonModel = emoticonModel;
                this.priceICON = priceICON;
                this.iconAtlas = iconAtlas;
                this.emoticonSprite = emoticonSprite;
                this.emoticonAtlas = emoticonAtlas;
                this.BuyButtonName_KR = BuyButtonName_KR;
                this.BuyButtonName_EN = BuyButtonName_EN;
                this.BuyButtonName_GER = BuyButtonName_GER;
                this.BuyButtonName_Fren = BuyButtonName_Fren;
                this.getBuyButtonNormalSprite = getBuyButtonNormalSprite;
                this.getBuyButtonPushedSprite = getBuyButtonPushedSprite;
                this.atlas = atlas;
                this.font = font;
                this.priceFontSize = priceFontSize;
                this.emoticonNameFontSize = emoticonNameFontSize;
            }

            public override string ToString()
            {
                System.Text.StringBuilder builder = new System.Text.StringBuilder();

                builder.Append("Param ID: " + this.ID);
                builder.Append("/EmoticonName_KR: " + this.emoticonName_KOR);
                builder.Append("/EmoticonName_EN: " + this.emoticonName_EN);
                builder.Append("/EmoticonName_GER: " + this.emoticonName_GER);
                builder.Append("/EmoticonName_Fren: " + this.emoticonName_Fren);
                builder.Append("/SelectRobotType: " + this.shopRobotSelectType.ToString());
                builder.Append("/EmoticonModel: " + this.emoticonModel);
                builder.Append("/PriceType: " + this.priceType.ToString());
                builder.Append("/PriceCount: " + this.priceCount);
                builder.Append("/ObjectType: " + this.objectType.ToString());
                builder.Append("/ObjectCount: " + this.objectCount);
                builder.Append("/PriceICON: " + this.priceICON);
                builder.Append("/ICONAtlas: " + this.iconAtlas);
                builder.Append("/EmoticonSprite: " + this.emoticonSprite);
                builder.Append("/EmoticonAtlas: " + this.emoticonAtlas);
                builder.Append("/BuyButtonName: " + this.BuyButtonName_KR);
                builder.Append("/BuyButtonName: " + this.BuyButtonName_EN);
                builder.Append("/BuyButtonName: " + this.BuyButtonName_GER);
                builder.Append("/BuyButtonName: " + this.BuyButtonName_Fren);
                builder.Append("/GetBuyButtonNormalSprite: " + this.getBuyButtonNormalSprite);
                builder.Append("/GetBuyButtonPushedSprite: " + this.getBuyButtonPushedSprite);
                builder.Append("/Atlas: " + this.atlas);
                builder.Append("/Font: " + this.font);
                builder.Append("/PriceFontSize: " + this.priceFontSize);
                builder.Append("/EmoticonNameFontSize: " + this.emoticonNameFontSize);

                return builder.ToString();
            }
        }
    }
}
