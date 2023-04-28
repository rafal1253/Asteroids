using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreenUIHandler : MonoBehaviour
{
    [SerializeField] Text _playerNameText;
    [SerializeField] Text _levelReachedText;
    [SerializeField] Text _pointsText;

    private void Start()
    {
        _playerNameText.text = Data.PlayerName;
        _levelReachedText.text = $"LEVEL {Data.ReachedLevel}";
        _pointsText.text = $"{Data.EarnedPoints}";
    }
}
