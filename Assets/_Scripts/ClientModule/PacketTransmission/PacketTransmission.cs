// 임의로 Enum 설정
using UnityEngine;
using System.Text;
using System;

public static class CommunicationInfo
{
    public static bool IsBoardGamePlaying = false;
    public static bool IsMobileActive = true;
    public static int PlayCount = 0;
    public static int ModuleChoiceRequestOrder = 0;
}

public static class PacketTransmission
{
    static PacketTransmission()
    {
    }
    
    public static void SendReconnectionPacket(string token)
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.ReConnectionPacket, buffer, ref startIndex);

        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt(Encoding.UTF8.GetBytes(token).Length, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.StringNumber;
        ByteConverter.FromString(token, buffer, ref startIndex);

         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendSignInPacket(int tokenLength, string token)
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.SignInPacket, buffer, ref startIndex);

        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt(Encoding.UTF8.GetBytes(token).Length, buffer, ref startIndex);
        
        buffer[startIndex++] = PacketInfo.StringNumber;
        ByteConverter.FromString(token, buffer, ref startIndex);

        DateTime dateTime = Volt.Time.GetGoogleDateTime();

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(dateTime.Year, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(dateTime.Month, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(dateTime.Day, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(dateTime.Hour, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(dateTime.Minute, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(dateTime.Second, buffer, ref startIndex);

        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);

        Debug.Log("Send SignIn Packet");
    }

    public static void SendSignInSuccessPacket(int userToken)
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.SignInSuccessPacket, buffer, ref startIndex);

        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt(userToken, buffer, ref startIndex);

         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendInternetConnectionCheckPacket()
    {
        if (!Volt_GameManager.S)
            return;

        //Debug.Log("Volt_GameManager.S : NOT NULL , " + Volt_GameManager.S);

        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.InternetConnectionCheckPacket, buffer, ref startIndex);

        ByteConverter.FromInt(0, buffer, ref startIndex);

         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendSignInFailPacket(int userToken)
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.SignInFailPacket, buffer, ref startIndex);

        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt(userToken, buffer, ref startIndex);

         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendSignUpPacket(int tokenLength, string token, int nickNameLength, string nickName)
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.SignUpPacket, buffer, ref startIndex);

        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt(tokenLength, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.StringNumber;
        ByteConverter.FromString(token, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(nickNameLength, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.StringNumber;
        ByteConverter.FromString(nickName, buffer, ref startIndex);

        DateTime dateTime = Volt.Time.GetGoogleDateTime();

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(dateTime.Year, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(dateTime.Month, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(dateTime.Day, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(dateTime.Hour, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(dateTime.Minute, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(dateTime.Second, buffer, ref startIndex);


        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendSignUpFailNicknameSamePacket(int userNickname)
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.SignUpFailNicknameSamePacket, buffer, ref startIndex);

        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt(userNickname, buffer, ref startIndex);

         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendBatteryShopInfoPacket()
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.BatteryShopInfoPacket, buffer, ref startIndex);

        ByteConverter.FromInt(0, buffer, ref startIndex);

         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendDiamondShopInfoPacket()
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.DiamondShopInfoPacket, buffer, ref startIndex);

        ByteConverter.FromInt(0, buffer, ref startIndex);

         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendDailyAchievementInfoPacket(int info)
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.DailyAchievementInfoPacket, buffer, ref startIndex);

        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt(info, buffer, ref startIndex);

         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendNormalAchievementInfoPacket(int info)
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.NormalAchievementInfoPacket, buffer, ref startIndex);

        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt(info, buffer, ref startIndex);

         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }
     
    public static void SendAchievementProgressPacket(int id, int playerNumber, bool needRoom)
    {
        if(Volt_GameManager.S != null)
            if(Volt_GameManager.S.IsTrainingMode || Volt_GameManager.S.IsTrainingMode)
                return;

        //Debug.Log($"SendAchievementProgressPacket {id}, {playerNumber}");
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;
        ByteConverter.FromInt((int)EPacketType.AchievementProgressPacket, buffer, ref startIndex);
        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt(id, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(playerNumber, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.BoolNumber;
        ByteConverter.FromBool(needRoom, buffer, ref startIndex);

         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);

    }
    public static void SendAchievementCompletePacket(int id, string aId)
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;
        ByteConverter.FromInt((int)EPacketType.AchievementCompletionPacket, buffer, ref startIndex);
        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt(id, buffer, ref startIndex);

        ByteConverter.FromInt(Encoding.UTF8.GetBytes(aId).Length, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.StringNumber;
        ByteConverter.FromString(aId, buffer, ref startIndex);

         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendDisConnectedPacket()
    {
        //Debug.Log("SendDisConnectedPacket");

        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.DisConnectedPacket, buffer, ref startIndex);
        ByteConverter.FromInt(0, buffer, ref startIndex);


         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendMatchingRequestPacket(int characterNumber, int skinType)
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.MatchingRequestPacket, buffer, ref startIndex);
        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt(characterNumber, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(skinType, buffer, ref startIndex);

         

        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendPlayerExitPacket()
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.PlayerExitPacket, buffer, ref startIndex);
        ByteConverter.FromInt(0, buffer, ref startIndex);

         
        if (Volt_GameManager.S.pCurPhase != Phase.gameOver)
            ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendCancelSearchingEnemyPlayerPacket()
    {
        byte[] buffer = IOBuffer.Dequeue();
        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.CancelSearchingEnemyPlayerPacket, buffer, ref startIndex);
        ByteConverter.FromInt(0, buffer, ref startIndex);

         
        if (Volt_GameManager.S.pCurPhase != Phase.gameOver)
            ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendFieldReadyCompletionPacket(Card cardType)
    {
        //Debug.Log("SendFieldReadyCompletionPacket");
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.FieldReadyCompletionPacket, buffer, ref startIndex);
        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt((int)cardType, buffer, ref startIndex);

         
        if (Volt_GameManager.S.pCurPhase != Phase.gameOver)
            ClientSocketModule.Send(buffer, startIndex);



        IOBuffer.Enqueue(buffer);
    }

    public static void SendCharacterPositionPacket(int playerNumber, int xPos, int yPos)
    {
        //Debug.Log("SendCharacterPositionPacket : " + playerNumber);
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;
        
        ByteConverter.FromInt((int)EPacketType.CharacterPositionPacket, buffer, ref startIndex);
        // PacketSize 위치
        ByteConverter.FromInt(0, buffer, ref startIndex);
        // PacketOrder 위치
        ByteConverter.FromInt(0, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(playerNumber, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(xPos, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(yPos, buffer, ref startIndex);

         
        if (Volt_GameManager.S.pCurPhase != Phase.gameOver)
            ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);   
    }

    public static void SendCharacterPositionCompletionPacket()
    {
        //Debug.Log("SendCharacterPositionCompletion");
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.CharacterPositionSettingCompletionPacket, buffer, ref startIndex);
        // PacketSize 위치
        ByteConverter.FromInt(0, buffer, ref startIndex);

         
        if (Volt_GameManager.S.pCurPhase != Phase.gameOver)
            ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendBehaviorOrderPacket(int playerNumber, Volt_RobotBehavior behaviour)
    {
        
        //Debug.Log("SendBehaviorOrderPacket");
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.BehaviorOrderPacket, buffer, ref startIndex);
        // PacketSize 위치
        ByteConverter.FromInt(0, buffer, ref startIndex);
        // PacketOrder 위치
        ByteConverter.FromInt(0, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(playerNumber, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(0, buffer, ref startIndex); // 여기에 서버에서 몇 번째로 패킷을 받았는지 순서값을 넣음
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(0, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(behaviour.BehaviorPoints, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(behaviour.Direction.ToString().Length,buffer,ref startIndex);
        buffer[startIndex++] = PacketInfo.StringNumber;
        ByteConverter.FromString(behaviour.Direction.ToString(), buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt((int)behaviour.BehaviourType, buffer, ref startIndex);
         
        if (Volt_GameManager.S.pCurPhase != Phase.gameOver)
            ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendBehaviorOrderCompletionPacket()
    {
        //Debug.Log("SendBehaviorOrderCompletionPacket" + DateTime.Now);
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.BehaviorOrderCompletionPacket, buffer, ref startIndex);
        // PacketSize 위치
        ByteConverter.FromInt(0, buffer, ref startIndex);

         
        if (Volt_GameManager.S.pCurPhase != Phase.gameOver)
            ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendVictoryPointPacket(int playerNumber, int vp)
    {
        //TODO
        if (Volt_GameManager.S.isEndlessGame) return; //추후 이 줄은 제거

        //Debug.Log("SendVictoryPointPacket");
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.VictoryPointPacket, buffer, ref startIndex);
        // PacketSize 위치
        ByteConverter.FromInt(0, buffer, ref startIndex);
        ByteConverter.FromInt(0, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(playerNumber, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(vp, buffer, ref startIndex);

         
        if (Volt_GameManager.S.pCurPhase != Phase.gameOver)
            ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }
    

   

    public static void SendSimulationCompletionPacket()
    {
        //Debug.Log("SendSimulationCompletionPacket");
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.SimulationCompletionPacket, buffer, ref startIndex);
        // PacketSize 위치
        ByteConverter.FromInt(0, buffer, ref startIndex);



         
        if (Volt_GameManager.S.pCurPhase != Phase.gameOver)
            ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendModuleActivePacket(int playerNumber,int slotNumber)
    {
        //Debug.Log("SendModuleActivePacket");
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.ModuleActivePacket, buffer, ref startIndex);
        // PacketSize 위치
        ByteConverter.FromInt(0, buffer, ref startIndex);
        // PacketOrder 위치
        ByteConverter.FromInt(0, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(playerNumber, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(slotNumber, buffer, ref startIndex);

         
        if (Volt_GameManager.S.pCurPhase != Phase.gameOver)
            ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendModuleUnActivePacket(int playerNumber, int slotNumber)
    {
        //Debug.Log("SendModuleUnActivePacket");
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.ModuleUnActivePacket, buffer, ref startIndex);
        // PacketSize 위치
        ByteConverter.FromInt(0, buffer, ref startIndex);
        // PacketOrder 위치
        ByteConverter.FromInt(0, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(playerNumber, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(slotNumber, buffer, ref startIndex);

         
        if (Volt_GameManager.S.pCurPhase != Phase.gameOver)
            ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendTotalTurnOverPacket(TileData[] newStateTileDatas, RobotData[] robotDatas, PlayerData[] playerDatas)
    {
        
        //Debug.Log("SendTotalTurnOverPacket");
        byte[] buffer = IOBuffer.Dequeue();
        

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.TotalTurnOverPacket, buffer, ref startIndex);
        // PacketSize 위치
        ByteConverter.FromInt(0, buffer, ref startIndex);

        
        ByteConverter.FromInt(Volt_GMUI.S.RoundNumber, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(Volt_GameManager.S.AmargeddonCount, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;

        int amargeddonPlayerNumber = 0;
        if (Volt_GameManager.S.AmargeddonPlayer != null)
            amargeddonPlayerNumber = Volt_GameManager.S.AmargeddonPlayer;
        ByteConverter.FromInt(amargeddonPlayerNumber, buffer, ref startIndex);
            

        //-------------------------수정--------------------------------------------------
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(Volt_GameManager.S.remainRoundCountToVpSetup, buffer, ref startIndex);
        //------------------------------------------------------------------------------


        if (newStateTileDatas == null)
        {
             
            ClientSocketModule.Send(buffer, startIndex);

            IOBuffer.Enqueue(buffer);

            return;
        }

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(newStateTileDatas.Length, buffer, ref startIndex);
        for (int i = 0; i < newStateTileDatas.Length; i++)
        {
            buffer[startIndex++] = PacketInfo.IntNumber;
            ByteConverter.FromInt(newStateTileDatas[i].tileIdx, buffer, ref startIndex);

            buffer[startIndex++] = PacketInfo.IntNumber;
            ByteConverter.FromInt((int)newStateTileDatas[i].tileType, buffer, ref startIndex);

            buffer[startIndex++] = PacketInfo.IntNumber;
            ByteConverter.FromInt((int)newStateTileDatas[i].tileInModule, buffer, ref startIndex);

            buffer[startIndex++] = PacketInfo.IntNumber;
            ByteConverter.FromInt(newStateTileDatas[i].numOfVP, buffer, ref startIndex);

            buffer[startIndex++] = PacketInfo.BoolNumber;
            ByteConverter.FromBool(newStateTileDatas[i].isHaveRepairKit, buffer, ref startIndex);

            buffer[startIndex++] = PacketInfo.BoolNumber;
            ByteConverter.FromBool(newStateTileDatas[i].isHaveTimeBomb, buffer, ref startIndex);

            buffer[startIndex++] = PacketInfo.BoolNumber;
            ByteConverter.FromBool(newStateTileDatas[i].isOnVoltage, buffer, ref startIndex);
            Debug.Log($"Send tileIdx : {newStateTileDatas[i].tileIdx}, isOnVoltage : {newStateTileDatas[i].isOnVoltage}");

            buffer[startIndex++] = PacketInfo.IntNumber;
            ByteConverter.FromInt(newStateTileDatas[i].timeBombOwnerPlayerNumber, buffer, ref startIndex);

            buffer[startIndex++] = PacketInfo.IntNumber;
            ByteConverter.FromInt(newStateTileDatas[i].timeBombCount, buffer, ref startIndex);

        }
        int numOfRobot = robotDatas.Length;

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(numOfRobot, buffer, ref startIndex);
        for (int i = 0; i < numOfRobot; i++)
        {
            buffer[startIndex++] = PacketInfo.IntNumber;
            ByteConverter.FromInt(robotDatas[i].tileIdx, buffer, ref startIndex);
            buffer[startIndex++] = PacketInfo.IntNumber;
            ByteConverter.FromInt(robotDatas[i].ownerPlayerNumber, buffer, ref startIndex);
            buffer[startIndex++] = PacketInfo.IntNumber;
            ByteConverter.FromInt((int)robotDatas[i].slot1Module, buffer, ref startIndex);
            buffer[startIndex++] = PacketInfo.IntNumber;
            ByteConverter.FromInt(robotDatas[i].module1State, buffer, ref startIndex);
            buffer[startIndex++] = PacketInfo.IntNumber;
            ByteConverter.FromInt((int)robotDatas[i].slot2Module, buffer, ref startIndex);
            buffer[startIndex++] = PacketInfo.IntNumber;
            ByteConverter.FromInt(robotDatas[i].module2State, buffer, ref startIndex);
            buffer[startIndex++] = PacketInfo.IntNumber;
            ByteConverter.FromInt(robotDatas[i].hitCount, buffer, ref startIndex);
            buffer[startIndex++] = PacketInfo.IntNumber;
            ByteConverter.FromInt(robotDatas[i].shieldPoint, buffer, ref startIndex);
            buffer[startIndex++] = PacketInfo.IntNumber;
            ByteConverter.FromInt(robotDatas[i].lookDirectionLength, buffer, ref startIndex);


            buffer[startIndex++] = PacketInfo.StringNumber;
            ByteConverter.FromString(robotDatas[i].lookDirection, buffer, ref startIndex);

            buffer[startIndex++] = PacketInfo.BoolNumber;
            ByteConverter.FromBool(robotDatas[i].isHaveTimeBomb,buffer, ref startIndex);

            buffer[startIndex++] = PacketInfo.IntNumber;
            ByteConverter.FromInt(robotDatas[i].timeBombCount, buffer, ref startIndex);

            buffer[startIndex++] = PacketInfo.IntNumber;
            ByteConverter.FromInt(robotDatas[i].timeBombOwnerNumber, buffer, ref startIndex);
        }
        for (int i = 0; i < 4; i++)
        {
            buffer[startIndex++] = PacketInfo.IntNumber;
            ByteConverter.FromInt(playerDatas[i].playerNumber, buffer, ref startIndex);
            buffer[startIndex++] = PacketInfo.IntNumber;
            ByteConverter.FromInt(playerDatas[i].numOfhasVP, buffer, ref startIndex);
            buffer[startIndex++] = PacketInfo.IntNumber;
            ByteConverter.FromInt((int)playerDatas[i].playerType, buffer, ref startIndex);
            buffer[startIndex++] = PacketInfo.BoolNumber;
            ByteConverter.FromBool(playerDatas[i].isRobotAlive, buffer, ref startIndex);
        }
        
         
        if (Volt_GameManager.S.pCurPhase != Phase.gameOver)
            ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendMobileActivePacket(int playerNumber)
    {
        //Debug.Log("SendMobileActivePacket");
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.MobileActivePacket, buffer, ref startIndex);
        // PacketSize 위치
        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt(playerNumber, buffer, ref startIndex);
        


         
        if (Volt_GameManager.S.pCurPhase != Phase.gameOver)
            ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendMobileUnActivePacket(int playerNumber)
    {
        CommunicationInfo.IsMobileActive = false;

        //Debug.Log("SendMobileUnActivePacket");
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.MobileUnActivePacket, buffer, ref startIndex);
        // PacketSize 위치
        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt(playerNumber, buffer, ref startIndex);


         
        if (Volt_GameManager.S.pCurPhase != Phase.gameOver)
            ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }
    
    public static void SendAchivementCompletionPacket(int achId)
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.AchievementCompletionPacket, buffer, ref startIndex);

        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt(achId, buffer, ref startIndex);

         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendAchivementProgressPacket(int achId, int playerNumber)
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.AchievementProgressPacket, buffer, ref startIndex);

        ByteConverter.FromInt(0, buffer, ref startIndex);
        ByteConverter.FromInt(achId, buffer, ref startIndex);

        ByteConverter.FromInt(0, buffer, ref startIndex);
        ByteConverter.FromInt(playerNumber, buffer, ref startIndex);


         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendItemPurchasePacket(int itemId)
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.ItemPurchasePacket, buffer, ref startIndex);

        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt(itemId, buffer, ref startIndex);

         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendGoldPacket(int goldVariation)
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.GoldPacket, buffer, ref startIndex);

        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt(goldVariation, buffer, ref startIndex);

         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendDiamondPacket(int diamondVariation)
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.DiamondPacket, buffer, ref startIndex);

        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt(diamondVariation, buffer, ref startIndex);

         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendBatteryPacket(int BatteryVariation)
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.BatteryPacket, buffer, ref startIndex);

        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt(BatteryVariation, buffer, ref startIndex);

         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }
    public static void SendBatteryTimeRegisterPacket(int year, int month, int day, int hour, int minute, int second)
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.BatteryTimeRegisterPacket, buffer, ref startIndex);

        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt(year, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(month, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(day, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(hour, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(minute, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(second, buffer, ref startIndex);

         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
        //Debug.Log("Send Battery Time complete");
    }
    public static void SendBatteryTimeRequestPacket()
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.BatteryTimeRequestPacket, buffer, ref startIndex);

        ByteConverter.FromInt(0, buffer, ref startIndex);

         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }
    
    public static void SendAttackPacket(int attackPlayer, int roundNumber)
    {
        if (Volt_GameManager.S.IsTrainingMode || Volt_GameManager.S.IsTutorialMode)
            return;

        //if (Volt_GameManager.S.isEndlessGame) return; //추후 이 줄은 제거
        //Debug.Log("SendAttackPacket");
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.AttackPacket, buffer, ref startIndex);
        // PacketSize 위치
        ByteConverter.FromInt(0, buffer, ref startIndex);
        ByteConverter.FromInt(0, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(attackPlayer, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(roundNumber, buffer, ref startIndex);

         
        if (Volt_GameManager.S.pCurPhase != Phase.gameOver)
            ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }
    public static void SendAttackSuccessPacket(int attackSuccessPlayer, int roundNumber)
    {
        if (Volt_GameManager.S.IsTrainingMode || Volt_GameManager.S.IsTutorialMode)
            return;

        //Debug.Log("SendAttackSuccessPacket");
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.AttackSuccessPacket, buffer, ref startIndex);
        // PacketSize 위치
        ByteConverter.FromInt(0, buffer, ref startIndex);
        ByteConverter.FromInt(0, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(attackSuccessPlayer, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(roundNumber, buffer, ref startIndex);

         
        if (Volt_GameManager.S.pCurPhase != Phase.gameOver)
            ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendKillPacket(int roundNumber, int killerPlayer, int destroyedPlayer)
    {
        if (Volt_GameManager.S.isEndlessGame) return; //추후 이 줄은 제거

        //Debug.Log("SendKillPacket");
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.KillPacket, buffer, ref startIndex);
        // PacketSize 위치
        ByteConverter.FromInt(0, buffer, ref startIndex);
        ByteConverter.FromInt(0, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(roundNumber, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(killerPlayer, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(destroyedPlayer, buffer, ref startIndex);

         
        if (Volt_GameManager.S.pCurPhase != Phase.gameOver)
            ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }
    public static void SendShopPurchasePacket(EShopPurchase itemType, int itemID)
    {
        //Debug.Log("SendShopPurchasepacket");

        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.ShopPurchasePacket, buffer, ref startIndex);
        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt((int)itemType, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(itemID, buffer, ref startIndex);

         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendBatteryPurchasePacket(int neededGoldPrice, int purchasedBatteryCount)
    {
       // Debug.Log("SendBatteryPurchasePacket");

        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.BatteryPurchasePacket, buffer, ref startIndex);
        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt(neededGoldPrice, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(purchasedBatteryCount, buffer, ref startIndex);

         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    // 
    public static void SendIAPPacket(int startSerial, int endSerial, int itemID, bool googlePlatForm, string productId, string purchaseToken, string payload)
    {
        //Debug.Log("SendIAPPacket");

        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.IAPPacket, buffer, ref startIndex);
        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt(startSerial, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(endSerial, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(itemID, buffer, ref startIndex);


        buffer[startIndex++] = PacketInfo.BoolNumber;
        ByteConverter.FromBool(googlePlatForm, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(productId.Length, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.StringNumber;
        ByteConverter.FromString(productId, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(purchaseToken.Length, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.StringNumber;
        ByteConverter.FromString(purchaseToken, buffer, ref startIndex);

        if(!googlePlatForm)
        {
            buffer[startIndex++] = PacketInfo.IntNumber;
            ByteConverter.FromInt(payload.Length, buffer, ref startIndex);
            buffer[startIndex++] = PacketInfo.StringNumber;
            ByteConverter.FromString(payload, buffer, ref startIndex);
        }

        //DateTime dateTime = Volt.Time.GetGoogleDateTime();
        //ByteConverter.FromInt(dateTime.Year, buffer, ref startIndex);
        //buffer[startIndex++] = PacketInfo.IntNumber;
        //ByteConverter.FromInt(dateTime.Month, buffer, ref startIndex);
        //buffer[startIndex++] = PacketInfo.IntNumber;
        //ByteConverter.FromInt(dateTime.Day, buffer, ref startIndex);
        //buffer[startIndex++] = PacketInfo.IntNumber;
        //ByteConverter.FromInt(dateTime.Hour, buffer, ref startIndex);
        //buffer[startIndex++] = PacketInfo.IntNumber;
        //ByteConverter.FromInt(dateTime.Minute, buffer, ref startIndex);
        //buffer[startIndex++] = PacketInfo.IntNumber;
        //ByteConverter.FromInt(dateTime.Second, buffer, ref startIndex);

         
        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }
    public static void SendUserDataForMovingLobbyPacket()
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.UserDataForMovingLobbyPacket, buffer, ref startIndex);

        ByteConverter.FromInt(0, buffer, ref startIndex);

         

        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }
    public static void SendDeleteUserPacket(int deleteUserNicknameLength, string deleteUserNickname)
    {
        byte[] buffer = IOBuffer.Dequeue();

        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;

        ByteConverter.FromInt((int)EPacketType.DeleteUserPacket, buffer, ref startIndex);

        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt(deleteUserNicknameLength, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.StringNumber;
        ByteConverter.FromString(deleteUserNickname, buffer, ref startIndex);
         

        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }
    public static void SendUseEmoticon(int playerNum, int emoticonID)
    {
        byte[] buffer = IOBuffer.Dequeue();
        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;
        ByteConverter.FromInt((int)EPacketType.UseEmoticonPacket, buffer, ref startIndex);
        // PacketSize 위치
        ByteConverter.FromInt(0, buffer, ref startIndex);
        // PacketOrder 위치
        ByteConverter.FromInt(0, buffer, ref startIndex);

        buffer[startIndex++] = PacketInfo.IntNumber;

        ByteConverter.FromInt(playerNum, buffer, ref startIndex);
        buffer[startIndex++] = PacketInfo.IntNumber;
        ByteConverter.FromInt(emoticonID, buffer, ref startIndex);

        ClientSocketModule.Send(buffer, startIndex);


        IOBuffer.Enqueue(buffer);

    }
    public static void SendAdsWatch()
    {
        byte[] buffer = IOBuffer.Dequeue();
        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;
        ByteConverter.FromInt((int)EPacketType.AdsWatchPacket, buffer, ref startIndex);
        // PacketSize 위치
        ByteConverter.FromInt(0, buffer, ref startIndex);

        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }

    public static void SendRequestBenefit(EBenefitType type)
    {
        byte[] buffer = IOBuffer.Dequeue();
        buffer[0] = PacketInfo.PacketStartNumber;
        int startIndex = 1;
        ByteConverter.FromInt((int)EPacketType.RequestBenefitPacket, buffer, ref startIndex);
        // PacketSize 위치
        ByteConverter.FromInt(0, buffer, ref startIndex);

        ByteConverter.FromInt((int)type, buffer, ref startIndex);

        ClientSocketModule.Send(buffer, startIndex);

        IOBuffer.Enqueue(buffer);
    }
}