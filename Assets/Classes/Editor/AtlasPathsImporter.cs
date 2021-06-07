using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class AtlasPathsImporter : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/Data/AtlasPaths.xls";
	private static readonly string exportPath = "Assets/Resources/Data/AtlasPaths.json";
	private static readonly string[] sheetNames = { "Sheet1", };
	private static readonly string fileKey = "Volt_Mobile";

	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			AtlasPaths data = new AtlasPaths ();
			
			data.sheets.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read)) {
				IWorkbook book = new HSSFWorkbook (stream);
				
				foreach(string sheetName in sheetNames) {
					ISheet sheet = book.GetSheet(sheetName);
					if( sheet == null ) {
						Debug.LogError("[Data] sheet not found:" + sheetName);
						continue;
					}

					AtlasPaths.Sheet s = new AtlasPaths.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i< sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						AtlasPaths.Param p = new AtlasPaths.Param ();
						
					cell = row.GetCell(0); p.atlasPath = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(1); p.atlasMaterialPath = (cell == null ? "" : cell.StringCellValue);
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
