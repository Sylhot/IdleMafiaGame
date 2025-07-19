using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectCharacter : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public TextMeshProUGUI charName;
    private UnityEngine.UI.Image charImage;
    public UnityEngine.UI.Image selectedImage;
    public GameScript gameScript;
    public bool selected;

    void Start()
    {
        charName = gameObject.GetComponentInChildren<TextMeshProUGUI>(); 
        selectedImage.color = new Color(255f/255f,255f/255f,255f/255f,255f/255f);  
        gameScript = GameObject.Find("GameScript").GetComponent<GameScript>();
        charImage = gameObject.GetComponent<UnityEngine.UI.Image>();
    }
    public void OnSelect(BaseEventData eventData)
    {
        selected = true;
        charName.color = new Color(97f/255f, 154f/255f, 147f/255f, 90f/255f);
        selectedImage.color = new Color(255f/255f,255f/255f,255f/255f,50f/255f);
        gameScript.selectedCharacter = true;
        gameScript.selectedCharacterName = charName.text;
        gameScript.profilePohotoGame = charImage;
        Debug.Log("suan secili "+gameScript.selectedCharacterName + " çalisti selectedcharacterscript");
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selected = false;
        charName.color = new Color(97f/255f, 154f/255f, 147f/255f, 255f/255f);
        selectedImage.color = new Color(255f/255f,255f/255f,255f/255f,255f/255f);    
    }

    

}
