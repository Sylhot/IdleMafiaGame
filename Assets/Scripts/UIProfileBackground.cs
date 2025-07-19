using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class UIProfileBackground : MonoBehaviour, IPointerClickHandler
{
    public GameScript gameScript;
    void Start()
    {
        gameScript = GameObject.Find("GameScript").GetComponent<GameScript>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        gameScript.selectedCharacter = false;
        gameScript.selectedCharacterName = null; 
        gameScript.profilePohotoGame = null; 
    }
}
