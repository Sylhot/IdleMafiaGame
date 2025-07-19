using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XClickScript : MonoBehaviour
{
    public TextMeshProUGUI ClickText;
    public void XButtonClick()
    {
   
        if(ClickText.text == "X1")
        {
            ClickText.text = "X10";
        }
        else if(ClickText.text == "X10")
        {
            ClickText.text = "X100";
        }
        else if(ClickText.text == "X100")
        {
            ClickText.text = "NEXT";
        }
        else if(ClickText.text == "NEXT")
        {
            ClickText.text = "MAX";
        }
        else if (ClickText.text == "MAX")
        {
            ClickText.text = "X1";
        }
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
        foreach (GameObject npc in npcs)
        {
            // NPC scriptini al
            NPC npcScript = npc.GetComponent<NPC>();

            if (npcScript != null)
            {
                // Scriptin içindeki bir fonksiyonu çağır
                npcScript.XTextControl();
            }
        }
    }
}
