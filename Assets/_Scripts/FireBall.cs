using System;
using UnityEngine;

namespace _Scripts
{
    public class FireBall: MonoBehaviour
    {
       
            [SerializeField] private float _damage = 5;
            [SerializeField] private float knockBackStrength = 1;
            [SerializeField] private float moveSpeed = 10;
            private bool isEnemy;
            private Vector3 shootDir;
            private Animator _animator;
            private bool hitSomething;
            [SerializeField] private AudioClip fireBallSpashSound;
            [SerializeField] private float fireBallVol = 1;

            private void Start()
            {
                _animator = GetComponent<Animator>();
            }

            public void Setup(Vector3 shootDir)
            {
                this.shootDir = shootDir;
    
                transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(shootDir));
            }
    
            public static float GetAngleFromVectorFloat(Vector3 dir)
            {
                dir = dir.normalized;
                float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                if (n < 0) n += 360;
                return n;
            }
            private void FixedUpdate()
            {
                if (!hitSomething)
                {
                     transform.position += shootDir * moveSpeed * Time.deltaTime;
                }
               
            }
    

            private void OnCollisionEnter2D(Collision2D col)

            {
                hitSomething = true;
                AudioSystem.Instance.PlaySound(fireBallSpashSound,fireBallVol); 
                _animator.SetTrigger("Hit");
                
                var enemy = col.collider.GetComponent<ITakeDamage>();
                var enemyKnockBack = col.collider.GetComponent<IGetKnockedBack>();
                if (enemy != null)
                {
                   
                    enemy.TakeDamage(_damage);
                }

                if (enemyKnockBack != null)
                {
                     enemyKnockBack.GetKnockedBack(shootDir , knockBackStrength);
                }
                if (col.collider != null)
                {Destroy(gameObject,0.4f);}
            }
        }
    }
