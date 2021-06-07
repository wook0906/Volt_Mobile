using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_TouchFeedbackImage : MonoBehaviour
{
    public float duration = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AutoDestroy());   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator AutoDestroy()
    {
        float timer = 0;
        
        while (timer<duration)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.5f, 0.5f, 0.5f), Time.fixedDeltaTime * 2f);
            yield return null;
        }
        Destroy(this.gameObject);
    }
    
}
