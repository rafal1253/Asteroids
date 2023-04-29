using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayScreenUIHandler : MonoBehaviour
{
    [SerializeField] Button _startPlayButton;
    [SerializeField] Text _levelText;
    [SerializeField] Text _lifesText;
    [SerializeField] Text _pointsText;

    private void OnEnable()
    {
        _startPlayButton.onClick.AddListener(StartPlay);

        EventManager.OnUpdatePlayerLifes += UpdatePlayerLifes;
        EventManager.OnUpdatePlayerPoints += UpdatePlayerPoints;
        EventManager.OnStartLevel += SetLevel;

    }
    private void OnDisable()
    {
        _startPlayButton.onClick.RemoveAllListeners();

        EventManager.OnUpdatePlayerLifes -= UpdatePlayerLifes;
        EventManager.OnUpdatePlayerPoints -= UpdatePlayerPoints;
        EventManager.OnStartLevel -= SetLevel;

    }

    void StartPlay()
    {
        _startPlayButton.gameObject.SetActive(false);
        EventManager.InvokeOnStartPlay();
    }

    void UpdatePlayerLifes(int actualLifes)
    {
        _lifesText.text = $"{actualLifes}";
    }

    void UpdatePlayerPoints(int points)
    {
        _pointsText.text = $"{points}";
    }
    void SetLevel(int index)
    {
        _levelText.text = $"LEVEL {index + 1}";
    }
}
