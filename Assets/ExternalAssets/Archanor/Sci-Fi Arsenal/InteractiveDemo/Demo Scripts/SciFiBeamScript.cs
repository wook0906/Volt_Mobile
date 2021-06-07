using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SciFiArsenal
{

    public class SciFiBeamScript : MonoBehaviour
    {

        [Header("Prefabs")]
        public GameObject[] beamLineRendererPrefab;
        public GameObject[] beamStartPrefab;
        public GameObject[] beamEndPrefab;

        private int currentBeam = 0;

        public GameObject beamStart;
        public GameObject beamEnd;
        public GameObject beam;
        private LineRenderer line;

        public bool isBeamOn = false;
        Transform target;
        [Header("Adjustable Variables")]
        public float beamEndOffset = 1f; //How far from the raycast hit point the end effect is positioned
        public float textureScrollSpeed = 8f; //How fast the texture scrolls along the beam
        public float textureLengthScale = 3; //Length of the beam texture

        [Header("Put Sliders here (Optional)")]
        public Slider endOffSetSlider; //Use UpdateEndOffset function on slider
        public Slider scrollSpeedSlider; //Use UpdateScrollSpeed function on slider

        [Header("Put UI Text object here to show beam name")]
        public Text textBeamName;

        public System.Action onEnd;
        // Use this for initialization
        void Start()
        {
            if (textBeamName)
                textBeamName.text = beamLineRendererPrefab[currentBeam].name;
            if (endOffSetSlider)
                endOffSetSlider.value = beamEndOffset;
            if (scrollSpeedSlider)
                scrollSpeedSlider.value = textureScrollSpeed;

        }
        
        // Update is called once per frame
        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Escape))
            //    Application.Quit();
            if (isBeamOn)
            {
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //RaycastHit hit;
                //if (Physics.Raycast(ray.origin, ray.direction, out hit))
                //{
                Vector3 targetPos = target.position;
                targetPos.y = beamStart.transform.position.y;
                
                ShootBeamInDir(beamStart.transform.position, targetPos);
                //}
            }

            /*
            if (Input.GetKeyDown(KeyCode.RightArrow)) //4 next if commands are just hotkeys for cycling beams
            {
                nextBeam();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                nextBeam();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                previousBeam();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                previousBeam();
            }*/

        }
        public IEnumerator BeamPlay(Transform target,Vector3 from)
        {
            BeamStart(target,from);
            yield return new WaitForSeconds(1f);
            BeamEnd();
        }

        public void BeamStart(Transform target, Vector3 from)
        {

            //beamStart = Instantiate(beamStartPrefab[currentBeam],from, Quaternion.identity) as GameObject;
            //beamEnd = Instantiate(beamEndPrefab[currentBeam], from, Quaternion.identity) as GameObject;
            //beam = Instantiate(beamLineRendererPrefab[currentBeam], from, Quaternion.identity) as GameObject;
            //Debug.Log("BeamStart");

            beamStart = Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_PowerbeamHead);
            beamEnd = Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_PowerbeamTail);
            beam = Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_PowerbeamBody);
            //Debug.Log(from);
            beamStart.GetComponent<AudioSource>().Play();
            beamStart.transform.position = from;
            beamStart.transform.rotation = Quaternion.identity;
            beamEnd.transform.position = from;
            beamEnd.transform.rotation = Quaternion.identity;
            beam.transform.position = from;
            beam.transform.rotation = Quaternion.identity;
            line = beam.GetComponent<LineRenderer>();
            this.target = target;
            isBeamOn = true;
            Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Soundsource_20200623/Module/module_POWERBEAM.wav",
                (result)=>
                {
                    Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
                });
            
        }
        public void BeamEnd()
        {
            //Debug.Log("BeamEnd");
            isBeamOn = false;
            //Destroy(beamStart);
            //Destroy(beamEnd);
            //Destroy(beam);
            Volt_PrefabFactory.S.PushEffect(beamStart.GetComponent<Poolable>());
            Volt_PrefabFactory.S.PushEffect(beamEnd.GetComponent<Poolable>());
            Volt_PrefabFactory.S.PushEffect(beam.GetComponent<Poolable>());
            beamStart = null;
            beamEnd = null;
            beam = null;
            if(onEnd != null)
            {
                onEnd.Invoke();
            }
            //Destroy(this.gameObject);
        }
        /*
    public void nextBeam() // Next beam
    {
        if (currentBeam < beamLineRendererPrefab.Length - 1)
            currentBeam++;
        else
            currentBeam = 0;

        if (textBeamName)
            textBeamName.text = beamLineRendererPrefab[currentBeam].name;
    }
	
	    public void previousBeam() // Previous beam
    {
        if (currentBeam > - 0)
            currentBeam--;
        else
            currentBeam = beamLineRendererPrefab.Length - 1;

        if (textBeamName)
            textBeamName.text = beamLineRendererPrefab[currentBeam].name;
    }
	
    */
        public void UpdateEndOffset()
        {
            beamEndOffset = endOffSetSlider.value;
        }

        public void UpdateScrollSpeed()
        {
            textureScrollSpeed = scrollSpeedSlider.value;
        }

        void ShootBeamInDir(Vector3 start, Vector3 targetPos)
        {
#if UNITY_5_5_OR_NEWER
            line.positionCount = 2;
#else
		line.SetVertexCount(2); 
#endif
            line.SetPosition(0, start);
            beamStart.transform.position = start;

            Vector3 end = Vector3.zero;
            Vector3 dir = (targetPos - start).normalized;
            end = targetPos;//transform.position + (dir.normalized * 1);

            float distance = Vector3.Distance(start, end);

            RaycastHit hit;
            LayerMask canBeTargetLayer;
            canBeTargetLayer = 1 << LayerMask.NameToLayer("Robot");
            canBeTargetLayer += 1 << LayerMask.NameToLayer("Wall");
            Debug.DrawRay(start, dir * distance, Color.green,1f);
            if (Physics.Raycast(start, dir, out hit, distance, canBeTargetLayer))
            {
                
                end = hit.point - (dir * beamEndOffset);
                //print("파워빔 적중!");

            }

            beamEnd.transform.position = end;
            line.SetPosition(1, end);

            beamStart.transform.LookAt(beamEnd.transform.position);
            beamEnd.transform.LookAt(beamStart.transform.position);

            
            line.sharedMaterial.mainTextureScale = new Vector2(distance / textureLengthScale, 1);
            line.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);
        }
    }
}