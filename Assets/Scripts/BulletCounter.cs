using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletCounter : MonoBehaviour
{
    public TextMeshProUGUI bulletCount;

    private GameObject[] getCount;
    public static int count = 0;
 
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        getCount = GameObject.FindGameObjectsWithTag ("Bullet");
        count = getCount.Length;
        bulletCount.text = $"Number of Bullets: {count}";
    }
}
