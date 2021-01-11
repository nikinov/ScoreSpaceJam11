using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(RigidbodyFirstPersonController))]
public class Player : MonoBehaviour
{
    public int score;
    public Transform currentSpawnPos;
    public RigidbodyFirstPersonController rbController;

    [SerializeField] private Transform startingPos;
    
    private Rigidbody _rb;
    private UIManager _uiManager;
    private GameManager _gameManager;
    private List<Transform> _spawnPossitions;

    // Start is called before the first frame update
    void Awake()
    {
        _spawnPossitions = new List<Transform>();
        foreach (Transform pos in startingPos)
        {
            _spawnPossitions.Add(pos);
        }
        _gameManager = FindObjectOfType<GameManager>();
        rbController = GetComponent<RigidbodyFirstPersonController>();
        _rb = GetComponent<Rigidbody>();
        currentSpawnPos = _spawnPossitions[0];
        _uiManager = FindObjectOfType<UIManager>();
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
            _gameManager.Finished();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Changers")
        {
            print(Convert.ToInt32(other.gameObject.name.Replace(")", "").Replace("MoveTrigger (", "")));
            currentSpawnPos = _spawnPossitions[Convert.ToInt32(other.gameObject.name.Replace(")", "").Replace("MoveTrigger (", ""))];
        }
    }

    private void ResetScore()
    {
        score = 0;
    }
}









