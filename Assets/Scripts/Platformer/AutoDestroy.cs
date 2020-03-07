using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{

    float startTime = 0;
    public float lifetimeInSec = 4f;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Time.time - startTime >= lifetimeInSec) {
            Debug.Log("Destroyed Self");
            Destroy(gameObject);
        }
    }
}
