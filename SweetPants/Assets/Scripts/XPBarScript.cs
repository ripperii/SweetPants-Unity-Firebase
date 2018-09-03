using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class XPBarScript : MonoBehaviour {

    public Image XPBarSlider;
    public Text LevelField;
    public float speed = 2; 
    public int currentXP;
    public int requiredXP;
    public int xp;
    public int xpToAdd;
    public bool addingXP;
    public KeyValuePair<int, List<int>> currentLevel;
    public KeyValuePair<int, List<int>> reachedLevel;

    public void UpdateXP(float curXP, float reqXP)
    {
        Debug.Log("Required XP: " + reqXP);
        Debug.Log("Current XP: " + curXP);
        Debug.Log("XPBar fill: " + (curXP == 0 ? 0 : curXP / reqXP));
        XPBarSlider.fillAmount = curXP == 0 ? 0 : curXP / reqXP;
    }
    public void AddXP(int curXP, int xp)
    {
        Debug.LogFormat("Current XP: {0} - XP: {1}", curXP, xp);

        if(xp > curXP)
        {
            this.currentLevel = Levels.FindPlayerLevel(curXP);

            this.xpToAdd = xp - curXP;
            //Whole XP
            this.xp = xp;
            //XP on current Level
            this.currentXP = (curXP - Levels.list[currentLevel.Key][1]);
            this.requiredXP = Levels.list[currentLevel.Key][0];
            this.addingXP = true;
        }
        else
        {
            LevelField.text = Levels.FindPlayerLevel(xp).Key.ToString();
            UpdateXP(xp, Levels.list.Where(x => x.Key.ToString() == LevelField.text).FirstOrDefault().Value[0]);
        }
    }
    private void Update()
    {
        if(addingXP)
        {
            XPBarSlider.fillAmount += Time.deltaTime/speed;
            if(XPBarSlider.fillAmount >= ((float)(xpToAdd+currentXP)/requiredXP))
            {
                XPBarSlider.fillAmount = ((float)(xpToAdd + currentXP) / requiredXP);
                Debug.Log("XPBarSlider Fill: " + XPBarSlider.fillAmount);
                addingXP = false;
            }
            if(XPBarSlider.fillAmount >= 1)
            {
                XPBarSlider.fillAmount = 0;
                currentLevel = Levels.GetNextLevel(currentLevel.Key);

                LevelField.text = currentLevel.Key.ToString();

                xpToAdd -= (requiredXP - currentXP);
                currentXP = 0;
                requiredXP = Levels.list[currentLevel.Key][0];
            }
        }
    }
}
