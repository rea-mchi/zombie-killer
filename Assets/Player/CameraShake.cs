using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float _shakingTimeLength = .1f;
    [SerializeField] private float _cameraRestoreTimeLength = .15f;

    private float _timer = 0f;
    private Vector3 _prevLocalPos; // only consider x,y
    private float _prevLocalZ; // only consider z euler angle;
    private Vector3 _finalLocalPos;
    private float _finalLocalZ;
    private bool _isShaking = false;

    public void StartShaking(float force) {
        _prevLocalPos = transform.localPosition;
        _prevLocalZ = transform.localEulerAngles.z;
        _timer = 0f;
        Vector3 rnd = UnityEngine.Random.insideUnitCircle * force;
        _finalLocalPos = _prevLocalPos + rnd;
        _finalLocalZ = _prevLocalZ + UnityEngine.Random.Range(0f, .5f) * force;
        _isShaking = true;
        
    }

    private void Update() {
        if(!_isShaking) return;
        if(_timer > _shakingTimeLength + _cameraRestoreTimeLength) {
            _isShaking = false;
            return;
        }
        if (_timer < _shakingTimeLength)
        {
            float ratio = _timer/_shakingTimeLength;
            transform.localPosition = Vector3.Lerp(_prevLocalPos, _finalLocalPos, ratio);
            var angles = transform.localEulerAngles;
            angles.z = Mathf.LerpAngle(_prevLocalZ, _finalLocalZ, ratio);
            transform.localEulerAngles = angles;
        } else {
            float ratio = (_timer - _shakingTimeLength)/_cameraRestoreTimeLength;
            transform.localPosition = Vector3.Lerp(_finalLocalPos, _prevLocalPos, ratio);
            var angles = transform.localEulerAngles;
            angles.z = Mathf.LerpAngle(_finalLocalZ, _prevLocalZ, ratio);
            transform.localEulerAngles = angles;
        }
        _timer += Time.deltaTime;
    }
}
