using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon_Data",order = 2)]
public class WeaponData : ScriptableObject
{
    public float weaponDamage;
    public float attackRange;
    public float magazine;
    public float reload;
    public float cooldown;
    public float weaponWeight;
}
