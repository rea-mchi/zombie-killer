using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHP = 10;
    [SerializeField] private int _def = 0;

    private int _hp;

    private void OnEnable() {
        _hp = _maxHP;
    }

    public void Hurted(int dmg) {
        _hp -= dmg-_def;
        if (_hp <= 0)
        {
            NotifyDeath();
        }
    }

    private void NotifyDeath() {
        // todo notify ui play is dead
        Debug.Log("You are dead");
    }
}
