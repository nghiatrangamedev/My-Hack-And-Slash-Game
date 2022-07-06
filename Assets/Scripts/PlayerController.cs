using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //[SerializeField] GameObject _sword;
    [SerializeField] GameObject _bestSword;

    Rigidbody2D _playerRb;

    float _horizontalInput;
    float _verticalInput;
    float _speed = 20.0f;
    float _jumpForce = 10.0f;
    float _dashForce = 5.0f;

    bool _isOnGround;
    bool _isFaceRight = true;
    bool _isDashed;
    bool _isAttacked;

    // Start is called before the first frame update
    void Start()
    {
        _playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        FlipPlayer();
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
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        else if (!_isFaceRight && _horizontalInput > 0)
        {
            _isFaceRight = true;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
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

    /*
    void ActiveSword()
    {
        _sword.SetActive(true);
    }

    IEnumerator DeactiveSword()
    {
        yield return new WaitForSeconds(0.2f);
        _sword.SetActive(false);
    }
    */

    void Dash()
    {
        _playerRb.AddForce(Vector2.right * _horizontalInput * _dashForce, ForceMode2D.Impulse);
        _isDashed = true;
        StartCoroutine(DashCD());
    }

    IEnumerator DashCD()
    {
        yield return new WaitForSeconds(1);
        _isDashed = false;
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
}
