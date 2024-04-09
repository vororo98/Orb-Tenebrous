using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TypePart : RulePart
{
    [SerializeField] private agentType type;
    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        text.text = type.ToString();
    }

    public override agentType GetType()
    {
        base.GetType();
        return type;
    }

    public override agentType SetType(agentType newType)
    {
        base.SetType(newType);
        return type = newType;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);

        TypePart pointer = eventData.pointerDrag.GetComponent<TypePart>();
        if (pointer != null && pointer.checkDragable())
        {
            type = pointer.GetType();
            text.text = type.ToString();
            Destroy(pointer);
        }
    }
}
