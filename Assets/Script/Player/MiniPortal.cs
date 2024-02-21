using UnityEngine;

public class MiniPortal : MonoBehaviour
{
    public Animator portalAnimator;
    public GameObject clonePrefab;
    public MiniPortal portalOut;

    private float moveSpeed;
    private Player player;

    private BuildPortal portalBuild;

    private void Start()
    {
        portalBuild = FindObjectOfType<BuildPortal>();

        player = FindObjectOfType<Player>();

        moveSpeed = player.MoveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (portalOut == null)
        {
            switch (other.transform.tag)
            {
                case "Player":
                    if (portalBuild.MyPortalOut != null)
                    {
                        Instantiate(clonePrefab, player.transform.position + new Vector3(5f, 0f, 5f), Quaternion.identity);
                        Instantiate(clonePrefab, player.transform.position + new Vector3(-5f, 0f, 5f), Quaternion.identity);
                        // เล่น Animation
                        if (portalAnimator != null)
                        {
                            portalAnimator.SetTrigger("DestroyPortal");
                        }
                        Destroy(portalBuild.MyPortalOut.gameObject, 0.3f);
                    }
                    break;
            }
            return;
        }

        switch (other.transform.tag)
        {
            case "MonsterBullet":
                Destroy(other.gameObject);
                if (portalAnimator != null)
                {
                    portalAnimator.SetTrigger("DestroyPortal");
                }
                Destroy(portalBuild.MyPortalIn.gameObject, 0.3f);
                break;
            case "Player":
                PlayerDash();
                if (portalAnimator != null)
                {
                    portalAnimator.SetTrigger("DestroyPortal");
                }
                Destroy(portalBuild.MyPortalIn.gameObject, 0.3f);
                break;
        }
    }

    private void PlayerDash()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        FindObjectOfType<PlayerAnimation>().SetAnim("Hurt");
        Debug.Log("Dash");
        player.MoveSpeed *= 4;
        GameObject Dash_Effect = Instantiate(ParticleManager.Instance.data.Dash_particle, player.transform.position,
            player.transform.rotation);
        Dash_Effect.transform.SetParent(player.gameObject.transform);

        Destroy(Dash_Effect, 0.5f);

        Invoke("ResetSpeed", 0.15f);
    }

    void ResetSpeed()
    {
        player.MoveSpeed = moveSpeed;
    }
}