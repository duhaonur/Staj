using UnityEngine;
[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    private Animator _playerAnimator;

    private int _stateHash;
    private int _stateOnHash;
    private void Awake()
    {
        _playerAnimator = GetComponent<Animator>();

        _stateHash = Animator.StringToHash(Constants.STATE);
        _stateOnHash = Animator.StringToHash(Constants.STATE_ON);
    }
    private void OnEnable()
    {
        PlayerEvents.OnRun += PlayRun;
        PlayerEvents.OnJump += PlayJump;
        PlayerEvents.OnFall += PlayFall;
        PlayerEvents.OnRoll += PlayRoll;
    }
    private void OnDisable()
    {
        PlayerEvents.OnRun -= PlayRun;
        PlayerEvents.OnJump -= PlayJump;
        PlayerEvents.OnFall -= PlayFall;
        PlayerEvents.OnRoll -= PlayRoll;
    }
    private void PlayRun()
    {
        _playerAnimator.SetTrigger(_stateOnHash);
        _playerAnimator.SetInteger(_stateHash, Constants.RUN_STATE);
        PlayerEvents.CallGetAnimationState(Constants.RUN_STATE);
    }
    private void PlayJump()
    {
        _playerAnimator.SetTrigger(_stateOnHash);
        _playerAnimator.SetInteger(_stateHash, Constants.JUMP_STATE);
        PlayerEvents.CallGetAnimationState(Constants.JUMP_STATE);
    }
    private void PlayFall()
    {
        _playerAnimator.SetTrigger(_stateOnHash);
        _playerAnimator.SetInteger(_stateHash, Constants.FALL_STATE);
        PlayerEvents.CallGetAnimationState(Constants.FALL_STATE);
    }
    private void PlayRoll()
    {
        _playerAnimator.SetTrigger(_stateOnHash);
        _playerAnimator.SetInteger(_stateHash, Constants.ROLL_STATE);
        PlayerEvents.CallGetAnimationState(Constants.ROLL_STATE);
    }
    private void EnableCollision()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(Constants.PLAYER_LAYER), LayerMask.NameToLayer(Constants.SLIDABLE_LAYER), false);
    }
}
