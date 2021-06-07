using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private Volt_Robot _owner;
    public Volt_Robot Owner { get { return _owner; } }
    private Animator _animator;
    public Animator Animator { get { return _animator; } }
    private AnimationEventHandler _aniEventHandler;
    public AnimationEventHandler AniEventHandler { get { return _aniEventHandler; } }

    public StateBase currentState;
    public StateBase prevState;
    public StateBase remainState;

    [HideInInspector] public Volt_RobotBehavior behavior = null;

    public Vector3 startPos = Vector3.positiveInfinity;
    public Vector3 destPos = Vector3.positiveInfinity;
    private Vector3 moveDir = Vector3.positiveInfinity;
    public Vector3 MoveDir
    {
        get
        {
            if (Mathf.Abs(moveDir.magnitude - 1f) > float.Epsilon * 10f)
            {
                moveDir.Normalize();
            }
            return moveDir;
        }
        set
        {
            moveDir = value;
            if (Mathf.Abs(moveDir.magnitude - 1f) > float.Epsilon * 10f)
                moveDir.Normalize();
        }
    }
    public float elapsedTime = 0f;
    public float u = 0f;
    public float moveTime = 0f;
    public float rotationTime = 0f;
    public Volt_Tile startTile = null; // 킬봇을 위한 변수
    public Queue<Volt_Tile> qTiles = new Queue<Volt_Tile>();

    public float anlge = 0f;
    public Quaternion fromRot = Quaternion.identity;
    public Quaternion toRot = Quaternion.identity;

    [HideInInspector] public GameObject targetRobot = null;
    [HideInInspector] public KnockbackInfor knockbackInfor = null;
    [HideInInspector] public AttackInfo attackInfo = null;
    [HideInInspector] public CollisionData collisionData = null;

    public LayerMask robotMask;
    public LayerMask trapMask;
    public LayerMask wallMask;
    public LayerMask holeMask;
    public LayerMask itemMask;
    public LayerMask timebombMask;
    public LayerMask tileMaks;

    public float moveSpeed = 7;
    public float knockbackSpeed = 8;
    public float fallSpeed = 7;
    public float angular = 20;

    public bool isDetectSomething = false; // 킬봇 전용 변
    [HideInInspector] public bool isDamaged = false;
    [HideInInspector] public bool isArriveDest = false;
    [HideInInspector] public bool isDodgeAttack = false;
    public bool isKnockback = false;
    [HideInInspector] public bool isInActivity = false;
    [HideInInspector] public bool isSpawn = false;
    [HideInInspector] public bool isMoving = false;

    [HideInInspector] public bool isDoneKnockback = false;
    [HideInInspector] public bool isDoneMove = false;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool isDoneFall = false;
    [HideInInspector] public bool isDoneWalkTo = false;
    [HideInInspector] public bool isDonePushDecision = false;

    public List<GameObject> collidedGOs = new List<GameObject>();
    public GameObject collidedRobot = null;
    public GameObject collidedTrap = null;
    public GameObject collidedWall = null;
    public List<GameObject> lastAttackRobots = new List<GameObject>();

    // 노데미지에 대한 애니메이션을 재상하기 위해
    // 이전 데미지가 있는 공격 때 실행시킨 애니메이션을 재생한다.
    public string prevDamagedAnimation = string.Empty;

    public AttackType attackType = AttackType.Attack;
    public Volt_ModuleCardBase attackModule = null;
    // 공격 애니메이션을 재생했는가?
    public bool isPlayAttackAnimation = false;

    public MoveController moveController;
    public BaseAttack attack;

    private void Awake()
    {
        robotMask = LayerMask.GetMask("Robot");
        trapMask = LayerMask.GetMask("Obstacle");
        wallMask = LayerMask.GetMask("Wall");
        holeMask = LayerMask.GetMask("Pit");
        itemMask = LayerMask.GetMask("Items");
        timebombMask = LayerMask.GetMask("TimeBomb");
        tileMaks = LayerMask.GetMask("Tile");

        _owner = GetComponent<Volt_Robot>();
        _animator = GetComponent<Animator>();
        _aniEventHandler = gameObject.GetOrAddComponent<AnimationEventHandler>();

        Managers.Resource.LoadAsync<StateBase>("Assets/_ScriptableObjects/States/SpawnState.asset",
            (result)=>
            {
                currentState = result.Result;
                currentState.OnEnterState(this);
            });
    }

    private void Start()
    {
        remainState = currentState;
    }

    public void ChangeState(StateBase newState)
    {
        //Debug.Log($"[{Owner.playerInfo.playerNumber}] Change state from {currentState.name} to {newState.name}");
        if (newState == null)
        {
            if (remainState != currentState)
            {
                ChangeState(remainState);
            }
            return;
        }



        if (currentState != null)
        {
            currentState.OnExitState(this);
            prevState = currentState;
        }

        StopAllCoroutines();
        currentState = newState;
        remainState = currentState;
        currentState.OnEnterState(this);
    }

    public void Update()
    {
        //Debug.Log($"[{Owner.playerInfo.playerNumber}player] fsm update!!");
        if (currentState == null)
        {
            //Debug.Log($"[{Owner.playerInfo.playerNumber}player] Warning!! currentState is null");
            return;
        }

        currentState.Action(this, Time.deltaTime);
        currentState.CheckTransition(this);
    }

    private void OnDrawGizmos()
    {
        if (currentState == null)
            return;

        Gizmos.color = currentState.gizmoColor;
        Gizmos.DrawSphere(transform.position + Vector3.up * 2.3f, 0.3f);
    }

    private void OnDisable()
    {
        //Debug.Log($"[{Owner.playerInfo.playerNumber} fsm disable");
    }

    private void OnDestroy()
    {
        //Debug.Log($"[{Owner.playerInfo.playerNumber} fsm destroy");
    }
}

