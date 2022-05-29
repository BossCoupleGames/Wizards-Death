using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace _Scripts
{
    public class SlimeMain : MonoBehaviour , IGetKnockedBack ,ITakeDamage
    {   [SerializeField] private  float timeDelay = 1f;
        [SerializeField] private float timeBeingKnocked = 2.5f;
        [SerializeField] private float afterAttckTime =1f; 
        [SerializeField] private float beforeAttckTime = 0.3f;
        [SerializeField] private Transform BoxAttackPrefab;
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private  float moveSpeed = 2f;
        [SerializeField] private Transform playerPos;
        [SerializeField] private float awarenessRange = 2f;
        [SerializeField] private float attackRange = 0.3f;
        [SerializeField] private float maxHealth = 5f;
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        private State _state;
        private int index;
        private Transform targetPos;
        private bool canAttack;
        private float currentHealth;
        private Vector3 moveDir;
        private bool beingKnocked;
        private float hurtTimer;


        public UnityEvent AtDeath;
        [SerializeField] private AudioClip slimeAttacksound;
        [SerializeField] private float attackVol = 1;

        public enum State
        {
            Roaming,
            Follow,
            Attack,
            Dead,
            Hurt,
        }

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponentInChildren<Animator>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _state = State.Roaming;
            index = 0;
            targetPos = waypoints[index];
            currentHealth = maxHealth;
        }

        private void Update()
        {
            FlipSprite();
            
            switch (_state)
            {
                    
                case State.Roaming:
                {    targetPos = waypoints[index];
                    if (Vector2.Distance(transform.position, targetPos.position) < 0.2f)
                    {
                        index++;
                        if (index >= 4)
                        {
                            index = 0;
                        }
                    }

                    if (Vector2.Distance(transform.position, playerPos.position) < awarenessRange)
                    {
                        _state = State.Follow;
                    }
                    break;
                }
                case State.Follow:
                {
                    canAttack = true;
                    targetPos.position = playerPos.position;
                    if (Vector2.Distance(transform.position, playerPos.position) > awarenessRange)
                    {
                        _state = State.Roaming;
                    }
                    if (Vector2.Distance(transform.position, playerPos.position) < attackRange)
                    {
                        _state = State.Attack;
                    }
                    break;
                }
                case State.Attack:
                {
                    targetPos.position = transform.position;
                    if (canAttack)
                    {
                        _animator.SetTrigger("Attack");
                        canAttack = false;
                        StartCoroutine(Attack());
                    }
                   
                    break;
                }
                case State.Dead:
                {
                     targetPos.position = transform.position;
                     _rigidbody2D.velocity = Vector2.zero;
                     break;
                }
                case State.Hurt:
                {
                    if (hurtTimer < 0)
                    {
                         beingKnocked = false;
                         _state = State.Follow;
                    }

                    hurtTimer -= Time.deltaTime;
                  break;
                } 
            }
        }

        private IEnumerator Attack()
        {
            AudioSystem.Instance.PlaySound(slimeAttacksound,attackVol); 
            yield return new WaitForSeconds(beforeAttckTime);
            Instantiate(BoxAttackPrefab, transform.position, quaternion.identity);
            yield return new WaitForSeconds(afterAttckTime);
            _state = State.Follow;
        }

        private void FlipSprite()
        {
            moveDir = (transform.position - targetPos.position);
            if (moveDir.x > 0)
            {
                _spriteRenderer.flipX = false;
            }

            if (moveDir.x < 0)
            {
                _spriteRenderer.flipX = true;
            }
        }

        private void FixedUpdate()
        {
            if (!beingKnocked)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos.position, moveSpeed *Time.deltaTime);
            }

        }

        public void GetKnockedBack(Vector3 direction, float knockBackStrength)
        {
            beingKnocked = true;
            
            hurtTimer = timeDelay;
            transform.position += direction * knockBackStrength;
        }

        public void TakeDamage(float damage)
        {
            if (_state != State.Dead)
            {
                 _state = State.Hurt;
                            _animator.SetTrigger("Hurt");
                            currentHealth -= damage;
                            if (currentHealth <= 0)
                            {
                                Death();
                            }
            }
           
        }
        private void Death()
        {
            AtDeath.Invoke();
            _state = State.Dead;
            
           _animator.SetTrigger("Dead");
        }
    }
}