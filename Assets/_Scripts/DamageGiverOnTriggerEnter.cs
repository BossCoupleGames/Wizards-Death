 using System;
 using UnityEditor.Rendering;
 using UnityEngine;

 namespace _Scripts
 {


     public class DamageGiverOnTriggerEnter : MonoBehaviour
     {
         [SerializeField] private float damage = 1f;
         [SerializeField] private float knockbackStrength = 2f;
         private float timerTillDestory;


         private void Start()
         {
             timerTillDestory = 0.3f;
         }

         private void Update()
         {
             if (timerTillDestory < 0)
             {
                 Destroy(gameObject);
             }
             timerTillDestory -= Time.deltaTime;
         }

         private void OnTriggerEnter2D(Collider2D collision)
         {
             var iKnockBack = collision.GetComponent<IGetKnockedBack>();
             var iDamage = collision.GetComponent<ITakeDamage>();
             if (iKnockBack != null && knockbackStrength < 0.1)
             {
                 Vector2 dir = (transform.position - collision.transform.position).normalized;
                 iKnockBack.GetKnockedBack(-dir, knockbackStrength);
             }

             if (iDamage != null)
             {
                 iDamage.TakeDamage(damage);
             }
         }
     }
 }