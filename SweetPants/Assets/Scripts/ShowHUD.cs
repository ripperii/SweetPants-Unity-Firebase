using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHUD : MonoBehaviour
{
    
    public UserDataUpdateScript udus;

    public void HUD()
    {
        udus.ShowHUD();
    }
    public void OpenAdventurerMenuAfterAnimation()
    {
        if (udus.p.pendingAdventurer != null)
        {
            //show adventurer dialog box
            udus.advMenu.OpenAdventurerMenu(udus.p.pendingAdventurer);
        }
    }
}
