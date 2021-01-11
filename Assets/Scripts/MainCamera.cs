using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
  public Transform portalContainer;

  private List<Portal> portals;

  private void Awake()
  {
    portals = new List<Portal>();
    foreach (Transform portal in portalContainer)
    {
      portals.Add(portal.GetComponent<Portal>());
    }
  }

  private void OnPreCull()
  {
    foreach (Portal portal in portals)
    {
      portal.Render();
    }
    foreach (Portal portal in portals)
    {
      portal.PostPortalRender();
    }
  }
}
