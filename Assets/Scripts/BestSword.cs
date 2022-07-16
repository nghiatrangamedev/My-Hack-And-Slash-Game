using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestSword : MonoBehaviour
{
    Transform _playerPos;
    // Start is called before the first frame update
    void Start()
    {
        _playerPos = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = _playerPos.position + _playerPos.right;
        StartCoroutine(AutoDestroy());
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }

}
