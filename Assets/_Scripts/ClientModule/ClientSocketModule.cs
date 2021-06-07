using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public enum EShopPurchase
{
    EShopPurchase,
    Package,
    Skin,
    Emoticon,
    Gold,
    Diamond,
    Battery
}
public enum EDailyConditionType
{
    SignIn,
    GamePlay,
    VictoryCount,
    VictoryCoin,
    KillCount,
    End
}

public enum ENormalConditionType
{
    GamePlay,
    KillCount,
    VictoryCoin,
    Dead,
    AttackTry,
    AttackSuccess,
    VictoryCount,
    BuySkin,
    AllEnemyKillOneGame,
    PeaceWin,
    NoDeathWin,
    AllEnemyKillWin,
    TaticModule,
    AttackModule,
    MoveModule,
    AllModule,
    AllEnemyAttack,
    AllEnemyKill,
    RapidAttack,
    Bombing,
    Saw,
    Hologram,
    Dodge,
    Teleport,
    Shield,
    Bomb,
    Hacking,
    Timebomb,
    Anchor,
    Emp,
    End
}
public enum EBenefitType
{
    Pack1Battery
}

public enum EAssetsType { EAssetsType, Gold, Diamond, Money, Battery }

public enum EPacketType
{
    DisConnectedPacket,
    ApplicationQuitPacket,

    InternetConnectionCheckPacket,
    ReConnectionPacket,
    MoveLobbyPacket,
    ReconnectBattlingRoomPacket,

    SignInPacket,
    SignInSuccessPacket,
    SignInFailPacket,
    SignUpPacket,
    SignUpSuccessPacket,
    SignUpFailNicknameSamePacket,

    OverLapConnection,

    DailyRewardPacket,
    BatteryShopInfoPacket,
    DiamondShopInfoPacket,
    SkinShopInfoPacket,
    EdgeShopInfoPacket,

    DailyAchievementInfoPacket,
    NormalAchievementInfoPacket,

    UserSkinOwnPacket,

    UserDailyAchConditionStatePacket,
    UserNormalAchConditionStatePacket,
    UserDailyAchSuccessPacket,
    UserNormalAchSuccessPacket,

    BatteryTimeRequestPacket,
    BatteryTimeResponsePacket,
    BatteryTimeRegisterPacket,

    TimeTickPacket,

    WaitingPlayerCountPacket,
    MatchingRequestPacket,
    MatchingCompletionPacket,
    CancelSearchingEnemyPlayerPacket,

    PlayerExitPacket,

    FieldReadyCompletionPacket,
    InteractionItemPositionPacket,
    CharacterPositionPacket,
    CharacterPositionSettingCompletionPacket,

    BehaviorOrderPacket,
    BehaviorOrderCompletionPacket,

    VictoryPointPacket,

    SimulationCompletionPacket,

    ModuleActivePacket,
    ModuleUnActivePacket,

    TotalTurnOverPacket,
    SynchronizationTileDatasPacket,
    SynchronizationElsePacket,

    GameOverPacket,

    MobileActivePacket,
    MobileUnActivePacket,

    AchievementCompletionPacket,
    AchievementProgressPacket,
    ItemPurchasePacket,

    GoldPacket,
    DiamondPacket,
    BatteryPacket,

    AttackPacket,
    AttackSuccessPacket,
    KillPacket,

    ShopPurchasePacket,
    BatteryPurchasePacket,
    IAPPacket,


    UserDataForMovingLobbyPacket,

    DeleteUserPacket,
    UseEmoticonPacket,
    EmoticonOwnPacket,
    AdsCheckPacket,
    AdsWatchPacket,
    PowerOwnPacket,
    PackageShopInfoPacket,
    GoldShopInfoPacket,
    EmoticonShopInfoPacket,
    ResultPurchasePacket,
    RequestBenefitPacket,
    UserBenefitPacket,
    ResultBenefitPacket,
        UserGetBenefitPacket

}


