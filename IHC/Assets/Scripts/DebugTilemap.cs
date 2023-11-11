using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class DebugTilemap : MonoBehaviour {
    
    public static int[,] cellsHP;
    static int origin_x, origin_y;
    // Start is called before the first frame update
    void Start() {
        Tilemap tm = GetComponent<Tilemap>();
        Debug.Log(tm.size);
        cellsHP = new int[tm.size.x, tm.size.y];
        fillMatrix(tm.size.x, tm.size.y);
        Debug.Log(tm.origin);
        origin_x = -tm.origin.x;
        origin_y = -tm.origin.y;
    }

    private void fillMatrix(int x, int y) {
        for (int lin = 0; lin < y; lin++) {
            // Debug.Log(lin);
            for (int col = 0; col < x; col++) {
                cellsHP[col, lin] = 100;
            }
        }
    }

    public static void getValueOnCell(int x, int y) {
        Debug.Log(cellsHP[x, y]);
    }

    public static int harvestHP(int x, int y) {
        int offset_x = x + origin_x, offset_y = y + origin_y, health = cellsHP[offset_x, offset_y];
        cellsHP[offset_x, offset_y] = 0;
        return health;
    }

    public static void sowHP(int x, int y, int hp_lost) {
        int offset_x = x + origin_x, offset_y = y + origin_y, health = cellsHP[offset_x, offset_y];
        cellsHP[offset_x-1, offset_y] += hp_lost;
        cellsHP[offset_x, offset_y+1] += hp_lost;
        cellsHP[offset_x, offset_y-1] += hp_lost;
        cellsHP[offset_x+1, offset_y] += hp_lost;
    }
}
