using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkSpell : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DestroySpellHole();
    }

    void DestroySpellHole()
    {
        StartCoroutine(WaitToDestroySpellHole());
    }

    IEnumerator WaitToDestroySpellHole()
    {
        yield return new WaitForSeconds(1.6f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(WaitForDestroy());
        }

        else if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
