using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float score;
    private int score_target;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score : " + score;
    }

    // Update is called once per frame
    void Update()
    {
        score = Mathf.Lerp(score, score_target, 0.1f);
        scoreText.text = "Score : " + (int) score;
    }

    public void IncreaseScore()
    {
        score_target += Random.Range(10, 50);
        
    }
}
