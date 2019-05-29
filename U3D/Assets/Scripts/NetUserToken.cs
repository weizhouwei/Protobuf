using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using ProtoBuf;

public class NetUserToken 
{
    public Socket socket;

    private int startIndex; //代表了存取了多少数据也代表了从数组什么位置存储
    public int StartIndex
    {
        get
        {
            return startIndex;
        }

        set
        {
            startIndex = value;
        }
    }

    private byte[] buffer;
    public byte[] Buffer
    {
        get
        {
            return buffer;
        }

        set
        {
            buffer = value;
        }
    }

    public int RemianSize
    {
        get
        {
            return buffer.Length - startIndex;
        }
    }

    public NetUserToken() {
        Buffer = new byte[1024];
    }

    private void ParseData(int length) {
        startIndex += length;
        while (true)
        {
            if (startIndex <= 4) return;
            int count = System.BitConverter.ToInt32(buffer, 0);
            if ((startIndex - 4) >= count)
            { //解析出来一条新的数据
                //string s = System.Text.Encoding.UTF8.GetString(buffer, 4, count);
                byte[] ss = new byte[count];
                for (int i = 0; i < count; i++)
                {
                    ss[i] = buffer[i + 4];
                }
                NetModel netModel = DeSerialize<NetModel>(ss);
                UnityEngine.Debug.Log("接收到消息:" + netModel.Message);
                System.Buffer.BlockCopy(buffer, count + 4, buffer, 0, startIndex - count - 4);
                startIndex -= (count + 4);
            }
            else
            {
                break;
            }
        }

        //while (true) {
        //    if (startIndex <= 4) return;
        //    int count = System.BitConverter.ToInt32(buffer, 0);
        //    if ((startIndex - 4) >= count)
        //    { //解析出来一条新的数据
        //        //string s = System.Text.Encoding.UTF8.GetString(buffer, 4, count);
        //        byte[] ss = new byte[count];
        //        for (int i = 0; i < count; i++) {
        //            ss[i] = buffer[i+4];
        //        }
        //        byte[] s = DeSerialize<byte[]>(ss);
        //        UnityEngine.Debug.Log("接收到消息:" + System.Text.Encoding.UTF8.GetString(s));
        //        System.Buffer.BlockCopy(buffer, count + 4, buffer, 0, startIndex - count - 4);
        //        startIndex -= (count + 4);
        //    }
        //    else {
        //        break;
        //    }
        //}
    }

    private T DeSerialize<T>(byte[] bytes)
    {
        using (MemoryStream ms = new MemoryStream(bytes))
        {
            T obj = Serializer.Deserialize<T>(ms);
            return obj;
        }
    }

    public void Receive(int length) {
        ParseData(length);
        //UnityEngine.Debug.Log("接收到消息:" + System.Text.Encoding.UTF8.GetString(data));
    }

    public void Send(byte[] data) {

    }
}
