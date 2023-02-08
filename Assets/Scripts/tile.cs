using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tile : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Color baseColor;
    [SerializeField] private Color offSetColor;

    private Color selectedColor;

    private SpriteRenderer gfx;

    void Awake(){
         gfx = this.gameObject.GetComponent<SpriteRenderer>();
    }

    public void init(bool isOffset){
        selectedColor = isOffset ? offSetColor : baseColor;
        gfx.color = selectedColor;
    }
    
    void OnMouseEnter(){
        Color temp = selectedColor;
        temp.a = 0.5f;
        gfx.color = temp;
    }

    void OnMouseExit(){
        gfx.color = selectedColor;
    }
}
