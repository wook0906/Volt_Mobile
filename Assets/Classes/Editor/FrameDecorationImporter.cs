using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class FrameDecorationImporter : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/Data/FrameDecoration.xls";
	private static readonly string exportPath = "Assets/Resources/Data/FrameDecoration.json";
	private static readonly string[] sheetNames = { "FrameDecorationShop", };
	private static readonly string fileKey = "Volt_Mobile";
    private static readonly string IDTypeFilePath = "Assets/Editor/ACHIDTypeTemplate.txt";
    private static readonly string dataType = "FrameDecoration";

	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			FrameDecorationShopTable data = new FrameDecorationShopTable ();
			
			data.sheets.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read)) {
				IWorkbook book = new HSSFWorkbook (stream);
				
				foreach(string sheetName in sheetNames) {
					ISheet sheet = book.GetSheet(sheetName);
					if( sheet == null ) {
						Debug.LogError("[Data] sheet not found:" + sheetName);
						continue;
					}

					FrameDecorationShopTable.Sheet s = new FrameDecorationShopTable.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i< sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						FrameDecorationShopTable.Param p = new FrameDecorationShopTable.Param ();
						
					cell = row.GetCell(0); p.ID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.edgeName = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.priceICON = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.iconAtlas = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.edgeSprite = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.edgeAtlas = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(6); p.getBuyButtonNormalSprite = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(7); p.getBuyButtonPushedSprite = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(8); p.atlas = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(9); p.font = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(10); p.priceFontSize = (int)(cell == null ? 0 : cell.NumericCellValue);
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

            string IDTypeTemplate = File.ReadAllText(IDTypeFilePath);
            StringBuilder IDTypeBuilder = new StringBuilder();
            foreach(var sheet in data.sheets)
            {
                foreach(var param in sheet.list)
                {
                    IDTypeBuilder.AppendLine();
                    IDTypeBuilder.AppendFormat("    {0}{1} = {2},", dataType, param.ID, param.ID);
                }
            }
            IDTypeTemplate = IDTypeTemplate.Replace("$Types$", IDTypeBuilder.ToString());
            IDTypeTemplate = IDTypeTemplate.Replace("$IDTYPE$", dataType + "IDType");

            Directory.CreateDirectory("Assets/Classes/IDType/");
            string IDTypeFileSavePath = "Assets/Classes/IDType/" + dataType + "IDType" + ".cs";
            if(File.Exists(IDTypeFileSavePath))
            {
                File.Delete(IDTypeFileSavePath);
            }
            File.WriteAllText(IDTypeFileSavePath, IDTypeTemplate);
		}
	}
}