public static class SocketInfo
{
    public const string Dns = "localhost";

    public const int Port = 10002;
    public const int BackLog = 100;

    // public const int MaxSizeOfPacket = 1024;
    public const int MaxSizeOfPacket = 8000;
    public const int MaxIOBufferCount = 15;
    public const int AddIOBufferCount = 3;
}

public static class PacketInfo
{
    public const int PacketSizePosition = 5;
    public const int PacketOrderPosition = 9;
    public const int FromServerPacketSettingIndex = 9;
    public const int FromServerPacketDataStartIndex = 14;
    public const byte PacketStartNumber = 77;

    public const byte ByteNumber = 11;
    public const byte IntNumber = 12;
    public const byte FloateNumber = 13;
    public const byte StringNumber = 14;
    public const byte BoolNumber = 15;
}

public class ClientSocketModule : MonoSingleton<ClientSocketModule>
{
    private Socket clientSocket;
    private IPEndPoint ipEndPoint;
    private int socketConnectionCheckCount;
    private int socketConnectionCheck;
    private bool isSocketConnectCompleted;
    private bool isSocketReSeted;
    private StringBuilder sb = new StringBuilder();
    private byte[] contentBuffer = new byte[SocketInfo.MaxSizeOfPacket]; 
    private Packet[] packetToClass;
    private byte[] buffer = new byte[16000];

    private readonly object SocketLock = new object();

    private void Awake()
    {
        socketConnectionCheckCount = 0;
        socketConnectionCheck = 30;
        isSocketConnectCompleted = false;
        isSocketReSeted = false;

        CommunicationWaitQueue communicationWaitQueue = CommunicationWaitQueue.Instance;
        IOBuffer ioBuffer = IOBuffer.Instance;
        CreatePacketClasses();
        InitializeIpEndPoint();

        try
        {
            Connect();

            //Debug.Log("Connect");

            Receive();

            //Debug.Log("Receive");

            isSocketReSeted = false;

            Volt_PlayerData.instance.NeedReConnection = false;
        }
        catch
        {
            // 인터넷 연결이 불가능합니다.
            // or
            // 서버 점검 중 입니다.
        }
    }
    
