using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_PitMonster : MonoBehaviour
{
    public AudioClip soundClip;
    public bool isSearchActive = false;
    public bool isMonsterActive = false;
    public GameObject scopeModel;
    public GameObject monsterModel;
    public ParticleSystem showEffect;
    [SerializeField]
    Volt_Robot targetRobot;
    public Volt_Tile targetTile;
    Animator monsterAnimator;
    Animator scopeAnimator;
    [SerializeField]
    Volt_Tile parentTile;
    AudioSource audio;
    public GameObject monsterPivot;
    // Start is called before the first frame update
    private void Awake()
    {
        audio = gameObject.AddComponent<AudioSource>();
    }
    void Start()
    {
        monsterAnimator = monsterModel.GetComponent<Animator>();
        scopeAnimator = scopeModel.GetComponent<Animator>();
        parentTile = transform.parent.GetComponent<Volt_Tile>();
        
        audio.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MonsterTargetSearchAnimationStart()
    {
        StartCoroutine(WaitSearchAnimation());
    }
    IEnumerator WaitSearchAnimation()
    {
        isSearchActive = true;
        //Debug.Log(parentTile.name + " Do Search");
        scopeAnimator.CrossFade("Search",0.01f);
        yield return new WaitForSeconds(1.5f);
        //Debug.Log(parentTile.name + " Search End");
        isSearchActive = false;
    }
    public void DoAttack(Vector3 targetDir)
    {
        if (targetDir == Vector3.zero) return;
        isMonsterActive = true;
        //Debug.Log(parentTile.tileIndex + " bug attack start");
        StartCoroutine(RotateAndAttack(targetDir));
    }
    IEnumerator RotateAndAttack(Vector3 targetDir)
    {
        Debug.Log(parentTile.name + " RotateAndAttack");

        targetTile = Volt_ArenaSetter.S.GetTile(transform.position,targetDir,1);
        targetTile.SetBlinkOption(BlinkType.Attack, 0.5f);
        targetTile.BlinkOn = true;

        targetRobot = targetTile.GetRobotInTile();

        float angle = Volt_Rotation.GetAngle(monsterPivot.transform.forward, targetDir);
        Debug.Log("monster angle : " + angle);
        Vector3 from = monsterPivot.transform.localRotation.eulerAngles;
        Vector3 to = monsterPivot.transform.localRotation.eulerAngles + Vector3.up * angle;
        Debug.Log(from + " , " + to);
        float rotationTime = Mathf.Abs(angle / 360f);
        float elapsedTime = 0f;
        float u = 0f;
        Debug.DrawRay(parentTile.transform.position, targetTile.transform.position, Color.red, 3f);
        while (u < 1f)
        {
            Vector3 eulerAnlges = Vector3.Lerp(from, to, u);
            Debug.Log("CurAngle : "+ eulerAnlges);
            monsterPivot.transform.localRotation = Quaternion.Euler(eulerAnlges);
            u = elapsedTime / rotationTime;
            elapsedTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        monsterPivot.transform.localRotation = Quaternion.Euler(to);
        //to.x = 0;
        //to.z = 0;
        //monsterPivot.transform.LookAt(to);

        monsterAnimator.Play("Attack");
        audio.PlayOneShot(soundClip);
        showEffect.Play();
        yield return new WaitForSeconds(1.8f);
        PullRobot();
        yield return new WaitUntil(() => targetRobot == null);

        OnAttackFinished();
    }
    public void OnAttackFinished()
    {
        isMonsterActive = false;
        Debug.Log(parentTile.tileIndex + " bug attack End");
        if (targetTile != null)
        {
            targetTile.SetDefaultBlinkOption();
            targetTile.BlinkOn = false;
            targetTile = null;
        }
    }
    public void PullRobot()
    {
        Vector3 dir = (parentTile.transform.position - Volt_ArenaSetter.S.GetTile(targetRobot.transform.position).transform.position).normalized;
        StateMachine otherFSM = targetRobot.transform.GetComponent<StateMachine>();
        otherFSM.knockbackInfor = new KnockbackInfor(this.gameObject,
            dir, parentTile);
        otherFSM.destPos = parentTile.transform.position;
        otherFSM.isKnockback = true;

       // Debug.Log(parentTile.name + " 이 부모 오브젝트라구!");
        Volt_Tile targetTile = Volt_ArenaSetter.S.GetTile(targetRobot.transform.position);
        
        //Debug.Log(targetRobot.playerInfo.playerNumber + " 땡긴다");
    }
    public void ForcedCancel()
    {
        StopAllCoroutines();
        isMonsterActive = false;
        isSearchActive = false;
    }
}
