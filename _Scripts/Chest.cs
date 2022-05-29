using System;
using UnityEngine;

namespace _Scripts
{
    public class Chest: MonoBehaviour
    {
        private Animator _animator;
        private bool opened;
        [SerializeField] private AudioClip openChestSound;
        [SerializeField] private float openChestVol = 1f;

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var player = col.GetComponent<PlayerController>();
            if (player != null&& !opened)
            {
                opened = true;
                _animator.SetTrigger("Hit");
                AudioSystem.Instance.PlaySound(openChestSound,openChestVol);
            }
                
                 
        }
    }
}