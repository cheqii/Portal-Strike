using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        //initial level
        //FindObjectOfType<LevelGenerator>().Generate( 8, 0.5f, 2.0f );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryAgain()
    {
        SceneManager.LoadScene("Again");
    }

    public void Escape()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
