using System;
using UnityEngine;

namespace _Scripts
{
    public class OnTriggerEnter2dPausePlayer:MonoBehaviour
    
    {
        private bool Hit;

        private void OnTriggerEnter2D(Collider2D col)
        {
            var player = col.GetComponent<PlayerController>();
            if (player != null)
            {
                if (!Hit)
                {
                     player.PausePlayer(); 
                     Hit = true;
                }
            }
        }
    }
}