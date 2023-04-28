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
        _nameInputField.onValueChanged.AddListener(delegate { OnNameChanged(); });
        _startGameButton.onClick.AddListener(StartGame);
        _quitGameButton.onClick.AddListener(QuitGame);
    }
    private void OnDisable()
    {
        _nameInputField.onValueChanged.RemoveAllListeners();
        _startGameButton.onClick.RemoveAllListeners();
        _quitGameButton.onClick.RemoveAllListeners();
    }
    private void OnNameChanged()
    {
        Data.PlayerName = _nameInputField.text;
    }
    private void StartGame()
    {
        SceneLoader.Instance.LoadGameplayScreen();
    }

    private void QuitGame()
    {
        SceneLoader.Instance.QuitGame();
    }
}
