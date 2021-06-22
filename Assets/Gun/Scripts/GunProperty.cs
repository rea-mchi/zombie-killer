using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunProperty : MonoBehaviour
{
    [SerializeField] public string Name;
    [SerializeField] public int Atk;
    [SerializeField] public float RangeOfFire;
    [SerializeField] public int CapacityLoad;
    [SerializeField][Range(1,2)] public float Kickback;
    [SerializeField] public ParticleSystem FireVFX;
    [SerializeField] public GameObject[] HitVFX;
    [SerializeField] public GameObject[] HitEnemyVFX;
}
