
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewCardModel",
    menuName = "Imagineer/Card Model Data")]
public class CardModelData : ScriptableObject
{
    [Header("Card Information")]
    public string cardId;
    public string cardName;

    [TextArea(2, 5)]
    public string description;

    [Header("Model Settings")]
    public GameObject modelPrefab;
    public Sprite cardImage;

    public Vector3 positionOffset = Vector3.zero;
    public Vector3 rotationOffset = Vector3.zero;

    [Min(0.01f)]
    public float modelScale = 1f;

    [Header("Battle Attributes")]
    [Min(1)]
    public int maxHealth = 100;

    [Min(0)]
    public int attackPower = 20;

    [Min(0)]
    public int defensePower = 5;

    [Header("Card Type")]
    public CardType cardType;

    public enum CardType
    {
        Warrior,
        Mage,
        Dragon,
        Support,
        Guardian
    }

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(cardId)
            && modelPrefab != null
            && maxHealth > 0
            && modelScale > 0;
    }
}
