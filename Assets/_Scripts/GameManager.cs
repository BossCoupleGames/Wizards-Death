using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts
{
    public class GameManager : StaticInstance<GameManager>
    {
        [SerializeField] private List<TurretMain> _turretMains;
        public UnityEvent UnityEvent;
        
        private float timeToCheckEnemysLeft;

        private void Update()
        {
            if (timeToCheckEnemysLeft < 0)
            {
                timeToCheckEnemysLeft = 1.5f;
                CheckEnemys();
            }

            timeToCheckEnemysLeft -= Time.deltaTime;
        }

        private void CheckEnemys()
        {
            if (_turretMains.Count < 1)
            {
                UnityEvent.Invoke();
            }
        }

        public void KillEnemy(TurretMain main)
        {
            _turretMains.Remove(main);
        }
    }
}