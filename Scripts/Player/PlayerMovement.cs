using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Referances")]
    [SerializeField] private Transform modelTransform;
    private Rigidbody _rigidbody;
    private Animator _animator;

    [Header("Movement")]
    [SerializeField] private string horizontalInputName;
    [SerializeField] private float movementSpeed;
    private bool _canMoveInAir = true;
    private Vector3 _moveDirection;
    private float _horizontalInput;

    [Header("Check Ground")]
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float fallMultipler;
    public bool _isGrounded;
    private float _inAirTimer;
    private float _groundCheckDelay;

    [Header("Jumping")]
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private float jumpSpeed;
    private bool _canJump = true;
    private bool _isJumping;

    [Header("Sounds")]
    [SerializeField] private AudioClip[] stepSounds;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landSound;
    private AudioSource _audioSource;


    //Animation IDS
    private int _speedParameter = Animator.StringToHash("Speed");
    private int _jumpParameter = Animator.StringToHash("Jumping");
    private int _fallingParameter = Animator.StringToHash("Falling");


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        _audioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1);
    }
    private void Update()
    {
        HandleInputCheck();
        HandleGroundCheck();
        HandleRotation();

        _animator.SetFloat(_speedParameter, Mathf.Abs(_horizontalInput), 0.1f, Time.deltaTime);
    }
    private void FixedUpdate()
    {
        HandleMovement();
        HandleGravity();
    }
    private void HandleInputCheck()
    {
        _horizontalInput = Input.GetAxisRaw(horizontalInputName);
        if (Input.GetKeyDown(jumpKey)) Jump();
    }
    private void HandleMovement()
    {
        if (_isJumping && !_canMoveInAir) return;
        _moveDirection = transform.forward * _horizontalInput;
        _moveDirection.Normalize();
        _moveDirection = _moveDirection * movementSpeed;
        if (!_isJumping) _moveDirection.y = 0;
        else _moveDirection.y = _rigidbody.velocity.y;

        _rigidbody.velocity = _moveDirection;
    }
    private void Jump()
    {
        if (!_isGrounded) return;
        if (!_canJump) return;
        Vector3 playerVelocity = _moveDirection;
        playerVelocity.y = jumpSpeed;
        _rigidbody.velocity = playerVelocity;
        _groundCheckDelay = 0.2f;
        _isJumping = true;
        _canJump = false;
        _animator.SetBool(_jumpParameter, true);
        _audioSource.PlayOneShot(jumpSound);
    }
    private void HandleRotation()
    {
        if (_horizontalInput == 0) return;
        modelTransform.rotation = Quaternion.Euler(0, -90 * _horizontalInput, 0);
    }
    private void HandleGroundCheck()
    {
        if (_groundCheckDelay > 0) _groundCheckDelay -= Time.deltaTime;
        _isGrounded = Physics.CheckSphere(transform.position, groundCheckRadius, groundLayers) && _groundCheckDelay <= 0;
    }
    private void HandleGravity()
    {
        if (!_isGrounded)
        {
            _inAirTimer = _inAirTimer + Time.deltaTime;
            if (!_isJumping)
                _rigidbody.AddForce(-Vector3.up * fallMultipler * _inAirTimer);

            if(_inAirTimer >= 0.5f) _animator.SetBool(_fallingParameter, true);
            return;
        }
        if (_isJumping)
        {
            _animator.SetBool(_jumpParameter, false);
            _isJumping = false;
            _canJump = true;
        }
        if (_inAirTimer > 0.5f) _audioSource.PlayOneShot(landSound);
        _inAirTimer = 0;
        _canMoveInAir = true;
        _animator.SetBool(_fallingParameter, false);
    }
    //public void AnimationEvent_ResetJump()
    //{
    //    _canJump = true;
    //}
    public void AnimationEvent_PlayStepSound()
    {
        int randomNumber = Random.Range(0, stepSounds.Length);
        _audioSource.PlayOneShot(stepSounds[randomNumber]);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isGrounded)
            _canMoveInAir = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
    }

}
