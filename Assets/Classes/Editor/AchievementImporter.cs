using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class AchievementImporter : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/Data/Achievement.xls";
	private static readonly string exportPath = "Assets/Resources/Data/Achievement.json";
	private static readonly string[] sheetNames = { "Daily","Normal", };
	private static readonly string fileKey = "Volt_Mobile";

	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			ACHTable data = new ACHTable ();
			
			data.sheets.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read)) {
				IWorkbook book = new HSSFWorkbook (stream);
				
				foreach(string sheetName in sheetNames) {
					ISheet sheet = book.GetSheet(sheetName);
					if( sheet == null ) {
						Debug.LogError("[Data] sheet not found:" + sheetName);
						continue;
					}

					ACHTable.Sheet s = new ACHTable.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i< sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						ACHTable.Param p = new ACHTable.Param ();
						
					cell = row.GetCell(0); p.ID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.title_KR = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.title_EN = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.title_GER = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.title_Fren = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.description_KR = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(6); p.description_EN = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(7); p.description_GER = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(8); p.description_Fren = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(9); p.rewardICON = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(10); p.iconAtlas = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(11); p.progressButtonName_KR = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(12); p.progressButtonName_EN = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(13); p.progressButtonName_GER = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(14); p.progressButtonName_Fren = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(15); p.getRewardButtonName_KR = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(16); p.getRewardButtonName_EN = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(17); p.getRewardButtonName_GER = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(18); p.getRewardButtonName_Fren = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(19); p.getRewardButtonActiveSprite = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(20); p.getRewardButtonUnActiveSprite = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(21); p.atlas = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(22); p.font = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(23); p.titleFontSize = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(24); p.descriptionFontSize = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(25); p.rewardCountFontSize = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(26); p.conditionCountFontSize = (int)(cell == null ? 0 : cell.NumericCellValue);
						s.list.Add (p);
					}
					data.sheets.Add(s);
				}
			}

            string jsonData = JsonUtility.ToJson(data);
			jsonData = Util.Encrypt(jsonData, fileKey);
            FileStream fileStream = new FileStream(string.Format("{0}", exportPath), FileMode.Create);
			byte[] bytes = Encoding.UTF8.GetBytes(jsonData);
			fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
		}
	}
}
