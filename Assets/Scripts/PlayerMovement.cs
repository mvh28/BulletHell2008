using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 toRotation = new Vector3(0,0,0);
    Quaternion endRotation = new Quaternion();

    public float playerSpeed = 30f;
    public float sideRotation = 30f;
    public float RotationSpeed = 2f;

    private float sideIn;
    private float vertIn;
    private float fireRate = 0.2f;
    private GameObject spawner;
    // Start is called before the first frame update
    void Start()
    {
        var cam = Camera.main;
        spawner = GameObject.Find("SpawnPoint");
        spawner.GetComponent<BulletSpawn>().fireRate = fireRate;
        spawner.GetComponent<BulletSpawn>().bulletPattern = BulletSpawn.BulletPattern.Straight;
    }

    // Update is called once per frame
    void Update()
    {
        // Source to stay in camera: https://gamedev.stackexchange.com/questions/146903/how-to-restrict-the-players-movement-win-relation-to-screen-bounds-in-unity-wi
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);

        // Control player speed
        sideIn = Input.GetAxis("Horizontal");
        vertIn = Input.GetAxis("Vertical");

        playerSpeed = ControlSpeed();

        transform.Translate(Vector3.right * Time.deltaTime * playerSpeed * sideIn, Space.World);
        transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed * vertIn, Space.World);

        RotateShip();

        // Shooting mechanic
        spawner.GetComponent<BulletSpawn>().setActive = ShootBullet();

    }

    void RotateShip()
    {
        // Rotation of ship when moving left an right
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)){
            toRotation = new Vector3(0f,0f,sideRotation);
            endRotation = Quaternion.Euler(toRotation);
            transform.rotation = Quaternion.Slerp(transform.rotation, endRotation, RotationSpeed * Time.deltaTime);
        }

        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)){
            toRotation = new Vector3(0f,0f,-sideRotation);
            endRotation = Quaternion.Euler(toRotation);
            transform.rotation = Quaternion.Slerp(transform.rotation, endRotation, RotationSpeed * Time.deltaTime);
        }
        
        else{
            toRotation = new Vector3(0f,0f,0f);
            endRotation = Quaternion.Euler(toRotation);
            transform.rotation = Quaternion.Slerp(transform.rotation, endRotation, RotationSpeed * Time.deltaTime);
        }
    }

    private float ControlSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
            return 15f;
        }
        else{
            return 30f;
        }
    }

    private bool ShootBullet()
    {
        if (Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.M)){
            return true;
        }
        else{
            return false;
        }
    }
}
