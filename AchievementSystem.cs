using UnityEngine;

public class AchievementSystem : MonoBehaviour
{
    // Achievement 1: Deal Damage
    public static readonly int[] DamageTargets = { 500, 3500, 8500, 15000, 35000 };
    public static readonly int[] DamageRewards = { 30, 75, 250, 500, 1250 }; // coins

    // Achievement 2: Spend Coins
    public static readonly int[] SpendTargets = { 300, 2000, 10000, 25000, 120000 };
    // Level 5 (index 4) reward: exclusive sword added to inventory

    // PlayerPrefs keys
    private const string TotalDamageKey = "Achiev_TotalDamage";
    private const string TotalSpentKey = "Achiev_TotalSpent";
    private const string DamageLevelKey = "Achiev_DamageLevel";
    private const string SpendLevelKey = "Achiev_SpendLevel";

    private float totalDamageDealt;
    private int totalCoinsSpent;
    private int damageAchievLevel; // number of levels already claimed (0-5)
    private int spendAchievLevel;  // number of levels already claimed (0-5)

    private void Awake()
    {
        LoadData();
    }

    private void LoadData()
    {
        totalDamageDealt = PlayerPrefs.GetFloat(TotalDamageKey, 0f);
        totalCoinsSpent = PlayerPrefs.GetInt(TotalSpentKey, 0);
        damageAchievLevel = PlayerPrefs.GetInt(DamageLevelKey, 0);
        spendAchievLevel = PlayerPrefs.GetInt(SpendLevelKey, 0);
    }

    public void SaveData()
    {
        PlayerPrefs.SetFloat(TotalDamageKey, totalDamageDealt);
        PlayerPrefs.SetInt(TotalSpentKey, totalCoinsSpent);
        PlayerPrefs.SetInt(DamageLevelKey, damageAchievLevel);
        PlayerPrefs.SetInt(SpendLevelKey, spendAchievLevel);
        PlayerPrefs.Save();
    }

    public static void ResetAchievements()
    {
        PlayerPrefs.DeleteKey(TotalDamageKey);
        PlayerPrefs.DeleteKey(TotalSpentKey);
        PlayerPrefs.DeleteKey(DamageLevelKey);
        PlayerPrefs.DeleteKey(SpendLevelKey);
        PlayerPrefs.DeleteKey("HasExclusiveSword");
        PlayerPrefs.Save();
        Debug.Log("🏆 Достижения сброшены!");
    }

    // ─── Damage achievement ───────────────────────────────────────────────────

    public void AddDamage(float damage)
    {
        if (damage <= 0f) return;
        totalDamageDealt += damage;
        // Update in-memory PlayerPrefs without flushing to disk on every hit
        PlayerPrefs.SetFloat(TotalDamageKey, totalDamageDealt);
    }

    public float GetTotalDamage() => totalDamageDealt;
    public int GetDamageLevel() => damageAchievLevel;

    public int GetDamageTarget(int level)
    {
        return (level >= 0 && level < DamageTargets.Length) ? DamageTargets[level] : 0;
    }

    public int GetDamageReward(int level)
    {
        return (level >= 0 && level < DamageRewards.Length) ? DamageRewards[level] : 0;
    }

    public bool IsDamageClaimable()
    {
        if (damageAchievLevel >= DamageTargets.Length) return false;
        return totalDamageDealt >= DamageTargets[damageAchievLevel];
    }

    public void ClaimDamageReward()
    {
        if (!IsDamageClaimable()) return;
        int reward = DamageRewards[damageAchievLevel];
        int coins = PlayerPrefs.GetInt("PlayerCoins", 0);
        PlayerPrefs.SetInt("PlayerCoins", coins + reward);
        damageAchievLevel++;
        SaveData();
        Debug.Log("🏆 Получена награда за урон: +" + reward + " монет");
    }

    // ─── Spend-coins achievement ──────────────────────────────────────────────

    public void AddSpentCoins(int amount)
    {
        if (amount <= 0) return;
        totalCoinsSpent += amount;
        // Update in-memory PlayerPrefs without flushing to disk on every purchase
        PlayerPrefs.SetInt(TotalSpentKey, totalCoinsSpent);
    }

    public int GetTotalSpent() => totalCoinsSpent;
    public int GetSpendLevel() => spendAchievLevel;

    public int GetSpendTarget(int level)
    {
        return (level >= 0 && level < SpendTargets.Length) ? SpendTargets[level] : 0;
    }

    public bool IsSpendClaimable()
    {
        if (spendAchievLevel >= SpendTargets.Length) return false;
        return totalCoinsSpent >= SpendTargets[spendAchievLevel];
    }

    public void ClaimSpendReward()
    {
        if (!IsSpendClaimable()) return;
        if (spendAchievLevel == 4)
        {
            // Level 5: exclusive sword
            PlayerPrefs.SetInt("HasExclusiveSword", 1);
            PlayerPrefs.Save();
            Debug.Log("🏆 Получен эксклюзивный меч!");
        }
        spendAchievLevel++;
        SaveData();
        Debug.Log("🏆 Получена награда за траты (уровень " + spendAchievLevel + ")");
    }
}
