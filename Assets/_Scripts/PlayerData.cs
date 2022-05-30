using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "playerData")]
    public class PlayerData : ScriptableObject
    {
        public float currentHealth;
        public float coolDownShotTime;
        public float coolDownWarpTime;
    }
}