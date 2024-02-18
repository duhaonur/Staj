using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _coinText;

    [SerializeField] private float _scoreMultiplier;

    private float _scoreHolder;
    private int _coinHolder;

    private bool _isGameEnded = false;

    private void Start()
    {
        _scoreText.text = "0";
        _coinText.text = "0";
        _isGameEnded = false;
    }
    private void OnEnable()
    {
        PlayerEvents.OnCoinCollected += UpdateCoin;
        PlayerEvents.OnGameEnded += GameEnded;
    }
    private void OnDisable()
    {
        PlayerEvents.OnCoinCollected -= UpdateCoin;
        PlayerEvents.OnGameEnded -= GameEnded;
    }
    private void Update()
    {
        if (_isGameEnded)
            return;

        _scoreHolder += Time.deltaTime * (_scoreMultiplier * _coinHolder == 0 ? 1 : _coinHolder);
        _scoreText.text = $"SCORE: {_scoreHolder:F0}";
    }
    private void GameEnded()
    {
        _isGameEnded = true;
        PlayerEvents.CallUpdateScoreCoin((int)_scoreHolder, _coinHolder);
    }
    private void UpdateCoin(int amount)
    {
        _coinHolder += amount;
        _coinText.text = $"COIN: {_coinHolder}";
    }
}
