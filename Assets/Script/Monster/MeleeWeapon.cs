using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private MonsterData monData;
    private void OnTriggerEnter(Collider other)
    {
        var target = FindObjectOfType<Player>();
        if (monData.monsterType == MonsterData.MonsterType.Melee)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player take damage from melee");
                target.TakeDamage(monData.atkDamage);
            }
        }
    }
}
