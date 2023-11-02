using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage;

    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnCollisionEnter(Collision col)
    {
        switch (col.gameObject.tag)
        {
            case "Enemy":
                GameObject blood = Instantiate(ParticleManager.Instance.data.BloodSplash_particle, transform.position, Quaternion.identity);
                blood.transform.SetParent(col.transform);
                col.gameObject.GetComponent<ITakeDamage>().TakeDamage(player.AtkDamage);
                break;
            case "Totem":
                Debug.Log("Totem");
                GameObject totemBomb = Instantiate(ParticleManager.Instance.data.BloodBomb_particle,
                    transform.position, Quaternion.identity);
                col.gameObject.GetComponent<ITakeDamage>().TakeDamage(player.AtkDamage);
                break;
        }
    }

    public void ChangeDamage(int dmg)
    {
        damage = dmg;
    }
}