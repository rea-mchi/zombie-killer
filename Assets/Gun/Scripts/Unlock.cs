using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlock : MonoBehaviour
{

    [SerializeField] string gunname;
    [SerializeField][Tooltip("Degree")] int angularVel = 60;
    // private void Awake() {
    //     TryGetComponent<SphereCollider>(out var trigger);
    //     trigger.isTrigger = true;
    // }

    private void Update() {
        transform.RotateAround(transform.position, Vector3.up, angularVel * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        string tag = other.gameObject.tag;
        if (tag != Strings.PlayerTag) return;
        other.gameObject.TryGetComponent<GunController>(out var gunController);
        if (gunController.Unlock(gunname)) {
            Destroy(gameObject);
        }
    }
}
