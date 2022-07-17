using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject _bestSword;
    Animator _playerAnimator;

    Rigidbody2D _playerRb;

    public bool _isGetHurt;

    float _horizontalInput;
    float _verticalInput;
    float _speed = 20.0f;
    float _jumpForce = 10.0f;
    float _dashForce = 15.0f;
    float _playerHeath = 100.0f;

    bool _isOnGround;
    bool _isFaceRight = true;
    bool _isDashed;
    bool _isAttacked;
    
    bool _isDeath;

    public float PlayerHeath
    {
        get { return _playerHeath; }
        set
        {
            if (value > 100)
            {
                value = 100;
            }

            else if (value < 0)
            {
                value = 0;
            }

            _playerHeath = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        FlipPlayer();
        PlayerAnimation();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    void PlayerInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && !_isAttacked)
        {
            PlayerAttack();
        }
    }

    void PlayerMovement()
    {
        if (_horizontalInput != 0)
        {
            MoveAround();
        }

        if (_verticalInput > 0 && _isOnGround)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!_isDashed)
            {
                Dash();
            }

        }
    }

    void MoveAround()
    {
        _playerRb.AddForce(Vector2.right * _horizontalInput * _speed);
    }

    void Jump()
    {
        _playerRb.AddForce(Vector2.up * _verticalInput * _jumpForce, ForceMode2D.Impulse);
    }

    void FlipPlayer()
    {
        if (_isFaceRight && _horizontalInput < 0)
        {
            _isFaceRight = false;
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

        else if (!_isFaceRight && _horizontalInput > 0)
        {
            _isFaceRight = true;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void PlayerAttack()
    {
        _isAttacked = true;
        StartCoroutine(AttackRate());

        Invoke("SpawnSword", 0.3f);
        //SpawnSword();

        //ActiveSword();
        //StartCoroutine(DeactiveSword());
    }

    void SpawnSword()
    {
        if (_isFaceRight)
        {
            Instantiate(_bestSword, transform.position + Vector3.right, _bestSword.transform.rotation);
        }

        else
        {
            Instantiate(_bestSword, transform.position + Vector3.left, _bestSword.transform.rotation);
        }
    }

    IEnumerator AttackRate()
    {
        yield return new WaitForSeconds(0.7f);
        _isAttacked = false;

    }

    void Dash()
    {
        if (_horizontalInput == 0)
        {
            _playerRb.AddForce(transform.right * _dashForce, ForceMode2D.Impulse);
        }

        else
        {
            _playerRb.AddForce(transform.right * _dashForce / 2, ForceMode2D.Impulse);
        }

        _isDashed = true;
        StartCoroutine(DashCD());
    }

    IEnumerator DashCD()
    {
        yield return new WaitForSeconds(0.2f);
        _isDashed = false;
    }

    void GetDamaged(/*Vector3 _enemyPos*/)
    {
        /*
        float _backForce = 10.0f;
        Vector2 _backDirection = new Vector3( (transform.position.x - _enemyPos.x) * _backForce, transform.position.y);

        _playerRb.AddForce(_backDirection , ForceMode2D.Impulse);
        */
        PlayerHeath -= 30;

        if (PlayerHeath <= 0 && !_isDeath)
        {
            //_isDeath = true;
            _playerAnimator.Play("Death", 0);
        }
    }

    void PlayerAnimation()
    {
        MoveAnimation();
        JumpToFallAnimation();
        IdieFallAnimation();
        DashAnimation();
        AttackAnimation();
        HurtAnimation();
        DeathAnimation();
    }

    void MoveAnimation()
    {
        _playerAnimator.SetFloat("speed", Mathf.Abs(_horizontalInput));
    }

    void JumpToFallAnimation()
    {
        if (_isOnGround)
        {
            if (_verticalInput > 0)
            {
                _playerAnimator.SetBool("isJump", true);
            }
        }

        else
        {
            _playerAnimator.SetBool("isJump", false);
        }
    }

    void IdieFallAnimation()
    {
        if (_isOnGround)
        {
            _playerAnimator.SetBool("isOnGround", true);
        }

        else
        {
            _playerAnimator.SetBool("isOnGround", false);
        }

    }

    void DashAnimation()
    {
        _playerAnimator.SetBool("isDash", _isDashed);
    }

    void AttackAnimation()
    {
        _playerAnimator.SetBool("isAttack", _isAttacked);
    }

    void HurtAnimation()
    {
        _playerAnimator.SetBool("isHurt", _isGetHurt);
    }

    void DeathAnimation()
    {
        _playerAnimator.SetBool("isDeath", _isDeath);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isOnGround = true;
        }

        if (collision.gameObject.CompareTag("EnemyDamage"))
        {
            GetDamaged();
            _isGetHurt = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isOnGround = false;
            Debug.Log("Is on the ground " + _isOnGround);
        }

        if (collision.gameObject.CompareTag("EnemyDamage"))
        {
            _isGetHurt = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GetDamaged();
            _isGetHurt = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _isGetHurt = false;
        }
    }
}
