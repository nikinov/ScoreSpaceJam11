using System;
using UnityEngine;

public class Portal : MonoBehaviour
{
  public Portal linkedPortal;
  public MeshRenderer screen;
  public Camera portalCamera;
  public int recursionLimit = 5;
  public float nearClipOffset = 0.05f;
  public float nearClipLimit = 0.2f;

  private Camera playerCamera;
  private MeshFilter screenMeshFilter;
  private RenderTexture viewTexture;
  private AudioManager _audioManager;
  private void Awake()
  {
    _audioManager = FindObjectOfType<AudioManager>();
    if (!linkedPortal)
    {
      Debug.LogWarning("Portal " + transform.name + " is not linked");
    }
    playerCamera = Camera.main;
    portalCamera.enabled = false;
    screenMeshFilter = screen.GetComponent<MeshFilter>();
    screen.material.SetInt("displayMask", 1);
  }

  public void SetLinkedPortal(Portal portal)
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

      Debug.Log(SideOfPortal(hitObject.transform.position));

      hitObject.position = linkedPortal.transform.position - (SideOfPortal(hitObject.transform.position) * linkedPortal.transform.forward * GetMaxValue(colliderBounds.size));
      //hitRigidbody.velocity = -linkedPortal.transform.forward * initialVelocity;
      _audioManager.PlaySound("Portal");
    }
  }

  private float GetMaxValue(Vector3 vector)
  {
    return Mathf.Max(vector.x, vector.y, vector.z);
  }

  // Manually render the camera attached to this portal
  // Called after PrePortalRender, and before PostPortalRender
  public void Render()
  {
    // Skip rendering the view from this portal if player is not looking at the linked portal
    if (!CameraUtility.VisibleFromCamera(linkedPortal.screen, playerCamera))
    {
      return;
    }

    CreateViewTexture();

    Matrix4x4 localToWorldMatrix = playerCamera.transform.localToWorldMatrix;
    Vector3[] renderPositions = new Vector3[recursionLimit];
    Quaternion[] renderRotations = new Quaternion[recursionLimit];

    int startIndex = 0;
    portalCamera.projectionMatrix = playerCamera.projectionMatrix;
    for (int index = 0; index < recursionLimit; index++)
    {
      if (index > 0)
      {
        // No need for recursive rendering if linked portal is not visible through this portal
        if (!CameraUtility.BoundsOverlap(screenMeshFilter, linkedPortal.screenMeshFilter, portalCamera))
        {
          break;
        }
      }
      localToWorldMatrix = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix * localToWorldMatrix;
      int renderOrderIndex = recursionLimit - index - 1;
      renderPositions[renderOrderIndex] = localToWorldMatrix.GetColumn(3);
      renderRotations[renderOrderIndex] = localToWorldMatrix.rotation;

      portalCamera.transform.SetPositionAndRotation(renderPositions[renderOrderIndex], renderRotations[renderOrderIndex]);
      startIndex = renderOrderIndex;
    }

    // Hide screen so that camera can see through portal
    screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
    linkedPortal.screen.material.SetInt("displayMask", 0);

    for (int index = startIndex; index < recursionLimit; index++)
    {
      portalCamera.transform.SetPositionAndRotation(renderPositions[index], renderRotations[index]);
      SetNearClipPlane();
      portalCamera.Render();

      if (index == startIndex)
      {
        linkedPortal.screen.material.SetInt("displayMask", 1);
      }
    }

    // Unhide objects hidden at start of render
    screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
  }

  // Called once all portals have been rendered, but before the player camera renders
  public void PostPortalRender()
  {
    ProtectScreenFromClipping(playerCamera.transform.position);
  }

  private void CreateViewTexture()
  {
    if (viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height)
    {
      if (viewTexture != null)
      {
        viewTexture.Release();
      }
      viewTexture = new RenderTexture(Screen.width, Screen.height, 0);
      // Render the view from the portal camera to the view texture
      portalCamera.targetTexture = viewTexture;
      // Display the view texture on the screen of the linked portal
      linkedPortal.screen.material.SetTexture("_MainTex", viewTexture);
    }
  }

  // Sets the thickness of the portal screen so as not to clip with camera near plane when player goes through
  private float ProtectScreenFromClipping(Vector3 viewPoint)
  {
    float halfHeight = playerCamera.nearClipPlane * Mathf.Tan(playerCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
    float halfWidth = halfHeight * playerCamera.aspect;
    float dstToNearClipPlaneCorner = new Vector3(halfWidth, halfHeight, playerCamera.nearClipPlane).magnitude;
    float screenThickness = dstToNearClipPlaneCorner;

    Transform screenT = screen.transform;
    bool camFacingSameDirAsPortal = Vector3.Dot(transform.forward, transform.position - viewPoint) > 0;
    screenT.localScale = new Vector3(screenT.localScale.x, screenT.localScale.y, screenThickness);
    screenT.localPosition = Vector3.forward * screenThickness * ((camFacingSameDirAsPortal) ? 0.5f : -0.5f);
    return screenThickness;
  }

  // Use custom projection matrix to align portal camera's near clip plane with the surface of the portal
  // Note that this affects precision of the depth buffer, which can cause issues with effects like screenspace AO
  private void SetNearClipPlane()
  {
    // Learning resource:
    // http://www.terathon.com/lengyel/Lengyel-Oblique.pdf
    Transform clipPlane = transform;
    int dot = Math.Sign(Vector3.Dot(clipPlane.forward, transform.position - portalCamera.transform.position));

    Vector3 camSpacePos = portalCamera.worldToCameraMatrix.MultiplyPoint(clipPlane.position);
    Vector3 camSpaceNormal = portalCamera.worldToCameraMatrix.MultiplyVector(clipPlane.forward) * dot;
    float camSpaceDst = -Vector3.Dot(camSpacePos, camSpaceNormal) + nearClipOffset;

    // Don't use oblique clip plane if very close to portal as it seems this can cause some visual artifacts
    if (Mathf.Abs(camSpaceDst) > nearClipLimit)
    {
      Vector4 clipPlaneCameraSpace = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);

      // Update projection based on new clip plane
      // Calculate matrix with player cam so that player camera settings (fov, etc) are used
      portalCamera.projectionMatrix = playerCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
    }
    else
    {
      portalCamera.projectionMatrix = playerCamera.projectionMatrix;
    }
  }

  /*
   ** Some helper/convenience stuff:
   */

  private int SideOfPortal(Vector3 pos)
  {
    return Math.Sign(Vector3.Dot(pos - transform.position, transform.forward));
  }

  private bool SameSideOfPortal(Vector3 posA, Vector3 posB)
  {
    return SideOfPortal(posA) == SideOfPortal(posB);
  }
}
