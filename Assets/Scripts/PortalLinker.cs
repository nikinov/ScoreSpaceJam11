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
    if (!linkedPortal)
    {
      Debug.LogError("Portal " + transform.name + " is not linked");
    }
    else
    {
      Transform hitObject = other.transform;
      float initialVelocity = hitObject.GetComponent<Rigidbody>().velocity.magnitude;
      Bounds colliderBounds = hitObject.GetComponent<Collider>().bounds;
      Debug.Log("Collider Center : " + colliderBounds.center);
      Debug.Log("Collider Size : " + colliderBounds.size);
      Debug.Log("Collider Bound Minimum : " + colliderBounds.max);
      Debug.Log("Collider Bound Maximum : " + colliderBounds.min);
      hitObject.position = linkedPortal.position + (linkedPortal.up * 2.0f);
      hitObject.GetComponent<Rigidbody>().velocity = linkedPortal.up * initialVelocity;
      Debug.Log(linkedPortal.up + " " + linkedPortal.up.magnitude);
      Debug.Log(transform.up + " " + transform.up.magnitude);
      Debug.Log(Mathf.Acos(Vector3.Dot(linkedPortal.up, transform.up)) * Mathf.Rad2Deg);
      Debug.Log(transform.up - hitObject.rotation.eulerAngles.normalized);
      test.position = linkedPortal.position + (linkedPortal.up * 2.0f);
      test.rotation = Quaternion.LookRotation((transform.up - hitObject.rotation.eulerAngles.normalized), linkedPortal.up);
    }
  }
}
