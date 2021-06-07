using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ACHTable : ICloneable
{	
	public List<Sheet> sheets = new List<Sheet> ();
	
	public object Clone()
    {
        ACHTable clone = new ACHTable();
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
		public string title_KR;
		public string title_EN;
		public string title_GER;
		public string title_Fren;
		public string description_KR;
		public string description_EN;
		public string description_GER;
		public string description_Fren;
		public string rewardICON;
		public string iconAtlas;
		public string progressButtonName_KR;
		public string progressButtonName_EN;
		public string progressButtonName_GER;
		public string progressButtonName_Fren;
		public string getRewardButtonName_KR;
		public string getRewardButtonName_EN;
		public string getRewardButtonName_GER;
		public string getRewardButtonName_Fren;
		public string getRewardButtonActiveSprite;
		public string getRewardButtonUnActiveSprite;
		public string atlas;
		public string font;
		public int titleFontSize;
		public int descriptionFontSize;
		public int rewardCountFontSize;
		public int conditionCountFontSize;

		public object Clone()
		{
			Param clone = new Param();
			
          clone.ID = this.ID;
          clone.title_KR = this.title_KR;
          clone.title_EN = this.title_EN;
          clone.title_GER = this.title_GER;
          clone.title_Fren = this.title_Fren;
          clone.description_KR = this.description_KR;
          clone.description_EN = this.description_EN;
          clone.description_GER = this.description_GER;
          clone.description_Fren = this.description_Fren;
          clone.rewardICON = this.rewardICON;
          clone.iconAtlas = this.iconAtlas;
          clone.progressButtonName_KR = this.progressButtonName_KR;
          clone.progressButtonName_EN = this.progressButtonName_EN;
          clone.progressButtonName_GER = this.progressButtonName_GER;
          clone.progressButtonName_Fren = this.progressButtonName_Fren;
          clone.getRewardButtonName_KR = this.getRewardButtonName_KR;
          clone.getRewardButtonName_EN = this.getRewardButtonName_EN;
          clone.getRewardButtonName_GER = this.getRewardButtonName_GER;
          clone.getRewardButtonName_Fren = this.getRewardButtonName_Fren;
          clone.getRewardButtonActiveSprite = this.getRewardButtonActiveSprite;
          clone.getRewardButtonUnActiveSprite = this.getRewardButtonUnActiveSprite;
          clone.atlas = this.atlas;
          clone.font = this.font;
          clone.titleFontSize = this.titleFontSize;
          clone.descriptionFontSize = this.descriptionFontSize;
          clone.rewardCountFontSize = this.rewardCountFontSize;
          clone.conditionCountFontSize = this.conditionCountFontSize;
           return clone;
		}
	}
}

