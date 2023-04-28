using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIHandler : MonoBehaviour
{
    [SerializeField] InputField _nameInputField;
    [SerializeField] Button _startGameButton;
    [SerializeField] Button _quitGameButton;

    private void OnEnable()
    {
        _startGameButton.onClick.AddListener(StartGame);
        _quitGameButton.onClick.AddListener(QuitGame);
    }
    private void OnDisable()
    {
        _startGameButton.onClick.RemoveAllListeners();
        _quitGameButton.onClick.RemoveAllListeners();
    }

    private void StartGame()
    {
        SceneLoader.Instance.LoadScene("GameplayScreen");
    }
    private void QuitGame()
    {
        SceneLoader.Instance.QuitGame();
    }
}
