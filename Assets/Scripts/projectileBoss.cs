using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileBoss : MonoBehaviour {

    int damage = 1;

    GameObject target;
    GameObject managerScene;

    ManagerScene sceneManagerUpdated;

    CameraShake cam;

    public void updateScore()
    {
        sceneManagerUpdated.score += 3;
    }

    void FixedUpdate()
    {
        //Parte que procura a câmera e pega o componente script CameraShake
        GameObject camSearch = GameObject.FindGameObjectWithTag("MainCamera");
        cam = camSearch.GetComponent<CameraShake>();

        managerScene = GameObject.FindGameObjectWithTag("SceneManager");
        sceneManagerUpdated = managerScene.GetComponent<ManagerScene>();
    }

    //Função para a colisão da bala com o player
    void OnTriggerEnter2D(Collider2D c)
    {
        Player damageableObject = c.GetComponent<Player>();

        //Se a tag do inimigo estiver como vermelha e como este projétil é o verde, então o dano deve ser causado. Função CamShake utilizada pra sacudir a tela 
        if (damageableObject != null && damageableObject.tag == "red")
        {
            TrashMan.spawn("Hit", gameObject.transform.position, gameObject.transform.rotation);
            cam.Shake(0.5f, 0.3f, 3f);
            damageableObject.takeDamage(damage);
        }

        //Se a tag for verde, então spawnar o efeito visual de absorção e despawnar a bala.
        if (damageableObject != null && damageableObject.tag == "green")
        {
            TrashMan.spawn("Hit_Absorbed_Green", damageableObject.transform.position, damageableObject.transform.rotation);
            updateScore();

        }

    }
}
