using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Player : MonoBehaviour {
    /*----------Atributos--------
    * Speed = velocidade do projétil
    * Damage = Quantidade de dano que o projétil causará, sendo passado como parâmetro na função takeDamage
    * Lifetime = Tempo de vida que a bala vai ter antes de ser despawnada
    * Target = Player que a bala procura atingir. Pode ser usado pra marcar outra coisa caso tenham itens de decoy por exemplo.
    * cam = Camera que vai ser procurada durante o update para poder utilizar a função Shake para sacudir a tela no impacto.
    */

    float speed;

    int damage = 1;
	float acceleration;
    GameObject target;
    float count;
    LivingEntity damageableObject;

    CameraShake cam;

    int randomDir;

    void FixedUpdate()
    {
        
   
       float moveDistance = speed * Time.deltaTime;

		count += 1 * Time.deltaTime;
        
		
            speed = 30f;
           
            transform.Translate(Vector2.up * moveDistance);

       
        

        //Parte que procura a câmera e pega o componente script CameraShake
        GameObject camSearch = GameObject.FindGameObjectWithTag("MainCamera");
        cam = camSearch.GetComponent<CameraShake>();

        //Movimentação do projétil
        if(count>0.7f)
		{
			count = 0;
			TrashMan.despawn(gameObject);

		}
   

    }

    //Função para a colisão da bala com o player
    void OnTriggerEnter2D(Collider2D c)
    {
        LivingEntity damageableObject = c.GetComponent<LivingEntity>();
        //Se a tag do inimigo estiver como vermelha e como este projétil é o verde, então o dano deve ser causado. Função CamShake utilizada pra sacudir a tela 
        if (damageableObject != null && damageableObject.tag == "Enemy")
        {
			TrashMan.spawn("VFX_HIT_PPLAYER", gameObject.transform.position, gameObject.transform.rotation);
			Debug.Log("Camshake ativado");
            cam.Shake(0.05f, 0.07f,1.2f);
            
            damageableObject.TakeDamage(damage);
            Debug.Log("dano tomado");
            speed = 0.5f;
            count = 0;

			

			TrashMan.despawn(this.gameObject);
            
        }
        
    }   
}
