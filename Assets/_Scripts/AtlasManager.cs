using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtlasManager : MonoBehaviour
{
    public static AtlasManager instance;

    public AtlasPaths atlasPathData;
    private Dictionary<string, INGUIAtlas> atlases = new Dictionary<string, INGUIAtlas>();
    private Dictionary<string, Material> atlasMaterials = new Dictionary<string, Material>();
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        AtlasPaths_Decrypt decrypt = new AtlasPaths_Decrypt();
        this.atlasPathData = decrypt.obj.Clone() as AtlasPaths;
        decrypt = null;

        foreach (AtlasPaths.Sheet sheet in atlasPathData.sheets)
        {
            foreach (AtlasPaths.Param param in sheet.list)
            {
                //Debug.Log("Path: " + param.atlasPath);
                try
                {
                    INGUIAtlas uIAtlas = Resources.Load(param.atlasPath) as INGUIAtlas;
                    string[] strs = param.atlasPath.Split('/');
                    string atlasName = strs[strs.Length - 1];
                    atlases.Add(atlasName, uIAtlas);

                    Material material = Resources.Load(param.atlasMaterialPath) as Material;
                    atlasMaterials.Add(atlasName, material);
                }
                catch (System.Exception ex)
                {
                    Debug.Log("Error: " + ex.Message);
                }
            }
        }
    }

    public INGUIAtlas GetAtlas(string key)
    {
        return atlases[key];
    }

    public Material GetMaterial(string key)
    {
        return atlasMaterials[key];
    }
}
