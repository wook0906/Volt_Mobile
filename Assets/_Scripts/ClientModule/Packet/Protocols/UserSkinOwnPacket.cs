using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class UserSkinOwnPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        Debug.Log("UserSkinOwnPacket Unpack");

        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        int columCount = ByteConverter.ToInt(buffer, ref startIndex);

        int id = 5000001;
        bool value;

        //List<UserACHCondition.InfoACH> achInfos = DBManager.instance.userNormalACHCondition.achInfos;
        for (int i = 0; i < columCount; i++)
        {
            value = ByteConverter.ToBool(buffer, ref startIndex);
            DBManager.instance.userSkinCondition.Add(id + i, value);
        }
        DBManager.instance.OnLoadedUserSkinCondition();
    }
}
