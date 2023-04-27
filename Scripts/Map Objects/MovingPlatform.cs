using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Movement")]
    [SerializeField] private Transform targetTransform;
    private Vector3 _originalPosition;
    [SerializeField] private float moveSpeed;
    [HideInInspector] public bool MovePlatform = false;

    private void Awake()
    {
        _originalPosition = transform.position;
    }

    private void Update()
    {
        if (MovePlatform)
        {
            MoveTowardsTarget(targetTransform.position);
            return;
        }
        MoveTowardsTarget(_originalPosition);
    }

    private void MoveTowardsTarget(Vector3 position)
    {
        transform.position = Vector3.MoveTowards(transform.position, position, moveSpeed * Time.deltaTime);

    }
    public void SwitchTarget()
    {
        if (MovePlatform) MovePlatform = false;
        else MovePlatform = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
