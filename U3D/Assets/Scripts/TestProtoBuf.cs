using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;
using System.IO;

public class TestProtoBuf : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetServer.Instance.Start();
    }

    private void Test() {
        NetModel netModel = new NetModel() {  Message = "Unity" };
        byte[] bytes = Serialize<NetModel>(netModel);
        NetModel temp = DeSerialize<NetModel>(bytes);
        Debug.Log(temp.Message);
    }

    private byte[] Serialize<T>(T model) {
        using (MemoryStream ms = new MemoryStream()) {
            Serializer.Serialize<T>(ms, model);
            return ms.ToArray();
        }
    }

    private T DeSerialize<T>(byte[] bytes) {
        using (MemoryStream ms = new MemoryStream(bytes)) {
            T obj = Serializer.Deserialize<T>(ms);
            return obj;
        }
    }
}
