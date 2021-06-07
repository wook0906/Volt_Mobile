using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UIBase : MonoBehaviour
{
    protected bool isInit = false;
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init();

    protected void Start()
    {
        if (!isInit)
        {
            Init();
        }
    }
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        if (names.Length == 0)
            return;

        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
            {
                objects[i] = Util.FindChild(gameObject, names[i], true);
            }
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.LogWarning($"Failed to bind @ {names[i]}");
        }

    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    protected GameObject GetGameObject(int idx) { return Get<GameObject>(idx); }
    protected UILabel GetLabel(int idx) { return Get<UILabel>(idx); }
    protected UIButton GetButton(int idx) { return Get<UIButton>(idx); }
    protected UISprite GetSprite(int idx) { return Get<UISprite>(idx); }
    protected UISlider GetSlider(int idx) { return Get<UISlider>(idx); }
    protected UITexture GetTexture(int idx) { return Get<UITexture>(idx); }
    protected UIInput GetInputField(int idx) { return Get<UIInput>(idx); }
    protected UIScrollView GetScrollView(int idx) { return Get<UIScrollView>(idx); }
    //public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    //{
    //    UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

    //    switch (type)
    //    {
    //        case Define.UIEvent.Click:
    //            evt.OnPointerClickHandler -= action;
    //            evt.OnPointerClickHandler += action;
    //            break;
    //        case Define.UIEvent.Drag:
    //            evt.OnDragHandler -= action;
    //            evt.OnDragHandler += action;
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
