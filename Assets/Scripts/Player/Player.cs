using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(RigidbodyFirstPersonController))]
public class Player : MonoBehaviour
{
    public Transform currentSpawnPos;
    public RigidbodyFirstPersonController rbController;

    [SerializeField] private Transform startingPos;
    [SerializeField] private AudioManager _audioManager;
    
    private Rigidbody _rb;
    private GameManager _gameManager;
    private UIManager _uiManager;
    private List<Transform> _spawnPossitions;
    private bool _startNewSound;
    private bool _startNewSound2;

    // Start is called before the first frame update
    void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _spawnPossitions = new List<Transform>();
        foreach (Transform pos in startingPos)
        {
            _spawnPossitions.Add(pos);
        }
        _gameManager = FindObjectOfType<GameManager>();
        rbController = GetComponent<RigidbodyFirstPersonController>();
        _rb = GetComponent<Rigidbody>();
        currentSpawnPos = _spawnPossitions[0];
        _audioManager.PlaySound("Music");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Death()
    {
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        rbController.EnableMovement = false;
    }

    public void Spawn()
    {
        transform.position = currentSpawnPos.position;
        rbController.EnableMovement = true;
        _rb.constraints = RigidbodyConstraints.None;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Obstacles")
        {
            other.collider.GetComponent<Obstacle>().Kill(this);
        }
        else if (other.gameObject.tag == "Finish")
        {
            PlayerPrefs.SetFloat("seconds", _uiManager.seconds);
            PlayerPrefs.SetInt("minutes", _uiManager.minutes);
            _gameManager.Finished();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Changers")
        {
            print(Convert.ToInt32(other.gameObject.name.Replace(")", "").Replace("MoveTrigger (", "")));
            currentSpawnPos = _spawnPossitions[Convert.ToInt32(other.gameObject.name.Replace(")", "").Replace("MoveTrigger (", ""))];
            if (!_startNewSound)
            {
                _startNewSound = true;
                _audioManager.StopSound("Music");
                _audioManager.PlaySound("Music2");
            }            
            if (!_startNewSound2 && Convert.ToInt32(other.gameObject.name.Replace(")", "").Replace("MoveTrigger (", "")) == 3)
            {
                _startNewSound2 = true;
                _audioManager.StopSound("Music2");
                _audioManager.PlaySound("Music3");
            }
        }
    }
}









