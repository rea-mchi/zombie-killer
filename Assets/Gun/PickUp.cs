using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    [SerializeField] AmmoType ammoType;
    [SerializeField] int amount;

    static readonly string _triggerableTag = "Player";

    private void OnTriggerEnter(Collider other) {
        string tag = other.gameObject.tag;
        if (tag != _triggerableTag) return;
        gameObject.SetActive(false);
    }
}
