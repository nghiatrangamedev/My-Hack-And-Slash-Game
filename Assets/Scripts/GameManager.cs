using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController _playerScript;
    [SerializeField] TextMeshProUGUI _playerHeathText;

    float _playerHeath;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DisplayPlayerHeath();
    }

    void DisplayPlayerHeath()
    {
        _playerHeath = _playerScript.PlayerHeath;
        _playerHeathText.SetText("Heath: " + _playerHeath);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Main Scene");
    }

    public void ExitPlayMode()
    {
        EditorApplication.ExitPlaymode();
    }
}
