using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class SkullBoss : MonoBehaviour, ITakeDamage
    {
        [SerializeField] private float talkRange = 8f;
        [SerializeField] private float awarenessRange = 4;
        [SerializeField] private Animator hands;
        [SerializeField] private float shortPausetime =4f;
        [SerializeField] private float maxHealth = 8f;
        
        
        public Dialog Dialog;
        public Dialog DeathDialog;
        public UnityEvent fightBegins;
        public UnityEvent Dead;
        
        private PlayerController _playerController;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider2D;
        private State _state;
        private bool talking;
        private List<State> attackStates = new List<State>();
        private bool notAttack;
        private float clockTime;
        private bool declocked;
        private float timeToState;
        private float currentHealth;
        private bool isDead;
        
        [SerializeField] private AudioClip hurtSound;
        [SerializeField] private float HurtVol = 1;
        [SerializeField] private AudioClip DeathSound;
        [SerializeField] private float DeathVol = 1;
        [SerializeField] private AudioClip BossMusic ;
        [SerializeField] private float BossMusicVol = 1;
        [SerializeField] private AudioClip backroundMusic;
        [SerializeField] private float backroundMusicVol =1f;


        public enum State
        {
            Idle,
            Talking,
            UnCloaked,
            LeftSwing,
            RightSwing,
            BothSwing,
            WideRSwing,
            WideLSwing,
            ShortPause,
            RotSwing,
            Dead,
        }
        private void Start()
        {
            _animator = GetComponentInChildren<Animator>(); 
            _collider2D = GetComponent<Collider2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _playerController = FindObjectOfType<PlayerController>();
            currentHealth = maxHealth;
            attackStates.Add(State.BothSwing);
            attackStates.Add(State.LeftSwing);
            attackStates.Add(State.RightSwing);
            attackStates.Add(State.WideLSwing);
            attackStates.Add(State.WideRSwing);
            attackStates.Add(State.RotSwing);
            
        }

        private void Update()
        {
            if (clockTime < 0&& declocked)
            {
                declocked = false;
                 DeClock();
            }
               
            switch (_state)
            {
                case State.Idle:
                {
                    if (Vector2.Distance(transform.position, _playerController.transform.position) < talkRange)
                    {
                        _state = State.Talking;
                    }
                    break;
                }
                case State.Talking:
                {
                    
                    if (!talking)
                    {
                        talking = true;
                        _animator.SetTrigger("Talking");
                        AudioSystem.Instance.PlayMusic(BossMusic,BossMusicVol);
                         DialogSystem.Instance.StartDialog(Dialog);
                    }

                    if (Vector2.Distance(transform.position, _playerController.transform.position) < awarenessRange)
                    {
                        
                        fightBegins.Invoke();
                        _state = State.UnCloaked;
                    }
                    break;
                }
                case State.UnCloaked:
                {
                    declocked = true;
                    notAttack = false;
                    _spriteRenderer.enabled = false;
                    _collider2D.enabled = false;
                    _animator.enabled = false;
                    clockTime = 2f;
                    timeToState = shortPausetime;
                    GetRandomAttackState();
                    break;
                }
                case State.BothSwing:
                {
                    if (!notAttack)
                    {
                        notAttack = true;
                         hands.SetTrigger("Both");
                         _state = State.ShortPause;
                    }
                    
                }
                    break;
                case State.LeftSwing:
                    if (!notAttack)
                    {
                        notAttack = true;
                        hands.SetTrigger("Left");
                        _state = State.ShortPause;
                    }
                    break;
                case State.RightSwing:
                    if (!notAttack)
                    {
                        notAttack = true;
                        hands.SetTrigger("Right");
                        _state = State.ShortPause;
                    }
                    break;
                case State.WideLSwing:
                    if (!notAttack)
                    {
                        notAttack = true;
                        hands.SetTrigger("WideLeft");
                        _state = State.ShortPause;
                    }
                    break;
                case State.WideRSwing:
                    if (!notAttack)
                    {
                        notAttack = true;
                        hands.SetTrigger("WideRight");
                        _state = State.ShortPause;
                    }
                    break;
                case State.RotSwing:
                    if (!notAttack)
                    {
                        notAttack = true;
                        hands.SetTrigger("Rot");
                        _state = State.ShortPause;
                    }
                    break;
                case State.ShortPause:
                {
                    if (timeToState < 0)
                    {
                        _state = State.UnCloaked;
                    }

                    timeToState -= Time.deltaTime;
                    break;
                }
                case State.Dead:
                {
                    _animator.SetTrigger("Dead");
                    break;
                }
            }

            clockTime -= Time.deltaTime;
        }

        private void DeClock()
        {
            
            _spriteRenderer.enabled = true;
            _collider2D.enabled = true;
            _animator.enabled = true;
        }

        private void FixedUpdate()
        {
            
        }
        private void GetRandomAttackState()
        {
            _animator.SetTrigger("Attack");
            if (attackStates.Count > 0)
            {
                int randomIndexMax = attackStates.Count;
                int attackIndex = Random.Range(0, randomIndexMax);
                _state = attackStates[attackIndex];
                attackStates.Remove(attackStates[attackIndex]);
            }

            if (attackStates.Count == 0)
            { 
                attackStates.Add(State.BothSwing);
                attackStates.Add(State.LeftSwing);
                attackStates.Add(State.RightSwing);
                attackStates.Add(State.WideLSwing);
                attackStates.Add(State.WideRSwing);
                attackStates.Add(State.RotSwing);
            }
      
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (!isDead)
            {
                _animator.SetTrigger("Hurt"); 
                AudioSystem.Instance.PlaySound(hurtSound,HurtVol);
                            if (currentHealth < 1)
                            {
                                AudioSystem.Instance.PlaySound(DeathSound,DeathVol);
                                isDead = true;
                                DialogSystem.Instance.StartDialog(DeathDialog); 
                                Dead.Invoke();
                                _animator.SetTrigger("Dead");
                                _state = State.Dead;
                                AudioSystem.Instance.PlayMusic(backroundMusic,backroundMusicVol); 
                            }
            }
            
            
        }
    }
}