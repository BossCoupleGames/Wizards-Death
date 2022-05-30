using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts
{
    public class DialogTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject flashingTab;
        public Dialog Dialog;
        public UnityEvent UnityEvent;
        private bool inRange;
        

        private void Update()
        {
            if (inRange)
            {
                if (Input.GetKeyDown("tab"))
                {
                    inRange = false;
                    flashingTab.SetActive(false);
                    DialogSystem.Instance.StartDialog(Dialog);
                    UnityEvent.Invoke();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var player = col.GetComponent<PlayerController>();
            if (player != null)
            {
                inRange = true;
                flashingTab.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                inRange = false;
                flashingTab.SetActive(false);
            }
        }
    }
}