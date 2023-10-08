using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPortal : MonoBehaviour
{
    [SerializeField] private GameObject portal_in;
    [SerializeField] private GameObject portal_out;
    private Player player;

    private MiniPortal myPortalIn;
    public MiniPortal MyPortalIn
    {
        get => myPortalIn;
        set => myPortalIn = value;
    }
    
    private MiniPortal myPortalOut;

    public MiniPortal MyPortalOut
    {
        get => myPortalOut;
        set => myPortalOut = value;
    }

    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void CreatePortal_In()
    {
        if (myPortalIn != null)
        {
            return;
        }
        
        var portal =Instantiate(portal_in, player.transform.position + player.transform.forward * 2, Quaternion.identity);
        portal.transform.eulerAngles = new Vector3(0,player.transform.rotation.eulerAngles.y + 90,90);

        myPortalIn = portal.GetComponent<MiniPortal>();
        if (myPortalOut == null)
        {
            if(myPortalOut != null) return;
            Debug.Log("portal in");
            myPortalIn.portalOut = portal_out.GetComponent<MiniPortal>();
        }
        MyPortalIn = portal.GetComponent<MiniPortal>();
        
        if (MyPortalOut != null)
        {
            MyPortalIn.portalOut = MyPortalOut.GetComponent<MiniPortal>();
        }
    }

    public void CreatePortal_Out()
    {
        if (myPortalOut != null)
        {
            return;
        }
        
        var portalout =Instantiate(portal_out, player.transform.position + player.transform.forward * 2, Quaternion.identity);
        portalout.transform.eulerAngles = new Vector3(0,player.transform.rotation.eulerAngles.y + 90,90);
        myPortalOut = portalout.GetComponent<MiniPortal>();
        
        if (myPortalIn != null)
        {
            myPortalIn.portalOut = portalout.GetComponent<MiniPortal>();
        }
    }
}
