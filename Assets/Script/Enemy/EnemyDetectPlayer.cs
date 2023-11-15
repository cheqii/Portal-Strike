using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyDetectPlayer : MonoBehaviour
{
    #region -Declared Variables-

    [Header("Monster Data")] 
    [SerializeField] private MonsterData monData;

    [Header("Animator")] 
    private new EnemyAnimations animation;
    
    private NavMeshAgent monsterNavmesh;
    private Enemy enemy;
    private Player target;
    
    [Header("Distance between player & monster")]
    private float awayFromPlayer;
    private Vector3 beginPos;
    private Vector3 targetPos;
    
    // centre point (floor) transform for find enemy random point to patrol in the room
    private Vector3 SpawnePoint;
    
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // get component enemy script from this object
        enemy = GetComponent<Enemy>();
        // get component from player
        target = FindObjectOfType<Player>();
        // get component NavMeshAgent from game object
        monsterNavmesh = GetComponent<NavMeshAgent>();
        // assign monster speed from monster data
        monsterNavmesh.speed = monData.moveSpeed;
        // assign monster stop distance in NavMeshAgent from monster data
        monsterNavmesh.stoppingDistance = monData.stopDistance;
        // get component enemy animations
        animation = GetComponent<EnemyAnimations>();
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * monData.range, Color.cyan);
        MoveToPlayer();
    }

    void MoveToPlayer()
    {
        if (target == null) return;
        
        CalculateTargetDistance();
        if (monsterNavmesh.remainingDistance <= monsterNavmesh.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(SpawnePoint, monData.viewRange, out point) 
                && awayFromPlayer >= monData.stopFollow) // pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                monsterNavmesh.speed = monData.moveSpeed;
                animation.TriggerWalkAnim();
                monsterNavmesh.SetDestination(point);
            }
            
            // if distance between player and monster is lower than stop following var then monster will follow the player
            if (awayFromPlayer <= monData.stopFollow)
            {
                animation.BlendTree();
                if (monData.monsterType == MonsterData.MonsterType.Range) monsterNavmesh.speed *= 2;
                monsterNavmesh.SetDestination(target.transform.position);
            }

            // if distance between player and monster is lower or equal stop distance then enemy will trigger idle animation
            // if (awayFromPlayer <= monsterNavmesh.stoppingDistance)
            //     animation.TriggerIdleAnim();

            Ray ray = new Ray(transform.position, transform.forward * monData.range);

            // if (monData.attackRange >= awayFromPlayer) return; // if monster attack range >= distance that monster away from player then monster can attack player
            switch (monData.monsterType)
            {
                case MonsterData.MonsterType.Melee:
                    if (awayFromPlayer <= monData.attackRange)
                    {
                        // Debug.Log("melee attack");
                        enemy.MeleeAttack();
                    }
                    // if (Physics.Raycast(ray, out hit))
                    // {
                    //     if(hit.collider.gameObject.tag != "Player") return; // if raycast hit something that's not player then return
                    //     enemy.MeleeAttack();
                    // }
                    break;
                case MonsterData.MonsterType.Range:
                    if (awayFromPlayer <= monData.attackRange)
                    {
                        Debug.Log("range attack");
                        animation.TriggerAttackAnim();
                    }
                    break;
            }
        }
    }

    void CalculateTargetDistance()
    {
        beginPos = transform.position;
        targetPos = target.transform.position;
        
        // Calculate distance between monster and player
        awayFromPlayer = Mathf.Sqrt(Mathf.Pow(targetPos.x - beginPos.x, 2) + Mathf.Pow(targetPos.z - beginPos.z, 2));
         // Debug.Log(awayFromPlayer);
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; // random point in a sphere
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    public void SetSpawnPoint(Vector3 target)
    {
        SpawnePoint = target;
    }
    
}
