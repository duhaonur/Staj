using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private float _minimumDistance = 0.2f;
    [SerializeField] private float _maximumTime = 1f;
    [SerializeField] private float _directionThreshold = 0.9f;

    private Vector2 _startPosition;
    private Vector2 _endPosition;

    private float _startTime;
    private float _endTime;

    private void OnEnable()
    {
        PlayerEvents.OnStartTouch += SwipeStart;
        PlayerEvents.OnEndTouch += SwipeEnd;
    }
    private void OnDisable()
    {
        PlayerEvents.OnStartTouch -= SwipeStart;
        PlayerEvents.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 position, float time)
    {
        _startPosition = position;
        _startTime = time;
    }
    private void SwipeEnd(Vector2 position, float time)
    {
        _endPosition = position;
        _endTime = time;
        DetectSwipte();
    }
    private void DetectSwipte()
    {
        if (Vector3.Distance(_startPosition.normalized, _endPosition.normalized) >= _minimumDistance && (_endTime - _startTime) <= _maximumTime)
        {
            Vector3 direction = _endPosition - _startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }
    }
    private void SwipeDirection(Vector2 direction)
    {
        if (Vector2.Dot(Vector2.up, direction) > _directionThreshold)
        {
            PlayerEvents.CallJump();
        }
        else if (Vector2.Dot(Vector2.down, direction) > _directionThreshold)
        {
            PlayerEvents.CallRoll();
        }
        else if (Vector2.Dot(Vector2.right, direction) > _directionThreshold)
        {
            PlayerEvents.CallSwipe(PlayerEvents.SwipeDirection.Right);
        }
        else if (Vector2.Dot(Vector2.left, direction) > _directionThreshold)
        {
            PlayerEvents.CallSwipe(PlayerEvents.SwipeDirection.Left);
        }

    }
}
