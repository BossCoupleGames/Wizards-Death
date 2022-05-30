using System;
using System.Collections;
using UnityEngine;

namespace _Scripts
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Portal otherPortal;
        [SerializeField] private float timeBeforeWarp = 1f;
        [SerializeField] private float timeAfterWarp = 1f;

        private Animator _animator;
        [SerializeField] private AudioClip portalSound;
        [SerializeField] private float portalVol =1f;

        public Transform SpawnPoint
        {
            get { return spawnPoint; }
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var player = col.GetComponent<PlayerController>();
            if (player != null)
            {
                _animator.SetTrigger("Hit");
                StartCoroutine(SpawnPlayerNewSpot(player));
            }
        }

        private IEnumerator SpawnPlayerNewSpot(PlayerController playerController)
        {
            playerController.PausePlayer();
            playerController.TurnOffSprite(); 
            AudioSystem.Instance.PlaySound(portalSound,portalVol);
            yield return new WaitForSeconds(timeBeforeWarp);
            playerController.transform.position = otherPortal.SpawnPoint.position;
            yield return new WaitForSeconds(timeAfterWarp);
            otherPortal._animator.SetTrigger("Hit");
            yield return new WaitForSeconds(0.3f);
            playerController.TurnOnSprite();
            playerController.UnpausePlayer();
        }
    }
}