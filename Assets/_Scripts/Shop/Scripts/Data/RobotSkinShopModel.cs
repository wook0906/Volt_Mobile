using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Volt
{
    namespace Shop
    {
        public class RobotSkinShopModel
        {
            public int ID;
            public string skinName_KOR;
            public string skinName_EN;
            public string skinName_GER;
            public string skinName_Fren;
            public EAssetsType priceType;
            public int priceCount;
            public ShopRobotSelectType shopRobotSelectType;
            public ObjectType objectType;
            public string skinModel;
            public int objectCount;
            public string priceICON;
            public string iconAtlas;
            public string skinSprite;
            public string skinAtlas;
            public string BuyButtonName_KR;
            public string BuyButtonName_EN;
            public string BuyButtonName_GER;
            public string BuyButtonName_Fren;
            public string getBuyButtonNormalSprite;
            public string getBuyButtonPushedSprite;
            public string atlas;
            public string font;
            public int priceFontSize;
            public int skinNameFontSize;

            public RobotSkinShopModel(int id, string skinName_KR, string skinName_EN, string skinName_GER,
                string skinName_Fren, int priceType, int priceCount,
                string skinType, string objectType, string skinModel, int objectCount,
                string priceICON, string iconAtlas, string skinSprite, string skinAtlas,
                string BuyButtonName_KR, string BuyButtonName_EN, string BuyButtonName_GER,
                string BuyButtonName_Fren, string getBuyButtonNormalSprite, string getBuyButtonPushedSprite,
                string atlas, string font, int priceFontSize, int skinNameFontSize)
            {
                this.ID = id;
                this.skinName_KOR = skinName_KR;
                this.skinName_EN = skinName_EN;
                this.skinName_GER = skinName_GER;
                this.skinName_Fren = skinName_Fren;
                this.priceType = (EAssetsType)priceType;
                this.priceCount = priceCount;
                this.shopRobotSelectType = (ShopRobotSelectType)System.Enum.Parse(typeof(ShopRobotSelectType), skinType);
                this.objectType = (ObjectType)System.Enum.Parse(typeof(ObjectType), objectType);
                this.objectCount = objectCount;
                this.skinModel = skinModel;
                this.priceICON = priceICON;
                this.iconAtlas = iconAtlas;
                this.skinSprite = skinSprite;
                this.skinAtlas = skinAtlas;
                this.BuyButtonName_KR = BuyButtonName_KR;
                this.BuyButtonName_EN = BuyButtonName_EN;
                this.BuyButtonName_GER = BuyButtonName_GER;
                this.BuyButtonName_Fren = BuyButtonName_Fren;
                this.getBuyButtonNormalSprite = getBuyButtonNormalSprite;
                this.getBuyButtonPushedSprite = getBuyButtonPushedSprite;
                this.atlas = atlas;
                this.font = font;
                this.priceFontSize = priceFontSize;
                this.skinNameFontSize = skinNameFontSize;
            }

            public override string ToString()
            {
                System.Text.StringBuilder builder = new System.Text.StringBuilder();

                builder.Append("Param ID: " + this.ID);
                builder.Append("/SkinName_KR: " + this.skinName_KOR);
                builder.Append("/SkinName_KR: " + this.skinName_EN);
                builder.Append("/SkinName_KR: " + this.skinName_GER);
                builder.Append("/SkinName_KR: " + this.skinName_Fren);
                builder.Append("/SkinType: " + this.shopRobotSelectType.ToString());
                builder.Append("/SkinModel: " + this.skinModel);
                builder.Append("/PriceType: " + this.priceType.ToString());
                builder.Append("/PriceCount: " + this.priceCount);
                builder.Append("/ObjectType: " + this.objectType.ToString());
                builder.Append("/ObjectCount: " + this.objectCount);
                builder.Append("/PriceICON: " + this.priceICON);
                builder.Append("/ICONAtlas: " + this.iconAtlas);
                builder.Append("/SkinSprite: " + this.skinSprite);
                builder.Append("/SkinAtlas: " + this.skinAtlas);
                builder.Append("/BuyButtonName: " + this.BuyButtonName_KR);
                builder.Append("/BuyButtonName: " + this.BuyButtonName_EN);
                builder.Append("/BuyButtonName: " + this.BuyButtonName_GER);
                builder.Append("/BuyButtonName: " + this.BuyButtonName_Fren);
                builder.Append("/GetBuyButtonNormalSprite: " + this.getBuyButtonNormalSprite);
                builder.Append("/GetBuyButtonPushedSprite: " + this.getBuyButtonPushedSprite);
                builder.Append("/Atlas: " + this.atlas);
                builder.Append("/Font: " + this.font);
                builder.Append("/PriceFontSize: " + this.priceFontSize);
                builder.Append("/SkinNameFontSize: " + this.skinNameFontSize);

                return builder.ToString();
            }
        }
    }
}