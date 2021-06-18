using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [Serializable]
    public class BehaviorSettings
    {
        float _patrolRadius;
        float _awareDistance;
        float _chaseDistance;
        float _attackDistance;

        public float PatrolRadius { get { return _patrolRadius; } }
        public float AwareDistance { get { return _awareDistance; } }
        public float ChaseDistance { get { return _chaseDistance; } }
        public float AttackDistance { get { return _attackDistance; } }
    } 
}
