using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using AdvancedInputFieldPlugin;

public class MainMenuUIHandler : MonoBehaviour
{
    [SerializeField] AdvancedInputField _nameInputField;
    [SerializeField] Button _startGameButton;
    [SerializeField] Button _quitGameButton;

    [SerializeField] Color _validationColor;

    private void OnEnable()
    {
        _nameInputField.OnValueChanged.AddListener(delegate { OnNameChanged(); });
        _startGameButton.onClick.AddListener(StartGame);
        _quitGameButton.onClick.AddListener(QuitGame);
    }
    private void OnDisable()
    {
        _nameInputField.OnValueChanged.RemoveAllListeners();
        _startGameButton.onClick.RemoveAllListeners();
        _quitGameButton.onClick.RemoveAllListeners();
    }
    private void Start()
    {
        if (!string.IsNullOrEmpty(Data.PlayerName))
            _nameInputField.Text = Data.PlayerName;
    }
    private void OnNameChanged()
    {
        Data.PlayerName = _nameInputField.Text;
    }
    private void StartGame()
    {
        if (!string.IsNullOrWhiteSpace(_nameInputField.Text))
            SceneLoader.Instance.LoadGameplayScreen();
        else
        {
            _nameInputField.targetGraphic.DOColor(_validationColor, 0.25f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo);
            _nameInputField.Text = null;
        }
    }

    private void QuitGame()
    {
        SceneLoader.Instance.QuitGame();
    }
}
