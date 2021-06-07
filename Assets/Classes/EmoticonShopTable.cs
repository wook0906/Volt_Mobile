using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EmoticonShopTable : ICloneable
{	
	public List<Sheet> sheets = new List<Sheet> ();
	
	public object Clone()
    {
        EmoticonShopTable clone = new EmoticonShopTable();
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
		public string emoticonName_KOR;
		public string emoticonName_EN;
		public string emoticonName_GER;
		public string emoticonName_Fren;
		public string robotType;
		public string emoticonModel;
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

		public object Clone()
		{
			Param clone = new Param();
			
          clone.ID = this.ID;
          clone.emoticonName_KOR = this.emoticonName_KOR;
          clone.emoticonName_EN = this.emoticonName_EN;
          clone.emoticonName_GER = this.emoticonName_GER;
          clone.emoticonName_Fren = this.emoticonName_Fren;
          clone.robotType = this.robotType;
          clone.emoticonModel = this.emoticonModel;
          clone.priceICON = this.priceICON;
          clone.iconAtlas = this.iconAtlas;
          clone.emoticonSprite = this.emoticonSprite;
          clone.emoticonAtlas = this.emoticonAtlas;
          clone.BuyButtonName_KR = this.BuyButtonName_KR;
          clone.BuyButtonName_EN = this.BuyButtonName_EN;
          clone.BuyButtonName_GER = this.BuyButtonName_GER;
          clone.BuyButtonName_Fren = this.BuyButtonName_Fren;
          clone.getBuyButtonNormalSprite = this.getBuyButtonNormalSprite;
          clone.getBuyButtonPushedSprite = this.getBuyButtonPushedSprite;
          clone.atlas = this.atlas;
          clone.font = this.font;
          clone.priceFontSize = this.priceFontSize;
          clone.emoticonNameFontSize = this.emoticonNameFontSize;
           return clone;
		}
	}
}

