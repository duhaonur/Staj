using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _endGameScoreText;
    [SerializeField] private TextMeshProUGUI _highestScoreText;
    [SerializeField] private TextMeshProUGUI _endGameCoinText;

    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private float _timeToDisplayCanvas;

    private float _timer;

    private int _currentScore;
    private int _collectedCoin;

    private bool _isGameEnded = false;

    private void Start()
    {
        _isGameEnded = false;
        _canvasGroup.alpha = 0;
    }
    private void OnEnable()
    {
        PlayerEvents.OnGameEnded += GameEnded;
        PlayerEvents.OnUpdateScoreCoin += GetScore;
    }
    private void OnDisable()
    {
        PlayerEvents.OnGameEnded -= GameEnded;
        PlayerEvents.OnUpdateScoreCoin -= GetScore;
    }
    private void Update()
    {
        if (!_isGameEnded || _canvasGroup.alpha >= 1f)
            return;

        _timer += Time.deltaTime;

        _canvasGroup.alpha = Mathf.InverseLerp(_canvasGroup.alpha, _timeToDisplayCanvas, _timer);
    }
    private void GameEnded()
    {
        _isGameEnded = true;
    }
    public void GetScore(int score, int coin)
    {
        _currentScore = score;
        _collectedCoin = coin;

        if (PlayerPrefs.HasKey(Constants.HIGHEST_SCORE_SAVE))
        {
            if (PlayerPrefs.GetInt(Constants.HIGHEST_SCORE_SAVE) < _currentScore)
            {
                PlayerPrefs.SetInt(Constants.HIGHEST_SCORE_SAVE, _currentScore);
                _highestScoreText.text = $"HIGHEST SCORE:{_currentScore}";
                _endGameScoreText.text = $"SCORE:{_currentScore}";
                _endGameCoinText.text = $"COIN:{_collectedCoin}";
            }
            else
            {
                _highestScoreText.text = $"HIGHEST SCORE:{PlayerPrefs.GetInt(Constants.HIGHEST_SCORE_SAVE)}";
                _endGameScoreText.text = $"SCORE:{_currentScore}";
                _endGameCoinText.text = $"COIN:{_collectedCoin}";
            }
        }
        else
        {
            PlayerPrefs.SetInt(Constants.HIGHEST_SCORE_SAVE, _currentScore);
            _endGameScoreText.text = $"SCORE:{_currentScore}";
            _highestScoreText.text = $"HIGHEST SCORE:{_currentScore}";
            _endGameCoinText.text = $"COIN:{_collectedCoin}";
        }
    }
    public void PlayAgainButton()
    {
        SceneManager.LoadScene(Constants.PLAY_SCENE);
    }
}
