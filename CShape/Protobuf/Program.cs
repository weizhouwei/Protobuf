using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ProtoBuf;
using System.Net.Sockets;
using System.Net;

[ProtoContract]
public class PersonByProtobuf
{
    [ProtoMember(1)]
    public string Name
    {
        get;
        set;
    }
    [ProtoMember(2)]
    public int Age
    {
        get;
        set;
    }
}

[Serializable]
public class Person
{
    public string name;
    public int age;
}

namespace Helper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("NetClient");
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.3.3"), 6555);
            socket.Connect(endPoint);

            //while (true)
            //{
            //    string message = Console.ReadLine();
            //    if (message == "c")
            //    {
            //        socket.Close();
            //        break;
            //    }
            //    NetModel netModel = new NetModel();
            //    netModel.Message = message;
            //    byte[] data = Serialize<NetModel>(netModel);
            //    byte[] newData = NetMessage.GetBytes(data);
            //    socket.Send(newData);
            //}

            for (int i = 0; i < 1000; i++) {
                NetModel netModel = new NetModel();
                netModel.Message = "客户端发送消息给服务器:"+i.ToString();
                byte[] data = Serialize<NetModel>(netModel);
                byte[] newData = NetMessage.GetBytes(data);
                socket.Send(newData);
            }
            Console.ReadKey();
        }

        private static void Test() {
            PersonByProtobuf person = new PersonByProtobuf();
            person.Name = "zwe";
            person.Age = 11;
            byte[] bytes = Serialize<PersonByProtobuf>(person);
            string s = Encoding.UTF8.GetString(bytes);
            PersonByProtobuf p = DeSerializeByProtoBuf<PersonByProtobuf>(bytes);
            Console.WriteLine(p.Name + " " + p.Age);
        } 
        #region Protobuf
        public static byte[] Serialize<T>(T model)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize<T>(ms, model);
                byte[] bytes = ms.ToArray();
                return bytes;
            }
        }
        public static T DeSerializeByProtoBuf<T>(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                T t = Serializer.Deserialize<T>(ms);
                return t;
            }
        }
        #endregion

        #region Common
        public static byte[] Serialize(object data) {
            using (MemoryStream ms = new MemoryStream()) {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, data);
                byte[] bytes = ms.GetBuffer();
                return bytes;
            }
        }
        public static object DeSerialize(byte[] buff) {
            using (MemoryStream ms = new MemoryStream(buff)) {
                BinaryFormatter bf = new BinaryFormatter();
                object obj = bf.Deserialize(ms);
                return obj;
            }
        }
        #endregion
    }
}
