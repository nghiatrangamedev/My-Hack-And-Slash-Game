using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    Rigidbody2D _enemyRb;
    Animator _enemyAnimator;

    float _heath = 20.0f;
    float _pushBackForce = 5.0f;

    bool _isGetHit;
    bool _isDead;
    bool _isAttack;
    // Start is called before the first frame update
    void Start()
    {
        _enemyRb = GetComponent<Rigidbody2D>();
        _enemyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyAnimation();
        Attack();
    }

    protected void GetDamaged(float damage)
    {
        _heath -= damage;
        Debug.Log("Enemy heath: " + _heath);

        if (_heath <= 0)
        {
            _isDead = true;
            StartCoroutine(WaitDeathAnimationEnd());
        }
    }

    void EnemyAnimation()
    {
        GetHitAnimation();
        DeathAnimation();
        // AttackAnimation();
    }

    void GetHitAnimation()
    {
        _enemyAnimator.SetBool("isGetHit", _isGetHit);
    }

    void DeathAnimation()
    {
        _enemyAnimator.SetBool("isDead", _isDead);
    }

    void AttackAnimation()
    {
        _enemyAnimator.SetBool("isAttack", _isAttack);
    }

    IEnumerator WaitDeathAnimationEnd()
    {
        yield return new WaitForSeconds(1.3f);
        Destroy(gameObject);
    }

    void PushBack(Vector2 swordPosition)
    {
        Vector2 forceDirection = new Vector2((transform.position.x - swordPosition.x) * _pushBackForce, transform.position.y);
        _enemyRb.AddForce(forceDirection, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sword"))
        {
            _isGetHit = true;
            StartCoroutine(WaitForGetHitAnimationEnd());
            GetDamaged(10);
            // PushBack(collision.gameObject.transform.position);
        }

    }

    IEnumerator WaitForGetHitAnimationEnd()
    {
        yield return new WaitForSeconds(0.34f);
        _isGetHit = false;
    }

    void Attack()
    {
        if (!_isAttack)
        {
            _isAttack = true;
            _enemyAnimator.SetBool("isAttack", true);
            StartCoroutine(AttackRate());
        }
    }

    IEnumerator AttackRate()
    {
        yield return new WaitForSeconds(0.8f);
        _enemyAnimator.SetBool("isAttack", false);

        yield return new WaitForSeconds(2);
        _isAttack = false;
    }
}
