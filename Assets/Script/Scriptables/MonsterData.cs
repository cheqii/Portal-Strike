using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ScripableObjects/Monster_Data", order = 1)]
public class MonsterData : ScriptableObject
{
    public enum MonsterType
    {
        Melee,
        Range
    }

    public MonsterType monsterType;

    public int hp;
    
    public float moveSpeed;
    
    public int atkDamage;
    
    public int def;
    
    public float atkCoolDown;

    public int expPoints;

    public float attackRange;
}
