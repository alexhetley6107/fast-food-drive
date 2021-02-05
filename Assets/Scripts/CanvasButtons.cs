using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour {
    
    public Sprite btn, btnPressed, musicOn, musicOff, pauseOn, pauseOff;
    private Image image;
    public GameObject homeBtn, musBtn;

    void Start()
    {
        image = GetComponent<Image>();
        if(gameObject.name == "Music Button")
        {
            if (PlayerPrefs.GetString("music") == "No")
                transform.GetChild(0).GetComponent<Image>().sprite = musicOff;
        }
    }
    public void MusicButton()
    {
        if(PlayerPrefs.GetString("music") == "No")
        {
            PlayerPrefs.SetString("music", "Yes");
            transform.GetChild(0).GetComponent<Image>().sprite = musicOn;
        }
        else
        {
            PlayerPrefs.SetString("music", "No");
            transform.GetChild(0).GetComponent<Image>().sprite = musicOff;
        }
        PlayButtonSound();
    }
    public void Shop()
    {
        StartCoroutine(LoadScene("Shop"));
        PlayButtonSound();
    }
    public void ExitShop()
    {
        StartCoroutine(LoadScene("Main"));
        PlayButtonSound();
    }
    public void PlayGame()
    {
        if (PlayerPrefs.GetString("First Game") == "No")
            StartCoroutine(LoadScene("Game"));
        else  
            StartCoroutine(LoadScene("Study"));
        PlayButtonSound();
    }
    public void RestartGame()
    {
       StartCoroutine(LoadScene("Game"));
        PlayButtonSound();
    }
    public void SetPressedButton()
    {
        image.sprite = btnPressed;
        transform.GetChild(0).localScale += new Vector3(0.2f, 0.2f, 0);
    }    
    public void SetDefaultButton()
    {
        image.sprite = btn;
        transform.GetChild(0).localScale -= new Vector3(0.2f, 0.2f, 0);
    }
    IEnumerator LoadScene(string name)
    {
        float fadeTime = Camera.main.GetComponent<Fading>().Fade(1f);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(name);
    }
    private void PlayButtonSound()
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();
    }
    public void PauseBtn()
    {
        if (PlayerPrefs.GetString("pause") == "No")
        {
            PlayerPrefs.SetString("pause", "Yes");
            transform.GetChild(0).GetComponent<Image>().sprite = pauseOn;
            Time.timeScale = 0f;
            homeBtn.SetActive(true);
            musBtn.SetActive(true);

        }
        else
        {
            PlayerPrefs.SetString("pause", "No");
            transform.GetChild(0).GetComponent<Image>().sprite = pauseOff;
            Time.timeScale = 1f;
            homeBtn.SetActive(false);
            musBtn.SetActive(false);
        }
    }
    public void LearnBtn()
    {
        StartCoroutine(LoadScene("Study"));
        PlayButtonSound();
    }
    public void HomeBtn()
    {
        StartCoroutine(LoadScene("Main"));
        Time.timeScale = 1f;
        PlayButtonSound();
    }

}
