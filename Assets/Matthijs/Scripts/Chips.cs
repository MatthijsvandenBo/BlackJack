using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Chips : MonoBehaviour
{
    public ulong chips;
    [SerializeField] private TMP_Text scoreTexts;

    private void Update()
    {
        ChipsUpdate();
    }

    private void ChipsUpdate()
    {
        scoreTexts.text = "Chips: " + chips;
        
    }
    public void scoreChangeMin(int change)
    {
        ulong vOut = Convert.ToUInt64(change);
        chips -= vOut;
    }

    public void scoreChangePlus(int change)
    {
        ulong vOut = Convert.ToUInt64(change);
        chips += vOut;
    }
}
