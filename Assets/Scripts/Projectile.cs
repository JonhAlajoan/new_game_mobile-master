using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------Código relacionado ao projétil verde inicial---------------------------------------------------

public class Projectile : MonoBehaviour {
    /*----------Atributos--------
     * Speed = velocidade do projétil
     * Damage = Quantidade de dano que o projétil causará, sendo passado como parâmetro na função takeDamage
     * Lifetime = Tempo de vida que a bala vai ter antes de ser despawnada
     * Target = Player que a bala procura atingir. Pode ser usado pra marcar outra coisa caso tenham itens de decoy por exemplo.
     * cam = Camera que vai ser procurada durante o update para poder utilizar a função Shake para sacudir a tela no impacto.
     */

    float speed = 5f;
    float lifetime;

    int damage = 1;
    
    GameObject target;

    Player damageableObject;

    CameraShake cam;

    //Função para modificar a velocidade da bala em runtime caso necessário
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Update()
    {
        //Parte que procura a câmera e pega o componente script CameraShake
        GameObject camSearch = GameObject.FindGameObjectWithTag("MainCamera");
        cam = camSearch.GetComponent<CameraShake>();

        //Movimentação do projétil
        float moveDistance = speed * Time.deltaTime;
        transform.Translate(Vector2.down * moveDistance);

        /* Os dois ifs procuram pelo player todo update baseado na tag atual green ou red, pra indicar se o shield no momento está verde ou vermelho
         * e o assimila para a variável target, a fim de que o player possa estar sempre na mira das balas.
         */
        if (GameObject.FindGameObjectWithTag("green"))
        {
            target = GameObject.FindGameObjectWithTag("green");
        }
        if (GameObject.FindGameObjectWithTag("red"))
        {
            target = GameObject.FindGameObjectWithTag("red");
        }

        //Não sei o que isso faz mas vou deixar por precaução
        damageableObject = target.GetComponent<Player>();

        //Setando o lifetime para ser 1 unidade de tempo por segundo
        lifetime += 1 * Time.deltaTime;
        
        //Mini função para despawnar a bala sem necessitar de uma corotina.
        if (lifetime > 3)
        {
            TrashMan.despawn(gameObject);
            lifetime = 0;
        }
        
    }
    
    //Função para a colisão da bala com o player
    void OnTriggerEnter2D(Collider2D c)
    {
        //Se a tag do inimigo estiver como vermelha e como este projétil é o verde, então o dano deve ser causado. Função CamShake utilizada pra sacudir a tela 
        if (damageableObject != null && damageableObject.tag == "red")
        {
            TrashMan.spawn("Hit", gameObject.transform.position, gameObject.transform.rotation);
            cam.Shake(0.5f, 0.3f);
            damageableObject.takeDamage(damage);
        }

        //Se a tag for verde, então spawnar o efeito visual de absorção e despawnar a bala.
        else
        {
            TrashMan.spawn("Hit_Absorbed_Green", target.transform.position, target.transform.rotation);
        }
        TrashMan.despawn(gameObject); 
    }

}
