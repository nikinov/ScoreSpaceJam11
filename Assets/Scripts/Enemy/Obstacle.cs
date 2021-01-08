using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Transform RespawnPlace;

    private void Awake()
    {
        if (RespawnPlace == null)
        {
            RespawnPlace = transform;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        print("sss");
        if (other.collider.gameObject.tag == "Player")
        {
            Kill(other.collider.GetComponent<Player>());
        }
    }

    public void Kill(Player player)
    {
        if(RespawnPlace != transform)
            player.currentSpawnPos = RespawnPlace;
        player.Spawn();
    }
}
