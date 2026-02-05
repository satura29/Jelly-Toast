using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    public static LevelManager Larry;

    // scene info
    public string levelName;
    public string nextLevel;

    void Awake()
    {
        Larry = this; // define the singleton
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (levelName == "BOSS: Dripping Dritten")
        {
            GameManager.Gary.BossStarted();
        } else if (levelName == "3: Factory Wastelines")
        {
            GameManager.Gary.FactoryStarted();
        } else
        {
            GameManager.Gary.LevelStarted();
        }
    }

    public void GoToNextLevel()
    {
        // load the next level
        SceneManager.LoadScene(nextLevel);
    }

    public void ReloadLevel()
    {
        // get the scene we are currently in, and reload it. 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void btn_GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void OnDestroy()
    {
        if (Larry == this)
        {
            Larry = null; // clear the static reference
        }
    }

}