using UnityEngine;

public class CoinSpawner : Spawner<Coin>
{
    private Vector3 _spawnPos;

    private float _spawnHeight;

    private void OnEnable()
    {
        PlayerEvents.OnSpawnCoin += SpawnCoin;
    }
    private void OnDisable()
    {
        PlayerEvents.OnSpawnCoin -= SpawnCoin;
    }
    protected override void Update()
    {
        
    }
    protected override void GetObject(Coin obj)
    {
        SpawnedObjectLength = PreviousObjectLength + obj.ObjectLength;
        PreviousObjectLength = SpawnedObjectLength;

        Vector3 newPosition = new Vector3(_spawnPos.x, _spawnHeight, _spawnPos.z + SpawnedObjectLength);

        obj.transform.position = newPosition;

        obj.SetSpawnPosition(newPosition);

        obj.SpawnedObjectReleased = false;
        obj.gameObject.SetActive(true);
    }
    private void SpawnCoin(int amount, float height, Vector3 spawnPos)
    {
        _spawnPos = spawnPos; 
        _spawnHeight = height;

        for (int i = 0; i < amount; i++)
        {
            _pool.Get();
        }
        SpawnedObjectLength = 0;
        PreviousObjectLength = 0;
    }
}
