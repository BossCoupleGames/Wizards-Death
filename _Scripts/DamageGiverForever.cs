using UnityEngine;

namespace _Scripts
{
    public class DamageGiverForever : MonoBehaviour
    {
        [SerializeField] private float damage = 1f;
        
    private void OnTriggerEnter2D(Collider2D collision)
        {
            var iDamage = collision.GetComponent<ITakeDamage>();
            if (iDamage != null)
            {
                iDamage.TakeDamage(damage);
            }
        }
    }
}