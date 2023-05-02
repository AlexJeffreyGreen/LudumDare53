using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Slider slider;
    private GameMode mode;

    private void Start()
    {
        //if (SceneManager.GetActiveScene().buildIndex == 0)
        //    return;
        //if (GameManager.Instance != null)
        //{
        //    mode = GameManager.Instance.GetGameMode();
        //    UpdateGameOverText();
        //    Destroy(GameManager.Instance.gameObject);
        //}
    }

    private void Update()
    {

    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void UpdateVolumeSlider(float value)
    {
        Debug.Log("Value of volumne: " +  value);
    }


    public void Retry()
    {
        if (GameManager.Instance != null)
            Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene(0);
    }

    public void ChangeVolume()
    {
        AudioManager.Instance.Volumne = this.slider.value;
    }
}
