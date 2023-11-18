using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Teste : MonoBehaviour {

    int vida;
    int xp;
    private AudioSource audioSource;
    public AudioClip sound1;  // Atribua seus clipes de áudio na Unity Editor
    public AudioClip sound2;

    public bool checking = true;
    void Start(){
        Debug.Log("Yha Teste");
        vida = 100;
        xp = 0;
        audioSource = GetComponent<AudioSource>();
    }

    public void setBool() {
        checking = true;
    }

    void PlaySound(AudioClip clip)
    {
        // Verifica se o áudio não está reproduzindo para evitar sobreposição
        if (!audioSource.isPlaying)
        {
            // Atribui o novo áudio ao AudioSource
            audioSource.clip = clip;
            // Reproduz o áudio
            audioSource.Play();
        }
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            Camera playerCamera = GetComponent<Camera>();

            if (playerCamera != null) {
                // Converte a posição do mouse para o mundo da câmera do jogador
                Vector2 mousePosition = Input.mousePosition;
                Vector2 worldPosition = playerCamera.ScreenToWorldPoint(mousePosition);

                Debug.Log(worldPosition);
                if (playerCamera.pixelRect.Contains(mousePosition)){
                    Vector3 newPosition = new Vector3(transform.position.x + 1.0f, transform.position.y, transform.position.z);
                    //transform.position = transform.position + (new Vector3(1.0f, 0.0f, 0.0f));
                    Collider2D myCollider = GetComponent<Collider2D>();

                    if (Physics2D.OverlapCircle(newPosition, myCollider.bounds.extents.x, LayerMask.GetMask("Player"))) {
                        // Não há colisão, então mova o jogador
                        //transform.position = newPosition;
                        Debug.Log("BATEU NO JOGADOR");
                        PlaySound(sound1);
                    }
                    else if (Physics2D.OverlapCircle(newPosition, myCollider.bounds.extents.x, LayerMask.GetMask("Wall"))) {
                        Debug.Log("BATEU NA PAREDE");
                        //RadialMenuSpawner.ins.SpawnMenu(Input.mousePosition);
                        Debug.Log("Spawn Menu " + myCollider);
                    }
                    else {
                        //Debug.Log("Teste Bateu");
                        // if (checking){
                        //     RadialMenuSpawner.ins.SpawnMenu(playerCamera.WorldToScreenPoint(transform.position), this);
                        //     checking = false;
                        // }
                        Debug.Log("Checking: " + checking);
                        Debug.Log("Spawn Menu " + myCollider);
                        // audioSource.Play();
                        // AudioManager.HarvestSound();
                       // AudioManager.HarvestSound();
                       PlaySound(sound1);
                       //PlaySound(sound2);
                    }
                }
            }     
        }
    }
}