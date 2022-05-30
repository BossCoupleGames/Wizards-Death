using Cinemachine;
using UnityEngine;

namespace _Scripts
{
    public class ScreenShake : StaticInstance<ScreenShake>
    {
        [SerializeField] private CinemachineVirtualCamera playerCamera;
       [SerializeField] private CinemachineVirtualCamera bossVirtualCamera;
      
       private float shakeTimer;
       private CinemachineVirtualCamera cinemachineVirtualCamera;
       private float timerToCheck;

       private void Start()
        {
            
        }

        private void Update()
        {
            CheckWhichCameraToShake();
           
            if (shakeTimer > 0)
            {
                shakeTimer -= Time.deltaTime;
                if (shakeTimer <= 0f)
                {
                    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                    cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
                }
            }
        }

        private void CheckWhichCameraToShake()
        {
            if (timerToCheck < 0)
            {
                timerToCheck = 1.5f;
                if (playerCamera.Priority > bossVirtualCamera.Priority) 
                            { 
                                cinemachineVirtualCamera = playerCamera;
                            }
                            if (playerCamera.Priority < bossVirtualCamera.Priority)
                            { 
                                cinemachineVirtualCamera = bossVirtualCamera;
                            }
            }

            timerToCheck -= Time.deltaTime;
        }

        public void ShakeCamera(float intensity, float time)
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
            shakeTimer = time;
        }
    }
}