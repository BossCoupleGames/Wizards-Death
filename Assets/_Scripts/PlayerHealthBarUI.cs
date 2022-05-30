using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class PlayerHealthBarUI : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        public int numOfHearts = 3;

        public Image[] hearts;
        [SerializeField] private Sprite fullheart;
        [SerializeField] private Sprite emptyheart;

        private void FixedUpdate()
        {
            if (_playerData.currentHealth > numOfHearts)
            {
                _playerData.currentHealth = numOfHearts;
            }
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < _playerData.currentHealth)
                {
                    hearts[i].sprite = fullheart;
                }
                else
                {
                    hearts[i].sprite = emptyheart;
                }

                if (i < numOfHearts)
                {
                    hearts[i].enabled = true;
                }
                else
                {
                    hearts[i].enabled = false;
                }
            }
        }
    }
}