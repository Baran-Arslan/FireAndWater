using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }


    [Header("Level Time")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private float levelTime;

    [Header("SCORE")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [HideInInspector] public int NeededScore;
    private int _currentScore;

    [Header("Finish Settings")]
    [SerializeField] private GameObject finishUI;
    [SerializeField] private bool finishScene;


    private void Awake()
    {
        if (_instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    private void Start()
    {
        scoreText.text = _currentScore.ToString() + "/" + NeededScore.ToString();
        PlayerPrefs.SetInt("LastLevel", SceneManager.GetActiveScene().buildIndex);
    }


    private void Update()
    {
        levelTime -= Time.deltaTime;
        UpdateText();
        if (levelTime < 0) RestartLevel();
    }

    public void AddScore()
    {
        _currentScore++;
        scoreText.text = _currentScore.ToString() +"/" + NeededScore.ToString();
    }

    public void FinishLevel()
    {
        if (_currentScore >= NeededScore)
        {
            if (finishScene)
            {
                Time.timeScale = 0;
                finishUI.SetActive(true);
                PlayerPrefs.SetInt("LastLevel", 1);
                return;
            }
            LoadNextLevel();
        }
    }
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void UpdateText()
    {
        timeText.text = Mathf.CeilToInt(levelTime).ToString();
    }

    public void BTN_LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
