using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.TryGetComponent(out IDamageable idamageable);
            idamageable.Damage();
        }
    }
}
