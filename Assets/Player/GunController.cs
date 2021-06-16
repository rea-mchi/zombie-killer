using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{

    private Animator _animator;
    private Mouse _mouse;
    
    private bool _canInput = true;
    private bool _isZoomIn = false;

    public bool CanInput { set => _canInput = value; }

    private void Awake() {
        TryGetComponent<Animator>(out _animator);
        _mouse = Mouse.current;
    }

    // public void OnFire(InputAction.CallbackContext value) {
    //     if (value.started) {
    //         Debug.Log("Fire!");
    //         _animator.SetTrigger("Fire");   
    //     }
    // }

    private void FixedUpdate() {
        // var mouse = Mouse.current;
        // if (mouse == null) {
        //     return;
        // }
        if (_canInput && _mouse.leftButton.isPressed)
        {
            Fire();
        }
    }

    private void Update() {
        if (_canInput && Input.GetKeyDown(KeyCode.Q))
        {
            if (_isZoomIn) {
                ZoomOut();
            } else {
                ZoomIn();
            }
        }
    }

    private void Fire(){
        _animator.SetTrigger("fire");
    }

    private void ZoomIn() {
        _isZoomIn = true;
        _animator.SetBool("zoomIn", true);
    }

    private void ZoomOut() {
        _isZoomIn = false;
        _animator.SetBool("zoomIn", false);
    }
}
