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
    Debug.Log("Portal");
    if (!linkedPortal)
    {
      Debug.LogError("Portal " + transform.name + " is not linked");
    }
    else
    {
      Transform hitObject = other.transform;

      Rigidbody hitRigidbody = hitObject.GetComponent<Rigidbody>();
      float initialVelocity = hitRigidbody.velocity.magnitude;
      Bounds colliderBounds = hitObject.GetComponent<Collider>().bounds;
      hitObject.position = linkedPortal.position + (linkedPortal.up * GetMaxValue(colliderBounds.size));
      hitRigidbody.velocity = linkedPortal.up * initialVelocity;
    }
  }

  private float GetMaxValue(Vector3 vector)
  {
    return Mathf.Max(vector.x, vector.y, vector.z);
  }
}
