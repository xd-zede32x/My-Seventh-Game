using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour
{
    public void RestartGames()
    {
        SceneManager.LoadScene(0);
    }
}