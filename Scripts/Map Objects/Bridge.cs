using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private GameObject trapVFX;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject newEffect = Instantiate(trapVFX, transform.position, Quaternion.identity);
            Destroy(newEffect, 2);
            Destroy(this.gameObject);
        }
    }
}
