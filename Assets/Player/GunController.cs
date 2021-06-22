using System;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    [SerializeField] private GameObject[] _equipGuns; // set init gun here
    [SerializeField] private GameObject[] _gunList;

    private Camera _camera;
    private Dictionary<string, GameObject> _gunDict;
    private Animator _animator;
    private CameraShake _cameraShake;
    
    private int _curGunIndex = 0;
    private int _equipGunCount = 1;
    private bool _canInput = true;
    private bool _isZoomIn = false;

    public bool CanInput { set => _canInput = value; }

    private void Awake() {
        GenerateGunDict();
        TryGetComponent<Animator>(out _animator);
        _camera = GetComponentInChildren<Camera>();
        _camera.transform.TryGetComponent<CameraShake>(out _cameraShake);
    }

    private void GenerateGunDict() {
        _gunDict = new Dictionary<string, GameObject>();
        foreach (GameObject gun in _gunList) {
            gun.TryGetComponent<GunProperty>(out var property);
            _gunDict.Add(property.name, gun);
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
        // if (_canInput && Input.GetButtonDown("Fire1"))
        // {
        //     Fire();
        // }
    }

    private void Update() {
                if (_canInput && Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
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
        _equipGuns[_curGunIndex].TryGetComponent<GunProperty>(out var property);
        property.FireVFX.Play();
        _cameraShake.StartShaking(property.Kickback);
        _animator.SetTrigger("fire");
        Vector3 fireDirection = _camera.transform.forward;
        if (Physics.Raycast(transform.position, fireDirection, out var hit, property.RangeOfFire)) {
            string tag = hit.transform.tag;
            Debug.DrawRay(transform.position, fireDirection * hit.distance, Color.red);
            if (tag == "EnemyHead") {
                hit.transform.parent.TryGetComponent<EnemyAI>(out var ai);
                ai.HeadBoom();
                ShowHitVFX(hit, property.HitEnemyVFX, hitEnemy: true);
            } else if (tag == "EnemyBody") {
                hit.transform.TryGetComponent<EnemyHealth>(out var health);
                health.Hurted(property.Atk);
                ShowHitVFX(hit, property.HitEnemyVFX, hitEnemy: true);
            } else {
                ShowHitVFX(hit, property.HitVFX);
            }
        }
    }

    private void ShowHitVFX(RaycastHit hitInfo, GameObject[] VFXPrefabs, bool hitEnemy = false) {
        foreach (GameObject prefab in VFXPrefabs) {
            var instance = Instantiate(prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            if (hitEnemy) {
                instance.transform.parent = hitInfo.transform;
            }
        }
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
