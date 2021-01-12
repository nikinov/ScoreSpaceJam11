using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI seconds;
    [SerializeField] private TextMeshProUGUI minutes;
    [SerializeField] private RectTransform playButton;

    [SerializeField] private RectTransform panel;
    
    private void Start()
    {
        
        seconds.text =  (Mathf.Round(PlayerPrefs.GetFloat("seconds") * 100) / 100).ToString();
        minutes.text = PlayerPrefs.GetInt("minutes").ToString() + ":";
        
        Cursor.visible = true;
        Screen.lockCursor = false;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
