using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] int _maxHP = 15;
    [SerializeField] int _def = 0;

    private int _hp;

    private void OnEnable() {
        _hp = _maxHP;
    }
    public void Hurted(int dmg) {
        if (_hp <= 0) return;
        _hp -= dmg - _def;
        if (_hp <= 0) {
            transform.TryGetComponent<EnemyAI>(out var ai);
            ai.Killed();
        }
    }
}
