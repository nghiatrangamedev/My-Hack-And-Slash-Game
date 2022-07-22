using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DestroyAfterSpawn();
    }

    void DestroyAfterSpawn()
    {
        StartCoroutine(WaitForBeDestroy());
    }

    IEnumerator WaitForBeDestroy()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

}
