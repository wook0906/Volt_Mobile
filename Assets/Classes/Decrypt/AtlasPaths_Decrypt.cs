using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class AtlasPaths_Decrypt
{	
	private static readonly string _key = "Volt_Mobile";
    private static readonly string _importPath = "Assets/Resources/Data/AtlasPaths.json";

    public AtlasPaths obj;

    public AtlasPaths_Decrypt()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/AtlasPaths");

        string jsonData = Util.Decrypt(textAsset.text, _key);
        obj = JsonUtility.FromJson<AtlasPaths>(jsonData);
        //using(FileStream stream = File.Open(_importPath, FileMode.Open, FileAccess.Read))
        //{
        //    byte[] data = new byte[stream.Length];
        //    stream.Read(data, 0, data.Length);

        //    string jsonData = Util.Decrypt(Encoding.UTF8.GetString(data), _key);

        //    obj = JsonUtility.FromJson<AtlasPaths>(jsonData);
        //}
    }
}

