using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts
{
    public class LevelManager : StaticInstance<LevelManager>
    {
        [SerializeField] private PlayerData _playerData;
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void LoadSceneAfterDelay(string sceneName)
        {
            StartCoroutine(Delay(sceneName));
        }

        private IEnumerator Delay(string sceneName)
        {
            yield return new WaitForSeconds(0.25f);
            SceneManager.LoadScene(sceneName);
        }

        public void Hard()
        {
            _playerData.currentHealth = 10f;
        }
        public void Normal()
        {
            _playerData.currentHealth = 15f;
        }
        public void Easy()
        {
            _playerData.currentHealth = 20f;
        }
    }
}