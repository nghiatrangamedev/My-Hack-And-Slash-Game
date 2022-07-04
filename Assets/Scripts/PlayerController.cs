using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject _sword;

    Rigidbody2D _playerRb;

    float _horizontalInput;
    float _verticalInput;
    float _speed = 20.0f;
    float _jumpForce = 10.0f;

    bool _isOnGround;

    // Start is called before the first frame update
    void Start()
    {
        _playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    void PlayerInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
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
    }

    void MoveAround()
    {
        _playerRb.AddForce(Vector2.right * _horizontalInput * _speed);
    }

    void Jump()
    {
        _playerRb.AddForce(Vector2.up * _verticalInput * _jumpForce, ForceMode2D.Impulse);
    }

    void PlayerAttack()
    {
        ActiveSword();
        StartCoroutine(DeactiveSword());
    }

    void ActiveSword()
    {
        _sword.SetActive(true);
    }

    IEnumerator DeactiveSword()
    {
        yield return new WaitForSeconds(0.5f);
        _sword.SetActive(false);
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
