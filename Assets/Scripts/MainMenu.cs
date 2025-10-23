using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Continue()
    {
        Debug.LogError("Pendiente de hacer!");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
