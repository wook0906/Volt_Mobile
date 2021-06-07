using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class GoldShopTable_Decrypt
{
    private static readonly string _key = "Volt_Mobile";
    private static readonly string _importPath = "Assets/Resources/Data/Gold.json";

    public GoldShopTable obj;

    public GoldShopTable_Decrypt()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/Gold");

        string jsonData = Util.Decrypt(textAsset.text, _key);
        obj = JsonUtility.FromJson<GoldShopTable>(jsonData);
        
    }
}