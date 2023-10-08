using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPortal : MonoBehaviour
{
    [SerializeField] private GameObject portal_in;
    [SerializeField] private GameObject portal_out;
    private Player player;

    private MiniPortal lastedPortal;
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(2))
        {

            var portal =Instantiate(portal_in, player.transform.position + player.transform.forward * 2, Quaternion.identity);
            portal.transform.eulerAngles = new Vector3(0,player.transform.rotation.eulerAngles.y + 90,90);

            lastedPortal = portal.GetComponent<MiniPortal>();
        }
        
        if(Input.GetMouseButtonDown(1))
        {
            var portalout =Instantiate(portal_out, player.transform.position + player.transform.forward * 2, Quaternion.identity);
            portalout.transform.eulerAngles = new Vector3(0,player.transform.rotation.eulerAngles.y + 90,90);
            lastedPortal.portalOut = portalout.GetComponent<MiniPortal>();
        }

    }
}
