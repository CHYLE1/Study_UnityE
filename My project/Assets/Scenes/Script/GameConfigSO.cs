using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game/GameConfig")]
public class GameConfigSO : ScriptableObject
{
    [Header("Game Settings")]
    public float GameDuration = 60f; // 게임 시간 (초)

    [Header("Player Settings")]
    public float MoveSpeed = 8f;
    public float JumpForce = 5f;
    public float LookSensitivity = 2f;
    public float ShootRange = 100f;

    [Header("Target Settings")]
    public float MinSpawnInterval = 0.2f;
    public float MaxSpawnInterval = 1.0f; // 1초 이하 랜덤
    public float TargetMoveSpeed = 5f;
    public float TargetLifeTime = 3f; // 안 맞으면 3초 뒤 사라짐
}