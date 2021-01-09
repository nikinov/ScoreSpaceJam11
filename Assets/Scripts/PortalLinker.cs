using UnityEngine;

public class PortalLinker : MonoBehaviour
{
  public Transform linkedPortal;
  public Transform test;

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
      //test.position = linkedPortal.position + (linkedPortal.up * 2.0f);
      //test.rotation = Quaternion.LookRotation(linkedPortal.up - transform.up - hitObject.forward, linkedPortal.up);
      //Debug.Log(Vector3.Angle(transform.up, hitObject.forward));
      //Debug.Log(Vector3.Angle(transform.up, linkedPortal.up));
    }
  }

  private float GetMaxValue(Vector3 vector)
  {
    return Mathf.Max(vector.x, vector.y, vector.z);
  }
}
