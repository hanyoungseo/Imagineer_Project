
using UnityEngine;
using UnityEngine.Events;

public class CardInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private bool interactionEnabled = true;
    [SerializeField] private GameObject selectionIndicator;

    [Header("Interaction Events")]
    [SerializeField] private UnityEvent onSelected;
    [SerializeField] private UnityEvent onDeselected;
    [SerializeField] private UnityEvent onHoverEnter;
    [SerializeField] private UnityEvent onHoverExit;

    private bool isSelected;
    private bool isHovered;

    private void Awake()
    {
        UpdateIndicator();
    }

    public void SelectCard()
    {
        if (!interactionEnabled || isSelected)
            return;

        isSelected = true;

        UpdateIndicator();

        onSelected?.Invoke();

        CardGameEventManager.RaiseCardSelected(gameObject);
    }

    public void DeselectCard()
    {
        if (!isSelected)
            return;

        isSelected = false;

        UpdateIndicator();

        onDeselected?.Invoke();
    }

    public void ToggleSelection()
    {
        if (isSelected)
            DeselectCard();
        else
            SelectCard();
    }

    public void OnHoverEnter()
    {
        if (!interactionEnabled || isHovered)
            return;

        isHovered = true;

        UpdateIndicator();
        onHoverEnter?.Invoke();
    }

    public void OnHoverExit()
    {
        if (!isHovered)
            return;

        isHovered = false;

        UpdateIndicator();
        onHoverExit?.Invoke();
    }

    private void UpdateIndicator()
    {
        if (selectionIndicator != null)
        {
            selectionIndicator.SetActive(
                isSelected || isHovered);
        }
    }

    public void SetInteractionEnabled(bool state)
    {
        interactionEnabled = state;

        if (!state)
        {
            DeselectCard();
            OnHoverExit();
        }
    }

    public bool IsSelected()
    {
        return isSelected;
    }
}
