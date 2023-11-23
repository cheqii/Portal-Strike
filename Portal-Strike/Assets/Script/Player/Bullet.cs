using UnityEngine;

public class Bullet : MonoBehaviour
{
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

                int calculatedDamage = CalculateDamage(player.AtkDamage, player.CritRate, player.CritDamage);
                col.gameObject.GetComponent<ITakeDamage>().TakeDamage(calculatedDamage);
                break;
            case "Totem":
                Debug.Log("Totem");
                GameObject explosion = Instantiate(ParticleManager.Instance.data.Explosion, transform.position, Quaternion.identity);

                int calculatedDamageTotem = CalculateDamage(player.AtkDamage, player.CritRate, player.CritDamage);
                col.gameObject.GetComponent<ITakeDamage>().TakeDamage(calculatedDamageTotem);
                break;
        }
    }

    private int CalculateDamage(int baseDamage, float critRate, float critDamage)
    {
        float randomValue = Random.value * 100.0f;

        if (randomValue <= critRate)
        {
            return Mathf.RoundToInt(baseDamage + critDamage);
        }
        else
        {
            return baseDamage;
        }
    }
}