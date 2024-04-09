using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionSlot : MonoBehaviour, IDropHandler
{
    UIAgent slot;
    RectTransform rectTransform;
    [SerializeField] InventoryManager InventoryManager;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    public agentType getEquippedType()
    {
        Debug.Log(slot.getType());
        return slot.getType();
    }

    public void DeleteEquipped()
    {
        if(slot != null) Destroy(slot.gameObject);
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        UIAgent pointer = eventData.pointerDrag.GetComponent<UIAgent>();
        if (pointer != null)
        {
            Debug.Log("got new equip");
            if (slot != null) Destroy(slot.gameObject);
            
            pointer.setOrigin(rectTransform.position);
            slot = pointer;
            InventoryManager.DeleteItem(slot.gameObject);
            pointer.gameObject.transform.SetParent(transform);
            //slot.GetComponent<RectTransform>().position = rectTransform.position;
        }
    }
}
