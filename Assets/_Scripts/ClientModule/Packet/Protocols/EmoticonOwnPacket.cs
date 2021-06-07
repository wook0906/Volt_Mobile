using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class EmoticonOwnPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("EmoticonOwnPacket Unpack");

        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        int columCount = ByteConverter.ToInt(buffer, ref startIndex);
        //Debug.Log(columCount);

        int id;
        bool value;

        for (int i = 0; i < columCount; i++)
        {
            id = ByteConverter.ToInt(buffer, ref startIndex);
            value = ByteConverter.ToBool(buffer, ref startIndex);
            DBManager.instance.userEmoticonCondition.Add(id, value);
            //Debug.Log($"EmoticonOwn {id} , {value}");
        }
        DBManager.instance.OnLoadedUserEmoticonCondition();
    }
}
