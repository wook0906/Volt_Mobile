using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class UIPrefab
{
    private static string POPUP_SCRIPT_EXPORT_PATH = "Assets/_Scripts/UI/Popup";
    private static string SCENEUI_SCRIPT_EXPORT_PATH = "Assets/_Scripts/UI/Scene";
    private static string SUBITEM_SCRIPT_EXPORT_PATH = "Assets/_Scripts/UI/SubItem";
    private static string UI_TEMPLATE_FILE_PATH = "Assets/Editor/UITemplate.txt";

    [MenuItem("Assets/UIPrefab")]
    static void GetUIPrefabInfo()
    {
        Debug.Log("UIPrefab");

        foreach (UnityEngine.Object obj in Selection.objects)
        {
            GameObject go = obj as GameObject;
            if (go == null)
                continue;

            if (!go.GetComponent<UIPanel>())
                continue;

            XmlDocument xdoc = new XmlDocument();

            XmlNode root = xdoc.CreateElement(go.name);

            foreach (UISprite sp in go.GetComponentsInChildren<UISprite>())
            {
                XmlNode node = xdoc.CreateElement(sp.name);
                root.AppendChild(node);
            }

            WriteSpriteInfo(xdoc, root.ChildNodes, go.GetComponentsInChildren<UISprite>());
            WriteButtonInfo(xdoc, root.ChildNodes, go.GetComponentsInChildren<UIButton>());

            xdoc.AppendChild(root);
            xdoc.Save(Path.Combine(Application.dataPath, $"UIPrefabInfo/{go.name}"));
        }
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/ChangeAtlas/CommonUIAtlas")]
    static void ChangeAtlasToCommonUIAtlas()
    {
        NGUIAtlas atlas = AssetDatabase.LoadAssetAtPath<NGUIAtlas>("Assets/_Atlas/RenewAtlas/CommonUIAtlas.asset");
        if(atlas == null)
        {
            Debug.LogError("Not Find [NGUIAtlas]CommonUIAtlas");
            return;
        }
        foreach (Object obj in Selection.objects)
        {
            GameObject go = obj as GameObject;
            if (go == null)
            {
                Debug.Log("Not object");
                continue;
            }

            if(go.GetComponent<UISprite>())
            {
                go.GetComponent<UISprite>().atlas = atlas;
            }
            
            foreach (var item in go.GetComponentsInChildren<UISprite>())
            {
                item.atlas = atlas;
            }
            PrefabUtility.SavePrefabAsset(go);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/ChangeAtlas/GameSceneUIAtlas")]
    static void ChangeAtlasToGameSceneUIAtlas()
    {
        NGUIAtlas atlas = AssetDatabase.LoadAssetAtPath<NGUIAtlas>("Assets/_Atlas/RenewAtlas/GameSceneUIAtlas.asset");
        if (atlas == null)
        {
            Debug.LogError("Not Find [NGUIAtlas]GameSceneUIAtlas");
            return;
        }
        foreach (Object obj in Selection.objects)
        {
            GameObject go = obj as GameObject;
            if (go == null)
            {
                Debug.Log("Not object");
                continue;
            }

            if (go.GetComponent<UISprite>())
            {
                go.GetComponent<UISprite>().atlas = atlas;
            }

            foreach (var item in go.GetComponentsInChildren<UISprite>())
            {
                item.atlas = atlas;
            }
            PrefabUtility.SavePrefabAsset(go);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/ChangeAtlas/LobbySceneUIAtlas")]
    static void ChangeAtlasToLobbySceneUIAtlas()
    {
        NGUIAtlas atlas = AssetDatabase.LoadAssetAtPath<NGUIAtlas>("Assets/_Atlas/RenewAtlas/LobbySceneUIAtlas.asset");
        if (atlas == null)
        {
            Debug.LogError("Not Find [NGUIAtlas]LobbySceneUIAtlas");
            return;
        }
        foreach (Object obj in Selection.objects)
        {
            GameObject go = obj as GameObject;
            if (go == null)
            {
                Debug.Log("Not object");
                continue;
            }

            if (go.GetComponent<UISprite>())
            {
                go.GetComponent<UISprite>().atlas = atlas;
            }

            foreach (var item in go.GetComponentsInChildren<UISprite>())
            {
                item.atlas = atlas;
            }
            PrefabUtility.SavePrefabAsset(go);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/ChangeAtlas/ResultSceneUIAtlas")]
    static void ChangeAtlasToResultSceneUIAtlas()
    {
        NGUIAtlas atlas = AssetDatabase.LoadAssetAtPath<NGUIAtlas>("Assets/_Atlas/RenewAtlas/ResultSceneUIAtlas.asset");
        if (atlas == null)
        {
            Debug.LogError("Not Find [NGUIAtlas]ResultSceneUIAtlas");
            return;
        }
        foreach (Object obj in Selection.objects)
        {
            GameObject go = obj as GameObject;
            if (go == null)
            {
                Debug.Log("Not object");
                continue;
            }

            if (go.GetComponent<UISprite>())
            {
                go.GetComponent<UISprite>().atlas = atlas;
            }

            foreach (var item in go.GetComponentsInChildren<UISprite>())
            {
                item.atlas = atlas;
            }
            PrefabUtility.SavePrefabAsset(go);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/ChangeAtlas/ShopSceneUIAtlas")]
    static void ChangeAtlasToShopSceneUIAtlas()
    {
        NGUIAtlas atlas = AssetDatabase.LoadAssetAtPath<NGUIAtlas>("Assets/_Atlas/RenewAtlas/ShopSceneUIAtlas.asset");
        if (atlas == null)
        {
            Debug.LogError("Not Find [NGUIAtlas]ShopSceneUIAtlas");
            return;
        }
        foreach (Object obj in Selection.objects)
        {
            GameObject go = obj as GameObject;
            if (go == null)
            {
                Debug.Log("Not object");
                continue;
            }

            if (go.GetComponent<UISprite>())
            {
                go.GetComponent<UISprite>().atlas = atlas;
            }

            foreach (var item in go.GetComponentsInChildren<UISprite>())
            {
                item.atlas = atlas;
            }
            PrefabUtility.SavePrefabAsset(go);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/ChangeAtlas/TitleSceneUIAtlas")]
    static void ChangeAtlasToTitleSceneUIAtlas()
    {
        NGUIAtlas atlas = AssetDatabase.LoadAssetAtPath<NGUIAtlas>("Assets/_Atlas/RenewAtlas/TitleSceneUIAtlas.asset");
        if (atlas == null)
        {
            Debug.LogError("Not Find [NGUIAtlas]TitleSceneUIAtlas");
            return;
        }
        foreach (Object obj in Selection.objects)
        {
            GameObject go = obj as GameObject;
            if (go == null)
            {
                Debug.Log("Not object");
                continue;
            }

            if (go.GetComponent<UISprite>())
            {
                go.GetComponent<UISprite>().atlas = atlas;
            }

            foreach (var item in go.GetComponentsInChildren<UISprite>())
            {
                item.atlas = atlas;
            }
            PrefabUtility.SavePrefabAsset(go);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Custom New/C# Script(UI_Popup)")]
    static void CreateUIPopupScript()
    {
        string uiTemplate = File.ReadAllText(UI_TEMPLATE_FILE_PATH);
        foreach (Object obj in Selection.objects)
        {
            GameObject go = obj as GameObject;
            if (go == null)
                continue;
            if (!go.GetComponent<UIPanel>())
                continue;

            string fileName = go.name;
            uiTemplate = uiTemplate.Replace("$FileName$", fileName);
            uiTemplate = uiTemplate.Replace("$Type$", "UI_Popup");

            WriteUIScript(go, uiTemplate, POPUP_SCRIPT_EXPORT_PATH);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Custom New/C# Script(UI_Scene)")]
    static void CreateUISceneScript()
    {
        string uiTemplate = File.ReadAllText(UI_TEMPLATE_FILE_PATH);
        foreach (Object obj in Selection.objects)
        {
            GameObject go = obj as GameObject;
            if (go == null)
                continue;
            if (!go.GetComponent<UIPanel>())
                continue;

            string fileName = go.name;
            uiTemplate = uiTemplate.Replace("$FileName$", fileName);
            uiTemplate = uiTemplate.Replace("$Type$", "UI_Scene");

            WriteUIScript(go, uiTemplate, SCENEUI_SCRIPT_EXPORT_PATH);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Custom New/C# Script(UI_Subitem)")]
    static void CreateSubItemScript()
    {
        string uiTemplate = File.ReadAllText(UI_TEMPLATE_FILE_PATH);
        foreach (Object obj in Selection.objects)
        {
            GameObject go = obj as GameObject;
            if (go == null)
                continue;
            if (!go.GetComponent<UIPanel>())
                continue;

            string fileName = go.name;
            uiTemplate = uiTemplate.Replace("$FileName$", fileName);
            uiTemplate = uiTemplate.Replace("$Type$", "UIBase");

            WriteUIScript(go, uiTemplate, SUBITEM_SCRIPT_EXPORT_PATH);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    static void WriteUIScript(GameObject go, string uiTemplate, string outputPath)
    {
        StringBuilder labelBuilder = new StringBuilder();
        StringBuilder buttonBuilder = new StringBuilder();
        StringBuilder scrollViewBuilder = new StringBuilder();
        StringBuilder sliderBuilder = new StringBuilder();
        StringBuilder progressbarBuilder = new StringBuilder();
        StringBuilder toggleBuilder = new StringBuilder();
        StringBuilder spriteBuilder = new StringBuilder();
        StringBuilder goBuilder = new StringBuilder();

        foreach(Transform item in go.GetComponentsInChildren<Transform>())
        {
            if (item.name == go.name)
                continue;
            if (!item.gameObject.activeSelf)
                continue;
            if (item.GetComponent<MeshRenderer>())
                continue;

            if (item.GetComponent<UILabel>())
            {
                labelBuilder.AppendLine();
                labelBuilder.AppendFormat("        {0},", item.name);

            }
            else if (item.GetComponent<UIButton>())
            {
                buttonBuilder.AppendLine();
                buttonBuilder.AppendFormat("        {0},", item.name);

            }
            else if (item.GetComponent<UIScrollView>())
            {
                scrollViewBuilder.AppendLine();
                scrollViewBuilder.AppendFormat("        {0},", item.name);

            }
            else if (item.GetComponent<UISlider>())
            {
                sliderBuilder.AppendLine();
                sliderBuilder.AppendFormat("        {0},", item.name);

            }
            else if (item.GetComponent<UIProgressBar>())
            {
                progressbarBuilder.AppendLine();
                progressbarBuilder.AppendFormat("        {0},", item.name);
            }
            else if (item.GetComponent<UIToggle>())
            {
                toggleBuilder.AppendLine();
                toggleBuilder.AppendFormat("        {0},", item.name);
            }
            else if (item.GetComponent<UISprite>())
            {
                spriteBuilder.AppendLine();
                spriteBuilder.AppendFormat("        {0},", item.name);
            }
            else
            {
                goBuilder.AppendLine();
                goBuilder.AppendFormat("        {0},", item.name);
            }
        }

        uiTemplate = uiTemplate.Replace("$UILabels$", labelBuilder.ToString());
        uiTemplate = uiTemplate.Replace("$UIButtons$", buttonBuilder.ToString());
        uiTemplate = uiTemplate.Replace("$UIScrollViews$", scrollViewBuilder.ToString());
        uiTemplate = uiTemplate.Replace("$UISliders$", sliderBuilder.ToString());
        uiTemplate = uiTemplate.Replace("$UIProgressbars$", progressbarBuilder.ToString());
        uiTemplate = uiTemplate.Replace("$UIToggles$", toggleBuilder.ToString());
        uiTemplate = uiTemplate.Replace("$UISprites$", spriteBuilder.ToString());
        uiTemplate = uiTemplate.Replace("$GameObjects$", goBuilder.ToString());

        string filePath = Path.Combine(outputPath, $"{go.name}.cs");
        if (File.Exists(filePath))
        {
            return;
        }
        File.WriteAllText(filePath, uiTemplate);
    }

    static void WriteSpriteInfo(XmlDocument xdoc, XmlNodeList nodes, UISprite[] sprites)
    {
        for(int i = 0; i < nodes.Count; ++i)
        {
            XmlNode spriteNode = xdoc.CreateElement("UISprite");
            XmlAttribute atlas = xdoc.CreateAttribute("Atlas");
            atlas.InnerText = sprites[i].atlas.ToString().Split(' ')[0];
            XmlAttribute sprite = xdoc.CreateAttribute("Sprite");
            sprite.InnerText = sprites[i].spriteName;

            spriteNode.Attributes.Append(atlas);
            spriteNode.Attributes.Append(sprite);

            nodes[i].AppendChild(spriteNode);
        }
    }

    static void WriteButtonInfo(XmlDocument xdoc, XmlNodeList nodes, UIButton[] uiButtons)
    {
        for(int i = 0; i < uiButtons.Length; ++i)
        {
            XmlNode node = null;
            foreach(XmlNode child in nodes)
            {
                if(child.Name == uiButtons[i].name)
                {
                    node = child;
                    break;
                }    
            }
            if (node == null)
                continue;

            XmlNode buttonNode = xdoc.CreateElement("UIButton");

            XmlAttribute normalSprite = xdoc.CreateAttribute("NormalSprite");
            normalSprite.InnerText = uiButtons[i].normalSprite;
            XmlAttribute hoverSprite = xdoc.CreateAttribute("HoverSprite");
            hoverSprite.InnerText = uiButtons[i].hoverSprite;
            XmlAttribute pressedSprite = xdoc.CreateAttribute("PressedSprite");
            pressedSprite.InnerText = uiButtons[i].pressedSprite;
            XmlAttribute disabledSprite = xdoc.CreateAttribute("DisabledSprite");
            disabledSprite.InnerText = uiButtons[i].disabledSprite;

            buttonNode.Attributes.Append(normalSprite);
            buttonNode.Attributes.Append(hoverSprite);
            buttonNode.Attributes.Append(pressedSprite);
            buttonNode.Attributes.Append(disabledSprite);

            node.AppendChild(buttonNode);
        }
    }
}
