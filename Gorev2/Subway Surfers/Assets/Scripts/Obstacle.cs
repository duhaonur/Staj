using UnityEngine;

public class Obstacle : Spawnable<Obstacle>
{
    [SerializeField] private float _heightForCoin;
    [SerializeField] private float _zSpawnPos;
    [SerializeField] private float _minCoinSpawnAmount;
    [SerializeField] private float _maxCoinSpawnAmount;
    private void OnEnable()
    {
        int spawnAmount = (int)Random.Range(_minCoinSpawnAmount, _maxCoinSpawnAmount);
        Vector3 spawnPos = transform.position;
        spawnPos.z -= _zSpawnPos;
        PlayerEvents.CallSpawnCoin(spawnAmount, _heightForCoin, spawnPos);
    }
}
