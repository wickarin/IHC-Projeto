using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RadialMenuSpawner : MonoBehaviour {

    [System.Serializable]
    public class Action {
        public Color color;
        public Sprite sprite;
        public string title;
    }

    public Action[] options;

    public static RadialMenuSpawner ins;
    public RadialMenu menuPrefab, newMenu;
    public bool state = true;

    public MultipleTouch tS;

    void Awake() {
        ins = this;
        tS = GetComponent<MultipleTouch>();
    }

    public void SpawnMenu(Vector2 position, MultipleTouch tst, int type) {
        if (type == 0) {
            newMenu = Instantiate(menuPrefab) as RadialMenu;
            newMenu.transform.SetParent(transform, false);
            newMenu.transform.position = position;
            newMenu.spawnButtons(options.Take(3).ToArray(), tst);
        } else {
            newMenu = Instantiate(menuPrefab) as RadialMenu;
            newMenu.transform.SetParent(transform, false);
            newMenu.transform.position = position;
            newMenu.spawnButtons(options.Skip(3).ToArray(), tst);
        }
    }

    // public Action[] SubArray<Action>(this Action[] data, int index, int length) {
    //     Action[] result = new Action[length];
    //     Array.Copy(data, index, result, 0, length);
    //     return result;
    //}

}
