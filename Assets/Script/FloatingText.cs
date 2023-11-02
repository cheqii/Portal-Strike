using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private float destroyTime;

    private Vector3 randomIntensity = new Vector3(0.5f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
        transform.localPosition += new Vector3(0f, 2f, 0f);
        transform.localPosition += new Vector3(Random.Range(randomIntensity.x, randomIntensity.x), 
            Random.Range(randomIntensity.y, randomIntensity.y),
            Random.Range(randomIntensity.z, randomIntensity.z));
    }
}