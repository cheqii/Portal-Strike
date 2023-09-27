using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ScripableObjects/Monster_Data", order = 1)]
public class MonsterData : ScriptableObject
{
    public int hp;
    
    public float moveSpeed;
    
    public int atkDamage;
    
    public int def;
    
    public float atkCoolDown;
    
    public float atkRange;

    public int expPoints;
}
