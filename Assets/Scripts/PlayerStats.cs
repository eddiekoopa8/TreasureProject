

using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerStats : MonoBehaviour
{
    [Title("Basic Info")]
    [InfoBox("This section contains the player's general information.")]
    public string playerName = "Hero";

    [Range(1, 100)]
    public int level = 1;

    [ProgressBar(0, 100, ColorMember = "GetHealthBarColor")]
    public float health = 75f;

    [Title("Inventory")]
    [TableList]
    public Item[] inventory;

    [Button(ButtonSizes.Large)]
    public void Heal()
    {
        health = Mathf.Min(100, health + 10);
        Debug.Log($"{playerName} healed. Current health: {health}");
    }

    private Color GetHealthBarColor(float value)
    {
        return Color.Lerp(Color.red, Color.green, value / 100f);
    }
}

[System.Serializable]
public class Item
{
    [HorizontalGroup("Split", 0.5f)]
    [PreviewField(Alignment = ObjectFieldAlignment.Left)]
    public Sprite icon;

    [VerticalGroup("Split/Right")]
    public string itemName;

    [VerticalGroup("Split/Right")]
    [Range(1, 99)]
    public int quantity = 1;
}

