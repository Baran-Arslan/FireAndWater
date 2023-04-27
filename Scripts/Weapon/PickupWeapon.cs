using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
    public Weapon m_Weapon;

    public void DestroyWeapon()
    {
        Destroy(gameObject);
        //TODO - ADD VFX
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            PlayerManager playerManager = other.GetComponent<PlayerManager>();
            playerManager.PickupWeapon(m_Weapon);
            DestroyWeapon();
        }
    }
}
