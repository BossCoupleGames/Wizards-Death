using UnityEngine;

namespace _Scripts
{
    public class AudioSystem : StaticInstance<AudioSystem>
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _soundsSource;
        [SerializeField] private AudioSource _soundsTransfrom2d;
        
        
        public void PlayMusic(AudioClip clip, float vol) {
            _musicSource.PlayOneShot(clip,vol);
            _musicSource.loop = true;
        }
        
        public void PlaySound(AudioClip clip, float vol ) {
            _soundsSource.PlayOneShot(clip, vol);
        }
        public void PlaySound(AudioClip clip, float vol , Transform transformpos)
        {
            transform.position = transformpos.position;
            _soundsTransfrom2d.PlayOneShot(clip,vol);
        }
    }
}