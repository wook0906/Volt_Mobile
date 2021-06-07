using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RobotSkinShopTable : ICloneable
{	
	public List<Sheet> sheets = new List<Sheet> ();
	
	public object Clone()
    {
        RobotSkinShopTable clone = new RobotSkinShopTable();
        clone.sheets.Clear();
        clone.sheets = new List<Sheet>();
        foreach (var sheet in this.sheets)
        {
            Sheet sheetClone = sheet.Clone() as Sheet;
            clone.sheets.Add(sheetClone);
        }

        return clone;
    }

	[System.SerializableAttribute]
	public class Sheet : ICloneable
	{
		public string name = string.Empty;
		public List<Param> list = new List<Param>();

		public object Clone()
        {
            Sheet clone = new Sheet();
            clone.name = this.name;
            clone.list.Clear();
            clone.list = new List<Param>();
            foreach (var param in this.list)
            {
                Param paramClone = param.Clone() as Param;
                clone.list.Add(paramClone);
            }
            return clone;
        }
	}

	[System.SerializableAttribute]
	public class Param : ICloneable
	{
		
		public int ID;
		public string skinName_KOR;
		public string skinName_EN;
		public string skinName_GER;
		public string skinName_Fren;
		public string skinType;
		public string skinModel;
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

		public object Clone()
		{
			Param clone = new Param();
			
          clone.ID = this.ID;
          clone.skinName_KOR = this.skinName_KOR;
          clone.skinName_EN = this.skinName_EN;
          clone.skinName_GER = this.skinName_GER;
          clone.skinName_Fren = this.skinName_Fren;
          clone.skinType = this.skinType;
          clone.skinModel = this.skinModel;
          clone.priceICON = this.priceICON;
          clone.iconAtlas = this.iconAtlas;
          clone.skinSprite = this.skinSprite;
          clone.skinAtlas = this.skinAtlas;
          clone.BuyButtonName_KR = this.BuyButtonName_KR;
          clone.BuyButtonName_EN = this.BuyButtonName_EN;
          clone.BuyButtonName_GER = this.BuyButtonName_GER;
          clone.BuyButtonName_Fren = this.BuyButtonName_Fren;
          clone.getBuyButtonNormalSprite = this.getBuyButtonNormalSprite;
          clone.getBuyButtonPushedSprite = this.getBuyButtonPushedSprite;
          clone.atlas = this.atlas;
          clone.font = this.font;
          clone.priceFontSize = this.priceFontSize;
          clone.skinNameFontSize = this.skinNameFontSize;
           return clone;
		}
	}
}

