using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageShopTable_Decrypt
{
    private static readonly string _key = "Volt_Mobile";
    private static readonly string _importPath = "Assets\\Resources\\Data\\Package.json";

    public PackageShopTable obj;
    
    public PackageShopTable_Decrypt()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/Package");

        string jsonData = Util.Decrypt(textAsset.text, _key);
        obj = JsonUtility.FromJson<PackageShopTable>(jsonData);
    }
}
