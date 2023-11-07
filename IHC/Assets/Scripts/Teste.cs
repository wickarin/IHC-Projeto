using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teste : MonoBehaviour {

    int vida;
    int xp;

    void Start(){
        Debug.Log("Yha Teste");
        vida = 100;
        xp = 0;
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

                    if (!Physics2D.OverlapCircle(newPosition, myCollider.bounds.extents.x, LayerMask.GetMask("Player")) && !Physics2D.OverlapCircle(newPosition, myCollider.bounds.extents.x, LayerMask.GetMask("Wall"))) {
                        // Não há colisão, então mova o jogador
                        //transform.position = newPosition;
                    }
                    else {
                        //Debug.Log("Teste Bateu");
                    }
                }
            }     
        }
    }
}