using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class Tourch: MonoBehaviour
    {

        private Animator _animator;
        private float timeTillStart;
        private bool Started;

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            timeTillStart = Random.Range(1, 5);
        }

        private void Update()
        {
            if(Started)
                return;
            
            if (timeTillStart < 0)
            {
               
                _animator.SetTrigger("Hit"); 
                Started = true;
            }

            timeTillStart -= Time.deltaTime;
        }
    }
}