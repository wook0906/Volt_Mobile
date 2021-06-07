using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class RobotSkinImporter : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/Data/RobotSkin.xls";
	private static readonly string exportPath = "Assets/Resources/Data/RobotSkin.json";
	private static readonly string[] sheetNames = { "RobotSkinShop", };
	private static readonly string fileKey = "Volt_Mobile";

	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			RobotSkinShopTable data = new RobotSkinShopTable ();
			
			data.sheets.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read)) {
				IWorkbook book = new HSSFWorkbook (stream);
				
				foreach(string sheetName in sheetNames) {
					ISheet sheet = book.GetSheet(sheetName);
					if( sheet == null ) {
						Debug.LogError("[Data] sheet not found:" + sheetName);
						continue;
					}

					RobotSkinShopTable.Sheet s = new RobotSkinShopTable.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i< sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						RobotSkinShopTable.Param p = new RobotSkinShopTable.Param ();
						
					cell = row.GetCell(0); p.ID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.skinName_KOR = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.skinName_EN = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.skinName_GER = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.skinName_Fren = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.skinType = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(6); p.skinModel = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(7); p.priceICON = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(8); p.iconAtlas = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(9); p.skinSprite = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(10); p.skinAtlas = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(11); p.BuyButtonName_KR = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(12); p.BuyButtonName_EN = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(13); p.BuyButtonName_GER = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(14); p.BuyButtonName_Fren = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(15); p.getBuyButtonNormalSprite = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(16); p.getBuyButtonPushedSprite = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(17); p.atlas = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(18); p.font = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(19); p.priceFontSize = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(20); p.skinNameFontSize = (int)(cell == null ? 0 : cell.NumericCellValue);
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
