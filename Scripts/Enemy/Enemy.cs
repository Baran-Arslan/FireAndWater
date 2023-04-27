using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour, IDamageable
{
    public enum EnemyTypes
    {
        Staying,
        Moving,
        Ranged
    }

    //Referances
    public EnemyTypes EnemyType;
    private Animator _animator;
    private Rigidbody _rigidbody;

    [Header("Range Settings")]
    [SerializeField] private float attackRange;
    [SerializeField] private float sightRange;
    [SerializeField] private Vector3 raycastOffset;
    [SerializeField] private int playerLayer;
    private bool _playerInSightRange;
    private RaycastHit _raycastHit;

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown;
    private float _currentAttackTime = 99; //We make this number high to attack instantly in first sight

    [Header("Patrol Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform targetPoint;
    [SerializeField] private Transform secondTargetPoint;
    private float _delay;
    public bool _changeRotation;

    [Header("Ranged Enemy Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootTransform;

    [Header("Die")]
    [SerializeField] private GameObject dieEffect;
    //Animation IDS
    private int _screamParameter = Animator.StringToHash("Scream");
    private int _attackParameter = Animator.StringToHash("Attack");
    private int _walkingParameter = Animator.StringToHash("Walking");
    private int _rangedParameter = Animator.StringToHash("Ranged");

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();


        if (EnemyType == EnemyTypes.Ranged)
        {
            _animator.SetBool(_rangedParameter, true);
            return;
        }
        _animator.SetBool(_rangedParameter, false);
    }
    private void Update()
    {
        HandleAnimations();

        if (EnemyType != EnemyTypes.Moving) return;
        HandlePatrolDistanceCheck();
    }
    private void FixedUpdate()
    {
        HandleRaycast();

        if (EnemyType != EnemyTypes.Moving) return;
        HandleMovement();
    }
    private void HandleMovement()
    {
        if (_playerInSightRange)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }
        Vector3 _moveDirection = transform.forward * moveSpeed;
        _moveDirection.y = 0;
        _rigidbody.velocity = _moveDirection;
    }
    private void HandlePatrolDistanceCheck()
    {
        if (_delay > 0) _delay -= Time.deltaTime;
        _changeRotation = (((transform.position - targetPoint.position).magnitude < 2) || (transform.position - secondTargetPoint.position).magnitude < 2) && _delay <= 0 && !_playerInSightRange;

        if (_changeRotation)
        {
            _delay = 0.5f;
            Quaternion currentRotation = transform.rotation;
            transform.rotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y * -1, currentRotation.eulerAngles.z);
        }
    }
    private void HandleRaycast()
    {
        Vector3 forwardDirection = transform.TransformDirection(Vector3.forward); //Calculate forward direction

        Debug.DrawRay(transform.position + raycastOffset, forwardDirection * sightRange, Color.green);
        if (Physics.Raycast(transform.position + raycastOffset, forwardDirection, out _raycastHit, sightRange, 1 << playerLayer)) //If player in sight range
        {
            Debug.DrawRay(transform.position + raycastOffset, forwardDirection * attackRange, Color.red);
            _playerInSightRange = true;

            if (Physics.Raycast(transform.position + raycastOffset, forwardDirection, out _raycastHit, attackRange, 1 << playerLayer)) //If player in attack range
            {
                _currentAttackTime += Time.fixedDeltaTime;
                if (_currentAttackTime >= attackCooldown) Attack();
                return;
            }
            return;
        }
        _playerInSightRange = false;
    }


    private void Attack()
    {
        _animator.SetTrigger(_attackParameter);
        _currentAttackTime = 0;
    }
    public void AnimationEvent_Hit()
    {
        if (EnemyType == EnemyTypes.Ranged)
        {
            Instantiate(bulletPrefab, shootTransform.position, shootTransform.rotation);
            return;
        }

        if (_raycastHit.transform == null) return;
        _raycastHit.transform.TryGetComponent(out IDamageable idamageable);
        if (idamageable != null) idamageable.Damage();
    }

    public void Damage()
    {
        Destroy(gameObject);
        Instantiate(dieEffect, transform.position + new Vector3(0,1,0), Quaternion.identity);
    }

    private void HandleAnimations()
    {
        _animator.SetBool(_screamParameter, _playerInSightRange);
        if(_rigidbody != null)
            _animator.SetBool(_walkingParameter, _rigidbody.velocity.magnitude > 0.1f);
    }

}
