using UnityEngine;
public class ClearPortal : MonoBehaviour
{
    [SerializeField] private GameObject clearGamePanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Clear"))
        {
            clearGamePanel.SetActive(true);
        }
    }
}