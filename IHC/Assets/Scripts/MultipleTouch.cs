using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTouch : MonoBehaviour {

    [SerializeField]
    int vida;
    [SerializeField]
    int xp;

    [SerializeField]
    bool DEBUG_MODE;
    
    public List<TouchLocation> touches = new List<TouchLocation>();
    Vector2 movimento;

    // Inicializamos as variáveis no que nós queremos
    void Start(){
        vida = 100;
        xp = 0;
    }

	// Está constantemente a executar
	void Update () {

        // Iteramos por todos os toques (multitouch)
        int i = 0;
        while(i < Input.touchCount){
            Touch t = Input.GetTouch(i);

            // Se o toque está na fase inicial guardamos o seu id e a posição inicial
            if(t.phase == TouchPhase.Began){
                if (DEBUG_MODE) Debug.Log("touch began");
                touches.Add(new TouchLocation(t.fingerId, t.position));
            }
            // O toque acabou logo temos de agir
            else if(t.phase == TouchPhase.Ended){
                if (DEBUG_MODE) Debug.Log("touch ended");

                // Vamos buscar o toque que guardamos e removê-lo da lista de toques ativos
                TouchLocation thisTouch = touches.Find(TouchLocation => TouchLocation.touchId == t.fingerId);
                touches.RemoveAt(touches.IndexOf(thisTouch));
                
                // Identificar a câmara em que estamos
                Camera playerCamera = GetComponent<Camera>();

                if (playerCamera != null) {
                    // Converte a posição do toque para o mundo da câmera do jogador
                    Vector2 worldPosition = playerCamera.ScreenToWorldPoint(thisTouch.positionBegan);

                    // Verifica se a posição do toque está dentro da área visível da câmera
                    if (playerCamera.pixelRect.Contains(thisTouch.positionBegan)) {
                        if (DEBUG_MODE) Debug.Log("HORA DE MEXER: Clique dentro do GameObject " + playerCamera);

                        // Para onde mexer, usamos a posição final e inicial do dedo para identificar a direção, se forem iguais é clique
                        movimento = thisTouch.direction(t.position);

                        // Se o movimento for 0 é um toque -->> Abre Menu Interação
                        if (movimento == Vector2.zero) {
                            Debug.Log("CLIQUE MENU INTERAÇÃO");
                        }
                        else {
                            // Vemos se há colisão
                            Vector3 newPosition = new Vector3(transform.position.x + movimento.x, transform.position.y + movimento.y, transform.position.z);
                            Collider2D myCollider = GetComponent<Collider2D>();

                            if (Physics2D.OverlapCircle(newPosition, myCollider.bounds.extents.x, LayerMask.GetMask("Player"))){
                                Debug.Log("BATEU PLAYER MENU BATALHA");
                            }
                            else if (Physics2D.OverlapCircle(newPosition, myCollider.bounds.extents.x, LayerMask.GetMask("Wall"))){
                                if (DEBUG_MODE) Debug.Log("WALL");
                                vida -= 1;
                            }
                            else {
                                transform.position = newPosition;
                                vida -= 1;
                            }
                        }
                    } else {
                        if (DEBUG_MODE) Debug.Log("Clique fora da camara!");
                    }
                }
                // Collider2D myCollider = GetComponent<Collider2D>();
                // if (myCollider.OverlapPoint(thisTouch.positionBegan)) {
                //     if (DEBUG_MODE) Debug.Log("HORA DE MEXER: Clique dentro do GameObject " + myCollider.tag);

                //     // Para onde mexer, usamos a posição final e inicial do dedo para identificar a direção, se forem iguais é clique
                //     movimento = thisTouch.direction(t.position);
                //     if (movimento != Vector2.zero) transform.position = (Vector2)transform.position + movimento;
                //     else Debug.Log("CLIQUE");
                // }
                
                
            }
            //else if(t.phase == TouchPhase.Moved){
            //     if (DEBUG_MODE) Debug.Log("touch is moving");
            //     TouchLocation thisTouch = touches.Find(TouchLocation => TouchLocation.touchId == t.fingerId);
            // }
            ++i;
        }
	}

    void OnCollisionEnter(Collision collision){
        Debug.Log("Bateu");
    }
}