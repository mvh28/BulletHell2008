using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletBehaviour : MonoBehaviour
{
    public float bulletSpeed = 1f;

    private Vector3 spawnPoint;

    void Start()
    {
        spawnPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    void Update()
    {
        transform.Translate(Vector3.up * bulletSpeed * Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Player")){
            GameManager.Instance.TriggerGameOver();
        }
        collision.GetComponent<Rigidbody>().isKinematic = true;
        Destroy(gameObject);
    }
}