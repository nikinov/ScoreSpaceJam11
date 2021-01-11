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
    [SerializeField] private CanvasGroup mainPanel;
    [SerializeField] private CanvasGroup finishPanel;
    [SerializeField] private TextMeshProUGUI timerText;
    public GameObject eButton;
    
    private float _fadePanelTime;
    private float _timer;
    private int _minutes;
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
            _timer += 1 * Time.deltaTime;
            timerText.text = "TIME: "  + _minutes + ":" + Mathf.Round(_timer * 100) / 100;
            if (_timer >= 60)
            {
                _minutes += 1;
                _timer = 0;
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
        finishPanel.alpha = 0;
        mainPanel.gameObject.SetActive(false);
        finishPanel.gameObject.SetActive(false);
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





















