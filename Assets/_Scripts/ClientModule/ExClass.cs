using System;
using System.Text;

public static class ExClass
{
    public static bool Contain(this int[] intArr, int value)
    {
        for (int index = 0; index < intArr.Length; index++)
            if (intArr[index] == value)
                return true;

        return false;
    }

    public static int IndexOf(this byte[] array, string value)
    {
        byte[] stov = Encoding.UTF8.GetBytes(value);
        for (int i = 0; i < array.Length - stov.Length + 1; i++)
        {
            if (array[i] == stov[0])
            {
                for (int j = 1; j < stov.Length; j++)
                {
                    if (array[i + j] != stov[j])
                        break;
                    if (j == stov.Length - 1)
                        return i;
                }
            }
        }
        return -1;
    }

    public static void Clear(this byte[] array)
    {
        Array.Clear(array, 0, array.Length);
    }

    public static int RealLength(this byte[] b1)
    {
        for (int i = b1.Length - 1; i > -1; i--)
        {
            if (b1[i] != 0)
                return i + 1;
        }
        return 0;
    }

    public static byte[] AddTo(this byte[] b1, byte[] add)
    {

        int b1L = b1.RealLength();
        if (b1L == 0)
            return add;
        int addL = add.RealLength();
        if (addL == 0)
            return b1;
        byte[] ret = new byte[b1L + addL];
        Array.Copy(b1, 0, ret, 0, b1L);
        Array.Copy(add, 0, ret, b1L, addL);
        return ret;
    }

    public static byte[] SubArray(this byte[] array, int idx)
    {
        byte[] ret = new byte[array.Length - idx];
        Array.Copy(array, idx, ret, 0, array.Length - idx);
        return ret;

    }

    public static void Print(this byte[] array)
    {
        int length = array.RealLength();

        for (int i = 0; i < length; i++)
            Console.Write(array[i]);
        //TestClient.MainWindow.AddMsgText("");
    }
}