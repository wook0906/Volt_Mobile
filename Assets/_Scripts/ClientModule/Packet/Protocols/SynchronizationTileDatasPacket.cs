using UnityEngine;

public class SynchronizationTileDatasPacket : Packet
{
    public override void UnPack(byte[] buffer)
    {
        //Debug.Log("SynchronizationTileDatasPacket Unpack");
        


        if (Volt_GameManager.S.pCurPhase != Phase.synchronization)
            Volt_GameManager.S.pCurPhase = Phase.synchronization;

        int startIndex = PacketInfo.FromServerPacketSettingIndex;

        // 9 * 9 = 81; 타일 패킷이 9번 날아옴
        int num = ByteConverter.ToInt(buffer, ref startIndex);

        for (int index = 0; index < num; index++)
        {
            int tileIdx = ByteConverter.ToInt(buffer, ref startIndex);
            Volt_Tile.TileType type = (Volt_Tile.TileType)ByteConverter.ToInt(buffer, ref startIndex);
            Card tileInModuleType = (Card)ByteConverter.ToInt(buffer, ref startIndex);
            int numOfCoins = ByteConverter.ToInt(buffer, ref startIndex);
            bool isHaveRepairKit = ByteConverter.ToBool(buffer, ref startIndex);
            bool isHaveTimeBomb = ByteConverter.ToBool(buffer, ref startIndex);
            bool isOnVoltage = ByteConverter.ToBool(buffer, ref startIndex);
            int timeBombOwner = ByteConverter.ToInt(buffer, ref startIndex);
            int timeBombCount = ByteConverter.ToInt(buffer, ref startIndex);


            TileData data;
            data.tileIdx = tileIdx;
            data.tileType = type;
            data.tileInModule = tileInModuleType;
            data.numOfVP = numOfCoins;
            data.isHaveRepairKit = isHaveRepairKit;
            data.isHaveTimeBomb = isHaveTimeBomb;
            data.isOnVoltage = isOnVoltage;
            data.timeBombOwnerPlayerNumber = timeBombOwner;
            data.timeBombCount = timeBombCount;

            Volt_GameManager.S.tileDataForSync.Enqueue(data);
            Debug.Log($"Receive idx : {data.tileIdx}, isOnVoltage : {data.isOnVoltage}");
            //Debug.LogFormat("idx : {6}, type : {0}, module : {1}, numofvp : {2}, ishaverepairkit : {3}, ishavetimebomb : {4}, timebombowner : {5}, isOnVoltage : {7} ",
                //type, tileInModuleType, numOfCoins, isHaveRepairKit, isHaveTimeBomb, timeBombOwner, tileIdx, isOnVoltage);

            
            

            //타일동기화는 여기서진행
        }
        

        if(Volt_GameManager.S.tileDataForSync.Count == 81)
        {
            Volt_GameManager.S.TileDataSynchronizationStart();
        }
        
        
    }
}