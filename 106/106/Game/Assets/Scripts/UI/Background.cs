using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : Singleton<Background>
{
    public SpriteRenderer background;
    public SpriteRenderer rederer;
    [SerializeField] Sprite[] sprites;


    public void ChangeBackground(int stage)
    {
        switch (stage)
        {
            case 1:
                background.color = Color.white;
                break;
                case 2:
                background.color = Color.red;
                break;
            default:
                break;
        }
        rederer.sprite = sprites[stage - 1];
    
    }
}
