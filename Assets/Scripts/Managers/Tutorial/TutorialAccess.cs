using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialAccess : MonoBehaviour
{
    public string Tutorial = "TutorialScene";
    public string Game = "GameScene";

    public void GoToTutorial()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Tutorial);
    }

    public void ReturnToGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Game);
    }
}
