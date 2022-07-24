using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] GameObject _bossSword;

    Rigidbody2D _bossRb;
    Animator _bossAnimator;

    float _walkDelay = 2.0f;
    float _speed = 10.0f;
    float _heath = 100.0f;

    bool _isWalk;
    bool _isAttack;
    bool _isCastSpell;
    bool _isDeath;
    
    // Start is called before the first frame update
    void Start()
    {
        _bossRb = GetComponent<Rigidbody2D>();
        _bossAnimator = GetComponent<Animator>();

        Invoke("WaitToMovement", _walkDelay);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDeath)
        {
            _isWalk = false;

        }

        if (_isWalk)
        {
            Attack();
        }

    }

    private void FixedUpdate()
    {
        if (_isWalk)
        {
            BossMovement();
        }
    }

    private void LateUpdate()
    {
        BossAnimation();
    }

    // Movement
    void WaitToMovement()
    {
        _isWalk = true;
    }

    void BossMovement()
    {
        _bossRb.AddForce(transform.right * _speed);
    }

    void BossMovementAnimation()
    {
        _bossAnimator.SetBool("IsWalk", _isWalk);
    }

    void ChangeMoveDirection(float yRotation)
    {
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    // Attack
    void Attack()
    {
        if (!_isAttack)
        {
            _isAttack = true;
            SetAttackAnimation(true);
            Invoke("SpawnSword", 0.5f);
            StartCoroutine(AttackRate());
        }
    }

    void SpawnSword()
    {
        float yPos = 20;
        Vector2 swordPostion = new Vector2(transform.position.x + transform.right.x * 2, yPos);
        Instantiate(_bossSword, swordPostion, _bossSword.transform.rotation);
    }

    void SetAttackAnimation(bool isAttack)
    {
        _bossAnimator.SetBool("IsAttack", isAttack);
    }

    IEnumerator AttackRate()
    {
        yield return new WaitForSeconds(0.2f);
        SetAttackAnimation(false);

        yield return new WaitForSeconds(2);
        _isAttack = false;
    }

    // Get Hurt
    void GetHurt()
    {
        _heath -= 10;
        GetHurtAnimation();

        if (_heath <= 0)
        {
            _isDeath = true;
            DeathAnimation();
        }
    }

    void GetHurtAnimation()
    {
        _bossAnimator.SetBool("IsHurt", true);
        StartCoroutine(WaitForHurtAnimationEnd());
    }

    IEnumerator WaitForHurtAnimationEnd()
    {
        yield return new WaitForSeconds(0.1f);
        _bossAnimator.SetBool("IsHurt", false);
    }

    // Death
    void DeathAnimation()
    {
        _bossAnimator.SetBool("IsDeath", true);
        StartCoroutine(WaitForDeathAnimationEnd());
    }

    IEnumerator WaitForDeathAnimationEnd()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    // Cast Spell
    void CastSpell()
    {
        _isWalk = false;
        _isCastSpell = true;
        CastSpellAnimation();
    }

    void CastSpellAnimation()
    {
        _bossAnimator.SetBool("IsCastSpell", _isCastSpell);
        StartCoroutine(WaitForCastSpellAnimationEnd());
    }

    IEnumerator WaitForCastSpellAnimationEnd()
    {
        yield return new WaitForSeconds(5);
        _isCastSpell = false;
    }

    // Animation
    void BossAnimation()
    {
        BossMovementAnimation();
    }

    // Collide
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Turn Left")
        {
            ChangeMoveDirection(180);
            CastSpell();
        }

        else if (collision.gameObject.tag == "Turn Right")
        {
            ChangeMoveDirection(0);
            CastSpell();
        }

        else if (collision.gameObject.tag == "Sword")
        {
            GetHurt();
        }
    }
}
