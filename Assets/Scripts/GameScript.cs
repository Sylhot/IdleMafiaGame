using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using System;
using System.Numerics;

public class GameScript : MonoBehaviour
{
    public static GameScript instance;
    public TextMeshProUGUI textCashCount;   
    public GameObject gameScreen;
    public GameObject profilSecim;
    public CanvasGroup menuItemCanvas;
    public CanvasGroup gameCanvas;
    public bool selectedCharacter;
    public string selectedCharacterName;
    public Image profilePohotoGame;
    public CultureInfo culture = new CultureInfo("en-US");
    public double timeTrackerCash;
    public double cashAmount ;
    public double Cash  // Public property
    {
        get { return cashAmount; }
        set
        {
            cashAmount = Math.Round(value);
            UpdateCash();
        }
    }
    private void Awake() 
    {
        if(instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

    }
    private void Start()
    {  
        if(!string.IsNullOrEmpty(PlayerPrefs.GetString("cash")))
        {
            Debug.Log("AFK zamanda kazanilan para = "+timeTrackerCash.ToString("F2"));
            cashAmount = double.Parse(PlayerPrefs.GetString("cash")) + timeTrackerCash;    
        } 
        textCashCount.text = "$"+ FormatNumber(cashAmount);         
        Debug.Log("Oyuncu ismi " +PlayerPrefs.GetString("profileName") +" başladi oyun çalıştı gamescript");
        if(string.IsNullOrEmpty(PlayerPrefs.GetString("profileName")))
        {
            gameScreen.SetActive(false);
            profilSecim.SetActive(true);
        }
        else
            profilSecim.SetActive(false);       
 
    }
    void UpdateCash() 
    {
        textCashCount.text = "$"+ FormatNumber(cashAmount);    
    }   

    public void ButtonSecCharacter()
    {       
        if(selectedCharacter == true && selectedCharacterName != null && profilePohotoGame.sprite != null)
        {
            profilSecim.SetActive(false);
            gameScreen.SetActive(true);
            PlayerPrefs.SetString("profileName", selectedCharacterName);
            PlayerPrefs.SetString("profilePhoto", profilePohotoGame.sprite.name);
            PlayerPrefs.Save();
            Debug.Log("Oyuncu ismi: " +PlayerPrefs.GetString("profileName") +" fotograf adi: "+PlayerPrefs.GetString("profilePhoto")+" oyuncu ismi prefabplayera kaydedildi");
        }
        else
            Debug.Log("Lütfen Seçim Yapınız");
        
    }
    public void MenuItem()
    {
        if(gameCanvas.alpha == 1)
        {
            gameCanvas.alpha = 0f;
            gameCanvas.interactable = false;
            gameCanvas.blocksRaycasts = false;

            menuItemCanvas.alpha = 1f;
            menuItemCanvas.interactable = true;
            menuItemCanvas.blocksRaycasts = true;
            Debug.Log("menu acildi");
        }
        else
        {
            gameCanvas.alpha = 1f;
            gameCanvas.interactable = true;
            gameCanvas.blocksRaycasts = true;

            menuItemCanvas.alpha = 0f;
            menuItemCanvas.interactable = false;
            menuItemCanvas.blocksRaycasts = false;
            Debug.Log("menu kapandi");
        }  
    }
    string FormatNumber(double number)
    {
        if (number >= MathF.Pow(10, 60)) // Novemdecillion ve üstü
        {
            return FormatBigNumber(number, 60, "novemdecillion");
        }
        else if (number >= MathF.Pow(10, 57)) // Octodecillion
        {
            return FormatBigNumber(number, 57, "octodecillion");
        }
        else if (number >= MathF.Pow(10, 54)) // Septendecillion
        {
            return FormatBigNumber(number, 54, "septendecillion");
        }
        else if (number >= MathF.Pow(10, 51)) // Sexdecillion
        {
            return FormatBigNumber(number, 51, "sexdecillion");
        }
        else if (number >= MathF.Pow(10, 48)) // Quindecillion
        {
            return FormatBigNumber(number, 48, "quindecillion");
        }
        else if (number >= MathF.Pow(10, 45)) // Quattuordecillion
        {
            return FormatBigNumber(number, 45, "quattuordecillion");
        }
        else if (number >= MathF.Pow(10, 42)) // Tredecillion
        {
            return FormatBigNumber(number, 42, "tredecillion");
        }
        else if (number >= MathF.Pow(10, 39)) // Duodecillion
        {
            return FormatBigNumber(number, 39, "duodecillion");
        }
        else if (number >= MathF.Pow(10, 36)) // Undecillion
        {
            return FormatBigNumber(number, 36, "undecillion");
        }
        else if (number >= MathF.Pow(10, 33)) // Decillion
        {
            return FormatBigNumber(number, 33, "decillion");
        }
        else if (number >= MathF.Pow(10, 30)) // Nonillion
        {
            return FormatBigNumber(number, 30, "nonillion");
        }
        if (number >= MathF.Pow(10, 27)) // Septillion ve üstü
        {
            return  FormatBigNumber(number, 27, "octillion");
        }
        else if (number >= MathF.Pow(10, 24)) // Septillion
        {
            return FormatBigNumber(number, 24, "septillion");
        }
        else if (number >= MathF.Pow(10, 21)) // Sextillion
        {
            return FormatBigNumber(number, 21, "sextillion");
        }
        else if (number >= MathF.Pow(10, 18)) // Quintillion
        {
            return FormatBigNumber(number, 18, "quintillion");
        }
        else if (number >= MathF.Pow(10, 15)) // Quadrillion
        {
            return FormatBigNumber(number, 15, "quadrillion");
        }
        else if (number >= MathF.Pow(10, 12)) // Trillion
        {
            return FormatBigNumber(number, 12, "trillion");
        }
        else if (number >= MathF.Pow(10, 9)) // Billion
        {
            return FormatBigNumber(number, 9, "billion");
        }
        else if (number >= MathF.Pow(10, 6)) // Million
        {
            return FormatBigNumber(number, 6, "million");
        }
        else
        {
            return number.ToString("N0", CultureInfo.InvariantCulture);
        }
    }
    string FormatBigNumber(double number, int power, string unit)
    {
        // Virgülden sonra iki basamağı göstermek için
        double divisor = MathF.Pow(10, power);
        double integerPart = Math.Truncate(Math.Truncate(number) / divisor);
        double fractionalPart =  Math.Truncate(Math.Truncate(number) % divisor * 1000 / divisor);
        if(fractionalPart.ToString().Length == 3)
        {
            return $"{integerPart}.{fractionalPart} {unit}";
        }
        else if(fractionalPart.ToString().Length == 2)
        {
            return $"{integerPart}.{0}{fractionalPart} {unit}";
        }
        else
        {
            return $"{integerPart}.{0}{0}{fractionalPart} {unit}";
        }
    }
}
