using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour
{
    public void RestartGames()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGitXub()
    {
        Application.OpenURL("https://github.com/xd-zede32x/My-seventh-gam");
    }
}