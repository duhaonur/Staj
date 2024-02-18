using System;
using UnityEngine;

public static class PlayerEvents
{
    #region InputEvents
    
    public static Action<Vector2, float> OnStartTouch;
    public static void CallStartTouch(Vector2 position, float time) { OnStartTouch?.Invoke(position, time); }
    public static Action<Vector2, float> OnEndTouch;
    public static void CallEndTouch(Vector2 position, float time) { OnEndTouch?.Invoke(position, time); }
    public static Action<Enum> OnSwipe;
    public static void CallSwipe(Enum direction) { OnSwipe?.Invoke(direction); }
    #endregion
    #region AnimationEvents
    public static Action OnRun;
    public static void CallRun() { OnRun?.Invoke(); }

    public static Action OnJump;
    public static void CallJump() { OnJump?.Invoke(); }
    public static Action OnFall;
    public static void CallFall() { OnFall?.Invoke(); }
    public static Action OnRoll;
    public static void CallRoll() { OnRoll?.Invoke(); }
    public static Action<int> OnGetAnimationState;
    public static void CallGetAnimationState(int animation) { OnGetAnimationState?.Invoke(animation); }
    #endregion
    #region CollisionEvents
    public static Action OnCollisionWithObstacle;
    public static void CallCollisionWithObstacle() { OnCollisionWithObstacle?.Invoke(); }
    #endregion
    #region ObjectSpawnEvents
    public static Action<Vector3> OnSpawnedPosition;
    public static void CallSpawnedPosition(Vector3 pos) { OnSpawnedPosition?.Invoke(pos); }
    public static Action<Vector3> OnRemoveSpawnedPosition;
    public static void CallRemoveSpawnedPosition(Vector3 pos) { OnRemoveSpawnedPosition?.Invoke(pos); }
    public static event Func<Vector3, bool> OnIsPositionOccupied;
    public static bool CallIsPositionOccupied(Vector3 pos) { return OnIsPositionOccupied?.Invoke(pos) ?? false; }
    public static Action<int, float, Vector3> OnSpawnCoin;
    public static void CallSpawnCoin(int amount, float height, Vector3 spawnPos) { OnSpawnCoin?.Invoke(amount, height, spawnPos); }
    #endregion
    #region ScoreEvents
    public static Action<int> OnCoinCollected;
    public static void CallCoinCollected(int amount) { OnCoinCollected?.Invoke(amount); }
    public static Action<int,int> OnUpdateScoreCoin;
    public static void CallUpdateScoreCoin(int score, int coin) { OnUpdateScoreCoin?.Invoke(score, coin); }
    #endregion
    public static Action OnGameEnded;
    public static void CallGameEnded() { OnGameEnded?.Invoke(); }

    public enum SwipeDirection
    {
        Left,
        Right
    }
}
