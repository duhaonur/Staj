using UnityEngine;
public class Platform : Spawnable<Platform>
{
    [SerializeField] private float _heightForCoin;
    [SerializeField] private float _minCoinSpawnAmount;
    [SerializeField] private float _maxCoinSpawnAmount;
    private void OnEnable()
    {
        int spawnAmount = (int)Random.Range(_minCoinSpawnAmount, _maxCoinSpawnAmount);
        PlayerEvents.CallSpawnCoin(spawnAmount, _heightForCoin, transform.position);
    }
}
