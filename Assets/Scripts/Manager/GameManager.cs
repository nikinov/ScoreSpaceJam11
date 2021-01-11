using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Transform table;
    [SerializeField] private Transform tablePlaceholderStart;
    [SerializeField] private Transform tablePlaceholderEnd;

    private int _finishCount;
    private UIManager _uiManager;
    private Player _player;
    private AudioManager _audioManager;
    private bool _eSet;
    
    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _uiManager = GetComponent<UIManager>();
        _audioManager = FindObjectOfType<AudioManager>();
        player.Spawn();
        _finishCount = -1;
        StartCoroutine(waitForTeleport(table));
    }

    private void Update()
    {
        if (Vector3.Distance(_player.transform.position, table.transform.position) <= .5f)
        {
            if (!_uiManager.eButton.activeInHierarchy)
            {
                _uiManager.eButton.SetActive(true);
                _eSet = true;
            }
        }
        else
        {
            if (_uiManager.eButton.activeInHierarchy)
            {
                _uiManager.eButton.SetActive(false);
                _eSet = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && _eSet)
        {
            _player.transform.position = tablePlaceholderStart.position;
            _audioManager.PlaySound("Button");
        }
    }

    public void Finished()
    {
        _player.rbController.EnableMovement = false;
        print("gameFinished");
        SceneManager.LoadScene(0);
    }

    IEnumerator waitForTeleport(Transform teleport)
    {
        yield return new WaitForSeconds(5);
        Vector3 scale = teleport.localScale;
        teleport.DOScale(Vector3.zero, .15f);
        yield return new WaitForSeconds(2f);
        _audioManager.PlaySound("Table");
        teleport.position = tablePlaceholderEnd.position;
        teleport.DOScale(scale, .2f);
    }
}









