using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class Util
{
    public static string Encrypt(string data, string key)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();
        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;

        rijndaelCipher.KeySize = 128;
        rijndaelCipher.BlockSize = 128;
        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
        byte[] keyBytes = new byte[16];
        int len = pwdBytes.Length;
        if (len > keyBytes.Length)
        {
            len = keyBytes.Length;
        }
        Array.Copy(pwdBytes, keyBytes, len);
        rijndaelCipher.Key = keyBytes;
        rijndaelCipher.IV = keyBytes;
        ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
        byte[] plainText = Encoding.UTF8.GetBytes(data);
        return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
    }

    public static string Decrypt(string data, string key)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();
        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;

        rijndaelCipher.KeySize = 128;
        rijndaelCipher.BlockSize = 128;
        byte[] encryptedData = Convert.FromBase64String(data);
        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
        byte[] keyBytes = new byte[16];
        int len = pwdBytes.Length;
        if (len > keyBytes.Length)
        {
            len = keyBytes.Length;
        }
        Array.Copy(pwdBytes, keyBytes, len);
        rijndaelCipher.Key = keyBytes;
        rijndaelCipher.IV = keyBytes;
        byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        return Encoding.UTF8.GetString(plainText);
    }

    public static Vector2Int ConvertWorldDirToTileDir(Vector3 direction)
    {
        Vector2Int dir2D = Vector2Int.zero;
        if (Volt_Utils.IsForwardRight(direction))
        {
            dir2D.x = 1;
            dir2D.y = 1;
        }
        else if (Volt_Utils.IsForwardLeft(direction))
        {
            dir2D.x = 1;
            dir2D.y = -1;
        }
        else if (Volt_Utils.IsBackRight(direction))
        {
            dir2D.x = -1;
            dir2D.y = 1;
        }
        else if (Volt_Utils.IsBackLeft(direction))
        {
            dir2D.x = -1;
            dir2D.y = -1;
        }
        else if (Volt_Utils.IsRight(direction))
            dir2D.y = 1;
        else if (Volt_Utils.IsLeft(direction))
            dir2D.y = -1;
        else if (Volt_Utils.IsForward(direction))
            dir2D.x = 1;
        else if (Volt_Utils.IsBackward(direction))
            dir2D.x = -1;

        return dir2D;
    }

    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        if (go == null)
        {
            Debug.LogError("Failed GetOrAddComponent because gameObject is null");
            return null;
        }

        T component = go.GetComponent<T>();
        if (component == null)
        {
            component = go.AddComponent<T>();
        }
        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool isReculsive = false)
    {
        return FindChild<Transform>(go, name, isReculsive).gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool isReculsive = false) where T : UnityEngine.Object
    {
        if (go == null)
        {
            Debug.LogError("Failed FindChild because gameObject is null");
            return null;
        }

        if (!isReculsive)
        {
            for (int i = 0; i < go.transform.childCount; i++)            {                Transform transform = go.transform.GetChild(i);                if (string.IsNullOrEmpty(name) || transform.name == name)                {                    T component = transform.GetComponent<T>();                    if (component != null)                        return component;                }            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())            {                if (string.IsNullOrEmpty(name) || component.name == name)                    return component;            }
        }

        return null;
    }

    public static T FindParent<T>(GameObject go, string name = null, bool isReculsive = false) where T : UnityEngine.Object
    {
        if (go == null)
        {
            Debug.LogError("Failed FindChild because gameObject is null");
            return null;
        }

        if (!isReculsive)
        {
            Transform parent = go.transform.parent;

            T component = parent.GetComponent<T>();
            if (component != null &&
                (string.IsNullOrEmpty(name) || component.name == name))
                return component;
        }
        else
        {
            Transform parent = go.transform.parent;
            while (parent != null)
            {
                T component = parent.GetComponent<T>();
                if (component != null && (string.IsNullOrEmpty(name) ||
                    component.name == name))
                    return component;

                parent = parent.parent;
            }
        }

        return null;
    }

    public static EventDelegate.Parameter MakeParameter(UnityEngine.Object value, System.Type type)
    {
        EventDelegate.Parameter param = new EventDelegate.Parameter();
        // 이벤트 parameter 생성.     
        param.obj = value;
        // 이벤트 함수에 전달하고 싶은 값.     
        param.expectedType = type;
        // 값의 타입.       
        return param;

    }
}
