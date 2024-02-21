using UnityEngine;

public class PortalTime : MonoBehaviour
{
    private MiniPortal miniPortal;
    private float portalTime = 8f;

    private void Awake()
    {
        miniPortal = FindObjectOfType<MiniPortal>();
    }

    private void Update()
    {
        portalTime -= Time.deltaTime;
        if (portalTime < 1)
        {
            // เล่น Animation
            if (miniPortal.portalAnimator != null)
            {
                miniPortal.portalAnimator.SetTrigger("DestroyPortal");
            }
            Destroy(miniPortal.portalAnimator.gameObject, 0.3f);
        }
    }
}