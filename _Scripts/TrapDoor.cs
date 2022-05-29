using System;
using System.Collections;
using UnityEngine;

namespace _Scripts
{
    public class TrapDoor: MonoBehaviour
    {

        [SerializeField] private bool BrokeAlready;
        private Animator _animator;
        [SerializeField]private float timebetweenSprites = 4f;
        [SerializeField] private Collider2D _collider2D;
        [SerializeField] private AudioClip TrapDoorSound;
        [SerializeField] private float trapDoorVol = 1;

        private void Start()
        {
            
            _animator = GetComponentInChildren<Animator>();

            if (BrokeAlready)
            {
                StartCoroutine(StartFall());
            }
                
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var something = col.GetComponent<ITakeDamage>();

            if (something != null)
            {
                StartCoroutine(StartFall());
            }
        }

        private IEnumerator StartFall()
        {
            _animator.SetTrigger("Hit");
            AudioSystem.Instance.PlaySound(TrapDoorSound,trapDoorVol,transform); 
            yield return new WaitForSeconds(timebetweenSprites);
            _collider2D.enabled = true;

        }
    }
}