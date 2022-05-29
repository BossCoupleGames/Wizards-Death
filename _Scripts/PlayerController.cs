using System.Collections;
using _Scripts;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour,IGetKnockedBack,ITakeDamage
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpSpeed = 3f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float decceleration = 1f;
    [SerializeField] private float stopPower = 0.5f;
    [SerializeField] private float turnPower = 1f;
    [SerializeField] private float accelPower =0.5f;
    [SerializeField] private Transform fireballprefab;
    [SerializeField] private Transform staffEndPos;
    [SerializeField] private Transform staffEndPos2;
    [SerializeField] private float timeBetweenfireShoot =0.5f;
    [SerializeField] private float timeAfterShot =0.25f;
    [SerializeField] private float timeBeforeWarp = 0.25f;
    [SerializeField] private float timeAfterWarp =0.25f ;
    [SerializeField] private PlayerData _playerData;
    
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private float currentHealth;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDir;
    private Vector3 lastMoveDir;
    private float lastJumpInputTime;
    private float jumpTime;
    private State _state;
    private Vector3 shootDir;
    private Vector3 aimDirection;
    private float coolDownShotTimer;
    private bool canShoot;
    private Transform staffends;
    private bool canWarp;
    private float fireOneReady;
    private float StartingHealth;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private float shotVol = 1;
    [SerializeField] private AudioClip warpingSound;
    [SerializeField] private float warpvol = 1f;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private float hurtvol = 1f;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private float deathvol = 1f;


    public enum State
    {
        Idle,
        Walking,
        Rolling,
        Shoot,
        Dead,
        Pause,
    }
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        currentHealth = _playerData.currentHealth;
        StartingHealth = _playerData.currentHealth;
    }

   

    private void Update()
    {
        _playerData.coolDownShotTime =coolDownShotTimer;
        _playerData.coolDownWarpTime = jumpTime;
        if (_state != State.Pause)
        {

            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            if (_state != State.Shoot || _state != State.Rolling)
            {
                moveDir = new Vector2(horizontalInput, verticalInput).normalized;
            }

            if (_state == State.Dead)
            {_playerData.currentHealth = StartingHealth;
                moveDir = new Vector3(0, 0, 0);
                                return;
            }
                
            HandleAiming();
            HandleShooting();
            FlipSprite();

            if (moveDir.x != 0 || moveDir.y != 0)
            {
                _animator.SetFloat("MoveX", lastMoveDir.x);
                _animator.SetFloat("MoveY", lastMoveDir.y);
                lastMoveDir = moveDir.normalized;
            }

            if (Input.GetButton("Jump"))
            {
                lastJumpInputTime = 0.1f;
            }

            if (lastJumpInputTime > 0 && jumpTime < 0)
            {
                _state = State.Rolling;
            }
        }

        switch (_state)
        {
            case State.Idle:
            {
                canWarp = true;
                canShoot = true;
                // play idle anim
                if (moveDir.x != 0 || moveDir.y != 0)
                {
                    _state = State.Walking;
                }
                break;
            }
            case State.Walking:
            {
                canWarp = true;
                canShoot = true;
                _animator.SetBool("Walking",true);
                _animator.SetFloat("RunX",moveDir.x);
                _animator.SetFloat("RunY",moveDir.y);
                if ((moveDir.x < 0.2f && moveDir.x >-0.2f) && (moveDir.y  < 0.2f && moveDir.y >-0.2f))
                {
                    _animator.SetBool("Walking", false);
                    Debug.Log("idle");
                    _state = State.Idle;
                }
                
                break;
               
            }
            case State.Rolling:
            {
                moveDir = new Vector3(0, 0);
                break;
            }
            case State.Shoot:
            {
                moveDir = new Vector3(0, 0);
                break;
            }
            case State.Dead:
                
            {
                _animator.SetTrigger("Death");
                moveDir = new Vector3(0, 0);
                
                break; 
            }
            case State.Pause:
            {
                moveDir = new Vector3(0, 0);
                break;
            }
        }


        coolDownShotTimer -= Time.deltaTime;
    }

    private void HandleShooting()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            fireOneReady = 0.15f;
        }
        if (fireOneReady>0 && coolDownShotTimer < 0)
        {
            coolDownShotTimer = 1f;
            _state = State.Shoot;
        }
        fireOneReady -= Time.deltaTime;
    }

    private void FlipSprite()
    {
        if (_state== State.Shoot)
        {
            if (shootDir.x < 0)
            {
                 _spriteRenderer.flipX = false;
                 staffends = staffEndPos;
            }

            if (shootDir.x > 0)
            {
                _spriteRenderer.flipX = true;
                staffends = staffEndPos2;
            }
        }

        if (_state != State.Shoot)
        {
             if (moveDir.x > 0 )
                    {   
                        staffends = staffEndPos2;
                        _spriteRenderer.flipX = true;
                        
                    }
                    if (moveDir.x < 0)
                    {
                        _spriteRenderer.flipX = false;
                        staffends = staffEndPos;
                    }
        }
       

    }

    private void HandleAiming()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         aimDirection = (worldPosition - transform.position).normalized;
        shootDir = new Vector3(aimDirection.x, aimDirection.y).normalized;
    }

    private void FixedUpdate()
    {
        _playerData.currentHealth = currentHealth;
        float targetSpeedx = moveDir.x * moveSpeed;
        float targetSpeedy = moveDir.y * moveSpeed;
                
        float speedDif = targetSpeedx - _rigidbody2D.velocity.x;
        float speedDify = targetSpeedy - _rigidbody2D.velocity.y;
                
        float accelRate = (Mathf.Abs(targetSpeedx) > 0.01) ? acceleration : decceleration;
        float accelRatey = (Mathf.Abs(targetSpeedy) > 0.01) ? acceleration : decceleration;
                      
        float velPower;
        if (Mathf.Abs(targetSpeedx) < 0.01f)
        {
            velPower = stopPower;
        }
        else if (Mathf.Abs(_rigidbody2D.velocity.x) > 0 && (Mathf.Sign(targetSpeedx) != Mathf.Sign(_rigidbody2D.velocity.x)))
        {
            velPower = turnPower;
        }
        else
        {
            velPower = accelPower;
        }
                
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        float movementy =Mathf.Pow(Mathf.Abs(speedDify) * accelRatey, velPower) * Mathf.Sign(speedDify);
        _rigidbody2D.AddForce(movement * Vector2.right);
        _rigidbody2D.AddForce(movementy * Vector2.up);
        
        switch (_state)
        {
            case State.Shoot:
            {
                if (canShoot)
                {
                    StartCoroutine(Shoot());
                     canShoot = false;
                }
               
                
                break;
            }
            
            case State.Rolling:
            {
                if (canWarp)
                {
                    StartCoroutine(Warping());
                    canWarp = false;
                }
                
                break;
            }
        }
        jumpTime -= Time.deltaTime;
        lastJumpInputTime -= Time.deltaTime;
    }

    private IEnumerator Warping()
    {
        _animator.SetTrigger("Warp");
        AudioSystem.Instance.PlaySound(warpingSound, warpvol); 
        jumpTime = 2.5f;
        _rigidbody2D.MovePosition (transform.position + lastMoveDir *jumpSpeed);
        yield return new WaitForSeconds(timeAfterWarp);
                       _state = State.Walking;
    }
    private IEnumerator Shoot()
    {
        _animator.SetFloat("ShotDirX", shootDir.x); 
        _animator.SetFloat("ShotDirY", shootDir.y);
        _animator.SetTrigger("Shoot");
        AudioSystem.Instance.PlaySound(shootSound,shotVol); 
        yield return new WaitForSeconds(timeBetweenfireShoot);
        Transform fireballTransform = Instantiate(fireballprefab, staffends.position, Quaternion.identity);
        fireballTransform.GetComponent<FireBall>().Setup(shootDir);
        yield return new WaitForSeconds(timeAfterShot);
        _state = State.Idle;
    }

    public void TurnOffSprite()
    {
        _spriteRenderer.enabled = false;
    }
    public void TurnOnSprite()
    {
        _spriteRenderer.enabled = true;
    }
    public void GetKnockedBack(Vector3 direction, float knockBackStrength)
    {
        _rigidbody2D.AddForce(direction * knockBackStrength,ForceMode2D.Impulse);
    }

    public void TakeDamage(float damage)
    {
        ScreenShake.Instance.ShakeCamera(2.5f,0.2f);
        if (_state != State.Dead)
        {
             _animator.SetTrigger("Hurt");
             AudioSystem.Instance.PlaySound(hurtSound,hurtvol);
                    currentHealth -= damage;
                    if (currentHealth < 1 && _state != State.Dead)
                    {
                        AudioSystem.Instance.PlaySound(deathSound,deathvol);
                        Death();
                        _animator.SetTrigger("Death");
                    }
        }
       
    }

    public void PausePlayer()
    {
        _state = State.Pause;
    }

    public void UnpausePlayer()
    {
        _state = State.Idle;
    }
    private void Death()
    {
        _state = State.Dead;
        DeathCanvas.Instance.PullUp();
       
    }
}
