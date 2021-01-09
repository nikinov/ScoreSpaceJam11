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

    [SerializeField] private Transform startingPos;
    
    private RigidbodyFirstPersonController _rbController;
    private Rigidbody _rb;
    private UIManager _uiManager;

    // Start is called before the first frame update
    void Awake()
    {
        _rbController = GetComponent<RigidbodyFirstPersonController>();
        _rb = GetComponent<Rigidbody>();
        currentSpawnPos = startingPos;
        _uiManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Death()
    {
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        _rbController.EnableMovement = false;
    }

    public void Spawn()
    {
        transform.position = currentSpawnPos.position;
        _rbController.EnableMovement = true;
        _rb.constraints = RigidbodyConstraints.None;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Collectable")
        {
            score += 1;
            Destroy(other.collider.gameObject);
            _uiManager.ChangeScore(score);
        }
        else if(other.gameObject.tag == "Obstacles")
        {
            other.collider.GetComponent<Obstacle>().Kill(this);
        }
    }

    private void ResetScore()
    {
        score = 0;
    }
}









