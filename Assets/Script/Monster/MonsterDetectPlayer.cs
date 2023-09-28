using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MonsterDetectPlayer : MonoBehaviour
{
    #region -Declared Variables-

    [Header("Monster Data")] 
    [SerializeField] private MonsterData monData;
    
    private NavMeshAgent monster;
    private Player target;
    
    [Header("Distance between player & monster")]
    private float awayFromPlayer;
    private Vector2 beginPos;
    private Vector2 targetPos;
    
    [SerializeField] private Transform centrePoint; // centre point (floor) transform for find enemy random point to patrol in the room

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // get component from player
        target = FindObjectOfType<Player>().GetComponent<Player>();
        // get component NavMeshAgent from game object
        monster = GetComponent<NavMeshAgent>();
        // assign monster speed from monster data
        monster.speed = monData.moveSpeed;
        // assign monster stop distance in NavMeshAgent from monster data
        monster.stoppingDistance = monData.stopDistance;
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPlayer();
    }

    void MoveToPlayer()
    {
        if (target == null) return;
        
        if (monster.remainingDistance <= monster.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, monData.viewRange, out point)) // pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                monster.SetDestination(point);
            }
        }
        
        CalculateTargetDistance();
        // if distance between player and monster is lower than stop following var then monster will follow the player
        if (awayFromPlayer <= monData.stopFollow) monster.SetDestination(target.transform.position);
        
        Ray ray = new Ray(transform.position, transform.forward * monData.attackRange);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawRay(transform.position, transform.forward * monData.attackRange, Color.cyan);
            if(hit.collider.gameObject.name != "Player") return; // if raycast hit something that's not player then return
            if (monData.attackRange <= awayFromPlayer) return; // if monster attack range >= distance that monster away from player then monster can attack player
            switch (monData.monsterType)
            {
                case MonsterData.MonsterType.Melee:
                    Debug.Log(hit.collider.gameObject.name);
                    Debug.Log("Melee Attack Player!");
                    break;
                case MonsterData.MonsterType.Range:
                    Debug.Log(hit.collider.gameObject.name);
                    Debug.Log("Range Attack Player!");
                    break;
            }
        }
    }

    void CalculateTargetDistance()
    {
        beginPos = transform.position;
        targetPos = target.transform.position;
        
        // Calculate distance between monster and player
        awayFromPlayer = Mathf.Sqrt(Mathf.Pow(targetPos.x - beginPos.x, 2) + Mathf.Pow(targetPos.y - beginPos.y, 2));
        Debug.Log(awayFromPlayer);
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
}
