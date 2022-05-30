using System;
using UnityEngine;

namespace _Scripts
{
    public class EndLevelCollider : MonoBehaviour
    {
        [SerializeField] private string NextScene;
        private void OnTriggerEnter2D(Collider2D col)
        {
            var player = col.GetComponent<PlayerController>();
            if (player != null)
            {
                LevelManager.Instance.LoadScene(NextScene);
            }
        }
    }
}