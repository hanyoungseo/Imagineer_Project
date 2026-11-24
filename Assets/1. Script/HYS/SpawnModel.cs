
using System.Collections.Generic;
using UnityEngine;

public class SpawnModel : MonoBehaviour
{
    [Header("Model Configuration")]
    [SerializeField] private CardModelRegistry registry;
    [SerializeField] private Transform modelParent;

    private readonly Dictionary<string, GameObject> activeModels
        = new Dictionary<string, GameObject>();

    public void SpawnCardModel(Vector3 pos, string id)
    {
        SpawnCardModel(pos, Quaternion.identity, id);
    }

    public void SpawnCardModel(
        Vector3 pos, Quaternion rotation, string id)
    {
        if (string.IsNullOrWhiteSpace(id) || registry == null)
            return;

        CardModelData data = registry.GetCardData(id);

        if (data == null || data.modelPrefab == null)
        {
            Debug.LogWarning($"Unknown card ID: {id}");
            return;
        }

        Vector3 spawnPosition = pos + rotation * data.positionOffset;

        Quaternion spawnRotation =
            rotation * Quaternion.Euler(data.rotationOffset);

        if (activeModels.TryGetValue(id, out GameObject existing)
            && existing != null)
        {
            CardModelController controller =
                existing.GetComponent<CardModelController>();

            if (controller != null)
                controller.SetTargetPose(spawnPosition, spawnRotation);
            else
                existing.transform.SetPositionAndRotation(
                    spawnPosition, spawnRotation);

            return;
        }

        GameObject model = Instantiate(
            data.modelPrefab,
            spawnPosition,
            spawnRotation,
            modelParent
        );

        model.name = $"CardModel_{id}";
        model.transform.localScale *= data.modelScale;

        activeModels[id] = model;

        CardBattleStatus status =
            model.GetComponent<CardBattleStatus>();

        if (status != null)
            status.Initialize(data.maxHealth, data.attackPower);

        CardSpawnEffect effect =
            model.GetComponent<CardSpawnEffect>();

        if (effect != null)
            effect.PlaySpawnEffect();

        CardGameEventManager.RaiseModelSpawned(id, model);

        Debug.Log($"Model spawned: {id}");
    }

    public void RemoveCardModel(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return;

        if (!activeModels.TryGetValue(id, out GameObject model))
            return;

        activeModels.Remove(id);

        if (model != null)
            Destroy(model);

        CardGameEventManager.RaiseModelRemoved(id);
    }

    public bool IsModelSpawned(string id)
    {
        return !string.IsNullOrWhiteSpace(id)
            && activeModels.TryGetValue(id, out GameObject model)
            && model != null;
    }

    public void ClearAllModels()
    {
        List<string> ids = new List<string>(activeModels.Keys);

        foreach (string id in ids)
            RemoveCardModel(id);
    }
}
