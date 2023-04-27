using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private Vector3 rotateAxis;
    [SerializeField] private float rotateSpeed;


    private void Update()
    {
        transform.Rotate(rotateAxis * rotateSpeed * Time.deltaTime);
    }
}
