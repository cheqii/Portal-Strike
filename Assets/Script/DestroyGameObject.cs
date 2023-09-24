using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    [SerializeField] private float objectDuration = 2.0f;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        // {objectDuration} is counted immediately after {this.gameObject} appears[Awake].
        Destroy(gameObject, objectDuration);
    }

    private void Update()
    {
        /* Checks if {this.gameObject} is outside the {mainCamera} view
         * by testing if {this.gameObject} collider is not within the camera's frustum. */

        /* A camera frustum is the 3D shape that defines what a camera can see.
         * This helps optimize rendering by excluding objects outside the frustum. */
        if (mainCamera && !GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(mainCamera), GetComponent<Renderer>().bounds))
        {
            Destroy(gameObject);
        }
    }
}