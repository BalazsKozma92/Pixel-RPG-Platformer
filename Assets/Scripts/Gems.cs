using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gems : MonoBehaviour
{
    GemBuffManager gemBuffManager;
    bool initialized = false;

    Coroutine healthCoroutine = null;
    Coroutine focusCoroutine = null;
    Coroutine phoenixRespawnCoroutine = null;

    public event Action reDrawToolTips;
    public event Action resetAbilityDict;
    public event Action<string> onCooldownChange;

    List<Coroutine> vigourCoroutines = new List<Coroutine>();
    List<Coroutine> deathCoroutines = new List<Coroutine>();
    List<Coroutine> balanceCoroutines = new List<Coroutine>();

    List<string> vigourGemWithCDParents = new List<string>();
    List<string> deathGemWithCDParents = new List<string>();
    List<string> balanceGemWithCDParents = new List<string>();

    public Dictionary<string, float> vigourCoroutineTimers = new Dictionary<string, float>();
    public Dictionary<string, float> deathCoroutineTimers = new Dictionary<string, float>();
    public Dictionary<string, float> balanceCoroutineTimers = new Dictionary<string, float>();

    public Dictionary<string, string> abilityNameToSocketName = new Dictionary<string, string>();

    Dictionary<string, string[]> tooltipDict = new Dictionary<string, string[]>();
    
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

    private void Awake()
    {
        gemBuffManager = GetComponent<GemBuffManager>();
    }

    private void Start()
    {
        // PlayerBase.Instance.onDeath += ResetPhoenixStoneTimer;
    }

    public void InitDicts(bool resetting)
    {
        if (resetting)
        {
            tooltipDict.Clear();
        }
        if (!initialized)
        {
            abilityNameToSocketName.Add("Meditate", "Head GemOfPeace");
            abilityNameToSocketName.Add("HealthFocus", "Sword FocusJewel");
            abilityNameToSocketName.Add("RayBurst", "Sword SunPearl");
            abilityNameToSocketName.Add("Kick", "Feet FocusJewel");

            vigourCoroutineTimers.Add("Head HealthGem", 18f);
            vigourCoroutineTimers.Add("Torso HealthGem", 22f);
            vigourCoroutineTimers.Add("Head PhoenixStone", 60f);
            vigourCoroutineTimers.Add("Torso PhoenixStone", 90f);
            vigourCoroutineTimers.Add("Torso FocusJewel", 15f);

            vigourCoroutineTimers.Add("Head GemOfPeace", 45f);
            vigourCoroutineTimers.Add("Sword FocusJewel", 50f);
            vigourCoroutineTimers.Add("Sword SunPearl", 15f);
            vigourCoroutineTimers.Add("Feet FocusJewel", 8f);
        }
        if (!initialized || resetting) {
            hourGlass = "<sprite="+quote+hourGlassString+quote+" index=0>";
            initialized = true;
            /////////////Vigour gems//////////////

            string[] gemOfLife = {headerContentColor + "Gem of life</color> " + lifeColor + "<size=80%>(+1 max <sprite index=0>)</color>",
            headerContentColor + "Makes its bearer feel exceptionally well.</color>",
            initColor + "Helmet:</color> " + lifeColor + "+1 max <sprite index=0></color>",
            initColor + "Torso:</color> " + lifeColor + "+2 max <sprite index=0></color>",
            initColor + "Sword:</color> " + baseTextColor + "Gives " + percentageColor + "15% bonus chance</color> for the enemy to drop a health orb.</color>",
            initColor + "Feet:</color> " + baseTextColor + "Gives you a " + percentageColor + "35% chance</color> to regain " + lifeColor + "1 <sprite index=0></color> when stomping on an enemy.</color>"};
            tooltipDict.Add("GemOfLife", gemOfLife);

            float timeIntervalHeadHealthGem = vigourCoroutineTimers["Head HealthGem"] <= 0 ? 1 : vigourCoroutineTimers["Head HealthGem"];
            float timeIntervalTorsoHealthGem = vigourCoroutineTimers["Torso HealthGem"] <= 0 ? 1 : vigourCoroutineTimers["Torso HealthGem"];
            string[] healthGem = {headerContentColor + "Health gem</color> " + lifeColor + "<size=80%>(+1 max <sprite index=0>)</color>",
            headerContentColor + "Those who carry this stone will feel replenished all the time.</color>",
            initColor + "Helmet:</color> " + baseTextColor + lifeColor + "+1 <sprite index=0></color> / " + timeColor + timeIntervalHeadHealthGem.ToString() + " seconds</color></color>",
            initColor + "Torso:</color> " + baseTextColor + lifeColor + "+2 <sprite index=0></color> / " + timeColor + timeIntervalTorsoHealthGem.ToString() + " seconds</color></color>",
            initColor + "Sword:</color> " + percentageColor + "15% chance</color> " + baseTextColor + "on enemy hit to reset all health gem cooldowns.</color>",
            initColor + "Feet:</color> " + baseTextColor + "Decreases all equipped health gem max cooldowns by " + timeColor + "5 seconds.</color></color>"};
            tooltipDict.Add("HealthGem", healthGem);

            float timeIntervalHeadPhoenixStone = vigourCoroutineTimers["Head PhoenixStone"] <= 0 ? 1 : vigourCoroutineTimers["Head PhoenixStone"];
            float timeIntervalTorsoPhoenixStone = vigourCoroutineTimers["Torso PhoenixStone"] <= 0 ? 1 : vigourCoroutineTimers["Torso PhoenixStone"];
            string[] phoenixStone = {headerContentColor + "Phoenix Stone</color> " + lifeColor + "<size=80%>(+1 max <sprite index=0>)</color>",
            headerContentColor + "You should not fear death when carrying this item.</color>",
            initColor + "Helmet:</color> " + baseTextColor + "When death would occur, regenerate back half of your health instead, while giving you immortality for " + timeColor + "2 seconds.</color></color> " + timeColor + "(" + timeIntervalHeadPhoenixStone.ToString() + " " + hourGlass + ")</color>",
            initColor + "Torso:</color> " + baseTextColor + "When death would occur, regenerate back all of your health instead, while giving you immortality for " + timeColor + "2 seconds.</color></color> " + timeColor + "(" + timeIntervalTorsoPhoenixStone.ToString() + " " + hourGlass + ")</color>",
            initColor + "Sword:</color> " + baseTextColor + "Revives a fallen enemy to fight on your side for " + timeColor + "15 seconds</color>, or until death.</color> " + timeColor + "(30 " + hourGlass + ")</color>",
            initColor + "Feet:</color> " + baseTextColor + "You can feel the phonenix's wings under your feet. You can jump higher.</color>"};
            tooltipDict.Add("PhoenixStone", phoenixStone);

            float timeIntervalHeadGemOfPeace = vigourCoroutineTimers["Head GemOfPeace"] <= 0 ? 1 : vigourCoroutineTimers["Head GemOfPeace"];
            string[] gemOfPeace = {headerContentColor + "Gem of peace</color> " + lifeColor + "<size=80%>(+1 max <sprite index=0>)</color>",
            headerContentColor + "A calm, peaceful force runs through your veins.</color>",
            initColor + "Helmet:</color> " + abilityColor + "Ability - Meditate:</color> " + percentageColor + "50% less damage</color> " + baseTextColor + " taken from enemies for " + timeColor + "15 seconds.</color> </color>" + timeColor + "(" + timeIntervalHeadGemOfPeace.ToString() + " " + hourGlass + ")</color>",
            initColor + "Torso:</color> " + baseTextColor + "Some of the enemies won't attack you because of your peaceful aura.</color>",
            initColor + "Sword:</color> " + baseTextColor + "Some of the enemies you hit with your peaceful swings will become friendly.</color>",
            initColor + "Feet:</color> " + baseTextColor + "You leave a sparkling trail of happiness wherever you go. Those who follow your footsteps have decreased " + adColor + "attack damage</color>.</color>"};
            tooltipDict.Add("GemOfPeace", gemOfPeace);

            float timeIntervalSwordFocusJewel = vigourCoroutineTimers["Sword FocusJewel"] <= 0 ? 1 : vigourCoroutineTimers["Sword FocusJewel"];
            float timeIntervalTorsoFocusJewel = vigourCoroutineTimers["Torso FocusJewel"] <= 0 ? 1 : vigourCoroutineTimers["Torso FocusJewel"];
            float timeIntervalFeetFocusJewel = vigourCoroutineTimers["Feet FocusJewel"] <= 0 ? 1 : vigourCoroutineTimers["Feet FocusJewel"];
            string[] focusJewel = {headerContentColor + "Focus jewel</color>" + lifeColor + "<size=80%>(+1 max <sprite index=0>)</color>",
            headerContentColor + "PLACEHOLDER",
            initColor + "Helmet:</color> " + baseTextColor +  "You can focus on your combat skills a lot better. " + adColor + "+1,5 attack damage</color></color>",
            initColor + "Torso:</color> " + baseTextColor +  "Your body and soul are in harmony. " + lifeColor + "+1 <sprite index=0></color> / " + timeColor + timeIntervalTorsoFocusJewel.ToString() + " seconds</color>, " + adColor + "+1 attack damage</color></color>",
            initColor + "Sword:</color> " + baseTextColor + abilityColor + "Ability - Health focus:</color> Gives " + lifeColor + "4 </color><sprite index=0> temporarily for " + timeColor + "8 seconds. (" + timeIntervalSwordFocusJewel.ToString() + " " + hourGlass + ")</color></color>",
            initColor + "Feet:</color> " + baseTextColor + abilityColor + "Ability - Kick:</color> Focus your strength in your right foot. Kick damage: " + adColor + "attack damage * 1.25</color> " + timeColor + "(" + timeIntervalFeetFocusJewel.ToString() + " " + hourGlass + ")</color></color>"};
            tooltipDict.Add("FocusJewel", focusJewel);

            string[] stoneOfVigour = {headerContentColor + "Stone of vigour</color>" + lifeColor + "<size=80%>(+1 max <sprite index=0>)</color>",
            headerContentColor + "PLACEHOLDER",
            initColor + "Helmet:</color> " + baseTextColor  + adColor + " +2 ad</color></color>",
            initColor + "Torso:</color> " + baseTextColor +  adColor + " +2 ad</color></color>",
            initColor + "Sword:</color> " + baseTextColor +  adColor + " +3 ad</color></color>",
            initColor + "Feet:</color> " + baseTextColor +  adColor + " +1 ad</color></color>"};
            tooltipDict.Add("StoneOfVigour", stoneOfVigour);

            float timeIntervalSwordSunPearl = vigourCoroutineTimers["Sword SunPearl"] <= 0 ? 1 : vigourCoroutineTimers["Sword SunPearl"];
            string[] sunPearl = {headerContentColor + "Sun pearl " + baseTextColor +  "<size=80%>(+1 max <sprite index=0>)</color>",
            headerContentColor + "PLACEHOLDER",
            initColor + "Helmet:</color> " + baseTextColor + lifeColor + "+2 max <sprite index=0></color> when adventuring in the sun, but " + deathColor + "-2 <sprite index=0></color> in the dark.</color>",
            initColor + "Torso:</color> " + baseTextColor + timeColor + "+2,5 </color>to light radius.</color>",
            initColor + "Sword:</color> " + baseTextColor + abilityColor + "Ability - Rayburst:</color> Bursts out a scorching ray of sunlight in the direction of the cursor, damaging all enemies it goes through. Rayburst damage: " + apColor + "magic damage * 1.45</color> " + timeColor + "(" + timeIntervalSwordSunPearl.ToString() + " " + hourGlass + ")</color></color>",
            initColor + "Feet:</color> " + baseTextColor +  "Leave the ground scorched behind you for " + timeColor + "3 seconds.</color> enemies are continuously damaged while on scorched ground</color>"};
            tooltipDict.Add("SunPearl", sunPearl);

            // string[] placeholder5 = {"Phoenix Stone " + baseTextColor +  "<size=80%>(+1 max <sprite index=0>)</color>",
            // "You should not fear death when carrying this item.",
            // initColor + "Helmet:</color> " + baseTextColor +  "</color>",
            // initColor + "Torso:</color> " + baseTextColor +  " +2 max <sprite index=0> / 10 seconds</color>",
            // initColor + "Sword:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Feet:</color> " + baseTextColor +  " placeholder</color>"};
            // tooltipDict.Add("PhoenixStone", phoenixStone);

            // /////////////Death gems//////////////
            string[] gemOfMadness = {headerContentColor + "Gem of Madness</color> " + deathColor + "<size=80%>(-1 max <sprite index=0>)</color>",
            headerContentColor + "PLCAEHOLDER",
            initColor + "Helmet:</color> " + baseTextColor + "Nearby enemies can't bear the gems negative vibration. Some of them choose to end their lives instead.</color>",
            initColor + "Torso:</color> " + baseTextColor +  "PLACEHOLDER</color>",
            initColor + "Sword:</color> " + baseTextColor +  "PLACEHOLDER</color>",
            initColor + "Feet:</color> " + baseTextColor +  "PLACEHOLDER</color>"};
            tooltipDict.Add("GemOfMadness", gemOfMadness);

            // string[] painBringer = {"Balance gem <color=purple><size=80%>(-1 <sprite index=0>)</color>",
            // "Find balance in nature.",
            // initColor + "Helmet:</color> <color=purple>-2 <sprite index=4></color>, " + baseTextColor +  " +1 max <sprite index=0> / 10 seconds</color>",
            // initColor + "Torso:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Sword:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Feet:</color> " + baseTextColor +  " placeholder</color>"};
            // tooltipDict.Add("PainBringer", painBringer);

            // string[] gemSepticEye = {"Balance gem <color=purple><size=80%>(-1 <sprite index=0>)</color>",
            // "Find balance in nature.",
            // initColor + "Helmet:</color> <color=purple>-2 <sprite index=4></color>, " + baseTextColor +  " +1 max <sprite index=0> / 10 seconds</color>",
            // initColor + "Torso:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Sword:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Feet:</color> " + baseTextColor +  " placeholder</color>"};
            // tooltipDict.Add("GemSepticEye", gemSepticEye);

            // string[] demonStone = {"Balance gem <color=purple><size=80%>(-1 <sprite index=0>)</color>",
            // "Find balance in nature.",
            // initColor + "Helmet:</color> <color=purple>-2 <sprite index=4></color>, " + baseTextColor +  " +1 max <sprite index=0> / 10 seconds</color>",
            // initColor + "Torso:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Sword:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Feet:</color> " + baseTextColor +  " placeholder</color>"};
            // tooltipDict.Add("DemonStone", demonStone);

            // string[] nightJewel = {"Balance gem <color=purple><size=80%>(-1 <sprite index=0>)</color>",
            // "Find balance in nature.",
            // initColor + "Helmet:</color> <color=purple>-2 <sprite index=4></color>, " + baseTextColor +  " +1 max <sprite index=0> / 10 seconds</color>",
            // initColor + "Torso:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Sword:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Feet:</color> " + baseTextColor +  " placeholder</color>"};
            // tooltipDict.Add("NightJewel", nightJewel);

            // string[] vampiricPearl = {"Balance gem <color=purple><size=80%>(-1 <sprite index=0>)</color>",
            // "Find balance in nature.",
            // initColor + "Helmet:</color> <color=purple>-2 <sprite index=4></color>, " + baseTextColor +  " +1 max <sprite index=0> / 10 seconds</color>",
            // initColor + "Torso:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Sword:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Feet:</color> " + baseTextColor +  " placeholder</color>"};
            // tooltipDict.Add("VampiricPearl", vampiricPearl);

            // string[] forbiddenGem = {"Balance gem <color=purple><size=80%>(-1 <sprite index=0>)</color>",
            // "Find balance in nature.",
            // initColor + "Helmet:</color> <color=purple>-2 <sprite index=4></color>, " + baseTextColor +  " +1 max <sprite index=0> / 10 seconds</color>",
            // initColor + "Torso:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Sword:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Feet:</color> " + baseTextColor +  " placeholder</color>"};
            // tooltipDict.Add("ForbiddenGem", forbiddenGem);

            // string[] dragonStone = {"Balance gem <color=purple><size=80%>(-1 <sprite index=0>)</color>",
            // "Find balance in nature.",
            // initColor + "Helmet:</color> <color=purple>-2 <sprite index=4></color>, " + baseTextColor +  " +1 max <sprite index=0> / 10 seconds</color>",
            // initColor + "Torso:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Sword:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Feet:</color> " + baseTextColor +  " placeholder</color>"};
            // tooltipDict.Add("DragonStone", dragonStone);

            // /////////////Balance gems////////////

            string[] balanceGem = {"Balance gem <size=80%>(-1 max <sprite index=0>)",
            "Find balance in nature.",
            "Helmet:</color> <color=purple>-2 max  <sprite index=4></color>, " + baseTextColor +  " +1 <sprite index=0> / 10 seconds</color>",
            "Torso:</color> " + baseTextColor +  " placeholder</color>",
            "Sword:</color> " + baseTextColor +  " placeholder</color>",
            "Feet:</color> " + baseTextColor +  " placeholder</color>"};
            tooltipDict.Add("BalanceGem", balanceGem);

            // string[] balanceGem1 = {"Balance gem <size=80%>(-1 <sprite index=0>)",
            // "Find balance in nature.",
            // initColor + "Helmet:</color> <color=purple>-2 <sprite index=4></color>, " + baseTextColor +  " +1 max <sprite index=0> / 10 seconds</color>",
            // initColor + "Torso:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Sword:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Feet:</color> " + baseTextColor +  " placeholder</color>"};
            // tooltipDict.Add("BalanceGem", balanceGem);

            // string[] balanceGem2 = {"Balance gem <size=80%>(-1 <sprite index=0>)",
            // "Find balance in nature.",
            // initColor + "Helmet:</color> <color=purple>-2 <sprite index=4></color>, " + baseTextColor +  " +1 max <sprite index=0> / 10 seconds</color>",
            // initColor + "Torso:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Sword:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Feet:</color> " + baseTextColor +  " placeholder</color>"};
            // tooltipDict.Add("BalanceGem", balanceGem);

            // string[] balanceGem3 = {"Balance gem <size=80%>(-1 <sprite index=0>)",
            // "Find balance in nature.",
            // initColor + "Helmet:</color> <color=purple>-2 <sprite index=4></color>, " + baseTextColor +  " +1 max <sprite index=0> / 10 seconds</color>",
            // initColor + "Torso:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Sword:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Feet:</color> " + baseTextColor +  " placeholder</color>"};
            // tooltipDict.Add("BalanceGem", balanceGem);

            // string[] balanceGem4 = {"Balance gem <size=80%>(-1 <sprite index=0>)",
            // "Find balance in nature.",
            // initColor + "Helmet:</color> <color=purple>-2 <sprite index=4></color>, " + baseTextColor +  " +1 max <sprite index=0> / 10 seconds</color>",
            // initColor + "Torso:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Sword:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Feet:</color> " + baseTextColor +  " placeholder</color>"};
            // tooltipDict.Add("BalanceGem", balanceGem);

            // string[] balanceGem5 = {"Balance gem <size=80%>(-1 <sprite index=0>)",
            // "Find balance in nature.",
            // initColor + "Helmet:</color> <color=purple>-2 <sprite index=4></color>, " + baseTextColor +  " +1 max <sprite index=0> / 10 seconds</color>",
            // initColor + "Torso:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Sword:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Feet:</color> " + baseTextColor +  " placeholder</color>"};
            // tooltipDict.Add("BalanceGem", balanceGem);

            // string[] balanceGem6 = {"Balance gem <size=80%>(-1 <sprite index=0>)",
            // "Find balance in nature.",
            // initColor + "Helmet:</color> <color=purple>-2 <sprite index=4></color>, " + baseTextColor +  " +1 max <sprite index=0> / 10 seconds</color>",
            // initColor + "Torso:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Sword:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Feet:</color> " + baseTextColor +  " placeholder</color>"};
            // tooltipDict.Add("BalanceGem", balanceGem);

            // string[] balanceGem7 = {"Balance gem <size=80%>(-1 <sprite index=0>)",
            // "Find balance in nature.",
            // initColor + "Helmet:</color> <color=purple>-2 <sprite index=4></color>, " + baseTextColor +  " +1 max <sprite index=0> / 10 seconds</color>",
            // initColor + "Torso:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Sword:</color> " + baseTextColor +  " placeholder</color>",
            // initColor + "Feet:</color> " + baseTextColor +  " placeholder</color>"};
            // tooltipDict.Add("BalanceGem", balanceGem);
        }
    }

    void ResetPhoenixStoneTimer(string parentObject)
    {
        StopCoroutine(phoenixRespawnCoroutine);
        vigourCoroutines.Remove(phoenixRespawnCoroutine);
        float timeIntervalPhoenixStone = 0;

        if (parentObject == "half")
        {
            timeIntervalPhoenixStone = vigourCoroutineTimers["Head PhoenixStone"] <= 0 ? 1 : vigourCoroutineTimers["Head PhoenixStone"];
        }
        else if (parentObject == "full")
        {
            timeIntervalPhoenixStone = vigourCoroutineTimers["Torso PhoenixStone"] <= 0 ? 1 : vigourCoroutineTimers["Torso PhoenixStone"];
        }
        phoenixRespawnCoroutine = StartCoroutine(GemBuffManager.Instance.CountDownPhoenixCD(timeIntervalPhoenixStone, parentObject));
        vigourCoroutines.Add(phoenixRespawnCoroutine);
    }

    void ChangeMaxCD(float seconds, string gemType)
    {
        Dictionary<string, float> tempVigourCoroutineTimers = new Dictionary<string, float>();

        foreach (KeyValuePair<string, float> nameAndTimer in vigourCoroutineTimers)
        {
            tempVigourCoroutineTimers.Add(nameAndTimer.Key, nameAndTimer.Value);
        }

        foreach (KeyValuePair<string, float> nameAndTimer in tempVigourCoroutineTimers)
        {
            vigourCoroutineTimers[nameAndTimer.Key] = nameAndTimer.Value + seconds;
        }
        ResetGemDictionary();
        resetAbilityDict();

        ResetCoroutines(gemType);

        reDrawToolTips();
        onCooldownChange(gemType);
    }

    void ResetGemDictionary()
    {
        InitDicts(true);
    }

    public void ResetCoroutines(string gemType)
    {
        if (gemType == "vigour")
        {
            foreach (Coroutine coroutine in vigourCoroutines)
            {
                StopCoroutine(coroutine);
            }
            vigourCoroutines.Clear();

            List<string> tempVigourParentList = new List<string>();
            vigourGemWithCDParents.ForEach((item) => { tempVigourParentList.Add(item); });
            vigourGemWithCDParents.Clear();

            foreach (string gemParentName in tempVigourParentList)
            {
                ReCalculateBuffs(gemParentName.Split(' ')[1], null, true, gemParentName.Split(' ')[0]);
            }
        }
        else if (gemType == "death")
        {
            foreach (Coroutine coroutine in deathCoroutines)
            {
                StopCoroutine(coroutine);
            }

            List<string> tempDeathParentList = new List<string>();
            deathGemWithCDParents.ForEach((item) => { tempDeathParentList.Add(item); });
            deathCoroutines.Clear();

            foreach (string gemParentName in deathGemWithCDParents)
            {
                ReCalculateBuffs(gemParentName.Split(' ')[1], null, true, gemParentName.Split(' ')[0]);
            }
        }
        else if (gemType == "balance")
        {
            foreach (Coroutine coroutine in balanceCoroutines)
            {
                StopCoroutine(coroutine);
            }

            List<string> tempBalanceParentList = new List<string>();
            balanceGemWithCDParents.ForEach((item) => { tempBalanceParentList.Add(item); });
            balanceCoroutines.Clear();

            foreach (string gemParentName in balanceGemWithCDParents)
            {
                ReCalculateBuffs(gemParentName.Split(' ')[1], null, true, gemParentName.Split(' ')[0]);
            }
        }
    }

    public string[] GetTooltip(string gemName) {
        return tooltipDict[gemName];
    }

    public bool IsKeyInDict(string gemName) {
        return tooltipDict.ContainsKey(gemName);
    }

    public void ReCalculateBuffs(string gemName, Transform parentTransform, bool isAdding, string parentString) {
        string parentTag = parentTransform != null ? parentTransform.parent.tag : parentString;
        
        if (string.IsNullOrEmpty(parentTag)) {return;}
        if (isAdding) {
            switch (parentTag) {
                case "Head":
                    switch (gemName) {
                        case "GemOfLife":
                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(2);
                            GemBuffManager.Instance.SetMaxHealth(2);
                            break;
                        case "HealthGem":
                            float timeIntervalHealthGem = vigourCoroutineTimers["Head HealthGem"] <= 0 ? 1 : vigourCoroutineTimers["Head HealthGem"];
                            healthCoroutine = StartCoroutine(GemBuffManager.Instance.RegenerateOverTime(1, timeIntervalHealthGem));
                            vigourCoroutines.Add(healthCoroutine);
                            vigourGemWithCDParents.Add("Head HealthGem");

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "PhoenixStone":
                            float timeIntervalPhoenixStone = vigourCoroutineTimers["Head PhoenixStone"] <= 0 ? 1 : vigourCoroutineTimers["Head PhoenixStone"];
                            phoenixRespawnCoroutine = StartCoroutine(GemBuffManager.Instance.CountDownPhoenixCD(timeIntervalPhoenixStone, "half"));
                            vigourCoroutines.Add(phoenixRespawnCoroutine);
                            vigourGemWithCDParents.Add("Head PhoenixStone");

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "GemOfPeace":
                            GemBuffManager.Instance.SetAbilityActive("Meditate", true);

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "FocusJewel":
                            GemBuffManager.Instance.SetAccumulativeAttackDamageByVigour(1.5f);

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "StoneOfVigour":
                            GemBuffManager.Instance.SetAccumulativeAttackDamageByVigour(2);

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "vigour2":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "vigour3":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "GemOfMadness":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "PainBringer":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "GemSepticEye":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "DemonStone":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "NightJewel":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "VampiricPearl":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "ForbiddenGem":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "DragonStone":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "balance0":
                            Debug.Log("placeholder");
                            break;
                        case "balance1":
                            Debug.Log("placeholder");
                            break;
                        case "balance2":
                            Debug.Log("placeholder");
                            break;
                        case "balance3":
                            Debug.Log("placeholder");
                            break;
                        case "balance4":
                            Debug.Log("placeholder");
                            break;
                        case "balance5":
                            Debug.Log("placeholder");
                            break;
                        case "balance6":
                            Debug.Log("placeholder");
                            break;
                        case "balance7":
                            Debug.Log("placeholder");
                            break;
                    }
                    break;
                case "Torso":
                    switch (gemName) {
                        case "GemOfLife":
                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(3);
                            GemBuffManager.Instance.SetMaxHealth(3);
                            break;
                        case "HealthGem":
                            float timeInterval = vigourCoroutineTimers["Torso HealthGem"] <= 0 ? 1 : vigourCoroutineTimers["Torso HealthGem"];
                            healthCoroutine = StartCoroutine(GemBuffManager.Instance.RegenerateOverTime(2, vigourCoroutineTimers["Torso HealthGem"]));
                            vigourCoroutines.Add(healthCoroutine);
                            vigourGemWithCDParents.Add("Torso HealthGem");

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "PhoenixStone":
                            float timeIntervalPhoenixStone = vigourCoroutineTimers["Torso PhoenixStone"] <= 0 ? 1 : vigourCoroutineTimers["Torso PhoenixStone"];
                            phoenixRespawnCoroutine = StartCoroutine(GemBuffManager.Instance.CountDownPhoenixCD(timeIntervalPhoenixStone, "full"));
                            vigourCoroutines.Add(phoenixRespawnCoroutine);
                            vigourGemWithCDParents.Add("Torso PhoenixStone");

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "GemOfPeace":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "FocusJewel":
                            GemBuffManager.Instance.SetAccumulativeAttackDamageByVigour(1f);
                            float timeIntervalFocusJewel = vigourCoroutineTimers["Torso FocusJewel"] <= 0 ? 1 : vigourCoroutineTimers["Torso FocusJewel"];
                            focusCoroutine = StartCoroutine(GemBuffManager.Instance.RegenerateOverTime(1, timeIntervalFocusJewel));
                            vigourCoroutines.Add(focusCoroutine);
                            vigourGemWithCDParents.Add("Torso FocusJewel");

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "StoneOfVigour":
                            GemBuffManager.Instance.SetAccumulativeAttackDamageByVigour(2);

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "vigour2":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "vigour3":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "GemOfMadness":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "PainBringer":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "GemSepticEye":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "DemonStone":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "NightJewel":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "VampiricPearl":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "ForbiddenGem":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "DragonStone":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "balance0":
                            Debug.Log("placeholder");
                            break;
                        case "balance1":
                            Debug.Log("placeholder");
                            break;
                        case "balance2":
                            Debug.Log("placeholder");
                            break;
                        case "balance3":
                            Debug.Log("placeholder");
                            break;
                        case "balance4":
                            Debug.Log("placeholder");
                            break;
                        case "balance5":
                            Debug.Log("placeholder");
                            break;
                        case "balance6":
                            Debug.Log("placeholder");
                            break;
                        case "balance7":
                            Debug.Log("placeholder");
                            break;
                    }
                    break;
                case "Sword":
                    switch (gemName) {
                        case "GemOfLife":
                            GemBuffManager.Instance.IncreaseHealthDropChance(.15f);

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "HealthGem":
                            GemBuffManager.Instance.AddChanceToCDReset(.15f, "vigour");

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "PhoenixStone":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "GemOfPeace":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "FocusJewel":
                            GemBuffManager.Instance.SetAbilityActive("HealthFocus", true);

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "StoneOfVigour":
                            GemBuffManager.Instance.SetAccumulativeAttackDamageByVigour(3);

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "SunPearl":
                            GemBuffManager.Instance.SetAbilityActive("RayBurst", true);

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "vigour3":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "GemOfMadness":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "PainBringer":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "GemSepticEye":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "DemonStone":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "NightJewel":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "VampiricPearl":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "ForbiddenGem":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "DragonStone":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "balance0":
                            Debug.Log("placeholder");
                            break;
                        case "balance1":
                            Debug.Log("placeholder");
                            break;
                        case "balance2":
                            Debug.Log("placeholder");
                            break;
                        case "balance3":
                            Debug.Log("placeholder");
                            break;
                        case "balance4":
                            Debug.Log("placeholder");
                            break;
                        case "balance5":
                            Debug.Log("placeholder");
                            break;
                        case "balance6":
                            Debug.Log("placeholder");
                            break;
                        case "balance7":
                            Debug.Log("placeholder");
                            break;
                    }
                    break;
                case "Feet":
                    switch (gemName) {
                        case "GemOfLife":
                            Debug.Log("placeholder");

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "HealthGem":
                            ChangeMaxCD(-5f, "vigour");

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "PhoenixStone":
                            GemBuffManager.Instance.SetJumpStrength(3f);

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "GemOfPeace":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "FocusJewel":
                            GemBuffManager.Instance.SetAbilityActive("Kick", true);

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "StoneOfVigour":
                            GemBuffManager.Instance.SetAccumulativeAttackDamageByVigour(1);

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "vigour2":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "vigour3":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "GemOfMadness":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "PainBringer":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "GemSepticEye":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "DemonStone":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "NightJewel":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "VampiricPearl":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "ForbiddenGem":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "DragonStone":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "balance0":
                            Debug.Log("placeholder");
                            break;
                        case "balance1":
                            Debug.Log("placeholder");
                            break;
                        case "balance2":
                            Debug.Log("placeholder");
                            break;
                        case "balance3":
                            Debug.Log("placeholder");
                            break;
                        case "balance4":
                            Debug.Log("placeholder");
                            break;
                        case "balance5":
                            Debug.Log("placeholder");
                            break;
                        case "balance6":
                            Debug.Log("placeholder");
                            break;
                        case "balance7":
                            Debug.Log("placeholder");
                            break;
                    }
                    break;
                default:
                    break;
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        } else if (!isAdding) {
            switch (parentTag) {
                case "Head":
                    switch (gemName) {
                        case "GemOfLife":
                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-2);
                            GemBuffManager.Instance.SetMaxHealth(-2);
                            break;
                        case "HealthGem":
                            StopCoroutine(healthCoroutine);
                            vigourCoroutines.Remove(healthCoroutine);
                            vigourGemWithCDParents.Remove("Head HealthGem");

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "PhoenixStone":
                            StopCoroutine(phoenixRespawnCoroutine);
                            vigourCoroutines.Remove(phoenixRespawnCoroutine);
                            vigourGemWithCDParents.Remove("Head PhoenixStone");

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "GemOfPeace":
                            GemBuffManager.Instance.SetAbilityActive("Meditate", false);

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "FocusJewel":
                            GemBuffManager.Instance.SetAccumulativeAttackDamageByVigour(-1.5f);

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "StoneOfVigour":
                            GemBuffManager.Instance.SetAccumulativeAttackDamageByVigour(-2);

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "vigour2":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "vigour3":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "GemOfMadness":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "PainBringer":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "GemSepticEye":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "DemonStone":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "NightJewel":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "VampiricPearl":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "ForbiddenGem":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "DragonStone":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "balance0":
                            Debug.Log("placeholder");
                            break;
                        case "balance1":
                            Debug.Log("placeholder");
                            break;
                        case "balance2":
                            Debug.Log("placeholder");
                            break;
                        case "balance3":
                            Debug.Log("placeholder");
                            break;
                        case "balance4":
                            Debug.Log("placeholder");
                            break;
                        case "balance5":
                            Debug.Log("placeholder");
                            break;
                        case "balance6":
                            Debug.Log("placeholder");
                            break;
                        case "balance7":
                            Debug.Log("placeholder");
                            break;
                    }
                    break;
                case "Torso":
                    switch (gemName) {
                        case "GemOfLife":
                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-3);
                            GemBuffManager.Instance.SetMaxHealth(-3);
                            break;
                        case "HealthGem":
                            StopCoroutine(healthCoroutine);
                            vigourCoroutines.Remove(healthCoroutine);
                            vigourGemWithCDParents.Remove("Torso HealthGem");

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "PhoenixStone":
                            StopCoroutine(phoenixRespawnCoroutine);
                            vigourCoroutines.Remove(phoenixRespawnCoroutine);
                            vigourGemWithCDParents.Remove("Torso PhoenixStone");

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "GemOfPeace":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "FocusJewel":
                            GemBuffManager.Instance.SetAccumulativeAttackDamageByVigour(-1f);
                            StopCoroutine(focusCoroutine);
                            vigourCoroutines.Remove(focusCoroutine);
                            vigourGemWithCDParents.Remove("Torso FocusJewel");

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "StoneOfVigour":
                            GemBuffManager.Instance.SetAccumulativeAttackDamageByVigour(-2);

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "vigour2":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "vigour3":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "GemOfMadness":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "PainBringer":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "GemSepticEye":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "DemonStone":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "NightJewel":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "VampiricPearl":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "ForbiddenGem":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "DragonStone":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "balance0":
                            Debug.Log("placeholder");
                            break;
                        case "balance1":
                            Debug.Log("placeholder");
                            break;
                        case "balance2":
                            Debug.Log("placeholder");
                            break;
                        case "balance3":
                            Debug.Log("placeholder");
                            break;
                        case "balance4":
                            Debug.Log("placeholder");
                            break;
                        case "balance5":
                            Debug.Log("placeholder");
                            break;
                        case "balance6":
                            Debug.Log("placeholder");
                            break;
                        case "balance7":
                            Debug.Log("placeholder");
                            break;
                    }
                    break;
                case "Sword":
                    switch (gemName) {
                        case "GemOfLife":
                            GemBuffManager.Instance.IncreaseHealthDropChance(-.15f);

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "HealthGem":
                            GemBuffManager.Instance.AddChanceToCDReset(-.15f, "vigour");

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "PhoenixStone":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "GemOfPeace":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "FocusJewel":
                            GemBuffManager.Instance.SetAbilityActive("HealthFocus", false);

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "StoneOfVigour":
                            GemBuffManager.Instance.SetAccumulativeAttackDamageByVigour(-3);

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "SunPearl":
                            GemBuffManager.Instance.SetAbilityActive("RayBurst", false);

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "vigour3":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "GemOfMadness":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "PainBringer":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "GemSepticEye":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "DemonStone":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "NightJewel":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "VampiricPearl":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "ForbiddenGem":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "DragonStone":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "balance0":
                            Debug.Log("placeholder");
                            break;
                        case "balance1":
                            Debug.Log("placeholder");
                            break;
                        case "balance2":
                            Debug.Log("placeholder");
                            break;
                        case "balance3":
                            Debug.Log("placeholder");
                            break;
                        case "balance4":
                            Debug.Log("placeholder");
                            break;
                        case "balance5":
                            Debug.Log("placeholder");
                            break;
                        case "balance6":
                            Debug.Log("placeholder");
                            break;
                        case "balance7":
                            Debug.Log("placeholder");
                            break;
                    }
                    break;
                case "Feet":
                    switch (gemName) {
                        case "GemOfLife":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "HealthGem":
                            ChangeMaxCD(5f, "vigour");

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "PhoenixStone":
                            GemBuffManager.Instance.SetJumpStrength(-3f);

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "GemOfPeace":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "FocusJewel":
                            GemBuffManager.Instance.SetAbilityActive("Kick", false);

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "StoneOfVigour":
                            GemBuffManager.Instance.SetAccumulativeAttackDamageByVigour(-1);

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "vigour2":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "vigour3":

                            GemBuffManager.Instance.SetAccumulativeHealthByVigour(-1);
                            GemBuffManager.Instance.SetMaxHealth(-1);
                            break;
                        case "GemOfMadness":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "PainBringer":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "GemSepticEye":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "DemonStone":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "NightJewel":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "VampiricPearl":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "ForbiddenGem":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "DragonStone":

                            GemBuffManager.Instance.SetAccumulativeHealthByDeath(1);
                            GemBuffManager.Instance.SetMaxHealth(1);
                            break;
                        case "balance0":
                            Debug.Log("placeholder");
                            break;
                        case "balance1":
                            Debug.Log("placeholder");
                            break;
                        case "balance2":
                            Debug.Log("placeholder");
                            break;
                        case "balance3":
                            Debug.Log("placeholder");
                            break;
                        case "balance4":
                            Debug.Log("placeholder");
                            break;
                        case "balance5":
                            Debug.Log("placeholder");
                            break;
                        case "balance6":
                            Debug.Log("placeholder");
                            break;
                        case "balance7":
                            Debug.Log("placeholder");
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
