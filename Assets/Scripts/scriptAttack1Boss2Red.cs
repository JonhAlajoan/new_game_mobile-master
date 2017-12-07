using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class scriptAttack1Boss2Red : MonoBehaviour {


    /*----------Atributos--------
     * Speed = velocidade do projétil
     * Damage = Quantidade de dano que o projétil causará, sendo passado como parâmetro na função takeDamage
     * Lifetime = Tempo de vida que a bala vai ter antes de ser despawnada
     * Target = Player que a bala procura atingir. Pode ser usado pra marcar outra coisa caso tenham itens de decoy por exemplo.
     * cam = Camera que vai ser procurada durante o update para poder utilizar a função Shake para sacudir a tela no impacto.
     */
    float lifetime;

    int damage = 1;

    GameObject target;

    GameObject managerScene;
    ManagerScene sceneManagerUpdated;

    CameraShake cam;

    public void Start()
    {
        lifetime = 1 * Time.deltaTime;
    }

    //Função para modificar a velocidade da bala em runtime caso necessário

    public void updateScore()
    {
        sceneManagerUpdated.score += 3;
    }

    void FixedUpdate()
    {
        try
        {
            //Parte que procura a câmera e pega o componente script CameraShake
            GameObject camSearch = GameObject.FindGameObjectWithTag("MainCamera");
            cam = camSearch.GetComponent<CameraShake>();

            managerScene = GameObject.FindGameObjectWithTag("SceneManager");
            sceneManagerUpdated = managerScene.GetComponent<ManagerScene>();
        }
        catch(NullReferenceException)
        {
            Debug.Log("nõa encontrado");
        }
    }

    //Função para a colisão da bala com o player
    void OnTriggerEnter2D(Collider2D c)
    {
        Player damageableObject = c.GetComponent<Player>();

        //Se a tag do inimigo estiver como verde e como este projétil é o vermelho, então o dano deve ser causado. Função CamShake utilizada pra sacudir a tela 
        if (damageableObject != null && damageableObject.tag == "green")
        {

            TrashMan.spawn("Hit_Red", gameObject.transform.position, gameObject.transform.rotation);
            cam.Shake(0.5f, 0.3f, 3f);
            damageableObject.takeDamage(damage);

        }
        //Se a tag for verde, então spawnar o efeito visual de absorção e despawnar a bala.
        if (damageableObject != null && damageableObject.tag == "red")
        {
            TrashMan.spawn("Hit_Absorbed_Red", damageableObject.transform.position, damageableObject.transform.rotation);
            updateScore();
        }

    }
}
