using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;

namespace _Scripts
{
    public class TurretMain: MonoBehaviour,ITakeDamage
    {
        [SerializeField] private float timeBetweenShots = 4f; 
        [SerializeField] private float awarenessDistance = 3.5f;
        [SerializeField] private Transform eye;
        [SerializeField] private Transform projectPrefab; 
        [SerializeField] private float animDelayTime = 0.5f;
        [SerializeField] private float maxHealth = 3;
        
        private PlayerController player;
        private Animator _animator;
        private State _state; 
       
        private float shotTimer;
        private bool isDead;
        private float currentHealth;
       
        [SerializeField] private AudioClip towerShotSound;
        [SerializeField] private float towerShotVol = 1f;


        public enum State
        {
            Idle,
            Attack,
            Dead,
        }

        private void Start()
        {
            player = FindObjectOfType<PlayerController>();
            _animator = GetComponentInChildren<Animator>();
            _state = State.Idle;
            currentHealth = maxHealth;
        }

        private void Update()
        {
            switch (_state)
            {
                case State.Idle:
                {
                    if ((Vector2.Distance(transform.position, player.transform.position) < awarenessDistance)&& shotTimer<0)
                    {
                        shotTimer = timeBetweenShots;
                        _state = State.Attack;
                    }
                    break;
                }
                case State.Dead:
                {
                    isDead = true;
                    break;
                }
            }

            shotTimer -= Time.deltaTime;
        }

        private void FixedUpdate()
        {

            switch (_state)
            {
                case State.Attack:
                {
                    StartCoroutine(Attack());
                    _state = State.Idle;
                    break;
                }
            }
        }

        private IEnumerator Attack()
        {  
           AudioSystem.Instance.PlaySound(towerShotSound,towerShotVol);
            _animator.SetTrigger("Attack"); 
            yield return new WaitForSeconds(animDelayTime);
            Vector3 shotDir = (  player.transform.position - transform.position).normalized;
            Transform projestile = Instantiate(projectPrefab, eye.position, Quaternion.identity);
            projestile.GetComponent<FireBall>().Setup(shotDir);
        }

        public void TakeDamage(float damage)
        {
            if (_state != State.Dead)
            {_animator.SetTrigger("Hurt");
                 currentHealth -= damage;
                            if (currentHealth <= 0)
                            { 
                                GameManager.Instance.KillEnemy(this);
                                _state = State.Dead;
                                _animator.SetTrigger("Dead");
                            }
            }
           
        }
    }
}