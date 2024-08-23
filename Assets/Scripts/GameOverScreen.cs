using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public void Setup(){
        gameObject.SetActive(true);
    }

    public void Restart(){
        SceneManager.LoadScene("Game");
        Time.timeScale = 1f;
    }
}
