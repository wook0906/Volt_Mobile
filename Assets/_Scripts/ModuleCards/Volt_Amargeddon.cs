using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_Amargeddon : MonoBehaviour
{
    [Header("Set Inspector")]
    public float speed;
    public float height;
    public GameObject explosionEffectPrefab;

    [Header("Set In Script")]
    [SerializeField]
    private Volt_Tile targetTile;
    private bool isTriggered = false;
    AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void Init(Volt_Tile targetTile)
    {
        //Debug.Log("난 "+targetTile.name + "에 떨어질거다 씨벌");
        this.targetTile = targetTile;
        float distance = height / Mathf.Tan(70f * Mathf.Deg2Rad);
        Vector3 pos = targetTile.transform.position;
        pos.y = targetTile.GetComponent<Collider>().bounds.extents.y + GetComponent<Collider>().bounds.extents.y + height;
        pos.x += distance;
        transform.position = pos;

        Vector3 targetPos = targetTile.transform.position;
        targetPos.y = targetTile.GetComponent<Collider>().bounds.extents.y;
        transform.LookAt(targetTile.transform);

        StartCoroutine(MoveTo(targetPos));

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tile") || other.CompareTag("Robot") ||
            other.CompareTag("Killbot") && !isTriggered)
        {
            isTriggered = true;
            GameObject explosionEffectGO = Instantiate(explosionEffectPrefab);
            Vector3 pos = targetTile.transform.position;
            pos.y += 2.2f;
            explosionEffectGO.transform.position = pos;

            List<Volt_Tile> targetTiles = new List<Volt_Tile>(targetTile.GetAdjecentTiles());

            Volt_Robot targetTileRobot = targetTile.GetRobotInTile();
            targetTiles.Add(targetTile);

            Volt_PlayerManager.S.I.playerCamRoot.SetShakeType(CameraShakeType.Amargeddon);
            Volt_PlayerManager.S.I.playerCamRoot.CameraShake();
            
            foreach (var item in targetTiles)
            {
                if (item == null) continue;

                Volt_Robot robot = item.GetRobotInTile();
                if (robot != null)
                {
                    robot.GetDamage(new AttackInfo(Volt_GameManager.S.AmargeddonPlayer,
                        1, CameraShakeType.Amargeddon,
                        Volt_PlayerManager.S.GetPlayerByPlayerNumber(Volt_GameManager.S.AmargeddonPlayer).GetHitEffect(),Card.AMARGEDDON));
                    //robot.PlayDamagedEffect(robot.transform);
                }
            }

            Volt_GameManager.S.CallFunctionDelayed("OnAmargeddonExploded",
                explosionEffectGO.GetComponent<ParticleSystem>().main.duration);
            Destroy(gameObject);
        }
    }

    private IEnumerator MoveTo(Vector3 targetPos)
    {
        Vector3 start = transform.position;
        float elapsedTime = 0;
        float time = (targetPos - transform.position).magnitude / speed;
        float u = 0f;

        while (true)
        {
            if (u == 0f)
            {

            }
            else if (u > 0f && u < 1f)
            {
                Vector3 pos = Vector3.Lerp(start, targetPos, u);
                transform.position = pos;
            }
            else
            {
                break;
            }

            elapsedTime += Time.fixedDeltaTime;
            u = elapsedTime / time;
            yield return new WaitForFixedUpdate();
        }
    }
}
