using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance { get; private set; }
    public float maxHp = 135.0f;
    public float currentHp;
    [SerializeField] private int defense = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentHp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        currentHp = Mathf.Clamp(currentHp - (damage - defense), 0, maxHp);
    }
}