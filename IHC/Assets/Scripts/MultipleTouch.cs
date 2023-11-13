using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System;

public class MultipleTouch : MonoBehaviour {

    [SerializeField]
    public int vida;
    [SerializeField]
    int xp;

    [SerializeField]
    bool DEBUG_MODE;
    
    public List<TouchLocation> touches = new List<TouchLocation>();
    Vector2 movimento;
    public bool checking;
    public int help;
    MultipleTouch player_collided;
    float last_c;
    public float debounce_t;

    // Inicializamos as variáveis no que nós queremos
    void Start(){
        this.vida = 100;
        this.xp = 0;
        this.checking = true;
        this.help = 0;
        this.last_c = 0f;
        this.debounce_t = 0.5f; 
    }

	// Está constantemente a executar
	void Update () {

        // Iteramos por todos os toques (multitouch)
        //Debug.Log(Time.time - last_c > debounce_t);
        int i = 0;
        while(i < Input.touchCount){
            // Debug.Log(Input.touchCount);
            Touch t = Input.GetTouch(i);

            // Se o toque está na fase inicial guardamos o seu id e a posição inicial
            if(t.phase == TouchPhase.Began){
                if (DEBUG_MODE) Debug.Log("touch began");
                TouchLocation thisTouch = touches.Find(TouchLocation => TouchLocation.touchId == t.fingerId);
                // Debug.Log(thisTouch);
                if (thisTouch == null){
                    touches.Add(new TouchLocation(t.fingerId, t.position));
                }
            }
            // O toque acabou logo temos de agir
            else if(t.phase == TouchPhase.Ended){
                if (DEBUG_MODE) Debug.Log("touch ended");

                // Vamos buscar o toque que guardamos e removê-lo da lista de toques ativos
                TouchLocation thisTouch = touches.Find(TouchLocation => TouchLocation.touchId == t.fingerId);
                if (thisTouch == null){
                    Debug.Log(thisTouch.touchId + " !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    continue;
                }
                Debug.Log(thisTouch.touchId);
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
                        if ((movimento == Vector2.zero) && (checking) && (Time.time - last_c > debounce_t)) {
                            // Debug.Log("CLIQUE MENU INTERAÇÃO");
                            RadialMenuSpawner.ins.SpawnMenu(playerCamera.WorldToScreenPoint(transform.position), this, 0);
                            checking = false;
                            last_c = Time.time;
                        }
                        else if (checking) {
                            // Vemos se há colisão
                            Vector3 newPosition = new Vector3(transform.position.x + movimento.x, transform.position.y + movimento.y, transform.position.z);
                            Collider2D myCollider = GetComponent<Collider2D>();

                            if (Physics2D.OverlapCircle(newPosition, myCollider.bounds.extents.x, LayerMask.GetMask("Player")) && (Time.time - last_c > debounce_t)){
                                Debug.Log(playerCamera + " Bateu " + Physics2D.OverlapCircle(newPosition, myCollider.bounds.extents.x, LayerMask.GetMask("Player")).gameObject);
                                player_collided = Physics2D.OverlapCircle(newPosition, myCollider.bounds.extents.x, LayerMask.GetMask("Player")).gameObject.GetComponent<MultipleTouch>();
                                //collided.GetComponent<MultipleTouch>().vida -= 10; 
                                transform.position = newPosition;
                                RadialMenuSpawner.ins.SpawnMenu(playerCamera.WorldToScreenPoint(transform.position), this, 1);
                                checking = false;
                                last_c = Time.time;
                            }
                            else if (Physics2D.OverlapCircle(newPosition, myCollider.bounds.extents.x, LayerMask.GetMask("Wall"))){
                                if (DEBUG_MODE) Debug.Log("WALL");
                                this.vida -= 1;
                            }
                            else {
                                transform.position = newPosition;
                                this.vida -= 1;
                            }
                        }
                    } else {
                        if (DEBUG_MODE) Debug.Log("Clique fora da camara!");
                    }
                }
            }
            ++i;
        }
        if ((this.help != 0) && (Time.time - last_c > debounce_t)) {
            switch(help){
                case 1:
                    Debug.Log("Harvest");
                    this.harvest();
                    break;
                case 2:
                    Debug.Log("Sow");
                    this.sow();
                    break;
                case 3:
                    this.saveXP();
                    this.updateVida();
                    break;
                case 4:
                    Debug.Log("Fight");
                    this.fight();
                    break;
                case 5:
                    Debug.Log("Steal");
                    this.steal();
                    break;
                case 6:
                    Debug.Log("Flee");

                    break;
                case 7:
                    Debug.Log("Share");
                    this.share();
                    break;
                default :
                    Debug.Log("GG");
                    break;
            }
            checking = true;
            help = 0;
            last_c = Time.time;
        }
        if (this.vida == 0) {
            UnityEngine.Rendering.Universal.Light2D player_light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            player_light.intensity = 0f;
            gameObject.SetActive(false);
            // Destroy(gameObject);
        }
	}

    private void saveXP(){
        this.xp += (int)(this.vida / 2);
    }

    private void updateVida(){
        this.vida = (int)(this.vida / 2);
    }

    private void fight() {
        int total_xp = player_collided.xp + this.xp;
        double chance_1, chance_2;
        // 0 - 0 -> 50% 50%
        if (total_xp == 0) {
            chance_1 = 50;
            chance_2 = 50;
        } else { // x!=0 - y!=0 -> a = x/(x+y) * 100  b = y/(x+y) * 100
            chance_1 = this.xp / total_xp;
            chance_2 = player_collided.xp / total_xp;
            if (chance_1 < 20) {
                chance_1 = 20;
                chance_2 = 80;
            } else if (chance_1 > 80) {
                chance_1 = 80;
                chance_2 = 20;
            }
        }
        // a < 20 -> 20% - b = 80
        // a > 80 -> 80% - b = 20
        int help = this.vida;
        if (randomNumber() <= chance_1){
            this.vida += (int)(player_collided.vida/2);
            if (this.vida > 100){
                this.vida = 100;
            }
            player_collided.vida -= (int)(player_collided.vida/2);
        } else {
            this.vida -= (int)(help/2);
            player_collided.vida += (int)(help/2);
        }
    }

    private int randomNumber() {
       return UnityEngine.Random.Range(0,100);
    }

    private void steal() {
        int chance = 25;
        if (randomNumber() <= chance) {
            this.vida += (int) (0.25 * player_collided.vida);
            if (this.vida > 100){
                this.vida = 100;
            }
            player_collided.vida -= (int) (0.25 * player_collided.vida);
        } else {
            // Dies
            gameObject.SetActive(false);
            Destroy(gameObject);
            UnityEngine.Rendering.Universal.Light2D player_light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            player_light.intensity = 0f;
        
        }
    }

    private void share() {
        int hp_transfer = this.vida/2;
        this.vida = (int)(this.vida/2) +  (int)(player_collided.vida/2);
        if (this.vida > 100){
            this.vida = 100;
        }
        player_collided.vida = (int)(player_collided.vida/2) + (int)hp_transfer;
    }

    private void harvest() {
        int x = (int)Math.Floor(transform.position.x), y = (int)Math.Floor(transform.position.y);
        this.vida += DebugTilemap.harvestHP(x, y);
        if (this.vida > 100){
            this.vida = 100;
        }
    }

    private void sow() {
        int x = (int)Math.Floor(transform.position.x), y = (int)Math.Floor(transform.position.y);
        int health_taken = (int)(this.vida * 0.3);
        DebugTilemap.sowHP(x, y, health_taken);
    }

}