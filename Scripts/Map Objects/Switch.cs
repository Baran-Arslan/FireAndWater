using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private MovingPlatform platformScript;
    [SerializeField] private Transform stickTransform;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            platformScript.SwitchTarget();
            SetRotation(new Vector3(40, 0, 0));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            platformScript.SwitchTarget();
            SetRotation(Vector3.zero);
        }
    }

    private void SetRotation(Vector3 rotation)
    {
        stickTransform.rotation = Quaternion.Euler(rotation);
    }
}
