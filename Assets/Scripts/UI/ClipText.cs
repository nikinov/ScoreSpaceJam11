using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ClipText : MonoBehaviour
{
    [SerializeField] private Transform follow;
    [SerializeField] private Camera cam;
    
    // Update is called once per frame
    void Update()
    {
        Vector3 namePos = cam.WorldToScreenPoint(follow.position);
        this.transform.position = namePos;
    }
}