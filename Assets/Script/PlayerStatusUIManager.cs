using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatusUIManager : MonoBehaviour
{
    [SerializeField] private Image currentHpBar;
    [SerializeField] private TMP_Text hp;
    [SerializeField] private TMP_Text currentHpIndex;
    private float lerpSpeed;

    private void Update()
    {
        lerpSpeed = 3.0f * Time.deltaTime;

        UpdateUI();
        ColorChanger();
    }

    private void UpdateUI()
    {
        currentHpBar.fillAmount = Mathf.Lerp(currentHpBar.fillAmount, PlayerStatus.Instance.currentHp / PlayerStatus.Instance.maxHp, lerpSpeed);
        currentHpIndex.text = $"{PlayerStatus.Instance.currentHp}/{PlayerStatus.Instance.maxHp}";
    }

    private void ColorChanger()
    {
        /* Create new color
         * color8CFF41 is green
         * colorFF4040 is red */
        Color color8CFF41 = new (140f / 255f, 1f, 65f / 255f);
        Color colorFF4040 = new (1f, 64f / 255f, 64f / 255f);

        Color healthColor = Color.Lerp(colorFF4040, color8CFF41, (PlayerStatus.Instance.currentHp / PlayerStatus.Instance.maxHp));
        currentHpBar.color = healthColor;
        hp.color = healthColor;
    }
}