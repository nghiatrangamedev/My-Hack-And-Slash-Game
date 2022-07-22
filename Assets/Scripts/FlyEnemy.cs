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
    float _damage = 5;

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
            _enemyRb.AddForce(transform.right * _speed);
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
        _playerController.PlayerHeath -= _damage;
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

        else if (collision.gameObject.CompareTag("Sword"))
        {
            Death();
        }

        else if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

        else if (collision.gameObject.CompareTag("Turn Left"))
        {
            if (!_isDeath)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            
        }

        else if (collision.gameObject.CompareTag("Turn Right"))
        {
            if (!_isDeath)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
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
