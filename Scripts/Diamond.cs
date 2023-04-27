using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;
    private void Start()
    {
        GameManager.Instance.NeededScore++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.GetComponent<AudioSource>().PlayOneShot(collectSound);
            GameManager.Instance.AddScore();
            Destroy(gameObject);
        }
    }
}
