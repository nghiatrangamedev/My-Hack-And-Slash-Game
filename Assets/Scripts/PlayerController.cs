using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject _bestSword;
    Animator _playerAnimator;

    Rigidbody2D _playerRb;

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
        SpawnSword();

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
        yield return new WaitForSeconds(0.5f);
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
        yield return new WaitForSeconds(0.1f);
        _isDashed = false;
    }

    void GetDamaged(Vector3 _enemyPos)
    {
        Vector2 _backDirection = transform.position - _enemyPos;
        float _backForce = 5.0f;

        _playerRb.AddForce(_backDirection * _backForce, ForceMode2D.Impulse);
        PlayerHeath -= 10;

        Debug.Log("Player heath: " + PlayerHeath);
        if (PlayerHeath <= 0)
        {
            Destroy(gameObject);
        }

    }

    void PlayerAnimation()
    {
        MoveAnimation();
        JumpToFallAnimation();
        IdieFallAnimation();
        DashAnimation();
        AttackAnimation();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isOnGround = true;
            Debug.Log("Is on the ground " + _isOnGround);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isOnGround = false;
            Debug.Log("Is on the ground " + _isOnGround);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GetDamaged(collision.gameObject.transform.position);
        }
    }
}
