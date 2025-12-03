using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private Button restartButton;

    private void Start()
    {
        gameOverPanel.SetActive(false);
        restartButton.onClick.AddListener(() => GameEvents.OnGameRestart?.Invoke());

        GameEvents.OnTargetHit += UpdateUI;
        GameEvents.OnGameOver += ShowGameOver;
        GameEvents.OnGameRestart += HideGameOver;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameActive)
            timerText.text = $"Time: {GameManager.Instance.GetTimeRemaining():F1}";
    }

    private void OnDestroy()
    {
        GameEvents.OnTargetHit -= UpdateUI;
        GameEvents.OnGameOver -= ShowGameOver;
        GameEvents.OnGameRestart -= HideGameOver;
    }

    private void UpdateUI()
    {
        scoreText.text = $"Score: {GameManager.Instance.GetScore()}";
    }

    private void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = $"Final Score: {GameManager.Instance.GetScore()}";
    }

    private void HideGameOver()
    {
        gameOverPanel.SetActive(false);
        UpdateUI();
    }
}