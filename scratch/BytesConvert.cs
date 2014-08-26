using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class BytesConvert
{
    public static byte[] toBase64Bytes(this byte[] bs)
    {
        var str = Convert.ToBase64String(bs);
        var len = bs.Length;
        var left = len%3;
        //var segs = (len/3) + (left == 0 ? 0 : 1);
        var ret = new List<byte>();
        for (int i = 0; i < str.Length; ++i)
        {
            char c = str[i];
            byte b = Convert.ToByte(c);
            ret.Add(b);
        }
        return ret.ToArray();
    }

    public static byte[] fromBase64(this byte[] bs)
    {
        var str = bs.bytesToString();
        var ret = Convert.FromBase64String(str);
        return ret;
    }

    public static byte[] to8BitsBytes(this string str)
    {
        var ret = new byte[str.Length];
        for (int i = 0; i < str.Length; ++i)
        {
            ret[i] = Convert.ToByte(str[i]);
        }
        return ret;
    }

    public static string bytesToString(this byte[] bs)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < bs.Length; ++i)
        {
            sb.Append(Convert.ToChar(bs[i]));
        }
        return sb.ToString();
    }

    public static string bytesToHexStr(this byte[] bytes)
    {
        string returnStr = "";
        if (bytes != null)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                returnStr += bytes[i].ToString("X2");
            }
        }
        return returnStr;
    } 

    public static byte[] hexStrToBytes(this string str)
    {
        int len = str.Length / 2;
        byte[] arr = new byte[len];
        for (int i = 0; i < len; i++)
        {
            arr[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);
        }
        return arr;
    }
}
