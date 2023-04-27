using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject normalMenu;

    //Volumes
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    private void Awake()
    {
        settingsMenu.SetActive(false);

        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
    }

    public void BTN_OpenSettings()
    {
        if(normalMenu != null) normalMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    public void BTN_CloseSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);

        if (normalMenu != null) normalMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }
    public void BTN_StartGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("LastLevel", 1));
    }
}
