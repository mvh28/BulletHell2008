using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStaging : MonoBehaviour
{
    private int scene = 0;
    private BulletSpawn spawn1;
    private BulletSpawn spawn2;
    private BulletSpawn spawn3;
    private BulletSpawn spawn4;
    private BulletSpawn spawn5;
    private BulletSpawn spawn6;

    [Header("Rotation Shooting")]
    public float rotRate = 0.1f;
    public float rotAngle = 90f;
    public int rotNum = 10;

    [Header("Spread Shooting")]
    public float spreadRate = 1f;
    public float spreadAngle = 90f;
    public int spreadNum = 12;

    [Header("Parabolic Shooting")]
    public float parRate = 1f;
    public float parAngle = 180f;
    public int parNum = 15;

    void Start(){
        transform.position = new Vector3(0,0,200);
        transform.rotation = Quaternion.identity;
        StartCoroutine(EnterBoss());

        spawn1 = GameObject.Find("SpawnPoint1").GetComponent<BulletSpawn>();
        spawn2 = GameObject.Find("SpawnPoint2").GetComponent<BulletSpawn>();
        spawn3 = GameObject.Find("SpawnPoint3").GetComponent<BulletSpawn>();
        spawn4 = GameObject.Find("SpawnPoint4").GetComponent<BulletSpawn>();
        spawn5 = GameObject.Find("SpawnPoint5").GetComponent<BulletSpawn>();
        spawn6 = GameObject.Find("SpawnPoint6").GetComponent<BulletSpawn>();
    }

    private void OnEnable(){
        TimeManager.changeScene += sceneChange;
    }

    private void OnDisable(){
        TimeManager.changeScene -= sceneChange;
    }

    private void sceneChange(){
        spawn1.setActive = false;
        spawn2.setActive = false;
        spawn3.setActive = false;
        spawn4.setActive = false;
        spawn5.setActive = false;
        spawn6.setActive = false;

        if (scene == 0){
            // Set spawn1
            spawn1.setActive = true;
            spawn1.bulletPattern = BulletSpawn.BulletPattern.Rotate;
            spawn1.bulletPerSpread = rotNum;
            spawn1.spreadAngle = rotAngle;
            spawn1.fireRate = rotRate;

            // Set spawn2
            spawn2.setActive = true;
            spawn2.bulletPattern = BulletSpawn.BulletPattern.Spread;
            spawn2.bulletPerSpread = spreadNum;
            spawn2.spreadAngle = spreadAngle;
            spawn2.fireRate = spreadRate;
            spawn2.targetAngle = 10f;

            // Set spawn3
            spawn3.setActive = true;
            spawn3.bulletPattern = BulletSpawn.BulletPattern.Spread;
            spawn3.bulletPerSpread = spreadNum;
            spawn3.spreadAngle = spreadAngle;
            spawn3.fireRate = spreadRate;
            spawn3.targetAngle = -10f;
        }
        else if (scene == 1){
            StartCoroutine(MoveSide());

            spawn1.setActive = true;
            spawn1.fireRate = rotRate;
            spawn2.setActive = true;
            spawn2.targetAngle = 0f;
            spawn3.setActive = true;
            spawn3.targetAngle = 0f;
        }
        else if (scene == 2){
            StartCoroutine(ChangePos());
        }
        else if (scene == 3){
            // Spawn 5 with spread shooter
            spawn5.setActive = true;
            spawn5.bulletPattern = BulletSpawn.BulletPattern.Spread;
            spawn5.bulletPerSpread = spreadNum;
            spawn5.spreadAngle = spreadAngle*2;
            spawn5.fireRate = spreadRate;

            // Spawn 4 and 6 with parabolic shooter
            spawn4.setActive = true;
            spawn4.bulletPattern = BulletSpawn.BulletPattern.Parabolic;
            spawn4.bulletPerSpread = parNum;
            spawn4.spreadAngle = parAngle;
            spawn4.fireRate = parRate;

            spawn6.setActive = true;
            spawn6.bulletPattern = BulletSpawn.BulletPattern.Parabolic;
            spawn6.bulletPerSpread = parNum;
            spawn6.spreadAngle = parAngle;
            spawn6.fireRate = parRate;
        }
        else if (scene == 4){
            if (BulletCounter.count > 0){
                scene -= 1;
            }
            else if (BulletCounter.count == 0){
                GameManager.Instance.TriggerGameOver();
            }
        }
        scene += 1;
    }

    private IEnumerator MoveSide(){
        float maxX = 50f;
        float minX = -1 * maxX;
        float speed = 20f;
        int direction = 1;
        float timeElapsed = 0;
        float timeToMove = TimeManager.stageTime;

        while (timeElapsed < timeToMove){
            transform.Translate(Vector3.right * Time.deltaTime * speed * direction, Space.World);

            if (transform.position.x > maxX){
                direction = -1;
            }
            else if (transform.position.x < minX){
                direction = 1;
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator ChangePos(){
        Vector3 targetPos1 = new Vector3(-160,0,-90);
        Vector3 targetPos2 = new Vector3(6,0,60);
        Vector3 startPos1 = transform.position;
        Vector3 startPos2 = new Vector3(-200,0,60);

        Vector3 targetRot1 = new Vector3(0,45,0);
        Vector3 targetRot2 = new Vector3(0,-90,0);

        float timeElapsed = 0;
        float timeToMove = TimeManager.stageTime/2;

        while (timeElapsed < timeToMove){
            transform.position = Vector3.Lerp(startPos1, targetPos1, Mathf.SmoothStep(0f,1f,timeElapsed/(timeToMove)));

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRot1), Mathf.SmoothStep(0f,1f,timeElapsed/(timeToMove)));

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        timeElapsed = 0;

        while (timeElapsed < timeToMove){
            transform.position = Vector3.Lerp(startPos2, targetPos2, Mathf.SmoothStep(0f,1f,timeElapsed/timeToMove));

            transform.rotation = Quaternion.Euler(targetRot2);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator EnterBoss(){
        Vector3 startPos = new Vector3(0,0,75);

        float timeElapsed = 0f;
        float timeToMove = TimeManager.stageTime;
        while (timeElapsed < timeToMove){
            transform.position = Vector3.Lerp(transform.position, startPos, Mathf.SmoothStep(0f,1f,timeElapsed/timeToMove));

            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }
}
