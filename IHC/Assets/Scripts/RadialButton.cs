using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RadialButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    
    public Image circle;
    public Image icon;
    public string title;
    public RadialMenu myMenu;

    Color defaultColor;

    public void OnPointerEnter (PointerEventData eventData) {
        myMenu.selected = this;
        myMenu.selected.title = title;
        // Debug.Log("Hovering!");
    }

    public void OnPointerExit (PointerEventData eventData) {
        myMenu.selected = null;
        
    }
}
