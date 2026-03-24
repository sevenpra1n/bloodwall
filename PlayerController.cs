using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Character
{
    [SerializeField] private float power = 100f;
    [SerializeField] private int maxMana = 100;
    private int currentMana;

    [SerializeField] private Text manaText;
    [SerializeField] private Image[] manaBarTextures;

    private PlayerSkill[] skills = new PlayerSkill[4];

    private int poisonDuration = 0;
    private float poisonDamage = 0f;

    private bool lastAttackWasCrit = false;
    private bool lastAttackWasMiss = false;

    private BattleManager battleManager;

    protected override void Start()
    {
        // ЗАГРУЖАЕМ МОЩЬ из EquipmentScene
        power = PlayerPrefs.GetInt("PlayerPower", 100);

        Debug.Log("Загружена мощь игрока: " + power);

        maxHP = power * 1f;
        currentHP = maxHP;
        currentMana = maxMana;

        battleManager = FindObjectOfType<BattleManager>();

        InitializeKnightSkills();

        UpdateHPUI();
        UpdateManaUI();
    }

    private void InitializeKnightSkills()
    {
        skills[0] = new PlayerSkill
        {
            name = "Тяжелый удар",
            manaCost = 30,
            isLocked = false,
            damageMultiplier = 0.3f
        };

        skills[1] = new PlayerSkill { name = "Заблокировано", manaCost = 0, isLocked = true, damageMultiplier = 0 };
        skills[2] = new PlayerSkill { name = "Заблокировано", manaCost = 0, isLocked = true, damageMultiplier = 0 };
        skills[3] = new PlayerSkill { name = "Заблокировано", manaCost = 0, isLocked = true, damageMultiplier = 0 };
    }

    public bool CanUseSkill(int skillIndex)
    {
        if (skills == null || skills.Length == 0)
            return false;

        if (skillIndex < 0 || skillIndex >= skills.Length)
            return false;

        if (skills[skillIndex] == null || skills[skillIndex].isLocked)
            return false;

        return currentMana >= skills[skillIndex].manaCost;
    }

    public void UseSkill(int skillIndex, Character enemy)
    {
        if (!CanUseSkill(skillIndex))
            return;

        currentMana -= skills[skillIndex].manaCost;
        UpdateManaUI();

        float damage = power * skills[skillIndex].damageMultiplier;

        if (lastAttackWasCrit)
            damage *= 2;

        enemy.TakeDamage(damage);
    }

    public void BasicAttack(Character enemy)
    {
        float damage = power * 0.15f;
        if (lastAttackWasCrit)
            damage *= 2;

        enemy.TakeDamage(damage);
    }

    public string GetSkillName(int index)
    {
        if (skills == null || index < 0 || index >= skills.Length || skills[index] == null)
            return "";
        return skills[index].name;
    }

    public int GetSkillManaCost(int index)
    {
        if (skills == null || index < 0 || index >= skills.Length || skills[index] == null)
            return 0;
        return skills[index].manaCost;
    }

    public bool IsSkillLocked(int index)
    {
        if (skills == null || index < 0 || index >= skills.Length || skills[index] == null)
            return true;
        return skills[index].isLocked;
    }

    private void UpdateManaUI()
    {
        if (manaText != null)
            manaText.text = currentMana + "/" + maxMana;

        UpdateManaBarTexture();

        if (battleManager != null)
        {
            battleManager.UpdateSkillUI();
        }
    }

    private void UpdateManaBarTexture()
    {
        if (manaBarTextures == null || manaBarTextures.Length == 0)
            return;

        foreach (Image texture in manaBarTextures)
        {
            if (texture != null)
                texture.enabled = false;
        }

        float manaPercent = (float)currentMana / maxMana;
        int textureIndex = Mathf.RoundToInt(manaPercent * 8);
        if (textureIndex > 8) textureIndex = 8;
        if (textureIndex < 0) textureIndex = 0;

        if (textureIndex < manaBarTextures.Length && manaBarTextures[textureIndex] != null)
        {
            manaBarTextures[textureIndex].enabled = true;
        }
    }

    public void RestoreMana(int amount)
    {
        currentMana += amount;
        if (currentMana > maxMana)
            currentMana = maxMana;
        UpdateManaUI();
    }

    public int GetCurrentMana()
    {
        return currentMana;
    }

    public void UpdatePoison()
    {
        if (poisonDuration > 0)
        {
            TakeDamage(poisonDamage);
            poisonDuration--;
        }
    }

    public bool CheckMiss()
    {
        lastAttackWasMiss = Random.value < 0.1f;
        return lastAttackWasMiss;
    }

    public bool CheckCrit()
    {
        lastAttackWasCrit = Random.value < 0.2f;
        return lastAttackWasCrit;
    }

    public bool WasLastAttackCrit()
    {
        return lastAttackWasCrit;
    }

    public bool WasLastAttackMiss()
    {
        return lastAttackWasMiss;
    }

    public float GetPower()
    {
        return power;
    }

    public void SetPower(float newPower)
    {
        power = newPower;
        maxHP = power * 1f;
        currentHP = maxHP;
        UpdateHPUI();
    }
}

public class PlayerSkill
{
    public string name;
    public int manaCost;
    public bool isLocked;
    public float damageMultiplier;
}