using System;

public static class GameEvents
{
    // 타겟 명중 시 발생
    public static Action OnTargetHit;

    // 게임 종료 시 발생
    public static Action OnGameOver;

    // 게임 재시작 시 발생
    public static Action OnGameRestart;
}

