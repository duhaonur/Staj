using System.Collections;
using UnityEngine;
public class PlayerCollisionController : MonoBehaviour
{
    [SerializeField] private Material _transparentMaterial;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Renderer _playerRenderer;

    [SerializeField] private float _onCollideAlphaChangeDuration;


    private Coroutine _collisionFeedBackCoroutine;

    private bool _canCollide = true;

    private void Start()
    {
        if (_playerRenderer == null)
            _playerRenderer = GetComponentInChildren<Renderer>();
    }
    private IEnumerator CollisionFeedBack()
    {
        _playerRenderer.material = _transparentMaterial;
        _canCollide = false;
        float timer = 0f;
        float stopTimer = 0;

        while (stopTimer < 3)
        {
            while (timer < _onCollideAlphaChangeDuration && stopTimer < 3)
            {
                float alpha = Mathf.PingPong(Time.time, _onCollideAlphaChangeDuration) / _onCollideAlphaChangeDuration * 0.5f + 0.5f;
                SetMaterialAlpha(alpha);
                stopTimer += Time.deltaTime;
                yield return null;
            }

            timer = 0f;
        }
        _canCollide = true;
        _playerRenderer.material = _defaultMaterial;
        SetMaterialAlpha(1);
    }
    private void SetMaterialAlpha(float alpha)
    {
        Color color = _transparentMaterial.color;
        color.a = alpha;
        _transparentMaterial.color = color;
    }
    private void CallCollisionFeedBack()
    {
        if (_collisionFeedBackCoroutine != null)
            StopCoroutine(_collisionFeedBackCoroutine);

        _collisionFeedBackCoroutine = StartCoroutine(CollisionFeedBack());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.OBSTACLE))
        {
            if (!_canCollide)
                return;

            if (other.gameObject.layer != LayerMask.NameToLayer(Constants.WALKABLE_LAYER))
            {
                PlayerEvents.CallCollisionWithObstacle();
                CallCollisionFeedBack();
            }
            else
            {
                if (Vector3.Dot(transform.position - other.transform.position.normalized, other.transform.up.normalized) < 0.9f)
                {
                    PlayerEvents.CallCollisionWithObstacle();
                    CallCollisionFeedBack();
                }
            }
        }
        if (other.CompareTag(Constants.COIN))
        {
            if (other.gameObject.TryGetComponent<ICollectable>(out var collectable))
            {
                collectable.OnCollect();
            }
        }
    }
}
