using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BatteryShopTable : ICloneable
{	
	public List<Sheet> sheets = new List<Sheet> ();
	
	public object Clone()
    {
        BatteryShopTable clone = new BatteryShopTable();
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
		public string objectName;
		public string objectICON;
		public string priceICON;
		public string iconAtlas;
		public string getBuyButtonNormalSprite;
		public string getBuyButtonPushedSprite;
		public string atlas;
		public string font;
		public int priceFontSize;
		public int objectFontSize;

		public object Clone()
		{
			Param clone = new Param();
			
          clone.ID = this.ID;
          clone.objectName = this.objectName;
          clone.objectICON = this.objectICON;
          clone.priceICON = this.priceICON;
          clone.iconAtlas = this.iconAtlas;
          clone.getBuyButtonNormalSprite = this.getBuyButtonNormalSprite;
          clone.getBuyButtonPushedSprite = this.getBuyButtonPushedSprite;
          clone.atlas = this.atlas;
          clone.font = this.font;
          clone.priceFontSize = this.priceFontSize;
          clone.objectFontSize = this.objectFontSize;
           return clone;
		}
	}
}

