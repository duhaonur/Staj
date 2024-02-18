using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region SerializeFields
    [SerializeField] private LayerMask _layerMaskForGravity;

    [Header("Lane Settings")]
    [SerializeField] private float[] _lanes;
    [SerializeField] private float _oneLaneWidth;

    [Header("Player SpeedSettings")]
    [SerializeField] private float _speed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _speedDivider;

    [Header("Gravity Settings")]
    [SerializeField] private float _fallSpeed = 4f;
    [SerializeField] private float _jumpForce = 4f;
    [SerializeField] private float _jumpHeight = 4f;
    [SerializeField] private float _gravity = -9.81f;

    [Header("Ray Settings")]
    [SerializeField] private float _rayCheckDistance = 0.1f;
    [SerializeField] private float _groundCheckDistance = 0.1f;
    #endregion
    
    private Rigidbody _playerRB;

    private HashSet<float> _lanesHash;

    private Vector3 _moveDirection;
    private Vector3 _yDirection = Vector3.zero;

    private float _currentLane;

    private int _currentAnimation;

    private bool _jumped = false;
    private bool _isGameEnded = false;
    private void Awake()
    {
        _lanesHash = new HashSet<float>();
        _playerRB = GetComponent<Rigidbody>();

        GetLanes();

        _isGameEnded = false;
    }
    private void OnEnable()
    {
        PlayerEvents.OnSwipe += GetSwipeDirection;
        PlayerEvents.OnJump += Jump;
        PlayerEvents.OnRoll += Roll;
        PlayerEvents.OnGetAnimationState += GetAnimationState;
        PlayerEvents.OnCollisionWithObstacle += OnCollision;
        PlayerEvents.OnGameEnded += GameEnded;
    }
    private void OnDisable()
    {
        PlayerEvents.OnSwipe -= GetSwipeDirection;
        PlayerEvents.OnJump -= Jump;
        PlayerEvents.OnRoll -= Roll;
        PlayerEvents.OnGetAnimationState -= GetAnimationState;
        PlayerEvents.OnCollisionWithObstacle -= OnCollision;
        PlayerEvents.OnGameEnded -= GameEnded;
    }
    private void Start()
    {
        float x = 0;
        for (int i = 0; i < _lanes.Length; i++)
        {
            x += _lanes[i];
        }

        transform.position = new Vector3(x / _lanes.Length, transform.position.y, transform.position.z);
    }
    private void Update()
    {
        if (_speed < _maxSpeed)
            _speed += Time.deltaTime / _speedDivider;
        else
            _speed = _maxSpeed;
    }
    private void FixedUpdate()
    {
        if (_isGameEnded)
            return;

        Jumped();
        MoveCharacter();
    }
    private void MoveCharacter()
    {
        if (_moveDirection.x > 0 && transform.position.x >= _currentLane || _moveDirection.x < 0 && transform.position.x <= _currentLane)
        {
            _moveDirection = Vector3.zero;
        }

        _playerRB.velocity = _speed * (Vector3.forward + _moveDirection);
    }
    private void GetAnimationState(int state)
    {
        _currentAnimation = state;
    }
    private void Jump()
    {
        _jumped = true;
    }
    private void Roll()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(Constants.PLAYER_LAYER), LayerMask.NameToLayer(Constants.SLIDABLE_LAYER), true);
    }
    private void Jumped()
    {
        if (transform.position.y <= _jumpHeight && _jumped)
        {

            _yDirection.y += -_gravity * _jumpForce * Time.deltaTime;
            _playerRB.AddForce(_speed * _yDirection);
        }
        else
        {
            _jumped = false;

            if (IsGrounded())
            {
                if (_currentAnimation != Constants.RUN_STATE && _currentAnimation != Constants.ROLL_STATE)
                {
                    _currentAnimation = Constants.RUN_STATE;
                    PlayerEvents.CallRun();
                }

                _yDirection.y = 0f;
                return;
            }
            else
            {
                if (_currentAnimation != Constants.FALL_STATE)
                {
                    _currentAnimation = Constants.FALL_STATE;
                    PlayerEvents.CallFall();
                }
            }

            if (_yDirection.y > _jumpHeight)
                _yDirection.y = _jumpHeight;

            _yDirection.y += _gravity * _fallSpeed * Time.deltaTime;
            _playerRB.AddForce(_speed * _yDirection);
        }
    }
    private void GetLanes()
    {
        foreach (var lane in _lanes)
        {
            _lanesHash.Add(lane);
        }
    }
    private void OnCollisionLaneChange(float pos1, float pos2)
    {

        if (_lanesHash.TryGetValue(pos1, out float value1))
        {
            _currentLane = value1;
            _moveDirection = Vector3.left;
            return;
        }
        if (_lanesHash.TryGetValue(pos2, out float value2))
        {
            _currentLane = value2;
            _moveDirection = Vector3.right;
            return;
        }
    }
    private void OnCollision()
    {
        OnCollisionLaneChange(_currentLane - _oneLaneWidth, _currentLane + _oneLaneWidth);
    }
    private void ChangeLane(float lanePos)
    {
        if (_lanesHash.TryGetValue(lanePos, out float value))
        {
            _currentLane = value;
        }
    }
    private void GetSwipeDirection(Enum direction)
    {
        if (_currentAnimation == Constants.ROLL_STATE)
            return;

        switch (direction)
        {
            case PlayerEvents.SwipeDirection.Left:
                ChangeLane(_currentLane - _oneLaneWidth);
                _moveDirection = Vector3.left;
                break;
            case PlayerEvents.SwipeDirection.Right:
                ChangeLane(_currentLane + _oneLaneWidth);
                _moveDirection = Vector3.right;
                break;
            default:
                break;
        }
    }
    private void GameEnded()
    {
        _isGameEnded = true;
        _playerRB.velocity = Vector3.zero;
    }
    private bool IsGrounded()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.01f;
        rayOrigin.y -= _groundCheckDistance;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, _rayCheckDistance, _layerMaskForGravity))
        {
            return true;
        }

        return false;
    }
}
