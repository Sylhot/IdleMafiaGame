using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  
using TMPro;
using System;

public class ProfileButton : MonoBehaviour
{
    public GameObject profileScript;
    public Image image;
    public Button  button;

    private void Start()    
    {
        if(  String.IsNullOrEmpty(PlayerPrefs.GetString("profilePhoto"))  && String.IsNullOrEmpty(PlayerPrefs.GetString("profileName")) )
        {
            Debug.Log( "profile yok" );
        }
        else
        {
            Debug.Log( "calisti profileButton script" );
            Sprite loadedSprite = Resources.Load<Sprite>("Image/" + PlayerPrefs.GetString("profilePhoto"));
            image.sprite = loadedSprite;
            button.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString("profileName");
        }
    }
    public void ProfileButtonClick()
    {
        profileScript.SetActive(true);
    }

}
