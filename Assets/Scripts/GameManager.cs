using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _startMenu;
    [SerializeField] GameObject _tutorailMenu;

    [SerializeField] PlayerController _playerScript;
    [SerializeField] TextMeshProUGUI _playerHeathText;
    [SerializeField] GameObject _gameOver;

    [SerializeField] Slider _heathSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DisplayPlayerHeath();
        CheckGameOver();
    }

    
    void DisplayPlayerHeath()
    {
        _heathSlider.value = _playerScript.PlayerHeath;
    }
    

    void CheckGameOver ()
    {
        if (_heathSlider.value <= 0)
        {
            _gameOver.SetActive(true);
        }

        else
        {
            _gameOver.SetActive(false);
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Main Scene");
    }

    public void ExitPlayMode()
    {
        EditorApplication.ExitPlaymode();
    }

    public void TurnOnTurorialMenu()
    {
        _startMenu.SetActive(false);
        _tutorailMenu.SetActive(true);
    }

    public void TurnOffTurorialMenu()
    {
        _startMenu.SetActive(true);
        _tutorailMenu.SetActive(false);
    }

}
