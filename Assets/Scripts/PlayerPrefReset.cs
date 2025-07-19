using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefReset : MonoBehaviour
{
    [ContextMenu("Reset PlayerPrefs")]
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Veriler Silindi");
    }
}
