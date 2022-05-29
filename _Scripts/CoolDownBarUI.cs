using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class CoolDownBarUI : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Image warpReadyColor;
        [SerializeField] private Image fireReadyColor;
        [SerializeField] private Image warpCoolDownwheel;
        [SerializeField] private Image firCoolDwonWheel;

        private float warpCoolDwonTime;
        private float shotCoolDownTime;
        

        private void Update()
        {
            warpCoolDwonTime = _playerData.coolDownWarpTime;
            shotCoolDownTime = _playerData.coolDownShotTime;

            if (warpCoolDwonTime > 0)
            {
                warpReadyColor.enabled = false;
                 var fillamount = (warpCoolDwonTime / 2.5f);
                 warpCoolDownwheel.fillAmount = fillamount;
            }
            if (shotCoolDownTime > 0)
            {
                fireReadyColor.enabled = false;
                var fillamount = (shotCoolDownTime / 1f);
                firCoolDwonWheel.fillAmount = fillamount;
            }
            
            if (_playerData.coolDownShotTime < 0)
            {
                fireReadyColor.enabled = true;
            }

            if (_playerData.coolDownWarpTime < 0)
            {
                warpReadyColor.enabled = true;
            }
        }
    }
}