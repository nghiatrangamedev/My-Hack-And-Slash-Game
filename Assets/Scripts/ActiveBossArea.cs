using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBossArea : MonoBehaviour
{
    [SerializeField] GameObject _bossArea;
    [SerializeField] GameObject _winUI;

    bool _isBossAreaActive;

    private void Update()
    {
        if (_isBossAreaActive && GameObject.Find("Boss") == null)
        {
            _bossArea.SetActive(false);
            _winUI.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _isBossAreaActive = true;
            _bossArea.SetActive(true);
        }
    }
}
