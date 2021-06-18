using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{

    [SerializeField] private Vector3 _birthLocation;
    [SerializeField] private float _patrolRadius;
    [SerializeField] private float _patrolSpeed;
    [SerializeField] private float _awareDistance;
    [SerializeField] private float _walkCloseSpeed;
    [SerializeField] private float _chaseDistance;
    [SerializeField] private float _chaseSpeed;
    [SerializeField] private float _attackDistance;

    // strength
    [SerializeField] private int _atk = 2;

    [SerializeField] private int _timeUntilDisappear = 5;
    public Transform target;

    private NavMeshAgent _agent;
    private Animator _animator;

    private bool _isAttacking = false;
    private bool _isDead = false;

    public bool IsAttacking { set { _isAttacking = value; } }

    private void Awake() {
        TryGetComponent<NavMeshAgent>(out _agent);
        TryGetComponent<Animator>(out _animator);
        _agent.stoppingDistance = _attackDistance;
        _agent.enabled = false;
    }
    
    private void Update() {
        if(!_isDead && !_isAttacking) calculateDistance();
    }

    private void calculateDistance() {
        float curDistance = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.z),
            new Vector2(target.position.x, target.position.z));
        if (curDistance > _awareDistance) {
            _agent.enabled = false;
            RandomPatrol();
        } else {
            _agent.enabled = true;
            _agent.SetDestination(target.position);
            if (curDistance > _chaseDistance){
                WalkClose();
            } else if (curDistance > _attackDistance) {
                Chase();
            } else Attack();
        }
    }
    private void RandomPatrol() {
        _animator.SetBool("in_range", false);
    }

    private void WalkClose() {
        _animator.SetBool("in_range", true);
        _agent.speed = _walkCloseSpeed;
    }

    private void Chase() {
        Debug.Log("call chase");
        _animator.SetBool("angry", true);
        _agent.speed = _chaseSpeed;
    }

    private void Attack() {
        Debug.Log("call attack");
        _animator.SetTrigger("attack");
        _agent.speed = 0f;
    }

    private void CauseDmg(){
        target.TryGetComponent<Health>(out var targetHealth);
        targetHealth.Hurted(_atk);
    }

    public void HeadBoom(){
        _isDead = true;
        _agent.enabled = false;
        _animator.SetTrigger("headshot");
    }

    public void Killed() {
        _isDead = true;
        _agent.enabled = false;
        _animator.SetTrigger("kill");
    }

    public void OnDeadAnimationFinish(){
        StartCoroutine(Disappear());
    }
    private IEnumerator Disappear() {
        yield return new WaitForSeconds(_timeUntilDisappear);
        gameObject.SetActive(false);
    }
}
