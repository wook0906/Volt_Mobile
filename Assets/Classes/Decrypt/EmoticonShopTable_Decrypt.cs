using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class EmoticonShopTable_Decrypt
{	
	private static readonly string _key = "Volt_Mobile";
    private static readonly string _importPath = "Assets\\Resources\\Data\\Emoticon.json";

    public EmoticonShopTable obj;

    public EmoticonShopTable_Decrypt()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/Emoticon");

        string jsonData = Util.Decrypt(textAsset.text, _key);
        obj = JsonUtility.FromJson<EmoticonShopTable>(jsonData);

        //using (FileStream stream = File.Open(_importPath, FileMode.Open, FileAccess.Read))
        //{
        //    byte[] data = new byte[stream.Length];
        //    stream.Read(data, 0, data.Length);

        //    string jsonData = Util.Decrypt(Encoding.UTF8.GetString(data), _key);

        //    obj = JsonUtility.FromJson<EmoticonShopTable>(jsonData);
        //}
    }
}

