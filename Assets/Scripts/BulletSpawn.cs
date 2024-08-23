using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawn : MonoBehaviour
{
    [Header("BasicChars")]
    public bool setActive = false;
    public enum BulletPattern {
        Straight,
        Rotate,
        Spread,
        Parabolic
    }
    public BulletPattern bulletPattern;
    public GameObject bulletPrefab;
    public float bulletSpeed = 1f;
    public float fireRate = 1f;

    [Header("SpreadChars")]
    public int bulletPerSpread;
    public float targetAngle;
    [Range(0,359)] public float spreadAngle;

    private GameObject tmpBullet;
    private float timer = 0f;

    private bool isShooting = false;
    private float currentAngle;
    private int direction = 1;
    private Quaternion bulletAngle;
    
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (setActive == true){
            if (bulletPattern == BulletPattern.Straight){
                shootStraight();
            }
            else if (bulletPattern == BulletPattern.Rotate){
                shootRotate();
            }
            else if (bulletPattern == BulletPattern.Spread){
                shootSpread();
            }
            else if (bulletPattern == BulletPattern.Parabolic){
                shootParabolic();
            }
            else{
                isShooting = false;
            }
        }
        if (timer <= -10){
            timer = 0f;
        }
    }

    private void shootBullet(Quaternion bulletAngle)
    {
        if (bulletPrefab)
        {
            GameObject tmpBullet = Instantiate(bulletPrefab,transform.position,Quaternion.identity);
            tmpBullet.transform.rotation = transform.rotation * bulletAngle;
            tmpBullet.GetComponent<BulletBehaviour>().bulletSpeed = bulletSpeed;
        }
    }

    // Different patterns
    private void shootStraight()
    {
        if (timer <= 0f){
                shootBullet(Quaternion.Euler(0,0,0));
                timer = fireRate;
            }
    }

    private void shootRotate()
    {
        float startAngle = targetAngle - spreadAngle;
        float endAngle = targetAngle + spreadAngle;
        float angleStep = spreadAngle / (bulletPerSpread - 1) * direction;

        if (!isShooting){
            currentAngle = targetAngle;
            isShooting = true;
        }
        
        if (timer <= 0f){
            bulletAngle = Quaternion.Euler(currentAngle,0,0);
            shootBullet(bulletAngle);

            currentAngle += angleStep;

            if (currentAngle < startAngle){
                direction = 1;
            }
            else if (currentAngle > endAngle){
                direction = -1;
            }
            timer = fireRate;
        }
    }

    private void shootSpread()
    {
        currentAngle = targetAngle;
        float halfAngle = 0f;
        float angleStep = 0f;

        if (spreadAngle != 0){
            halfAngle = spreadAngle / 2f;
            currentAngle = targetAngle - halfAngle;
        }

        if (timer <= 0f){
            angleStep = spreadAngle / (bulletPerSpread - 1);
            for (int i = 0; i < bulletPerSpread; i++){
                bulletAngle = Quaternion.Euler(currentAngle,0,0);
                shootBullet(bulletAngle);
                currentAngle += angleStep;
            }
            timer = fireRate;
        }
    }

    private void shootParabolic()
    {
        currentAngle = targetAngle;
        float halfAngle = 0f;
        float angleStep = 0f;

        if (spreadAngle != 0){
            halfAngle = spreadAngle / 2f;
            currentAngle = targetAngle - halfAngle;
        }

        if (timer <= 0f){
            angleStep = spreadAngle / (bulletPerSpread - 1);
            for (int i = 0; i < bulletPerSpread; i++){
                float parSpeed = bulletSpeed + Convert.ToSingle(Math.Pow((-targetAngle - currentAngle)/Math.PI,2))/bulletSpeed;

                bulletAngle = Quaternion.Euler(currentAngle,0,0);
                
                GameObject tmpBullet = Instantiate(bulletPrefab,transform.position,Quaternion.identity);
                tmpBullet.transform.rotation = transform.rotation * bulletAngle;
                tmpBullet.GetComponent<BulletBehaviour>().bulletSpeed = parSpeed;

                currentAngle += angleStep;
            }
            timer = fireRate;
        }
    }
}
