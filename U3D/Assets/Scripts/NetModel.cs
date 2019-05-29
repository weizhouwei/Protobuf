using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;

[ProtoContract]
public class NetModel
{
    [ProtoMember(1)]
    public string Message
    {
        get;
        set;
    }
}
