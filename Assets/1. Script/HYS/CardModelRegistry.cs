
using System.Collections.Generic;
using UnityEngine;

public class CardModelRegistry : MonoBehaviour
{
    [Header("Registered Card Assets")]
    [SerializeField]
    private List<CardModelData> cardDataList
        = new List<CardModelData>();

    private readonly Dictionary<string, CardModelData> registry
        = new Dictionary<string, CardModelData>();

    private bool initialized;

    private void Awake()
    {
        BuildRegistry();
    }

    public void BuildRegistry()
    {
        registry.Clear();

        foreach (CardModelData data in cardDataList)
        {
            if (data == null || !data.IsValid())
                continue;

            if (registry.ContainsKey(data.cardId))
            {
                Debug.LogWarning(
                    $"Duplicate card ID: {data.cardId}");
                continue;
            }

            registry.Add(data.cardId, data);
        }

        initialized = true;

        Debug.Log($"Registered cards: {registry.Count}");
    }

    public CardModelData GetCardData(string cardId)
    {
        if (!initialized)
            BuildRegistry();

        if (string.IsNullOrWhiteSpace(cardId))
            return null;

        registry.TryGetValue(cardId, out CardModelData data);

        return data;
    }

    public GameObject GetModelPrefab(string cardId)
    {
        CardModelData data = GetCardData(cardId);

        return data != null ? data.modelPrefab : null;
    }

    public bool ContainsCard(string cardId)
    {
        return GetCardData(cardId) != null;
    }

    public int GetRegisteredCardCount()
    {
        if (!initialized)
            BuildRegistry();

        return registry.Count;
    }

    public List<string> GetRegisteredCardIds()
    {
        if (!initialized)
            BuildRegistry();

        return new List<string>(registry.Keys);
    }
}
