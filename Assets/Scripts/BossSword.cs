using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSword : MonoBehaviour
{
    float _waitForDestroy = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("AutoDestroy", _waitForDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AutoDestroy()
    {
        Destroy(gameObject);
    }

}
