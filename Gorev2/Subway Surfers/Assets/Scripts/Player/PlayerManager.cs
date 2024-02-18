using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private int _playerHealth = 3;

    private void Start()
    {
        _playerHealth = 3;
    }
    private void OnEnable()
    {
        PlayerEvents.OnCollisionWithObstacle += OnCollisionDecreaseHealth;
    }
    private void OnDisable()
    {
        PlayerEvents.OnCollisionWithObstacle -= OnCollisionDecreaseHealth;
    }
    private void OnCollisionDecreaseHealth()
    {
        _playerHealth--;
        if (_playerHealth <= 0)
            PlayerEvents.CallGameEnded();
    }
}
