using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour
{
    [SerializeField] private Sprite _musicOn, _musicOf;

    private void Start()
    {
        if (PlayerPrefs.GetString("music") == "No" && gameObject.name == "SoundButtons")
        {
            GetComponent<Image>().sprite = _musicOf;
        }
    }

    public void RestartGames()
    {
        if (PlayerPrefs.GetString("music") != "No")
        {
            GetComponent<AudioSource>().Play();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadGitXub()
    {
        if (PlayerPrefs.GetString("music") != "No")
        {
            GetComponent<AudioSource>().Play();
        }

        Application.OpenURL("https://github.com/xd-zede32x/My-seventh-gam");
    }

    public void OpenShop()
    {
        if (PlayerPrefs.GetString("music") != "No")
        {
            GetComponent<AudioSource>().Play();
        }

        SceneManager.LoadScene("Shop");
    }

    public void LoseShop()
    {
        if (PlayerPrefs.GetString("music") != "No")
        {
            GetComponent<AudioSource>().Play();
        }

        SceneManager.LoadScene("Arcada");
    }

    public void MusicWork()
    {
        if (PlayerPrefs.GetString("music") == "No")
        {
            GetComponent<AudioSource>().Play();
            PlayerPrefs.SetString("music", "Yes");
            GetComponent<Image>().sprite = _musicOn;
        }

        else
        {
            PlayerPrefs.SetString("music", "No");
            GetComponent<Image>().sprite = _musicOf;
        }
    }
}