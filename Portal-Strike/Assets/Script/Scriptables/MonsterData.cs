using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObjects/Monster_Data", order = 1)]
public class MonsterData : ScriptableObject
{
    public enum MonsterType
    {
        Melee,
        Range
    }

    [Header("Monster Prefab")] 
    public GameObject monsterPrefab;
    public GameObject bullet;

    [Header("Monster Stat")]
    public MonsterType monsterType;

    public string monName;
    
    public int hp;
    
    public float moveSpeed;
    
    public int atkDamage;

    public float atkSpeed;
    
    public int def;
    
    public float atkCoolDown;
    
    public int expPoints;
    
    public float attackRange;

    public float range;

    [Header("NavMeshAgent Data")]
    public float stopFollow; // to stop following player if player away than stopFollow var
    
    public float stopDistance; // stop distance in Navmesh

    public float viewRange; // range for enemy patrolling in the room
}
