using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AtlasPaths : ICloneable
{	
	public List<Sheet> sheets = new List<Sheet> ();
	
	public object Clone()
    {
        AtlasPaths clone = new AtlasPaths();
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
		
		public string atlasPath;
		public string atlasMaterialPath;

		public object Clone()
		{
			Param clone = new Param();
			
          clone.atlasPath = this.atlasPath;
          clone.atlasMaterialPath = this.atlasMaterialPath;
           return clone;
		}
	}
}

