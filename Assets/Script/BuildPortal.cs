using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPortal : MonoBehaviour
{
    [SerializeField] private GameObject portal_in;
    [SerializeField] private GameObject portal_out;
    private Player player;

    private MiniPortal MyPortalIn;
    private MiniPortal MyPortalOut;

    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void CreatePortal_In()
    {
        if (MyPortalIn != null)
        {
            return;
        }
        
        var portal =Instantiate(portal_in, player.transform.position + player.transform.forward * 2, Quaternion.identity);
        portal.transform.eulerAngles = new Vector3(0,player.transform.rotation.eulerAngles.y + 90,90);

        MyPortalIn = portal.GetComponent<MiniPortal>();
    }

    public void CreatePortal_Out()
    {
        if (MyPortalOut != null)
        {
            return;
        }
        
        var portalout =Instantiate(portal_out, player.transform.position + player.transform.forward * 2, Quaternion.identity);
        portalout.transform.eulerAngles = new Vector3(0,player.transform.rotation.eulerAngles.y + 90,90);
        MyPortalOut = portalout.GetComponent<MiniPortal>();

        if (MyPortalIn != null)
        {
            MyPortalIn.portalOut = portalout.GetComponent<MiniPortal>();
        }
    }
}
