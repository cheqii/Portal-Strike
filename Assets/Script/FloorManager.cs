using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FloorManager : MonoBehaviour
{
    [SerializeField] private GameObject level_holder;
    [SerializeField] private List<GameObject> level;
    [SerializeField] private List<GameObject> _floor;

    [SerializeField] private GameObject enemy;
    
    [SerializeField] private GameObject Portal;


    // Start is called before the first frame update
    void Start()
    {
        //get level into lists
        for (int i = 0; i < level_holder.transform.childCount; i++)
        {
            level.Add(level_holder.transform.GetChild(i).gameObject);
        }

        
        //get floor into lists
        foreach (var i in level)
        {
            _floor.Add(i.transform.GetChild(0).gameObject);
        }


        GameObject enemies = new GameObject("Enemies");
        
        //doing something with floor
        foreach (var f in _floor)
        {
            //build navmesh surface
            f.gameObject.AddComponent<NavMeshSurface>();
            f.GetComponent<NavMeshSurface>().BuildNavMesh();

            //monster spawn
            if (f.transform.name != "Floor0")
            {
                for (int i = 0; i < 5; i++)
                {
                    enemy = Instantiate(enemy, f.transform.position + new Vector3(Random.Range(-5,5),0,Random.Range(-5,5))
                        
                        , Quaternion.identity);
                    enemy.GetComponent<MonsterDetectPlayer>().SetSpawnPoint(f.transform.position);
                    enemy.transform.name = "enemy | " + f.transform.name;
                
                    enemy.transform.SetParent(enemies.transform);
                }
            }
        }

        //generate portal
        Instantiate(Portal, _floor[_floor.Count - 1].transform.position, Quaternion.identity);
        
    }

}
