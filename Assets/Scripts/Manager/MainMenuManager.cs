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
    
    private void Start()
    {
        seconds.text = PlayerPrefs.GetFloat("seconds").ToString();
        minutes.text = PlayerPrefs.GetFloat("minutes").ToString() + ":";
        StartCoroutine(waitForPlayButton());
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    IEnumerator waitForPlayButton()
    {
        playButton.position = new Vector3(Random.Range(300, 1300), Random.Range(100, 700));
        yield return new WaitForSeconds(1);
        StartCoroutine(waitForPlayButton());
    }
}
