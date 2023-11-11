using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour {
    
    public RadialButton buttonPrefab;
    public RadialButton selected;
    private GameObject gameObj;
    public bool spawned = false;
    public MultipleTouch mtt;

    // Start is called before the first frame update
    public void spawnButtons(RadialMenuSpawner.Action[] menu, MultipleTouch mt) {
        // Debug.Log(menu.Length);
        for (int i=0; i < menu.Length; i++) {
            RadialButton newButton = Instantiate(buttonPrefab) as RadialButton;
            newButton.transform.SetParent(transform, false);
            // Debug.Log(menu[i].title);
            float theta;
            if (menu.Length == 4) {
                theta = (2 * Mathf.PI / menu.Length) * i + (45 * Mathf.PI / 180.0f);
            } else {
                theta = (2 * Mathf.PI / menu.Length) * i;
            }
            float xPos = Mathf.Sin(theta);
            float yPos = Mathf.Cos(theta);
            newButton.transform.localPosition = new Vector3(xPos, yPos, 0f) * 45f;
            newButton.circle.color = menu[i].color;
            newButton.icon.sprite = menu[i].sprite;
            newButton.title = menu[i].title;
            newButton.myMenu = this;
        }
        spawned = true;
        gameObj = gameObject;
        // Debug.Log("Game Object: ");
        // Debug.Log(gameObj);
        mtt = mt;
    }


    void Update() {
        if (selected) {
            Debug.Log(selected.title + " was selected!");
            if (spawned) {
                // Debug.Log("Game Object DESTROY: ");
                // Debug.Log(gameObj);
                Destroy(gameObj);
                if(!mtt.checking) {
                    if (selected.title.Equals("Harvest")){
                        mtt.help = 1;
                    } else if (selected.title.Equals("Sow")){
                        mtt.help = 2;
                    } else if (selected.title.Equals("Save")){
                        mtt.help = 3;
                    } else if (selected.title.Equals("Fight")){
                        mtt.help = 4;
                    } else if (selected.title.Equals("Steal")){
                        mtt.help = 5;
                    } else if (selected.title.Equals("Flee")){
                        mtt.help = 6;
                    } else if (selected.title.Equals("Share")){
                        mtt.help = 7;
                    }
                }
            }
        }
    }

    

}
