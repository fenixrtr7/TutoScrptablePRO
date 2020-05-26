using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "NewStats", menuName = "Character/Stats", order = 1)]
public class CharacterStats_SO : ScriptableObject
{
    [System.Serializable]
    public class CharLevelUps
    {
        public int maHealth;
        public int maxMana;
        public int maxWealth;
        public int baseDamage;
        public int baseResistance;
        public float maxEncumberance;
    }
    #region Fields

    public bool SetManually = false;
    public bool saveDataOnClose = false;

    public ItemPickUp weapon { get; private set; }
    public ItemPickUp headArmor { get; private set; }
    public ItemPickUp chestArmor { get; private set; }
    public ItemPickUp handArmor { get; private set; }
    public ItemPickUp legArmor { get; private set; }
    public ItemPickUp footArmor { get; private set; }
    public ItemPickUp misc1 { get; private set; }
    public ItemPickUp misc2 { get; private set; }
    public int maxHealth = 0;
    public int currentHealth = 0;

    public int maxWealth = 0;
    public int currentWealth = 0;

    public int maxMana = 0;
    public int currentMana = 0;


    public int baseDamage = 0;
    public int currentDamage = 0;

    public float baseResistance = 0f;
    public float currentResistance = 0f;

    public float currentEncumbrance = 0f;
	public float maxEncumbrance;

    public int charExperience = 0;
    public int charLevel = 0;
    public CharLevelUps[] charLevelUps;

    #endregion

    #region Stat Increasers

    public void ApplyHealth(int healthAmount)
    {
        if ((currentHealth + healthAmount) > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += healthAmount;
        }
    }

    public void ApplyMana(int manaAmount)
    {
        if ((currentMana + manaAmount) > maxMana)
        {
            currentMana = maxMana;
        }
        else
        {
            currentMana += manaAmount;
        }
    }

    public void GiveWealth(int wealthAmount)
    {
        if ((currentWealth + wealthAmount) > maxWealth)
        {
            currentWealth = maxWealth;
        }
        else
        {
            currentWealth += wealthAmount;
        }
    }

    public void EquipWeapon(ItemPickUp weaponPickUp, CharacterInventory characterInvenyory, GameObject weaponSlot)
    {
        weapon = weaponPickUp;
        currentDamage = baseDamage + weapon.itemDefinition.itemAmount;
    }

    public void EquipArmor(ItemPickUp armorPickup, CharacterInventory characterInventory)
    {
        switch (armorPickup.itemDefinition.itemArmorSubType)
        {
            case ItemArmorSubType.Head:
                headArmor = armorPickup;
                currentResistance += armorPickup.itemDefinition.itemAmount;
                break;

            case ItemArmorSubType.Cheast:
                chestArmor = armorPickup;
                currentResistance += armorPickup.itemDefinition.itemAmount;
                break;

            case ItemArmorSubType.Hands:
                handArmor = armorPickup;
                currentResistance += armorPickup.itemDefinition.itemAmount;
                break;

            case ItemArmorSubType.Legs:
                legArmor = armorPickup;
                currentResistance += armorPickup.itemDefinition.itemAmount;
                break;

            case ItemArmorSubType.Boots:
                footArmor = armorPickup;
                currentResistance += armorPickup.itemDefinition.itemAmount;
                break;
        }
    }

    #endregion

    #region Stat Reducers
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            // Death();
        }
    }

    public void TakeMana(int amount)
    {
        currentMana -= amount;
        if (currentMana < 0)
        {
            currentMana = 0;
        }
    }

    public bool UnEquipWeapon(ItemPickUp weaponPickUp, CharacterInventory characterInventory, GameObject weaponSlot)
    {
        bool previusWeaponSame = false;

        if (weapon != null)
        {
            if (weapon == weaponPickUp)
            {
                previusWeaponSame = true;
            }

            DestroyObject(weaponSlot.transform.GetChild(0).gameObject);
            weapon = null;
            currentDamage = baseDamage;
        }

        return previusWeaponSame;
    }

    public bool UnEquipArmor(ItemPickUp armorPickUp, CharacterInventory characterInventory)
    {
        bool previusWeaponSame = false;

        switch (armorPickUp.itemDefinition.itemArmorSubType)
        {
            case ItemArmorSubType.Head:
                if (headArmor != null)
                {
                    if (headArmor == armorPickUp)
                    {
                        previusWeaponSame = true;
                    }
                    currentResistance -= armorPickUp.itemDefinition.itemAmount;
                    headArmor = null;
                }
                break;

            case ItemArmorSubType.Cheast:
                if (chestArmor != null)
                {
                    if (chestArmor == armorPickUp)
                    {
                        previusWeaponSame = true;
                    }
                    currentResistance -= armorPickUp.itemDefinition.itemAmount;
                    chestArmor = null;
                }
                break;

			case ItemArmorSubType.Hands:
                if (handArmor != null)
                {
                    if (handArmor == armorPickUp)
                    {
                        previusWeaponSame = true;
                    }
                    currentResistance -= armorPickUp.itemDefinition.itemAmount;
                    handArmor = null;
                }
                break;

			case ItemArmorSubType.Legs:
                if (legArmor != null)
                {
                    if (legArmor == armorPickUp)
                    {
                        previusWeaponSame = true;
                    }
                    currentResistance -= armorPickUp.itemDefinition.itemAmount;
                    legArmor = null;
                }
                break;

			case ItemArmorSubType.Boots:
                if (footArmor != null)
                {
                    if (footArmor == armorPickUp)
                    {
                        previusWeaponSame = true;
                    }
                    currentResistance -= armorPickUp.itemDefinition.itemAmount;
                    footArmor = null;
                }
                break;
        }
		return previusWeaponSame;
    }

    #endregion

    #region Character Level Up and Death
    private void Death()
    {
        Debug.Log("You're Dead");
    }

	private void LevelUp()
	{
		charLevel += 1;

		maxHealth = charLevelUps[charLevel -1].maHealth;
		maxMana = charLevelUps[charLevel -1].maxMana;
		maxWealth = charLevelUps[charLevel -1].maxWealth;
		baseDamage = charLevelUps[charLevel -1].baseDamage;
		baseResistance = charLevelUps[charLevel -1].baseResistance;
		maxEncumbrance = charLevelUps[charLevel -1].maxEncumberance;
	}
    #endregion

    #region SaveCharacterData

    public void SaveCharacterData()
    {
        saveDataOnClose = true;
        EditorUtility.SetDirty(this);
    }

    #endregion
}