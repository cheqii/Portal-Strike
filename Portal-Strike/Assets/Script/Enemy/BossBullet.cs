using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [SerializeField] private int damage;
    public Transform target;

    

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, 20 * Time.deltaTime);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<ITakeDamage>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
