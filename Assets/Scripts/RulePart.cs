using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RulePart : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private RectTransform rectTransform;
    private bool dragable = true;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
    }

    public new virtual agentType GetType()
    {
        return agentType.Null;
    }

    public new virtual agentType SetType(agentType newType)
    {
        return agentType.Null;
    }

    public virtual action GetAction()
    {
        return action.Null;
    }

    public virtual action SetAction(action newAction)
    {
        return action.Null;
    }

    public void SetDragable(bool val)
    {
        dragable = val;
    }

    public bool checkDragable()
    {
        return dragable;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(dragable) rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Unexpected!!!");
    }
}
