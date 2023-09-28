using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonster : MonoBehaviour
{
    [SerializeField] private GameObject level_holder;
    [SerializeField] private List<GameObject> level;
    [SerializeField] private List<GameObject> _floor;

    [SerializeField] private GameObject enemy;
    
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

        //doing something with floor
        foreach (var f in _floor)
        {
           
            Instantiate(enemy, f.transform.position,Quaternion.identity);
         
           
        }
    }

}