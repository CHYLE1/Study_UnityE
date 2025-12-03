using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameConfigSO Config;

    // 인터페이스가 private이므로, 이를 사용하는 함수도 private이어야 합니다.
    private interface IGameState
    {
        void Enter();
        void UpdateState();
        void Exit();
    }

    private IGameState currentState;
    private int currentScore;
    private float gameTimer;
    public bool IsGameActive { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // 내부에서 호출하므로 private이어도 상관없습니다.
        ChangeState(new PlayingState(this));

        GameEvents.OnTargetHit += AddScore;
        GameEvents.OnGameRestart += RestartGame;
    }

    private void Update() => currentState?.UpdateState();

    private void OnDestroy()
    {
        GameEvents.OnTargetHit -= AddScore;
        GameEvents.OnGameRestart -= RestartGame;
    }

    // [수정된 부분] public -> private 으로 변경
    private void ChangeState(IGameState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    private void AddScore()
    {
        if (!IsGameActive) return;
        currentScore++;
    }

    private void RestartGame()
    {
        currentScore = 0;
        ChangeState(new PlayingState(this));
    }

    // --- STATES (내부 클래스) ---
    private class PlayingState : IGameState
    {
        private GameManager gm;
        public PlayingState(GameManager gm) { this.gm = gm; }
        public void Enter()
        {
            gm.IsGameActive = true;
            gm.gameTimer = gm.Config.GameDuration;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        public void UpdateState()
        {
            gm.gameTimer -= Time.deltaTime;
            if (gm.gameTimer <= 0) gm.ChangeState(new GameOverState(gm));
        }
        public void Exit() { gm.IsGameActive = false; }
    }

    private class GameOverState : IGameState
    {
        private GameManager gm;
        public GameOverState(GameManager gm) { this.gm = gm; }
        public void Enter()
        {
            GameEvents.OnGameOver?.Invoke();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        public void UpdateState() { }
        public void Exit() { }
    }

    public int GetScore() => currentScore;
    public float GetTimeRemaining() => Mathf.Max(0, gameTimer);
}