using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionProjectileBombardier : MonoBehaviour {
    int damage = 10;

    void OnTriggerEnter2D(Collider2D c)
    {
        LivingEntity damageableObject = c.GetComponent<LivingEntity>();
        //Se a tag do inimigo estiver como vermelha e como este projétil é o verde, então o dano deve ser causado. Função CamShake utilizada pra sacudir a tela 
        if (damageableObject != null && damageableObject.tag == "Enemy")
        {
            //TrashMan.spawn("Hit_Enemy", gameObject.transform.position, gameObject.transform.rotation);
            damageableObject.TakeDamage(damage);            
        }

    }
}