    private void Update()
    {
        if (!isSocketConnectCompleted)
            return;

        socketConnectionCheckCount++;

        if (clientSocket.Connected && socketConnectionCheckCount == socketConnectionCheck)
        {
            //Debug.Log("PacketTransmission.SendInternetConnectionCheckPacket : " + socketConnectionCheckCount);
            //Debug.Log("InternetConnectionCheckPacket Time : " + DateTime.Now);
            PacketTransmission.SendInternetConnectionCheckPacket();
            socketConnectionCheckCount = 0;
        }

        if(!clientSocket.Connected && !isSocketReSeted)
        {
            // 인터넷 연결이 불안정합니다. 패널 표시
            //Debug.LogError("인터넷 연결이 불안정한 상태");
            InitializeIpEndPoint();
        }

        if (!clientSocket.Connected && socketConnectionCheckCount == socketConnectionCheck)
        {
            //Debug.Log("Try Reconnection");
            Volt_DontDestroyPanel.S.NetworkErrorHandle(NetworkErrorType.InternetInstable);

            try
            {
                Connect();

                PacketTransmission.SendInternetConnectionCheckPacket();

                if (clientSocket.Connected)
                {
                    Volt_DontDestroyPanel.S.NetworkErrorHandle(NetworkErrorType.Resolved);
                    //Debug.Log("Reconnection Success");

                    Receive();

                    Volt_PlayerData.instance.NeedReConnection = true;

                    PacketTransmission.SendReconnectionPacket(Volt_PlayerData.instance.UserToken);

                    isSocketReSeted = false;
                }
            }
            catch (Exception e)
            {
                Debug.Log("Connection Error");
                Debug.Log("e.Message : " + e.Message);            }
            finally
            {
                socketConnectionCheckCount = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        byte[] buffer = CommunicationWaitQueue.Instance.DequeuePacket();

        if (buffer == null)
            return;

        Classify(buffer);
    }

    private void CreatePacketClasses()
    {
        string[] classNames = typeof(EPacketType).GetEnumNames();
        packetToClass = new Packet[classNames.Length];

        for (int num = 0; num < classNames.Length; num++)
        {
            Type type = Type.GetType(classNames[num]);
            if (type == null)
                continue;
            var obj = Activator.CreateInstance(type);
            packetToClass[num] = (Packet)obj;
        }
    }
    
    private bool InitializeIpEndPoint()
    {
        Monitor.Enter(Instance.SocketLock);

        try
        {
            IPHostEntry ipHostInfo = null;
            IPAddress ipAddress = null;

            //try
            //{
            //    ipHostInfo = Dns.GetHostEntry(SocketInfo.Dns);

            //    for (int index = 0; index < ipHostInfo.AddressList.Length; index++)
            //    {
            //        if (ipHostInfo.AddressList[index].AddressFamily == AddressFamily.InterNetwork)
            //            ipAddress = ipHostInfo.AddressList[index];
            //    }
            //}
            //catch
            //{
            //    return false;
            //}


            //ipAddress = new IPAddress(new byte[] { 127, 0, 0, 1 });
            ipAddress = new IPAddress(new byte[] { 3, 34, 173, 85 });
            //Debug.Log("UNITY_EDITOR IPADDRESS");
            //Debug.Log(ipAddress.ToString());

            //#if UNITY_ANDROID
            //ipAddress = new IPAddress(new byte[] { 192, 168, 0, 4 });
            //#endif

            //Debug.Log("Init : clientSocket = new Socket");

            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.NoDelay = true;

            //Debug.Log("Init : ipEndPoint = new IPEndPoint(ipAddress, SocketInfo.Port)");

            ipEndPoint = new IPEndPoint(ipAddress, SocketInfo.Port);

            //Debug.Log("Init : isSocketReSeted = true");

            isSocketReSeted = true;

            //Debug.Log("Init : clientSocket.Connected : " + clientSocket.Connected);

            return true;
        }
        catch(Exception e)
        {
            Debug.Log("InitializeIpEndPoint");
            Debug.Log("e.Message : " + e.Message);

            return false;
        }
        finally
        {
            Monitor.Exit(Instance.SocketLock);
        }
    }

    private void Connect()
    {
        try
        {
            clientSocket.Connect(ipEndPoint);
        }
        catch(Exception e)
        {
            Debug.Log("Socket.Connect");
            Debug.Log("e.Message : " + e.Message);
        }

        isSocketConnectCompleted = true;
    }

    private void Receive()
    {
        byte[] packet = IOBuffer.Dequeue();

        Task task = null;

        task = Task.Factory.FromAsync(
            clientSocket.BeginReceive(packet, 0, packet.Length, SocketFlags.None, null, clientSocket),
            clientSocket.EndReceive);

        task.ContinueWith((result) =>
        {
            byte[] content = buffer.AddTo(packet);

            //while 문 돌아갈 때 내부에서 content는 계속 변함. Don't touch
            while (content.IndexOf("<EOF>") > -1)
            {
                int indexEOF = content.IndexOf("<EOF>") + 5;

                EPacketType type = (EPacketType)ByteConverter.ToInt(content, 1);
                string log = "";
                for (int i = 0; i < content.RealLength(); i++)
                    log += content[i] + " ";
                //Debug.Log(log);
                if (type == EPacketType.DisConnectedPacket)
                {
                    Close();
                    return;
                }

                byte[] enqPacket = new byte[indexEOF];
                Array.Copy(content, enqPacket, indexEOF);
                CommunicationWaitQueue.Instance.EnqueuePacket(enqPacket);

                //content 뒤에 남았다면 잘라서 저장
                //EOF까지 수행되었으므로 버퍼클리어
                //변경된 content는 다음번 while문이 돌아갈 때 EOF가 없으면 #1에서 버퍼에 저장될 것.
                content = content.SubArray(indexEOF);
                buffer.Clear();
            }

            //#1 남았다면 버퍼 저장.
            if (content.RealLength() > 0)
            {
                Array.Copy(content, 0, buffer, 0, content.RealLength());
            }
            IOBuffer.Enqueue(packet);

            try
            {
                Receive();
            }
            catch (Exception e)
            {
                return;
            }
        });

    }


    /// <summary>
    /// 구
    /// </summary>
    /// <param name="start"></param>
    //private void Receive()
    //{
    //    byte[] buffer = IOBuffer.Dequeue();

    //    Task<int> task = null;

    //    task = Task<int>.Factory.FromAsync(
    //        clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, null, clientSocket),
    //        clientSocket.EndReceive);


    //    task.ContinueWith((result) =>
    //    {
    //        try
    //        {
    //            int bytesRead = result.Result;

    //            Debug.LogError("bytesRead : " + bytesRead);

    //            int appendIndexEOF = 0;

    //            string content = sb.ToString() + Encoding.UTF8.GetString(buffer, 0, bytesRead);

    //            Array.Copy(buffer, 0, contentBuffer, sb.Length, bytesRead);

    //            while (content.IndexOf("<EOF>") > -1)
    //            {
    //                EPacketType type = (EPacketType)ByteConverter.ToInt(contentBuffer, 1);

    //                int packetSize = ByteConverter.ToInt(contentBuffer, PacketInfo.PacketSizePosition);



    //                int indexEOF = content.IndexOf("<EOF>") + 5;

    //                //EdgeShopInfoPacket 에서 indexEOF 가 -1 낮게 변수에 설정된다.
    //                // 왜 인지 몰라서 packetSize로 일딴 대체
    //                appendIndexEOF += packetSize;


    //                Debug.LogError("Receive : " + type + " : " + DateTime.Now + " ,  packet : " + packetSize + " , indexEOF : " + indexEOF);

    //                if (type == EPacketType.DisConnectedPacket)
    //                {
    //                    Close();
    //                    return;
    //                }

    //                byte[] bytes = new byte[packetSize];
    //                Array.Copy(contentBuffer, 0, bytes, 0, packetSize);

    //                CommunicationWaitQueue.Instance.EnqueuePacket(bytes);

    //                Array.Copy(buffer, appendIndexEOF, contentBuffer, 0, content.Length - packetSize);
    //                ClearContentBuffer(content.Length - packetSize);

    //                content = content.Substring(packetSize);

    //                sb.Clear();
    //            }

    //            if (content.Length > 0)
    //                sb.Append(sb);
    //        }
    //        catch(Exception e)
    //        {
    //            //Debug.Log("Message : " + e.Message);
    //        }
    //        finally
    //        {
    //            IOBuffer.Enqueue(buffer);
    //        }

    //        try
    //        {
    //            Receive();
    //        }
    //        catch (Exception e)
    //        {
    //            //Debug.Log("In Receive Message : " + e.Message);
    //        }
    //    });
    //}

    private void ClearContentBuffer(int start)
    {
        for (int index = start; index < SocketInfo.MaxSizeOfPacket; index++)
            contentBuffer[index] = 0;
    }

    public void Classify(byte[] buffer)
    {
        int packetType = ByteConverter.ToInt(buffer, 1);

        //Debug.Log("PacketType : " + (EPacketType)packetType);

        packetToClass[packetType].UnPack(buffer);


        //if (buffer[packetSize] == PacketInfo.PacketStartNumber)
        //    ProcessChainedPacket(buffer);
    }
    
    public void ProcessChainedPacket(byte[] buffer)
    {
        int packetSize = ByteConverter.ToInt(buffer, PacketInfo.PacketSizePosition);

        //Debug.Log("<------- Packet is Chained with next Packet : " + (EPacketType)ByteConverter.ToInt(buffer, packetSize + 1) + "  ------->");

        Array.Copy(buffer, packetSize, buffer, 0, buffer.Length - packetSize);
        Array.Clear(buffer, buffer.Length - packetSize, packetSize);

        CommunicationWaitQueue.Instance.EnqueuePacket(buffer);
        // Classify(buffer);        
    }

    public static void Send(byte[] buffer, int size)
    {
        buffer[size++] = PacketInfo.StringNumber;

        ByteConverter.FromString("<EOF>", buffer, ref size);

        ByteConverter.FromInt(size, buffer, PacketInfo.PacketSizePosition);


        EPacketType packetType = (EPacketType)ByteConverter.ToInt(buffer, 1);

        //Debug.Log("Send : " + packetType + " : " + DateTime.Now);

        if (Volt_GameManager.S)
        {
            if (Volt_GameManager.S.IsTrainingMode ||
                Volt_GameManager.S.IsTutorialMode ||
                Volt_GameManager.S.isGameOverWaiting)
            {
                if(packetType != EPacketType.InternetConnectionCheckPacket)
                    return;
            }
        }

        if(Instance.clientSocket == null)
        {
            Debug.Log("Instance.clientSocket == null");
        }

        try
        {
            Instance.clientSocket.Send(buffer, size, SocketFlags.None);
        }
        catch(SocketException e)
        {
            Debug.Log("SocketException.Message : " + e.Message);
            Debug.Log("SocketException.StackTrace : " + e.StackTrace);

            Close();
        }
    }

    public static void Close()
    {
        Monitor.Enter(Instance.SocketLock);

        try
        {
            Debug.Log("Before clientSocket.Shutdown(SocketShutdown.Both)");

            Instance.clientSocket.Shutdown(SocketShutdown.Both);

            Debug.Log("After clientSocket.Shutdown(SocketShutdown.Both)");
        }
        finally
        {
            Instance.clientSocket.Close();

            Debug.Log("Socket Close");

            Instance.isSocketReSeted = false;

            Monitor.Exit(Instance.SocketLock);
        }
        Volt_DontDestroyPanel.S.OnDisconnected();
    }
}


public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance = null;
    private static object _syncobj = new object();
    private static bool appIsClosing = false;

    public static T Instance
    {
        get
        {
            if (appIsClosing)
                return null;

            lock (_syncobj)
            {
                if (_instance == null)
                {
                    T[] objs = FindObjectsOfType<T>();

                    if (objs.Length > 0)
                        _instance = objs[0];

                    if (objs.Length > 1)
                        UnityEngine.Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");

                    if (_instance == null)
                    {
                        string goName = typeof(T).ToString();
                        GameObject go = GameObject.Find(goName);
                        if (go == null)
                            go = new GameObject(goName);
                        _instance = go.AddComponent<T>();
                    }
                }
                DontDestroyOnLoad(_instance);///////////////////////////////////////////////////////////////////////////////
                return _instance;
            }
        }
    }

    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed,
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    protected virtual void OnApplicationQuit()
    {
        // release reference on exit
        appIsClosing = true;
    }
}


public class Singleton<T> where T : class
{
    private static object _syncobj = new object();
    private static volatile T _instance = null;
    
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                CreateInstance();
            }
            return _instance;
        }
    }

    private static void CreateInstance()
    {
        lock (_syncobj)
        {
            if (_instance == null)
            {
                Type t = typeof(T);

                // Ensure there are no public constructors...  
                ConstructorInfo[] ctors = t.GetConstructors();

                if (ctors.Length > 0)
                {
                    throw new InvalidOperationException(string.Format("{0} has at least one accesible ctor making it impossible to enforce singleton behaviour", t.Name));
                }

                // Create an instance via the private constructor  
                _instance = (T)Activator.CreateInstance(t, true);
                
            }
        }
    }
}