using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public float seconds;
    public int minutes;
    public GameObject eButton;
    
    [SerializeField] private CanvasGroup mainPanel;
    [SerializeField] private TextMeshProUGUI timerText;
    
    private float _fadePanelTime;
    private bool _timerOn;
    
    void Start()
    {
        //_player = FindObjectOfType<Player>();
        SetPanel(mainPanel, 1);
        _timerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timerOn)
        {
            seconds += 1 * Time.deltaTime;
            timerText.text = "TIME: "  + minutes + ":" + Mathf.Round(seconds * 100) / 100;
            if (seconds >= 60)
            {
                minutes += 1;
                seconds = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene(0);
        }
    }


    private void ResetPanels()
    {
        mainPanel.alpha = 0;
        mainPanel.gameObject.SetActive(false);
    }

    private void SetPanel(CanvasGroup panel, int endValue)
    {
        ResetPanels();
        panel.gameObject.SetActive(true);
        StartCoroutine(waitForPanel(panel, endValue));
    }

    IEnumerator waitForPanel(CanvasGroup panel, int endValue)
    {
        panel.DOFade(endValue, _fadePanelTime);
        yield return new WaitForSeconds(_fadePanelTime);
    }
}





















