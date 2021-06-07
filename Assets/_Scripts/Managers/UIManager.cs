using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UIManager
{
    int _order = 1000;

    List<string> _popups = new List<string>();
    List<UI_Popup> _popupStack = new List<UI_Popup>();
    UI_Scene _sceneUI = null;

    Dictionary<System.Type, GameObject> _sceneUIs = new Dictionary<System.Type, GameObject>();
    Stack<UI_Scene> _sceneUIStack = new Stack<UI_Scene>();

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("UI Root");
            if (root == null)
                root = new GameObject { name = "UI Root" };
            return root;
        }
    }

    public GameObject PopupRoot
    {
        get
        {
            GameObject root = GameObject.Find("Popup Root");
            if (root == null)
                return Root;
            return root;
        }
    }

    public List<UI_Popup> GetPopupStack()
    {
        return _popupStack;
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        //Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        //canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        //canvas.overrideSorting = true;

        UIPanel panel = Util.GetOrAddComponent<UIPanel>(go);

        if (sort)
        {
            if(_popupStack.Contains(go.GetComponent<UI_Popup>()))
            {
                if(_popupStack[0].gameObject == go)
                {
                    panel.depth = _order;
                    _order++;
                }
                else
                {
                    _popupStack[0].GetComponent<UIPanel>().depth = _order;
                    panel.depth = _order - 1;
                    _order++;
                }
            }
        }
        else
        {
            panel.sortingOrder = 1;
        }
    }

    public T MakeWorldSpace<T>(Transform parent = null, string name = null) where T : UIBase
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/WorldSpace/{name}");
        if (parent != null)
            go.transform.SetParent(parent);

        Canvas canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        return Util.GetOrAddComponent<T>(go);
    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UIBase
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");
        if (parent != null)
            go.transform.SetParent(parent);

        return Util.GetOrAddComponent<T>(go);
    }

    public AsyncOperationHandle<GameObject> MakeSubItemAsync<T>(Transform parent = null, string name = null, System.Action<GameObject> callback = null) where T : UIBase
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        return Managers.Resource.InstantiateAsync($"UI/SubItem/{name}.prefab",
            (result) =>
            {
                GameObject go = result.Result;
                go.GetOrAddComponent<T>();
                if (parent != null)
                    go.transform.SetParent(parent);
                if (callback != null)
                    callback.Invoke(go);
            });
    }

    //public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    //{
    //    if (string.IsNullOrEmpty(name))
    //        name = typeof(T).Name;

    //    GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");

    //    T sceneUI = Util.GetOrAddComponent<T>(go);
    //    _sceneUI = sceneUI;

    //    go.transform.SetParent(Root.transform);
    //    return sceneUI;
    //}

    public T ShowSceneUI<T>() where T : UI_Scene
    {
        GameObject go;
        if(!_sceneUIs.TryGetValue(typeof(T), out go))
        {
            Debug.LogError($"Not Find {typeof(T)}");
            return null;
        }

        T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;
        _sceneUI.gameObject.SetActive(true);
        _sceneUI.OnActive();

        if (_sceneUIStack.Count > 0)
            _sceneUIStack.Peek().gameObject.SetActive(false);

        _sceneUIStack.Push(sceneUI);
        
        return sceneUI;
    }

    public void CloseSceneUI(UI_Scene sceneUI)
    {
        if (_sceneUIStack.Count == 1)
            return;

        if (_sceneUIStack.Peek() != sceneUI)
        {
            Debug.Log("Close Scene Failed");
            return;
        }

        CloseSceneUI();
    }
    public void CloseSceneUI()
    {
        if (_sceneUIStack.Count == 1)
            return;

        UI_Scene scene = _sceneUIStack.Pop();
        scene.gameObject.SetActive(false);

        _sceneUIStack.Peek().gameObject.SetActive(true);
    }

    public void ShowSceneUIAsync<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        Managers.Resource.InstantiateAsync($"UI/Scene/{name}.prefab", (result) =>
        {
            GameObject go = result.Result;
            go.name = name;

            T sceneUI = Util.GetOrAddComponent<T>(go);
            _sceneUI = sceneUI;

            go.transform.SetParent(Root.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;

            _sceneUIs.Add(typeof(T), go);
        });
    }

    public void ShowSceneUIAsync<T>(System.Action callback, string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        Managers.Resource.InstantiateAsync($"UI/Scene/{name}.prefab", (result) =>
        {
            GameObject go = result.Result;
            go.name = name;

            T sceneUI = Util.GetOrAddComponent<T>(go);
            _sceneUI = sceneUI;

            go.transform.SetParent(Root.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;

            _sceneUIs.Add(typeof(T), go);
            callback.Invoke();
        });
    }

    public T GetSceneUI<T>() where T : UI_Scene
    {
        if(_sceneUI is T)
            return _sceneUI as T;
        return null;
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;
        Debug.Log(name);
        
        GameObject go =  Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrAddComponent<T>(go);
        _popupStack.Add(popup);

        go.transform.SetParent(PopupRoot.transform);
        return popup;
    }

    public AsyncOperationHandle<GameObject> ShowPopupUIAsync<T>(string name = null, bool isAddTop = true) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        if (_popups.Contains(name))
            return new AsyncOperationHandle<GameObject>();

        _popups.Insert(0, name);
        return Managers.Resource.InstantiateAsync($"UI/Popup/{name}.prefab", (result) =>
        {
            GameObject go = result.Result;
            
            go.name = name;
            T popup = Util.GetOrAddComponent<T>(go);

            if (isAddTop)
                _popupStack.Insert(0, popup);
            else
            {
                _popupStack.Insert(1, popup);
                popup.GetComponent<UIPanel>().depth = _popupStack[0].GetComponent<UIPanel>().depth;
                _popupStack[_popupStack.Count - 1].GetComponent<UIPanel>().depth = _order;
                _order++;
            }

            go.transform.SetParent(PopupRoot.transform);
            go.layer = go.transform.parent.gameObject.layer;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
        });
    }

    public void ShowPopupUIAsync<T>(System.Action callback, string name = null, bool isAddTop = true) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        if (_popups.Contains(name))
            return;

        _popups.Insert(0, name);
        Managers.Resource.InstantiateAsync($"UI/Popup/{name}.prefab", (result) =>
        {
            GameObject go = result.Result;

            go.name = name;

            T popup = Util.GetOrAddComponent<T>(go);
            if (isAddTop)
                _popupStack.Insert(0, popup);
            else
            {
                _popupStack.Insert(1, popup);
                popup.GetComponent<UIPanel>().depth = _popupStack[0].GetComponent<UIPanel>().depth;
                _popupStack[0].GetComponent<UIPanel>().depth = _order;
                _order++;
            }

            go.transform.SetParent(PopupRoot.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            callback.Invoke();
        });
    }

    public T GetPopupUI<T>(string name) where T : UI_Popup
    {
        UI_Popup popup = _popupStack[0];
        if (popup.name != name)
            return null;

        if (popup is T)
            return popup as T;
        return null;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;

        if (_popupStack[0] != popup)
        {
            Debug.Log("Close Popup Failed");
            return;
        }

        ClosePopupUI();
    }
    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack[0];
        _popupStack.RemoveAt(0);
        _popups.RemoveAt(0);
        popup.OnClose();
        Managers.Resource.DestoryAndRelease(popup.gameObject);
        popup = null;

        _order--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }

    public void Clear()
    {
        CloseAllPopupUI();
        _popups.Clear();

        _sceneUIStack.Clear();
        foreach (var item in _sceneUIs.Values)
        {
            if (item != null)
                Managers.Resource.DestoryAndRelease(item);
        }
        _sceneUIs.Clear();
        _sceneUI = null;
    }
    public bool IsOnPopupUI(string UIName)
    {
        foreach (var item in _popupStack)
        {
            Debug.Log(item.name);
            if(item.name == UIName)
            {
                return true;
            }
        }
        return false;
    }
}
