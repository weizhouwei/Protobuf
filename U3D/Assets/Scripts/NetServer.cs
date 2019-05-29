using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;

public class NetServer
{
    public static readonly NetServer Instance = new NetServer();
    private int port = 6555;
    private Socket server;
    private int maxClient = 10;
    //用户池
    private Stack<NetUserToken> pools;

    private NetServer() {
        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        server.Bind(new IPEndPoint(IPAddress.Parse("192.168.3.3"), port));
    }

    public void Start() {
        server.Listen(maxClient);
        UnityEngine.Debug.Log("Server OK!");
        //实例化客户端的用户池
        pools = new Stack<NetUserToken>(maxClient);
        for (int i = 0; i < maxClient; i++)
        {
            NetUserToken usertoken = new NetUserToken();
            pools.Push(usertoken);
        }
        //可以异步接受客户端, BeginAccept函数的第一个参数是回调函数，当有客户端连接的时候自动调用
        server.BeginAccept(AsyncAccept, null);
    }

    //回调函数， 有客户端连接的时候会自动调用此方法
    private void AsyncAccept(IAsyncResult ar) {
        NetUserToken userToken = null;
        try
        {
            //结束监听，同时获取到客户端
            Socket client = server.EndAccept(ar);
            UnityEngine.Debug.Log("有客户端连接");
            userToken  = pools.Pop();
            userToken.socket = client;
            //客户端连接之后，可以接受客户端消息
            BeginReceive(userToken);
            //尾递归，再次监听是否还有其他客户端连入
            server.BeginAccept(AsyncAccept, null);
        }
        catch (Exception ex) {
            UnityEngine.Debug.Log(ex.ToString());
            if (userToken.socket != null) {
                userToken.socket.Close();
            }
        }
    }

    //异步监听消息
    private void BeginReceive(NetUserToken userToken) {
        try
        {
            userToken.socket.BeginReceive(userToken.Buffer, userToken.StartIndex, userToken.RemianSize, SocketFlags.None, AsyncRecive, userToken);
        }
        catch (Exception ex) {
            UnityEngine.Debug.Log(ex.ToString());
        }
    }

    //监听到消息之后调用的函数
    private void AsyncRecive(IAsyncResult ar) {
        try
        {
            NetUserToken userToken = ar.AsyncState as NetUserToken;
            int length = userToken.socket.EndReceive(ar);//结束接收
            if (length == 0) {
                userToken.socket.Close();
                return;
            }
            userToken.Receive(length);
            BeginReceive(userToken);

            //if (length > 0) {
            //    byte[] data = new byte[length];
            //    Buffer.BlockCopy(userToken.Buffer, 0, data, 0, length);
            //    //用户接受消息
            //    userToken.Receive(data);
            //    //尾递归，再次监听客户端消息
            //    BeginReceive(userToken);
            //}
        }
        catch (Exception ex) {
            UnityEngine.Debug.Log(ex.ToString());
        }
    }
}
