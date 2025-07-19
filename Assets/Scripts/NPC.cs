using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using JetBrains.Annotations;
using Unity.Mathematics;
using System.Numerics;
using System;
using System.Threading;

public class NPC : MonoBehaviour
{
    public GameScript gameScript;
    public TimeTracker timeTracker;
    public XClickScript ClickScript;
    public Button buttonUpgrade;
    public Button buttonNpc;
    public Button buttonNpcUnlock;
    public Sprite spriteNpc;
    public Image expBar;
    public Image timeBar;
    public TextMeshProUGUI textXClick;
    public TextMeshProUGUI textvalueExp;
    public TextMeshProUGUI textValueTime;
    public TextMeshProUGUI textUpgradeButton;  
    public TextMeshProUGUI textNpcCash; 
    public TextMeshProUGUI textNpcName; 
    public string npcName;
    public int npcNextLevel;
    public int npcOldLevel;
    public int npcExp;
    public double npcCash;
    public double upgradeCost;
    [SerializeField]public float operationTime;
    [SerializeField]public float upgradeMultiplier;
    // [SerializeField]public int upgradeDivide;
    [SerializeField]public float cashMultiplier;
    //[SerializeField]public int cashDivide;
    public float totalSecond;
    public float elapsed; 
    private bool timeIsStart;
    public bool item;
    public bool npcUnlock;
    public int npcOpenMoney;
    public double tryCostValue;
    public int[] arrayNpcNextLevel;
    public float[] arrayNpcCashMultiplier;
    private int intArrayNpcNextLevel;
    private int intArrayNpcCashMultiplier;

    public CultureInfo culture = new CultureInfo("en-US");
    void OnApplicationQuit()
    {
        SaveCurrentValue();
    }

