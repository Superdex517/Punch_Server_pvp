using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PacketHandler
{
    public static void C_TestHandler(PacketSession session, IMessage packet)
    {
        C_Test pkt = packet as C_Test;
        System.Console.WriteLine(pkt.Temp);
    }
}
