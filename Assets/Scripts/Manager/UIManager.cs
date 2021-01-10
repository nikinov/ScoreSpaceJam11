using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainPanel;
    [SerializeField] private CanvasGroup finishPanel;
    [SerializeField] private TextMeshProUGUI ScoreText;
    public GameObject eButton;
    private float _fadePanelTime;
    //private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        //_player = FindObjectOfType<Player>();
        ChangeScore(0);
        SetPanel(mainPanel, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void ChangeScore(int Score)
    {
        ScoreText.text = "SCORE: " + Score;
    }

    IEnumerator waitForPanel(CanvasGroup panel, int endValue)
    {
        panel.DOFade(endValue, _fadePanelTime);
        yield return new WaitForSeconds(_fadePanelTime);
    }
}





















