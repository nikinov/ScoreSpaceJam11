using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform FinishLine;
    [SerializeField] private Player player;
    [SerializeField] private Transform finishPlaceholderParent;
    [SerializeField] private Transform table;
    [SerializeField] private Transform tablePlaceholderStart;
    [SerializeField] private Transform tablePlaceholderEnd;

    private int _finishCount;
    private List<Transform> _finishPlaces;
    private UIManager _uiManager;
    private Player _player;
    private bool _eSet;
    
    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _uiManager = GetComponent<UIManager>();
        player.Spawn();
        _finishCount = -1;
        _finishPlaces = new List<Transform>();
        foreach (Transform placeholder in finishPlaceholderParent)
        {
            _finishPlaces.Add(placeholder);
        }
        NextStage();
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
        }
    }

    public void NextStage()
    {
        if (_finishCount + 1 != _finishPlaces.Count)
        {
            _finishCount += 1;
            FinishLine.position = _finishPlaces[_finishCount].position;
        }
        else
        {
            Finished();
        }
    }

    public void Finished()
    {
        _player.rbController.EnableMovement = false;
    }

    IEnumerator waitForTeleport(Transform teleport)
    {
        yield return new WaitForSeconds(5);
        Vector3 scale = teleport.localScale;
        teleport.DOScale(Vector3.zero, .15f);
        yield return new WaitForSeconds(2f);
        teleport.position = tablePlaceholderEnd.position;
        teleport.DOScale(scale, .2f);
    }
}









