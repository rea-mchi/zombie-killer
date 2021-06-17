using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SerializableGunData {
    public string name;
    public GameObject gun;
}

public class GunController : MonoBehaviour
{

    [SerializeField] private GameObject[] _equipGuns; // set init gun here
    [SerializeField] private SerializableGunData[] _gunList;

    private Dictionary<string, GameObject> _gunDict;
    private Animator _animator;
    
    private int _curGunIndex = 0;
    private int _equipGunCount = 1;
    private bool _canInput = true;
    private bool _isZoomIn = false;

    public bool CanInput { set => _canInput = value; }

    private void Awake() {
        GenerateGunDict();
        TryGetComponent<Animator>(out _animator);
    }

    private void GenerateGunDict() {
        _gunDict = new Dictionary<string, GameObject>();
        foreach (var gundata in _gunList) {
            _gunDict.Add(gundata.name, gundata.gun);
        }
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
        if (_canInput && Input.GetButton("Fire1"))
        {
            Fire();
        }
    }

    private void Update() {
        if (_canInput && Input.GetKeyDown(KeyCode.Q)) {
            if (_isZoomIn) {
                ZoomOut();
            } else {
                ZoomIn();
            }
        }
        CheckMouseScroll();
    }

    private void CheckMouseScroll() {
        if(!_canInput) return;
        if (Input.mouseScrollDelta.y > 0) {
                SwitchTo((_curGunIndex+1)%_equipGunCount);
            } else if (Input.mouseScrollDelta.y < 0) {
                SwitchTo((_curGunIndex+_equipGunCount-1)%_equipGunCount);
            }
    }

    public bool Unlock(string gunname) {
        if (!_gunDict.ContainsKey(gunname)) {
            Debug.Log($"Incorrect gunname: {gunname} set in unlock point");
            return false;
        } else {
            _equipGuns[_equipGunCount++] = _gunDict[gunname];
            SwitchTo(_equipGunCount-1);
            return true;
        }
    }

    private void SwitchTo(int gunindex){
        if (gunindex >= _equipGunCount || gunindex == _curGunIndex) return;
        _equipGuns[_curGunIndex].SetActive(false);
        _equipGuns[gunindex].SetActive(true);
        _curGunIndex = gunindex;
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

    public void OnStartRunning() {
        _animator.SetBool("run", true);
    }

    public void OnStopRunning() {
        _animator.SetBool("run", false);
    }
}
