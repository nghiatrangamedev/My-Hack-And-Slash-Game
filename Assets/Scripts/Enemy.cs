using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float _heath = 100.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sword"))
        {
            GetDamaged();
            Debug.Log("Heath: " + _heath);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
    }

    void GetDamaged()
    {
        _heath -= 10;
        if (_heath <= 0)
        {
            Destroy(gameObject);
        }
    }
}
