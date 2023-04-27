using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private bool isEnemyBullet;

    private void Awake()
    {
        Destroy(gameObject, 3);
        _rigidbody = GetComponent<Rigidbody>();
        AddForce();
    }


    private void AddForce()
    {
        _rigidbody.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && isEnemyBullet)
        {
            other.transform.TryGetComponent(out IDamageable idamageable);
            idamageable.Damage();
        }
        if (other.transform.CompareTag("Enemy") && !isEnemyBullet)
        {
            other.transform.TryGetComponent(out IDamageable idamageable);
            idamageable.Damage();
        }
    }
}
