using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

class IOBuffer : Singleton<IOBuffer>
{
    private Queue<byte[]> buffers;

    private readonly object IOBufferLocker = new object();

    private IOBuffer()
    {
        buffers = new Queue<byte[]>();

        for (int num = 0; num < SocketInfo.MaxIOBufferCount; num++)
            buffers.Enqueue(new byte[SocketInfo.MaxSizeOfPacket]);
    }

    public static byte[] Dequeue()
    {
        Monitor.Enter(Instance.IOBufferLocker);

        try
        {
            if (Instance.buffers.Count == 0)
                Instance.AddIOBuffer();

            byte[] buffer = Instance.buffers.Dequeue();

            for (int index = 0; index < buffer.Length; index++)
                buffer[index] = 0;

            return buffer;
        }
        finally
        {
            Monitor.Exit(Instance.IOBufferLocker);
        }
    }

    public static void Enqueue(byte[] buffer)
    {
        if (buffer.Length < 8000)
        {
            //Debug.LogError("buffer.Length : " + buffer.Length);

            //Debug.LogError("Buffer.Length Error");
        }

        Monitor.Enter(Instance.IOBufferLocker);

        try
        {
            Array.Clear(buffer, 0, buffer.Length);

            if (Instance.buffers.Contains(buffer))
                return;
            Instance.buffers.Enqueue(buffer);
        }
        finally
        {
            Monitor.Exit(Instance.IOBufferLocker);
        }
    }

    private void AddIOBuffer()
    {
        for (int num = 0; num < SocketInfo.AddIOBufferCount * 2; num++)
            buffers.Enqueue(new byte[SocketInfo.MaxSizeOfPacket]);
    }
}