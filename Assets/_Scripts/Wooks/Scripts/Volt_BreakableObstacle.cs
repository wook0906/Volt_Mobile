using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_BreakableObstacle : MonoBehaviour
{
    public List<GameObject> pieces;
    Rigidbody rb;
    [SerializeField]
    bool isDetroyed = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            GetDamage();
        }
    }
    public void GetDamage()
    {
        if (!isDetroyed)
        {
            rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;
            foreach (var item in pieces)
            {
                item.GetComponent<Rigidbody>().isKinematic = false;
                item.GetComponent<Collider>().isTrigger = false;
            }
            InvokeRepeating("DelayedDestroy", 8f, 1f);
        }
    }
    void DelayedDestroy()
    {
        Destroy(gameObject);
    }
}
