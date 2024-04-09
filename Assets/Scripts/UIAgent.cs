using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAgent : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private agentType type = agentType.Null;
    private Image image;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector3 origin;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
    }

    public void setType(agentType newType)
    {
        type = newType;
        image = GetComponent<Image>();

        switch (newType)
        {
            case agentType.Player:
                image.color = Color.yellow;
                break;
            case agentType.Skeleton:
                image.color = Color.white;
                break;
            case agentType.Slime:
                image.color = Color.green;
                break;
            case agentType.SlimeTrail:
                image.color = Color.blue;
                break;
            case agentType.Wall:
                image.color = Color.grey;
                break;
            case agentType.Fire:
                image.color = Color.red;
                break;
            case agentType.Chest:
                image.color = Color.cyan;
                break;
            case agentType.Loot:
                image.color = Color.magenta;
                break;
            case agentType.Key:
                image.color = Color.yellow;
                break;
            case agentType.Door:
                image.color = new Color(230f, 0f, 50f, 255f);
                break;
            default:
                break;
        }
    }

    public agentType getType()
    {
        return type;
    }

    public void setOrigin(Vector3 position)
    {
        origin = position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        origin = rectTransform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        rectTransform.position = origin;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        ActionSlot slot = transform.parent.GetComponent<ActionSlot>();
        if (slot != null)
        {
            slot.OnDrop(eventData);
        }
    }
}
