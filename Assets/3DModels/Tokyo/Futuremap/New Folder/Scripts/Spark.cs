using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark : MonoBehaviour {
    
    public GameObject particlePrefab;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            GameObject particle = Instantiate(particlePrefab);
            particle.transform.position = collision.collider.transform.position;
            particle.transform.rotation = Quaternion.Euler(transform.up);
        }
    }

}
