using UnityEngine;

public class BouncePad : MonoBehaviour
{
  public float bounceStrength = 100.0f;

  private void OnTriggerEnter(Collider other)
  {
    Rigidbody hitRigidbody = other.transform.GetComponent<Rigidbody>();
    hitRigidbody.AddForce(transform.up * hitRigidbody.velocity.magnitude * bounceStrength);
  }
}
