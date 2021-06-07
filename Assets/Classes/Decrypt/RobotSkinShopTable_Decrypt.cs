using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class RobotSkinShopTable_Decrypt
{	
	private static readonly string _key = "Volt_Mobile";
    private static readonly string _importPath = "Assets\\Resources\\Data\\RobotSkin.json";

    public RobotSkinShopTable obj;

    public RobotSkinShopTable_Decrypt()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/RobotSkin");

        string jsonData = Util.Decrypt(textAsset.text, _key);
        obj = JsonUtility.FromJson<RobotSkinShopTable>(jsonData);
    }
}

