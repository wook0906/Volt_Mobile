using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager
{
    public T Load<T>(string path) where T : UnityEngine.Object
    {
        
        return Resources.Load<T>(path);
    }

    public void LoadAsync<T>(string key, Action<AsyncOperationHandle<T>> callback)
    {
        Addressables.LoadAssetAsync<T>(key).Completed += callback;
    }

    public void LoadAllAsync<T>(string key, Action<T> callback)
    {
        Addressables.LoadAssetsAsync<T>(key, callback);
    }

    public void LoadAllAsync<T>(string key, Action<AsyncOperationHandle<IList<T>>> callback)
    {
        Addressables.LoadAssetsAsync<T>(key, null).Completed += callback;
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        GameObject go = GameObject.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    public AsyncOperationHandle<GameObject> InstantiateAsync(string key, System.Action<AsyncOperationHandle<GameObject>> callback = null)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync($"Assets/_Prefabs/{key}");
        handle.Completed += callback;
        return handle;
    }

    public XmlDocument LoadXmlDoc(string fileName)
    {
        string path = $"{Application.streamingAssetsPath}/UIPrefabInfo/{fileName}";
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);

        return xmlDoc;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        GameObject.Destroy(go);
    }

    public void DestoryAndRelease(GameObject go)
    {
        if(!Addressables.ReleaseInstance(go))
        {
            Debug.LogError("Fail release");
        }
    }

    public void Release<T>(T obj)
    {
        Addressables.Release<T>(obj);
    }
}
