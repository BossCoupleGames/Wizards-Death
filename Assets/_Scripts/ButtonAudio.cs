using UnityEngine;

namespace _Scripts
{
    public class ButtonAudio: MonoBehaviour

    {

        [SerializeField] private AudioClip deathSound;
        [SerializeField] private float deathvol = 3f;

        public void PlayClick()
        {
            AudioSystem.Instance.PlaySound(deathSound,deathvol);
        }
    }
}