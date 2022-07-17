using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : MonoBehaviour
{

    PlayerController _playerController;
    Rigidbody2D _enemyRb;
    Animator _flyEnemyAnimator;
    BoxCollider2D _flyEnemyCollider;

    float _speed = 10.0f;

    bool _isDeath;
    // Start is called before the first frame update
    void Start()
    {
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        _enemyRb = GetComponent<Rigidbody2D>();
        _flyEnemyAnimator = GetComponent<Animator>();
        _flyEnemyCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        EnemyMovement();
    }

    private void LateUpdate()
    {
        FlyEnemyAnimation();
    }



    void FlyEnemyAnimation()
    {
        DeathAnimation();
    }

    void DeathAnimation()
    {
        _flyEnemyAnimator.SetBool("isDeath", _isDeath);
    }

    void EnemyMovement()
    {
        if (!_isDeath)
        {
            _enemyRb.AddForce(Vector2.left * _speed);
        }

    }

    void Death()
    {
        _isDeath = true;

        _enemyRb.constraints = RigidbodyConstraints2D.None;
        _enemyRb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void GetDamageToPlayer()
    {
        _playerController._isGetHurt = true;
        _playerController.PlayerHeath -= 10;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            if (!_isDeath)
            {
                GetDamageToPlayer();
            }
        }

        if (collision.gameObject.CompareTag("Sword"))
        {
            Death();
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerController._isGetHurt = false;
        }
    }
}
