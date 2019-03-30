using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    public Vector2 _axisInput;
    public bool _jumpInput;
    public bool _rewindTimeInput;
    public bool _forwardTimeInput;
    public bool _pauseTimeInput;
    public bool _dashInput;

    private bool _isFacingLeft;
    private bool _isFacingRight;
    private bool _isDashing;

    private const float _dashDuration = 0.15f;
    private const float _dashSpeed = 30f;
    private const float _jumpHeight = 4;
    private const float _timeToJumpApex = .4f;
    private const float _accelerationTimeAirborne = .2f;
    private const float _accelerationTimeGrounded = .1f;
    private const float _moveSpeed = 6f;
    
    
    private float _gravity; 
    private Vector3 _velocity;
    private float _jumpSpeed;
    private float _velocityXSmoothing;

    private SpriteRenderer _spriteRenderer;
    private PlayerController _controller;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _controller = GetComponent<PlayerController>();
        _gravity = -(2 * _jumpHeight) / Mathf.Pow(_timeToJumpApex, 2);
        _jumpSpeed = Mathf.Abs(_gravity) * _timeToJumpApex;
    }

    void Update()
    {
        ParseInput();
        if (!(_rewindTimeInput || _forwardTimeInput))
        {
            ExecuteInput();
        }
        
    }

    private void ParseInput()
    {
        _axisInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));

        _jumpInput = Input.GetButtonDown("Jump");

        _rewindTimeInput = Input.GetButton("RewindTime");

        _forwardTimeInput = Input.GetButton("ForwardTime");

        _pauseTimeInput = (_rewindTimeInput && _forwardTimeInput);

        _dashInput = Input.GetButtonDown("Dash");
    }

    private void ExecuteInput()
    {
        if (_dashInput)
        {
            StartCoroutine(Dash());
        }
        if (!_isDashing)
        {
            CalculateVelocity();
        }
        
        FlipSprite();
        _controller.Move(_velocity * Time.deltaTime);
    }

    IEnumerator Dash()
    {
        _isDashing = true;
        _velocity = new Vector2(_dashSpeed, 0);

        if (_isFacingLeft)
        {
            _velocity.x = -_velocity.x;
        }

        yield return new WaitForSeconds(_dashDuration);

        _velocity = Vector2.zero;
        _axisInput = Vector2.zero;
        _isDashing = false;
    }

    private void FlipSprite()
    {
        Debug.Log("Initial flip: " + _spriteRenderer.flipX);
        if (_velocity.x > 0)
        {
            Debug.Log(1);
            _isFacingLeft = false;
            _isFacingRight = true;
            _spriteRenderer.flipX = false;
        }
        else if (_velocity.x < 0)
        {
            Debug.Log(2);
            _isFacingLeft = true;
            _isFacingRight = false;
            _spriteRenderer.flipX = true;
        }
        Debug.Log("End flip: " + _spriteRenderer.flipX);
        Debug.Log("Flipping: " + _spriteRenderer.flipX + "_velocity.x: " + _velocity.x);
    }

    private void CalculateVelocity()
    {
        if (_axisInput.x == 0)
        {
            _velocity.x = 0;
        }
        else {
            float targetVelocityX = _axisInput.x * _moveSpeed;
            float accelerationTime;
            if (_controller._collisionInfo.below)
            {
                accelerationTime = _accelerationTimeGrounded;
            }
            else
            {
                accelerationTime = _accelerationTimeAirborne;
            }

            _velocity.x = Mathf.SmoothDamp(
                _velocity.x,
                targetVelocityX,
                ref _velocityXSmoothing,
                accelerationTime);
        }
        


        if (_controller._collisionInfo.above || _controller._collisionInfo.below)
        {
            _velocity.y = 0;
        }

        if (_jumpInput && _controller._collisionInfo.below)
        {
            _velocity.y = _jumpSpeed;
        }

        _velocity.y += _gravity * Time.deltaTime;
    }
}