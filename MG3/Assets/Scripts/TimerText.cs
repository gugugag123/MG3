using UnityEngine;
using TMPro;
public class TimerText : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverText;
    [SerializeField] private Player _player;
    [SerializeField] private float _maxGameTime = 30.0f;
    [SerializeField] private TMP_Text _timerDisplay;
    [SerializeField] private Timer _gameTimer;
    private void Start() { _gameTimer.BeginTimer(_maxGameTime); }
    private void Update()
    {
        _timerDisplay.text = _gameTimer.GetTimeLeft().ToString("F1");
        if (_gameTimer.IsFinished())
        {
            _player.SetGameOver();
            _gameOverText.SetActive(true);
            enabled = false;
        }
    }
}
