using UnityEngine;

public class MatchingCompletionPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        //Debug.Log("MatchingCompletionPacket UnPack");

       
        CommunicationWaitQueue.Instance.ResetOrder();
        CommunicationInfo.IsBoardGamePlaying = true;
        CommunicationInfo.PlayCount = 0;
        CommunicationInfo.ModuleChoiceRequestOrder = 0;


        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        int mapType = ByteConverter.ToInt(buffer, ref startIndex);

        // 1???? ????????, 2???? ????????
        int player1Type = ByteConverter.ToInt(buffer, ref startIndex);
        int player2Type = ByteConverter.ToInt(buffer, ref startIndex);
        int player3Type = ByteConverter.ToInt(buffer, ref startIndex);
        int player4Type = ByteConverter.ToInt(buffer, ref startIndex);

        int character1Type = ByteConverter.ToInt(buffer, ref startIndex);
        int character2Type = ByteConverter.ToInt(buffer, ref startIndex);
        int character3Type = ByteConverter.ToInt(buffer, ref startIndex);
        int character4Type = ByteConverter.ToInt(buffer, ref startIndex);

        int skin1 = ByteConverter.ToInt(buffer, ref startIndex);
        int skin2 = ByteConverter.ToInt(buffer, ref startIndex);
        int skin3 = ByteConverter.ToInt(buffer, ref startIndex);
        int skin4 = ByteConverter.ToInt(buffer, ref startIndex);

        int nickName1Length = ByteConverter.ToInt(buffer, ref startIndex);
        string nickName1 = ByteConverter.ToString(buffer, ref startIndex, nickName1Length);
        int nickName2Length = ByteConverter.ToInt(buffer, ref startIndex);
        string nickName2 = ByteConverter.ToString(buffer, ref startIndex, nickName2Length);
        int nickName3Length = ByteConverter.ToInt(buffer, ref startIndex);
        string nickName3 = ByteConverter.ToString(buffer, ref startIndex, nickName3Length);
        int nickName4Length = ByteConverter.ToInt(buffer, ref startIndex);
        string nickName4 = ByteConverter.ToString(buffer, ref startIndex, nickName4Length);



        //Debug.Log("mapType : " + mapType);

        //Debug.Log("player1 : " + ((PlayerType)player1Type).ToString() + "RobotType : " + ((RobotType)character1Type).ToString() + "SkinType : " + ((SkinType)skin1).ToString());
        //Debug.Log("player2 : " + ((PlayerType)player2Type).ToString() + "RobotType : " + ((RobotType)character2Type).ToString() + "SkinType : " + ((SkinType)skin2).ToString());
        //Debug.Log("player3 : " + ((PlayerType)player3Type).ToString() + "RobotType : " + ((RobotType)character3Type).ToString() + "SkinType : " + ((SkinType)skin3).ToString());
        //Debug.Log("player4 : " + ((PlayerType)player4Type).ToString() + "RobotType : " + ((RobotType)character4Type).ToString() + "SkinType : " + ((SkinType)skin4).ToString());


        //Debug.Log("character1Type : " + character1Type);
        //Debug.Log("character2Type : " + character2Type);
        //Debug.Log("character3Type : " + character3Type);
        //Debug.Log("character4Type : " + character4Type);
        //?????????? ???????? ?????? ?????? 0~3?????? ???? ???????? ??????.
        //Debug.Log("MatchingCompletion");

        //Debug.Log("skin1Type : " + skin1);
        //Debug.Log("skin2Type : " + skin2);
        //Debug.Log("skin3Type : " + skin3);
        //Debug.Log("skin4Type : " + skin4);

        //Debug.Log("nickName1 : " + nickName1);
        //Debug.Log("nickName2 : " + nickName2);
        //Debug.Log("nickName3 : " + nickName3);
        //Debug.Log("nickName4 : " + nickName4);

        //?????? ?????? ?????? ?????? ???? ??????.
        Volt_PlayerManager.S.SetupPlayersInfo((PlayerType)player1Type, (RobotType)character1Type, (SkinType)skin1, nickName1,
                                            (PlayerType)player2Type, (RobotType)character2Type, (SkinType)skin2, nickName2,
                                            (PlayerType)player3Type, (RobotType)character3Type, (SkinType)skin3, nickName3,
                                            (PlayerType)player4Type, (RobotType)character4Type, (SkinType)skin4, nickName4);
        Volt_GameManager.S.ArenaSetupStart(mapType);
        Volt_PlayerData.instance.OnPlayGame();
        
    }
}