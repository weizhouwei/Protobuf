using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class NetMessage
{
    public static byte[] GetBytes(string data) {
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        int length = dataBytes.Length;
        byte[] lengthBytes = BitConverter.GetBytes(length);
        byte[] newBytes = lengthBytes.Concat(dataBytes).ToArray();
        return newBytes;
    }

    public static byte[] GetBytes(byte[] data)
    {
        byte[] dataBytes = data;
        int length = dataBytes.Length;
        byte[] lengthBytes = BitConverter.GetBytes(length);
        byte[] newBytes = lengthBytes.Concat(dataBytes).ToArray();
        return newBytes;
    }
}
