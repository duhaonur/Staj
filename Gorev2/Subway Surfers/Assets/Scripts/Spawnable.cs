using UnityEngine;
using UnityEngine.Pool;
public abstract class Spawnable<T> : MonoBehaviour where T : class
{
    [SerializeField] private float _releaseLength;

    public float ObjectLength;

    public bool SpawnedObjectReleased = false;

    protected ObjectPool<T> _pool;

    protected Camera _mainCamera;

    private float _spawnPosition;

    private void Start()
    {
        _mainCamera = Camera.main;
    }
    private void Update()
    {
        if (!SpawnedObjectReleased && _mainCamera.transform.position.z > _spawnPosition)
        {
            SpawnedObjectReleased = true;
            _pool?.Release(this as T);
        }
    }

    public void GetPool(ObjectPool<T> pool)
    {
        _pool = pool;
    }
    public void SetSpawnPosition(Vector3 spawnPosition)
    {
        _spawnPosition = spawnPosition.z + _releaseLength;
    }
}
