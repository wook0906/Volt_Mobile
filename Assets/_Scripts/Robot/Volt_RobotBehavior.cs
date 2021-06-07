using UnityEngine;

[System.Serializable]
public class Volt_RobotBehavior
{
    [SerializeField]
    private Vector3 direction;
    public  Vector3 Direction { get { return direction; } set { direction = value; /*Debug.Log($"Robot direction set:{direction}");*/ } }
    [SerializeField]
    private int behaviourPoints;
    public int BehaviorPoints { get { return behaviourPoints; } set { behaviourPoints = value; } }
    [SerializeField]
    private BehaviourType behaviourType;
    public  BehaviourType BehaviourType { get { return behaviourType; } set { behaviourType = value; } }
    [SerializeField]
    private ActiveTime activeTime;
    public  ActiveTime ActiveTime { get { return activeTime; } set { activeTime = value; } }

    [SerializeField]
    private int playerNumber;
    public int PlayerNumber { get { return playerNumber; } }

    public int OrderNumber { get; set; }

    private Volt_Tile selectTile;
    public Volt_Tile SelectTile
    {
        get { return selectTile; }
        set { selectTile = value;}
    }


    public Volt_RobotBehavior()
    {
        this.behaviourPoints = 0;
        this.direction = Vector3.zero;
        this.behaviourType = BehaviourType.None;
        this.activeTime = ActiveTime.None;
        this.playerNumber = 0;
    }
    public Volt_RobotBehavior(Volt_RobotBehavior behaviour)
    {
        this.behaviourPoints = behaviour.BehaviorPoints;
        this.direction = behaviour.Direction;
        this.behaviourType = behaviour.BehaviourType;
        this.activeTime = behaviour.ActiveTime;
        this.playerNumber = behaviour.PlayerNumber;
    }

    public Volt_RobotBehavior(int behaviorPoints, Vector3 direction, BehaviourType behaviourType,int playerNumber)
    {
        this.behaviourPoints = behaviorPoints;
        this.direction = direction;
        this.behaviourType = behaviourType;
        this.activeTime = ActiveTime.Doing;
        this.playerNumber = playerNumber;
    }
    public Volt_RobotBehavior( int behaviorPoints, Vector3 direction, BehaviourType behaviourType, int playerNumber, Volt_Tile selectTile)
    {
        this.behaviourPoints = behaviorPoints;
        this.direction = direction;
        this.behaviourType = behaviourType;
        this.activeTime = ActiveTime.Doing;
        this.playerNumber = playerNumber;
        this.selectTile = selectTile;
    }

    public void SetBehaviour( ActiveTime activeTime,int playerNumber)
    {
        this.behaviourPoints = 0;
        this.behaviourType = BehaviourType.None;
        this.direction = Vector3.zero;
        this.activeTime = activeTime;
        this.playerNumber = playerNumber;
    }

    public void SetBehaivor( int behaviorPoints, BehaviourType behaviourType, int playerNumber)
    {

        this.behaviourPoints = behaviorPoints;
        this.behaviourType = behaviourType;
        this.direction = Vector3.zero;
        this.activeTime = ActiveTime.Doing;
        this.playerNumber = playerNumber;
    }

    public void SetBehaivor(int behaviorPoints, Vector3 direction, BehaviourType behaviourType, int playerNumber)
    {
        this.behaviourPoints = behaviorPoints;
        this.direction = direction;
        this.behaviourType = behaviourType;
        this.activeTime = ActiveTime.Doing;
        this.playerNumber = playerNumber;
    }
    public void SetBehaivor(int behaviourPoints, Vector3 direction, BehaviourType behaviourType, int playerNumber, Volt_Tile SelectTile)
    {
        this.behaviourPoints = behaviourPoints;
        this.direction = direction;
        this.behaviourType = behaviourType;
        this.activeTime = ActiveTime.Doing;
        this.playerNumber = playerNumber;
        this.SelectTile = SelectTile;
    }

    public void UsedBehaviourPoints()
    {
        this.behaviourPoints = 0;
    }

    public void UsedBehaviourPoints(int points)
    {
        this.behaviourPoints -= points;
    }

    public void Init()
    {
        this.behaviourType = BehaviourType.None;
        this.behaviourPoints = 0;
        this.direction = Vector3.zero;
        this.playerNumber = 0;
    }
}
