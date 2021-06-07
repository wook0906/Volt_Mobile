using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CommunicationWaitQueue : Singleton<CommunicationWaitQueue>
{
    private int order;

    private Queue<byte[]> packetQueue = new Queue<byte[]>();
    private LinkedList<byte[]> outOfSequencePacket = new LinkedList<byte[]>();

    private object lockObject = new object();

    private CommunicationWaitQueue()
    {
    }

    public void SetOrder(int order)
    {
        this.order = order;
    }

    public void ResetOrder()
    {
        order = 0;
    }

    public byte[] DequeuePacket()
    {
        if (packetQueue.Count == 0 && outOfSequencePacket.Count == 0)
            return null;

        if(packetQueue.Count == 0 && outOfSequencePacket.Count > 0)
        {
            if (!CheckOutOfSequence())
                return null;
        }

        return DequeuePacketQueue();
    }

    public void EnqueuePacket(byte[] buffer)
    {
        Monitor.Enter(lockObject);

        try
        {
            int packetType = ByteConverter.ToInt(buffer, 1);

            if (Volt_PlayerData.instance.NeedReConnection)
            {
                EnqueuePacketQueue(buffer);
                return;
            }

            if (IsDirectExecutionPacket((EPacketType)packetType))
            {
                packetQueue.Enqueue(buffer);
                return;
            }

            if (!CommunicationInfo.IsMobileActive)
            {
                Debug.LogError("걸러짐 IsMobileActive");
                return;
            }


            if (CommunicationInfo.IsBoardGamePlaying)
            {
                int receivedOrder = ByteConverter.ToInt(buffer, PacketInfo.PacketOrderPosition);

                Debug.LogFormat("Type : {0}, ReceivedOrder : {1}", (EPacketType)packetType, receivedOrder);
                

                if (order == receivedOrder)
                    EnqueuePacketQueue(buffer);

                if (order < receivedOrder)
                    outOfSequencePacket.AddLast(buffer);

            }
            else
            {
                EnqueuePacketQueue(buffer);
            }
        }
        catch(Exception e)
        {
            //Debug.Log("In EnqueuePacket, Message : " + e.Message);
        }
        finally
        {
            Monitor.Exit(lockObject);
        }
    }

    public bool IsDirectExecutionPacket(EPacketType type)
    {
        if (type == EPacketType.PlayerExitPacket ||
            type == EPacketType.MoveLobbyPacket ||
            type == EPacketType.GameOverPacket ||
            type == EPacketType.UserDailyAchConditionStatePacket ||
            type == EPacketType.UserDailyAchSuccessPacket ||
            type == EPacketType.UserNormalAchConditionStatePacket ||
            type == EPacketType.UserNormalAchSuccessPacket ||
            type == EPacketType.BatteryPacket ||
            type == EPacketType.BatteryTimeResponsePacket ||
            type == EPacketType.InternetConnectionCheckPacket ||
            type == EPacketType.GoldPacket ||
            type == EPacketType.SynchronizationTileDatasPacket ||
            type == EPacketType.SynchronizationElsePacket)
            return true;
        return false;
    }

    private byte[] DequeuePacketQueue()
    {
        return packetQueue.Dequeue();
    }

    private void EnqueuePacketQueue(byte[] buffer)
    {
        if (CommunicationInfo.IsBoardGamePlaying)
        {
            //Debug.Log("EnqueuePacketQueue order : " + order);
            order++;
        }

        packetQueue.Enqueue(buffer);

    }

    private bool CheckOutOfSequence()
    {
        bool result = false;
        while(MoveOutOfSequencePacketToPacketQueue())
        {
            result = true;
        };

        return result;
    }

    private bool MoveOutOfSequencePacketToPacketQueue()
    {
        Monitor.Enter(lockObject);

        try
        {
            byte[] findedBuffer = null;
            foreach(var buffer in outOfSequencePacket)
            {
                int checkingOrder = ByteConverter.ToInt(buffer, PacketInfo.PacketOrderPosition);

                //Debug.Log("MoveOutOfSequencePacketToPacketQueue");
                
                //Debug.Log("Order : " + order + ", CheckingOrder : " + checkingOrder + ", CheckingPacketType : " + (EPacketType)ByteConverter.ToInt(buffer, 1));

                if (order == checkingOrder)
                {
                    findedBuffer = buffer;
                    break;
                }
            }

            if (findedBuffer == null)
                return false;

            outOfSequencePacket.Remove(findedBuffer);
            EnqueuePacketQueue(findedBuffer);
            return true;
        }
        finally
        {
            Monitor.Exit(lockObject);
        }
    }
}