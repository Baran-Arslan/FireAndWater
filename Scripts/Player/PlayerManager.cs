using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour, IDamageable
{
    //Referances
    private Animator _animator;
    [SerializeField] private Transform modelTransform;

    [Header("Weapon")]
    [SerializeField] private Transform weaponHolder;
    private Weapon _currentWeaponType;
    private GameObject _currentWeapon;

    [Header("Attack Settings")]
    [SerializeField] private KeyCode attackKey;
    [SerializeField] private float attackRange;
    [SerializeField] private Vector3 raycastOffset;
    [SerializeField] private int enemyLayer;
    private RaycastHit _raycastHit;
    private float _currentAttackTime;

    [Header("Ranged Attack Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootTransform;

    [Header("Sounds")]
    [SerializeField] private AudioClip equipSound;
    private AudioSource _audioSource;

    //Animation IDS
    private int _attackParameter = Animator.StringToHash("Attack");
    private int _rangedParameter = Animator.StringToHash("Ranged");




    private void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        _audioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1);

    }

    private void Update()
    {
        HandleAttacking();
    }


    public void PickupWeapon(Weapon weapon)
    {
        _currentWeaponType = weapon;
        if (_currentWeapon != null) Destroy(_currentWeapon);
        _currentWeapon = Instantiate(_currentWeaponType.WeaponPrefab, weaponHolder.transform.position, weaponHolder.rotation, weaponHolder);
        _audioSource.PlayOneShot(equipSound);
    }

    private void HandleAttacking()
    {
        if (_currentAttackTime > 0) _currentAttackTime -= Time.deltaTime;
        if (_currentWeapon != null && Input.GetKeyDown(attackKey) && _currentAttackTime <= 0)
        {
            _animator.SetTrigger(_attackParameter);
            _currentAttackTime = _currentWeaponType.AttackCooldown;
        }
    }
    public void AnimationEvent_Hit()
    {
        _audioSource.PlayOneShot(_currentWeaponType.AttackSound);
        if (_currentWeaponType.Type == Weapon.WeaponType.Ranged)
        {
            Instantiate(bulletPrefab, shootTransform.position, shootTransform.rotation);
            return;
        }


        Vector3 forwardDirection = modelTransform.TransformDirection(Vector3.forward);
        Debug.DrawRay(modelTransform.position + raycastOffset, forwardDirection * attackRange, Color.red);
        if (Physics.Raycast(transform.position + raycastOffset, forwardDirection, out _raycastHit, attackRange, 1 << enemyLayer))
        {
            _raycastHit.transform.TryGetComponent(out IDamageable idamageable);
            if (idamageable != null) idamageable.Damage();
        }

    }
    public void Damage()
    {
        GameManager.Instance.RestartLevel();
    }

}
