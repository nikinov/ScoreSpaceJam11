using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(RigidbodyFirstPersonController))]
public class Player : MonoBehaviour
{
    private RigidbodyFirstPersonController _rbController;
    private Rigidbody _rb;
    // Start is called before the first frame update
    void Awake()
    {
        _rbController = GetComponent<RigidbodyFirstPersonController>();
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Deth()
    {
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        _rbController.EnableMovement = false;
    }

    public void Spawn()
    {
        _rbController.EnableMovement = true;
        _rb.constraints = RigidbodyConstraints.None;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
