using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();
        Touch.onFingerDown += StartTouch;
        Touch.onFingerUp += EndTouch;
    }
    private void OnDisable()
    {
        Touch.onFingerDown -= StartTouch;
        Touch.onFingerUp -= EndTouch;
        EnhancedTouchSupport.Disable();
        TouchSimulation.Disable();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
    private void StartTouch(Finger finger)
    {
        if (PlayerEvents.OnStartTouch != null)
        {
            PlayerEvents.CallStartTouch(finger.currentTouch.startScreenPosition, (float)finger.currentTouch.startTime);
        }
    }
    private void EndTouch(Finger finger)
    {
        if (PlayerEvents.OnEndTouch != null)
        {
            PlayerEvents.CallEndTouch(finger.currentTouch.screenPosition, (float)finger.currentTouch.time);
        }
    }
}
