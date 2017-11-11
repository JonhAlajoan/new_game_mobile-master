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

    GameObject target;
    float count;
    LivingEntity damageableObject;

    CameraShake cam;

    int randomDir;
    //Função para modificar a velocidade da bala em runtime caso necessário

    private void Start()
    {
        randomDir = Random.Range(0, 2);
        speed = 0.5f;
       count = 1 * Time.deltaTime;
    }
    void Update()
    {
        
       target = GameObject.FindGameObjectWithTag("Enemy");
       float moveDistance = speed * Time.deltaTime;
       
        count += 1 * Time.deltaTime;
        
        
            if (randomDir == 0)
                transform.Translate(Vector2.left * moveDistance);
            if(randomDir == 1)
                transform.Translate(Vector2.right * moveDistance);
        
        
        if (count > 1.5f)
        {
            speed = 7f;
            gameObject.transform.up = target.transform.position - gameObject.transform.position;
            transform.Translate(Vector2.up * moveDistance);

        }

        

        //Parte que procura a câmera e pega o componente script CameraShake
        GameObject camSearch = GameObject.FindGameObjectWithTag("MainCamera");
        cam = camSearch.GetComponent<CameraShake>();

        //Movimentação do projétil
        
   

    }

    //Função para a colisão da bala com o player
    void OnTriggerEnter2D(Collider2D c)
    {
        LivingEntity damageableObject = c.GetComponent<LivingEntity>();
        //Se a tag do inimigo estiver como vermelha e como este projétil é o verde, então o dano deve ser causado. Função CamShake utilizada pra sacudir a tela 
        if (damageableObject != null && damageableObject.tag == "Enemy")
        {
            //TrashMan.spawn("Hit_Enemy", gameObject.transform.position, gameObject.transform.rotation);
            Debug.Log("Camshake ativado");
            cam.Shake(0.1f, 0.1f,1.2f);
            Debug.Log("dano tomado");
            damageableObject.TakeDamage(damage);
            speed = 0.5f;
            count = 0;
            TrashMan.despawn(gameObject);
            
        }
        
    }   
}
