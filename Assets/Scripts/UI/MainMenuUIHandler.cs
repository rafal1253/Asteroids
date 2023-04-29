using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuUIHandler : MonoBehaviour
{
    [SerializeField] InputField _nameInputField;
    [SerializeField] Button _startGameButton;
    [SerializeField] Button _quitGameButton;

    [SerializeField] Color _validationColor;

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
    private void Start()
    {
        if (!string.IsNullOrEmpty(Data.PlayerName))
            _nameInputField.text = Data.PlayerName;
    }
    private void OnNameChanged()
    {
        Data.PlayerName = _nameInputField.text;
    }
    private void StartGame()
    {
        if (!string.IsNullOrWhiteSpace(_nameInputField.text))
            SceneLoader.Instance.LoadGameplayScreen();
        else
        {
            _nameInputField.targetGraphic.DOColor(_validationColor, 0.25f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo);
            _nameInputField.text = null;
        }
    }

    private void QuitGame()
    {
        SceneLoader.Instance.QuitGame();
    }
}
