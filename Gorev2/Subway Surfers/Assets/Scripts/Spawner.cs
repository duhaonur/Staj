using UnityEngine;
using UnityEngine.Pool;
public abstract class Spawner<T> : MonoBehaviour where T : Spawnable<T>
{
    [SerializeField] private T[] _objectsToSpawn;

    [SerializeField] protected float[] _xSpawnPositions;

    [SerializeField] private int _maxPoolSize;

    [SerializeField] private Camera _mainCamera;

    protected ObjectPool<T> _pool;

    private Vector3 _camPos;

    private float _farClipPlane;

    public float SpawnedObjectLength;
    public float PreviousObjectLength;

    private void Start()
    {
        _pool = new ObjectPool<T>(SpawnRandomObject, GetObject, ReturnObjectToPool, null, true, 1, _maxPoolSize);

        _mainCamera = Camera.main;
        _farClipPlane = _mainCamera.farClipPlane;
    }

    protected virtual void Update()
    {
        _camPos = _mainCamera.transform.TransformPoint(0, 0, _farClipPlane);

        if (SpawnedObjectLength < _camPos.z)
        {
            _pool.Get();
        }
    }

    private T SpawnRandomObject()
    {
        var obj = Instantiate(_objectsToSpawn[Random.Range(0, _objectsToSpawn.Length)], transform);

        obj.gameObject.SetActive(false);
        obj.GetPool(_pool);

        return obj;
    }

    protected virtual void GetObject(T obj)
    {
        SpawnedObjectLength += PreviousObjectLength;
        PreviousObjectLength = obj.ObjectLength;

        float spawnPosition = _xSpawnPositions[Random.Range(0, _xSpawnPositions.Length)];
        Vector3 newPosition = new Vector3(spawnPosition, obj.transform.position.y, SpawnedObjectLength);

        obj.transform.position = newPosition;

        obj.SetSpawnPosition(newPosition);

        obj.SpawnedObjectReleased = false;
        obj.gameObject.SetActive(true);
    }

    protected void ReturnObjectToPool(T obj)
    {
        obj.gameObject.SetActive(false);
    }
}
