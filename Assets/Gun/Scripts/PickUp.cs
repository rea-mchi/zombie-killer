using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    [SerializeField] GunType ammoType;
    [SerializeField] int amount;


    private void OnTriggerEnter(Collider other) {
        string tag = other.gameObject.tag;
        if (tag != Strings.PlayerTag) return;
        gameObject.SetActive(false);
    }
}
