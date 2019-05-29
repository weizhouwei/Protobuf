using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

[ProtoContract]
class NetModel
{
    [ProtoMember(1)]
    public string Message { get; set; }
}
