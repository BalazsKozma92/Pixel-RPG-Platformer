using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    Dictionary<string, string[]> tooltipDict = new Dictionary<string, string[]>();
    Gems gems;
    bool initialized = false;

    // public event Action ReDrawAbilityToolTips;

    string hourGlassString = "hourglass";
    string quote = '"'.ToString();
    string hourGlass;
    string headerContentColor = "<color=#E2A476>";
    string abilityColor = "<color=#EC81FF>";
    string initColor = "<color=#9EA79D>";
    string baseTextColor = "<color=#92DA8B>";
    string percentageColor = "<color=yellow>";
    string timeColor = "<color=lightblue>";
    string lifeColor = "<color=red>";
    string vigourColor = "<color=#B3A91C>";
    string deathColor = "<color=#D52EF6>";
    string adColor = "<color=orange>";
    string apColor = "<color=#6E8DE1>";

    private void Start() {
        gems = GetComponent<Gems>();
        gems.resetAbilityDict += ResetAbilityDict;
    }

    void ResetAbilityDict()
    {
        InitDicts(true);
    }

    public void InitDicts(bool resetting)
    {
        if (resetting)
        {
            tooltipDict.Clear();
        }
        if (!initialized || resetting)
        {
            hourGlass = "<sprite="+quote+hourGlassString+quote+" index=0>";
            initialized = true;

            float timeIntervalHeadGemOfPeace = gems.vigourCoroutineTimers["Head GemOfPeace"] <= 0 ? 1 : gems.vigourCoroutineTimers["Head GemOfPeace"];
            string[] meditate = {headerContentColor + "Meditate</color>",
            initColor + "Take " +  percentageColor + "50%</color> less damage from enemies for " + timeColor + "15 seconds.</color> " + timeColor + "(" + timeIntervalHeadGemOfPeace.ToString() + "</color> " + hourGlass + ")</color>"};

            tooltipDict.Add("Meditate", meditate);

            float timeIntervalSwordFocusJewel = gems.vigourCoroutineTimers["Sword FocusJewel"] <= 0 ? 1 : gems.vigourCoroutineTimers["Sword FocusJewel"];
            string[] healthFocus = {headerContentColor + "Health Focus</color>",
            initColor + "Gives " + lifeColor + "+4 temporary <sprite index=0></color> for " + timeColor + "8 seconds. (" + timeIntervalSwordFocusJewel.ToString() + " " + hourGlass + ")</color></color>"};

            tooltipDict.Add("HealthFocus", healthFocus);

            float timeIntervalSwordSunPearl = gems.vigourCoroutineTimers["Sword SunPearl"] <= 0 ? 1 : gems.vigourCoroutineTimers["Sword SunPearl"];
            string[] rayBurst = {headerContentColor + "Ray Burst</color>",
            initColor + "Bursts out a scorching ray of sunlight in the direction of the cursor, damaging all enemies it goes through. Rayburst damage: " + apColor + "magic damage * 1.45</color> " + timeColor + "(" + timeIntervalSwordSunPearl.ToString() + " " + hourGlass + ")</color>"};

            tooltipDict.Add("RayBurst", rayBurst);

            float timeIntervalFeetFocusJewel = gems.vigourCoroutineTimers["Feet FocusJewel"] <= 0 ? 1 : gems.vigourCoroutineTimers["Feet FocusJewel"];
            string[] kick = {headerContentColor + "Kick</color>",
            initColor + "Focus your strength in your right foot. Kick damage: " + adColor + "attack damage * 1.25</color> " + timeColor + "(" + timeIntervalFeetFocusJewel.ToString() + " " + hourGlass + ")</color></color>"};

            tooltipDict.Add("Kick", kick);

            string[] ability5 = {headerContentColor + "Ability 5</color>",
            initColor + "Placeholder.</color>"};

            tooltipDict.Add("Ability5", ability5);

            string[] ability6 = {headerContentColor + "Ability 6</color>",
            initColor + "Placeholder.</color>"};

            tooltipDict.Add("Ability6", ability6);

            string[] ability7 = {headerContentColor + "Ability 7</color>",
            initColor + "Placeholder.</color>"};

            tooltipDict.Add("Ability7", ability7);
        }
    }

    public string[] GetTooltip(string abilityName)
    {
        return tooltipDict[abilityName];
    }

    public bool IsKeyInDict(string abilityName)
    {
        return tooltipDict.ContainsKey(abilityName);
    }
}
