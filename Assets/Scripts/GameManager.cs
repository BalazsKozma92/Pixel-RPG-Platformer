using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldValText;
    [SerializeField] TextMeshProUGUI vigourFragmentValText;
    [SerializeField] TextMeshProUGUI deathFragmentValText;
    [SerializeField] TextMeshProUGUI balanceFragmentValText;
    [SerializeField] TextMeshProUGUI maxHealthValText;
    [SerializeField] TextMeshProUGUI currentHealthValText;
    [SerializeField] TextMeshProUGUI attackDamageValText;
    [SerializeField] TextMeshProUGUI magicDamageValText;

    [Header("Respawn")]
    [SerializeField] Transform respawnPoint;
    [SerializeField] GameObject player;
    [SerializeField] float respawnTime;
    [SerializeField] CinemachineVirtualCamera cmCamera;

    // public HUD hud;
    public Dictionary<string, Sprite> inventory = new Dictionary<string, Sprite>();

    float respawnTimeStart;
    bool respawn;

    int slimeKillCount;

    PlayerCombat playerAttack;

    private static GameManager instance;

    int gold;
    int vigourFragments;
    int deathFragments;
    int balanceFragments;
    
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<GameManager>();
            return instance;
        }
    }

    private void Awake()
    {
        playerAttack = FindObjectOfType<PlayerCombat>();
    }

    private void Start()
    {
        // maxHealthValText.text = "<color=red>" + PlayerBase.Instance.maxHealth.ToString() + "</color> (<color=#B3A91C>+" + GemBuffManager.Instance.GetBonusVigourHealth().ToString() + " vigour</color> <color=#D52EF6>-" + GemBuffManager.Instance.GetDamageFromDeath().ToString() + " death</color>)";
        // currentHealthValText.text = "<color=red>" + PlayerBase.Instance.maxHealth.ToString() + "</color>";
        // attackDamageValText.text = "<color=orange>" + playerAttack.GetAttackDamage().ToString() + "</color> (<color=#B3A91C>+" + GemBuffManager.Instance.GetBonusVigourAttackDamage().ToString() + " vigour</color> <color=#D52EF6>-" + GemBuffManager.Instance.GetBonusDeathAttackDamage().ToString() + " death</color>)";
        // magicDamageValText.text = "<color=#6E8DE1>" + playerAttack.GetMagicDamage().ToString() + "</color>";
        goldValText.text = "<color=yellow>0</color>";
        vigourFragmentValText.text = "<color=#B3A91C>0 fragments</color>";
        deathFragmentValText.text =  "<color=#D52EF6>0 fragments</color>";
        balanceFragmentValText.text = "<color=lightblue>0 fragments</color>";
    }

    private void Update() {
        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            AudioPlayer.Instance.PlayReceiveGold(); 
            ChangeGoldBy(1);
        }

        CheckRespawn();
    }

    public void Respawn()
    {
        respawnTimeStart = Time.time;
        respawn = true;
    }

    void CheckRespawn()
    {
        if (Time.time >= respawnTimeStart + respawnTime && respawn)
        {
            var tempPlayer = Instantiate(player, respawnPoint);
            cmCamera.m_Follow = tempPlayer.transform;
            respawn = false;
        }
    }

    public void AddToKillCount(EnemyName enemyName)
    {
        switch (enemyName)
        {
            case EnemyName.Slime:
                slimeKillCount += 1;
                break;
        }
    }

    public int GetKillCount(EnemyName enemyName)
    {
        switch (enemyName)
        {
            case EnemyName.Slime:
                return slimeKillCount;
        }
        return 0;
    }

    public void SetMaxHealthText(float setToThisValue)
    {
        maxHealthValText.text = "<color=red>" + setToThisValue.ToString() + "</color> (<color=#B3A91C>+" + GemBuffManager.Instance.GetBonusVigourHealth().ToString() + " from vigour</color> <color=#D52EF6>-" + Mathf.Abs(GemBuffManager.Instance.GetDamageFromDeath()).ToString() + " from death</color>)";
    }

    public void SetCurrentHealthText(float setToThisValue)
    {
        currentHealthValText.text = "<color=red>" + setToThisValue.ToString() + "</color>";
    }

    public void SetAttackDamageText(float setToThisValue)
    {
        attackDamageValText.text = "<color=orange>" + setToThisValue.ToString() + "</color> (<color=#B3A91C>+" + GemBuffManager.Instance.GetBonusVigourAttackDamage().ToString() + " from vigour</color> <color=#D52EF6>-" + GemBuffManager.Instance.GetBonusDeathAttackDamage().ToString() + " from death</color>)";
    }

    public void SetMagicDamageText(float setToThisValue)
    {
        magicDamageValText.text = "<color=#6E8DE1>" + setToThisValue.ToString() + "</color>";
    }

    public int GetGold()
    {
        return gold;
    }

    public int GetVigourFragments()
    {
        return vigourFragments;
    }

    public int GetDeathFragments()
    {
        return deathFragments;
    }

    public int GetBalanceFragments()
    {
        return balanceFragments;
    }

    public void ChangeGoldBy(int amount)
    {
        gold += amount;
        goldValText.text = "<color=yellow>" + gold.ToString() + "</color>";
    }

    public void ChangeVigourFragmentAmount(int amount)
    {
        vigourFragments += amount;
        vigourFragmentValText.text = "<color=#B3A91C>" + vigourFragments.ToString() + " fragments</color>";
    }

    public void ChangeDeathFragmentAmount(int amount)
    {
        deathFragments += amount;
        deathFragmentValText.text = "<color=#D52EF6>" + deathFragments.ToString() + " fragments</color>";
    }

    public void ChangeBalanceFragmentAmount(int amount)
    {
        balanceFragments += amount;
        balanceFragmentValText.text = "<color=lightblue>" + balanceFragments.ToString() + " fragments</color>";
    }

    public void GetInventoryItem(string name, Sprite image)
    {
        inventory.Add(name, image);

        if (image != null)
        {
            // hud.SetInventoryImage(inventory[name]);
        }
    }

    public void RemoveInventoryItem(string name)
    {
        inventory.Remove(name);
        // hud.SetInventoryImage(hud.blankUI);
    }

    public void ClearInventory()
    {   
        inventory.Clear();
        // hud.SetInventoryImage(hud.blankUI);
    }
}
