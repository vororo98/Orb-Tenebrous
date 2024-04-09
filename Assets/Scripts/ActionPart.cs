using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionPart : RulePart
{
    [SerializeField] private action action;
    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        text.text = action.ToString();
    }

    public override action GetAction()
    {
        base.GetAction();
        return action;
    }

    public override action SetAction(action newAction)
    {
        base.SetAction(newAction);
        return action = newAction;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);

        ActionPart pointer = eventData.pointerDrag.GetComponent<ActionPart>();
        if (pointer != null && pointer.checkDragable())
        {
            action = pointer.GetAction();
            text.text = action.ToString();
            Destroy(pointer);
        }
    }
}
