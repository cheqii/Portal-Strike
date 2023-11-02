using UnityEngine;

public class Rotate : MonoBehaviour {

    // The material shaders in this card must use this number
    [Tooltip("Material Shader Mask Number")]
    public int maskNumber = 1;

    [Tooltip("Horizontal Rotation Speed")]
    [Range(-1, 1)]
    public float rotationSpeedH = 0.7f;

    [Tooltip("Horizontal Rotation Speed")]
    [Range(-1, 1)]
    public float rotationSpeedV = 0.4f;

    [Tooltip("Maximum Horizontal Angle")]
    [Range(0, 60)]
    public float angleH = 20;

    [Tooltip("Maximum Vertical Angle")]
    [Range(0, 60)]
    public float angleV = 8;

    private float rotationCounter = 0;
    private Transform windowTransform;
    private Transform worldTransform;

    private void Awake() {
        windowTransform = transform.GetChild(1);
        worldTransform = transform.GetChild(2);

        SetStencilMask(maskNumber);
    }

    void Update () {
        rotationCounter += Time.deltaTime;
        transform.eulerAngles = new Vector3(Mathf.Sin(rotationCounter * rotationSpeedV) * angleV, Mathf.Sin(rotationCounter * rotationSpeedH) * angleH, 0);
    }

    // Set the mask numbers of the shaders, so windows will fit to the card world objects
    public void SetStencilMask(int maskNumber) {
        this.maskNumber = maskNumber;

        windowTransform.GetComponent<Renderer>().material.SetFloat("_StencilMask", maskNumber);
        foreach (Transform worldObject in worldTransform.GetComponentInChildren<Transform>()) {
            foreach (Material material in worldObject.GetComponent<Renderer>().materials) {
                material.SetFloat("_StencilMask", maskNumber);
            }
        }
    }
}
