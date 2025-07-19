using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileScript : MonoBehaviour
{
    public GameScript gameScript;
    public Image profilePhoto;
    public string profileName; 
    void OnEnable()
    {
        gameScript = GameObject.Find("GameScript").GetComponent<GameScript>();
        if(String.IsNullOrEmpty(PlayerPrefs.GetString("profilePhoto"))  && String.IsNullOrEmpty(PlayerPrefs.GetString("profileName")))
        {
            Debug.Log("Profil yok");
        }
        else
        {
            profileName = PlayerPrefs.GetString("profileName");
            Sprite loadedSprite = Resources.Load<Sprite>("Image/" + PlayerPrefs.GetString("profilePhoto"));
            profilePhoto.sprite = loadedSprite;
            Debug.Log("player name: " + profileName + profilePhoto.sprite.name +" çalisti profilescript");
            Debug.Log("Profil acildi");
        }
    }

}
