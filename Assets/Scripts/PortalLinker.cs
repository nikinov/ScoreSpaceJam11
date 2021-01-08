using UnityEngine;

public class PortalLinker : MonoBehaviour
{
  public Transform linkedPortal;

  private void Awake()
  {
    if (!linkedPortal)
    {
      Debug.LogWarning("Portal " + transform.name + " is not linked");
    }
  }

  public void SetLinkedPortal(Transform portal)
  {
    linkedPortal = portal;
  }

  private void OnTriggerEnter(Collider other)
  {
    if (!linkedPortal)
    {
      Debug.LogError("Portal " + transform.name + " is not linked");
    }
    else
    {
      Transform hitObject = other.transform;
      float initialVelocity = hitObject.GetComponent<Rigidbody>().velocity.magnitude;
      hitObject.position = linkedPortal.position + (linkedPortal.up * 2.0f);
      hitObject.GetComponent<Rigidbody>().velocity = linkedPortal.up * initialVelocity;
    }
  }
}
