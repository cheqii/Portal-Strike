using UnityEngine;

[CreateAssetMenu(fileName = "Particle_Data", menuName = "ScriptableObjects/Particle_Data", order = 1)]
public class ParticleData : ScriptableObject
{
    public GameObject Xp_particle;
    public GameObject LevelUP_particle;
    public GameObject BloodSplash_particle;
    public GameObject BloodBomb_particle;
    
    public GameObject Dash_particle;
}