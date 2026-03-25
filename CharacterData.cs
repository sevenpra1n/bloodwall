using UnityEngine;

public enum CharacterType { Knight, Archer }

[System.Serializable]
public class CharacterData
{
    public CharacterType type;
    public string characterName;
    public string description;
    public Sprite icon;
    public int price;       // 0 for Knight, 5000 for Archer
    public bool isUnlocked; // Knight always true; Archer requires purchase
    public float hpMultiplier;  // Knight: 1.0f, Archer: 0.8f
    public float dodgeChance;   // Knight: 0.1f (10%), Archer: 0.2f (20%)
    public Sprite[] idleSprites;
    public Sprite[] attackSprites;
}