    void OnApplicationPause(bool pauseStatus)
    {        
        if (pauseStatus)
        {
            SaveCurrentValue();
        }
    }
    void SaveCurrentValue()
    {
        PlayerPrefs.SetString(npcName+"cash", npcCash.ToString());
        PlayerPrefs.SetInt(npcName+"exp", npcExp);
        PlayerPrefs.SetString(npcName+"upgradeCost", upgradeCost.ToString());
        PlayerPrefs.SetFloat(npcName+"operationTime", operationTime);
        PlayerPrefs.SetInt(npcName+"oldLevelNPC", npcOldLevel);
        PlayerPrefs.SetInt(npcName+"intArrayNpcNextLevel", intArrayNpcNextLevel);
        PlayerPrefs.SetInt(npcName+"intArrayNpcCashMultiplier", intArrayNpcCashMultiplier);
        PlayerPrefs.SetFloat(npcName+"totalSecond", totalSecond);
        PlayerPrefs.SetInt(npcName+"item", item ? 1 : 0);
        PlayerPrefs.SetInt(npcName+"npcUnlock",npcUnlock ? 1 : 0);
        PlayerPrefs.Save();
    }
    void Start()
    {
        gameScript = GameObject.Find("GameScript").GetComponent<GameScript>();
        timeTracker = GameObject.Find("TimeScript").GetComponent<TimeTracker>();
        ClickScript = GameObject.Find("GameScript").GetComponent<XClickScript>();
        buttonUpgrade = gameObject.transform.Find("UpgradeButton").GetComponentInChildren<Button>();
        buttonNpc = gameObject.transform.Find("NPCbutton").GetComponentInChildren<Button>();
        buttonNpcUnlock = gameObject.transform.Find("OpenButton").GetComponentInChildren<Button>();
        textNpcName.text = npcName;    
        buttonNpc.image.sprite = spriteNpc;
        
        if(!string.IsNullOrEmpty(PlayerPrefs.GetString("profileName")))
        {
            npcCash = double.Parse(PlayerPrefs.GetString(npcName+"cash"));
            npcExp = PlayerPrefs.GetInt(npcName+"exp");
            upgradeCost = double.Parse(PlayerPrefs.GetString(npcName+"upgradeCost"));
            operationTime = PlayerPrefs.GetFloat(npcName+"operationTime");
            npcOldLevel = PlayerPrefs.GetInt(npcName+"oldLevelNPC");
            intArrayNpcNextLevel = PlayerPrefs.GetInt(npcName+"intArrayNpcNextLevel");
            intArrayNpcCashMultiplier = PlayerPrefs.GetInt(npcName+"intArrayNpcCashMultiplier");
            totalSecond = PlayerPrefs.GetFloat(npcName+"totalSecond");
            item = PlayerPrefs.GetInt(npcName+"item") == 1;
            npcUnlock = PlayerPrefs.GetInt(npcName+"npcUnlock") == 1;
            float somevalue = (int)(timeTracker.elapsedTime.TotalSeconds + (operationTime - totalSecond));
            if(somevalue > operationTime)
            {
                gameScript.timeTrackerCash += Mathf.RoundToInt(somevalue/operationTime) * npcCash;
                timeBar.fillAmount =  (int)(somevalue % operationTime)/ operationTime;
                if(item == true)
                    StartCoroutine(StartCash(0f,1f,operationTime,(int)(somevalue % operationTime)));
            }
            else
            {
                timeBar.fillAmount = somevalue  / operationTime;
                if(item == true)
                    StartCoroutine(StartCash(0f,1f,operationTime, somevalue));
            }
        }   
        if(npcUnlock)
        {
            buttonNpcUnlock.gameObject.SetActive(false);
            buttonUpgrade.gameObject.SetActive(true);
            buttonNpc.gameObject.SetActive(true);
            if(intArrayNpcNextLevel == 0)
            {
                npcNextLevel = arrayNpcNextLevel[intArrayNpcNextLevel];
            }
            else
            {
                npcNextLevel = arrayNpcNextLevel[intArrayNpcNextLevel];
            }
            Debug.Log("npc açik");
        }
        else
        {
            buttonUpgrade.gameObject.SetActive(false);
            buttonNpc.gameObject.SetActive(false);
            TextMeshProUGUI text = buttonNpcUnlock.GetComponentInChildren<TextMeshProUGUI>();
            text.text = "Hire\n"+npcName+"\n"+"$"+npcOpenMoney.ToString(culture);
            npcNextLevel = arrayNpcNextLevel[0];
            Debug.Log("npc kapali");
        }  
        expBar.fillAmount = (float)(npcExp - npcOldLevel) / (npcNextLevel - npcOldLevel);
        TextUpdate();
        tryCostValue = upgradeCost;
        textXClick.text = "X1";
    }
    void Update()
    {   
        if(buttonNpcUnlock.gameObject.activeSelf)
        {
            buttonUpgrade.gameObject.SetActive(false);
            buttonNpc.gameObject.SetActive(false);
            if(gameScript.Cash >= npcOpenMoney)
            {
                buttonNpcUnlock.interactable = true;               
            }
            else
            {
                buttonNpcUnlock.interactable = false;
            }
        }


        if (intArrayNpcNextLevel + 1 < arrayNpcNextLevel.Length)
        {
            if ((npcExp - npcOldLevel) >= (npcNextLevel - npcOldLevel))
            {
                npcOldLevel = npcNextLevel;
                intArrayNpcNextLevel++;
                npcNextLevel = arrayNpcNextLevel[intArrayNpcNextLevel];
                expBar.fillAmount = 0f;
                npcCash = Math.Truncate(npcCash * arrayNpcCashMultiplier[intArrayNpcCashMultiplier]);
                intArrayNpcCashMultiplier++;
                TextUpdate();
            }
        }
        else
            Debug.Log(npcName+" MAX levela ulaştı");   
        
        if (gameScript.Cash >= tryCostValue)
        {
            if (tryCostValue != 0)
            {
                buttonUpgrade.interactable = true;
            }
            else
                buttonUpgrade.interactable = false;
        }
        else
        {
            buttonUpgrade.interactable = false;
        }
    }
    private void TextUpdate()
    {
        textvalueExp.text = npcExp.ToString()+"/"+arrayNpcNextLevel[intArrayNpcNextLevel].ToString();
        textUpgradeButton.text = FormatNumber(upgradeCost);
        textNpcCash.text = FormatNumber(Math.Truncate(npcCash));
    }
    public void ClickCash()
    {
        if(timeIsStart == false)
        {
            timeIsStart = true;
            StartCoroutine(StartCash(0f,1f,operationTime,0f));           
        }
        
    }
    public void UpgradeNpcCash()
    {
        npcCash =  Math.Truncate(npcCash + (npcCash / cashMultiplier)); 
        Debug.Log("cash artti"+npcCash);
    }
    public void UpgradeUpgradeCost()
    {
        upgradeCost = upgradeCost + (upgradeCost / upgradeMultiplier); 
        upgradeCost = UpgradeCostComme(upgradeCost);
        Debug.Log("cost artti"+upgradeCost);
    }
    public double UpgradeCostComme(double value)
    { 
     if(Math.Truncate(value).ToString().Length < 4)
        {
            return Math.Round(value,2);
        }
        else
        {
            return Math.Truncate(value);
        }
    }
    IEnumerator StartCash(float startValue, float endValue, float duration, float remainingTime)
    {                   
        if(remainingTime != 0)
        {
            elapsed = remainingTime;
        }
        else
            elapsed = 0f; 
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            totalSecond = Mathf.Round((duration-elapsed)*100/100);
            timeBar.fillAmount = Mathf.Lerp(startValue, endValue, elapsed / duration);
            textValueTime.text = durationTime(totalSecond);
            yield return null;
        }

