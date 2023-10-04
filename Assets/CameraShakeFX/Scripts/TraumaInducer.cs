 using System;
 using UnityEngine;
using System.Collections;

/* Example script to apply trauma to the camera or any game object */
public class TraumaInducer : MonoBehaviour 
{
    [Tooltip("Seconds to wait before trigerring the explosion particles and the trauma effect")]
    public float Delay = 1;
    [Tooltip("Maximum stress the effect can inflict upon objects Range([0,1])")]
    public float MaximumStress = 0.6f;
    [Tooltip("Maximum distance in which objects are affected by this TraumaInducer")]
    public float Range = 45;

    private StressReceiver receiver;

    private void Awake()
    {
        receiver = FindObjectOfType<StressReceiver>();
    }

    public void Shake()
    {
        float distance = Vector3.Distance(transform.position, receiver.transform.position);
        /* Apply stress to the object, adjusted for the distance */
        //if(distance > Range) return;
        float distance01 = Mathf.Clamp01(distance / Range);
        float stress = (1 - Mathf.Pow(distance01, 2)) * MaximumStress;
        receiver.InduceStress(stress/3);
    }
    
    public void HardShake()
    {
        float distance = Vector3.Distance(transform.position, receiver.transform.position);
        /* Apply stress to the object, adjusted for the distance */
        //if(distance > Range) return;
        float distance01 = Mathf.Clamp01(distance / Range);
        float stress = (1 - Mathf.Pow(distance01, 2)) * MaximumStress;
        receiver.InduceStress(stress);
    }


}