using System;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    public class EquippedGunState {
        readonly GameObject _gun;
        readonly int _capacityLoad;

        public int Load { get; private set; }

        public bool IsLoadFull => Load == _capacityLoad;

        public GunProperty Property {
            get {
                _gun.TryGetComponent<GunProperty>(out var property);
                return property;
            }
        }

        public EquippedGunState(GameObject gun) {
            this._gun = gun;
            gun.TryGetComponent<GunProperty>(out var property);
            this._capacityLoad = property.CapacityLoad;
            this.Load = property.CapacityLoad;
            gun.SetActive(false);
        }
        public bool AddLoad(int add) {
            if (IsLoadFull) return false;
            Load = Mathf.Min(_capacityLoad, Load+add);
            return true;
        }

        public void SwitchOn() { _gun.SetActive(true); }
        public void SwitchOff() { _gun.SetActive(false); }
    }

    [SerializeField] private GameObject[] _initGuns; // int equipped gun;
    [SerializeField] private GameObject[] _gunList; // all gun data 

    private Camera _camera;
    private EquippedGunState[] _equipGuns;
    private Dictionary<string, GameObject> _gunDict;
    private Animator _animator;
    private CameraShake _cameraShake;
    
    private int _curGunIndex = -1;
    private int _equipGunCount = 0;
    private bool _canInput = true;
    private bool _isZoomIn = false;

    public bool CanInput { set => _canInput = value; }

    private void Awake() {
        GenerateGunDict();
        EquipInitGuns();
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

    private void EquipInitGuns() {
        _equipGuns = new EquippedGunState[_gunList.Length];
        foreach (GameObject gun in _initGuns) {
            _equipGuns[++_curGunIndex] = new EquippedGunState(gun);
            _equipGunCount++;
        }
        _equipGuns[0]?.SwitchOn();
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
            _equipGuns[_equipGunCount++] = new EquippedGunState(_gunDict[gunname]);
            SwitchTo(_equipGunCount-1);
            return true;
        }
    }

    private void SwitchTo(int gunindex){
        if (gunindex >= _equipGunCount || gunindex == _curGunIndex) return;
        _equipGuns[_curGunIndex].SwitchOff();
        _equipGuns[gunindex].SwitchOn();
        _curGunIndex = gunindex;
    }


    private void Fire(){
        GunProperty property = _equipGuns[_curGunIndex].Property;
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
