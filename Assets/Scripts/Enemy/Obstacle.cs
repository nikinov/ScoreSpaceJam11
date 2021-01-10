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

    public void Kill(Player player)
    {
        if(RespawnPlace != transform)
            player.currentSpawnPos = RespawnPlace;
        player.Spawn();
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
