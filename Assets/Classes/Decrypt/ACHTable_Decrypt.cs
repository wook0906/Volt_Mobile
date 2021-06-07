using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class ACHTable_Decrypt
{	
	private static readonly string _key = "Volt_Mobile";
    private static readonly string _importPath = "Assets\\Resources\\Data\\Achievement.json";

    public ACHTable obj;

    public ACHTable_Decrypt()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/Achievement");
        
        string jsonData = Util.Decrypt(textAsset.text, _key);
        obj = JsonUtility.FromJson<ACHTable>(jsonData);
    }
}

