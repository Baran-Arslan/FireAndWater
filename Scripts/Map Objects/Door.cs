using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform door;
    [SerializeField] private float doorOpenSpeed;
    [SerializeField] private float openedDoorYPosition;
    private float doorStartYPosition;
    public bool _openDoor;

    private float _playerCount;

    private void Awake()
    {
        doorStartYPosition = door.localPosition.y;
    }

    private void Update()
    {
        if (_openDoor)
        { 
            if(door.transform.localPosition.y <= openedDoorYPosition)
            door.transform.localPosition += new Vector3(0, doorOpenSpeed * Time.deltaTime, 0);
        }
        else if (door.transform.localPosition.y > doorStartYPosition)
            door.transform.localPosition -= new Vector3(0, doorOpenSpeed * Time.deltaTime, 0);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerCount++;
            if(_playerCount >= 2) GameManager.Instance.FinishLevel();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _openDoor = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _openDoor = false;
            _playerCount--;
        }
    }
}
