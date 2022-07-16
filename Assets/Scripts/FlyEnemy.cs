using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : MonoBehaviour
{
    Rigidbody2D _enemyRb;
    Animator _flyEnemyAnimator;

    float _speed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        _enemyRb = GetComponent<Rigidbody2D>();
        _flyEnemyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        EnemyMovement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sword"))
        {
            Death();
        }
    }

    void EnemyMovement()
    {
        _enemyRb.AddForce(Vector2.left * _speed);
    }

    void Death()
    {
        _enemyRb.constraints = RigidbodyConstraints2D.None;
        _enemyRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _flyEnemyAnimator.SetBool("isDeath", true);
        StartCoroutine(WaitDeathAnimationEnd());
    }

    IEnumerator WaitDeathAnimationEnd()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