        // İşlemin sonunda fillAmount'u tam olarak endValue'ya ayarla
        timeBar.fillAmount = endValue;
        Debug.Log("Cash kazanildi");
        gameScript.Cash += Math.Truncate(npcCash);
        XTextControl();
        timeBar.fillAmount = 0f;
        timeIsStart = false;
        if(item == true)
        {
            buttonNpc.interactable = false;
            runClick();      
        }
    }
    private string durationTime(float fullTime)
    {
        int hours = Mathf.FloorToInt(fullTime / 3600);
        int minutes = Mathf.FloorToInt((fullTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(fullTime % 60);
        if(hours > 0)
        {
            return string.Format("{0}h {1}m {2}s", hours, minutes, seconds);
        }
        else if( minutes > 0)
        {
            return string.Format("{0}m {1}s", minutes, seconds);
        }
        else
            return string.Format("{0}s", seconds);
        
    
    }
    public void itemAvailable()
    {
        if( item == false && npcUnlock)
        {
            item = true;  
            runClick();    
        }
         
    }
    public void runClick()
    {
        StartCoroutine(StartCash(0f,1f,operationTime,0f));
    }
    
    public void npcUnlockButtonClick()
    {
        buttonNpcUnlock.gameObject.SetActive(false);
        buttonUpgrade.gameObject.SetActive(true);
        buttonNpc.gameObject.SetActive(true);
        gameScript.Cash -= npcOpenMoney;
        npcUnlock = true;
    }
    public void XTextControl()
    {
        if(npcUnlock == true)
        {
            Debug.Log(npcName+" calisti");
            textXClick.text = ClickScript.ClickText.text;
            tryCostValue = TryCost(textXClick.text);
        }
    }
    public double TryCost(string text)
    {       
        double value = upgradeCost;
        double number = value;
        if(text == "X1")
        {
            textUpgradeButton.text = FormatNumber(number);
            return number;
        }
        else if(text == "X10")
        {
            for(int i = 0; i < 10; i++)
            {
                value = value + (value / upgradeMultiplier);
                value = UpgradeCostComme(value);
                number += value;
            }
            textUpgradeButton.text = FormatNumber(number);
            return number;       
        }
        else if(text == "X100")
        {   
            for(int i = 0; i < 100; i++)
            {
                value = value + (value / upgradeMultiplier);
                value = UpgradeCostComme(value);
                number += value;
            }
            textUpgradeButton.text = FormatNumber(number);
            return number;       
        }
        else if(text == "NEXT")
        {
            int x = npcNextLevel - npcExp;
            for(int i = 0; i < x; i++)
            {
                value = value + (value / upgradeMultiplier);
                value = UpgradeCostComme(value);
                number += value;
            }
            textUpgradeButton.text = FormatNumber(number);
            return number;       
        }
        else
        {
            int counter = 0;
            while(number < gameScript.Cash)
            {
                value = value + (value / upgradeMultiplier);
                value = UpgradeCostComme(value);
                number += value;
                counter++;
            }
            if(counter != 0)
            {
                number -= value;
                textXClick.text = "X"+counter.ToString();
                textUpgradeButton.text = FormatNumber(number);
                return number;
            }
            else
            {
                textXClick.text = "X"+counter.ToString();  
                textUpgradeButton.text = FormatNumber(number);   
                return 0;  
            } 
           
        }
    }
    
    public void  UpgradeButton()
    {
        Debug.Log("fonksiyona girdi");
        if(textXClick.text == "X1")
        {
            gameScript.Cash -= tryCostValue;
            UpgradeUpgradeCost();
            UpgradeNpcCash();
            npcExp++;
            expBar.fillAmount = (float)(npcExp - npcOldLevel) / (npcNextLevel - npcOldLevel);
            TextUpdate();
        }
        else if(textXClick.text == "X10")
        {
            for(int i = 0; i < 10; i++)
            {
                UpgradeUpgradeCost();
                UpgradeNpcCash();
                npcExp++;
            }
            gameScript.Cash -= tryCostValue;
            expBar.fillAmount = (float)(npcExp - npcOldLevel) / (npcNextLevel - npcOldLevel);
            TextUpdate();
        }
        else if(textXClick.text == "X100")
        {
            for(int i = 0; i < 100; i++)
            {
                UpgradeUpgradeCost();
                UpgradeNpcCash();
                npcExp++;
            }
            gameScript.Cash -= tryCostValue;
            expBar.fillAmount = (float)(npcExp - npcOldLevel) / (npcNextLevel - npcOldLevel);
            TextUpdate();
        }
        else if(textXClick.text == "NEXT")
        {
            int value = npcNextLevel - npcExp;
            for(int i = 0; i < value; i++)
            {
                UpgradeUpgradeCost();
                UpgradeNpcCash();
                npcExp++;
            }
            gameScript.Cash -= tryCostValue;
            expBar.fillAmount = (float)(npcExp - npcOldLevel) / (npcNextLevel - npcOldLevel);
            TextUpdate();
        }
        else
        {
            int counter = int.Parse(textXClick.text.Substring(1));
            for(int i = 0; i < counter; i++)
            {
                UpgradeUpgradeCost();
                UpgradeNpcCash();
                npcExp++;
            }
            gameScript.Cash -= tryCostValue;
            expBar.fillAmount = (float)(npcExp - npcOldLevel) / (npcNextLevel - npcOldLevel);
            TextUpdate();
        }
        XTextControl();
    }
    string FormatNumber(double number)
    {
        if (number >= Math.Pow(10, 60)) // Novemdecillion ve üstü
        {
            return FormatBigNumber(number, 60, "novemdecillion");
        }
        else if (number >= Math.Pow(10, 57)) // Octodecillion
        {
            return FormatBigNumber(number, 57, "octodecillion");
        }
        else if (number >= Math.Pow(10, 54)) // Septendecillion
        {
            return FormatBigNumber(number, 54, "septendecillion");
        }
        else if (number >= Math.Pow(10, 51)) // Sexdecillion
        {
            return FormatBigNumber(number, 51, "sexdecillion");
        }
        else if (number >= Math.Pow(10, 48)) // Quindecillion
        {
            return FormatBigNumber(number, 48, "quindecillion");
        }
        else if (number >= Math.Pow(10, 45)) // Quattuordecillion
        {
            return FormatBigNumber(number, 45, "quattuordecillion");
        }
        else if (number >= Math.Pow(10, 42)) // Tredecillion
        {
            return FormatBigNumber(number, 42, "tredecillion");
        }
        else if (number >= Math.Pow(10, 39)) // Duodecillion
        {
            return FormatBigNumber(number, 39, "duodecillion");
        }
        else if (number >= Math.Pow(10, 36)) // Undecillion
        {
            return FormatBigNumber(number, 36, "undecillion");
        }
        else if (number >= Math.Pow(10, 33)) // Decillion
        {
            return FormatBigNumber(number, 33, "decillion");
        }
        else if (number >= Math.Pow(10, 30)) // Nonillion
        {
            return FormatBigNumber(number, 30, "nonillion");
        }
        if (number >= Math.Pow(10, 27)) // Septillion ve üstü
        {
            return  FormatBigNumber(number, 27, "octillion");
        }
        else if (number >= Math.Pow(10, 24)) // Septillion
        {
            return FormatBigNumber(number, 24, "septillion");
        }
        else if (number >= Math.Pow(10, 21)) // Sextillion
        {
            return FormatBigNumber(number, 21, "sextillion");
        }
        else if (number >= Math.Pow(10, 18)) // Quintillion
        {
            return FormatBigNumber(number, 18, "quintillion");
        }
        else if (number >= Math.Pow(10, 15)) // Quadrillion
        {
            return FormatBigNumber(number, 15, "quadrillion");
        }
        else if (number >= Math.Pow(10, 12)) // Trillion
        {
            return FormatBigNumber(number, 12, "trillion");
        }
        else if (number >= Math.Pow(10, 9)) // Billion
        {
            return FormatBigNumber(number, 9, "billion");
        }
        else if (number >= Math.Pow(10, 6)) // Million
        {
            return FormatBigNumber(number, 6, "million");
        }
        else if(number < Math.Pow(10, 3))
        {
            return number.ToString("N2", CultureInfo.InvariantCulture);
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
        double integerPart = Math.Truncate( Math.Truncate(number) / divisor);
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
