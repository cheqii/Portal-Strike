using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Xp : MonoBehaviour
{
    private Transform target; // The target to follow (usually the player)
    [Range(0,10)]
    public float smoothSpeed = 5.0f;
    private Vector3 offset;

    [Range(10,100)]
    [SerializeField] private int xpMin;
    [Range(10,100)]
    [SerializeField] private int xpMax;

    [SerializeField] private Nf_GameEvent xp_event;

    private void Start()
    {
        target = FindObjectOfType<Player>().transform;
        offset = Vector3.zero;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            target = FindObjectOfType<Player>().transform;
        }
        // Calculate the desired camera position
        Vector3 targetPosition = target.position + offset;

        // Keep the y-axis position constant
        targetPosition.y = transform.position.y;

        // Smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            GameObject xp_glow = Instantiate(ParticleManager.Instance.data.LevelUP_particle,transform.position,Quaternion.identity);
            xp_glow.transform.SetParent(col.transform);
            
            xp_event.Raise(this,Random.Range(xpMin,xpMax));

            Destroy(this.gameObject);
        }
    }
}