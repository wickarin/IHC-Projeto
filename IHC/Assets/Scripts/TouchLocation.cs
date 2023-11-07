using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchLocation {
    public int touchId;

    public Vector2 positionBegan;

    public TouchLocation(int newTouchId, Vector2 touchBegan){
        this.touchId = newTouchId;
        this.positionBegan = touchBegan;
    }	

    public Vector2 direction(Vector2 touchEnded){
        if ((this.positionBegan.x >= touchEnded.x - 2 && this.positionBegan.x <= touchEnded.x + 2) && (this.positionBegan.y >= touchEnded.y - 2 && this.positionBegan.y <= touchEnded.y + 2)){
            return Vector2.zero;
        }
        if (Mathf.Abs(touchEnded.x - this.positionBegan.x) > Mathf.Abs(touchEnded.y - this.positionBegan.y))
            {
                if (touchEnded.x - this.positionBegan.x > 0) return new Vector2(1.0f, 0.0f);
                else return new Vector2(-1.0f, 0.0f);
            }
            else
            {
                if (touchEnded.y - this.positionBegan.y > 0) return new Vector2(0.0f, 1.0f);
                else return new Vector2(0.0f, -1.0f);
            }
        
    }
}