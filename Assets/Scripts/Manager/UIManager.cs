using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup MainPanel;
    [SerializeField] private TextMeshProUGUI ScoreText;
    private float _fadePanelTime;
    //private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        //_player = FindObjectOfType<Player>();
        ChangeScore(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ResetPanels()
    {
        MainPanel.alpha = 0;
        MainPanel.gameObject.SetActive(false);
    }

    private void SetPanel(CanvasGroup panel, int endValue)
    {
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





















