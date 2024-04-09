using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] GameObject inventoryContainer;
    [SerializeField] GameObject itemContainerPrefab;
    [SerializeField] GameObject UITypePartPrefab, UIActionPartPrefab,UIAgentPrefab;
    private List<GameObject> items = new();
    private List<GameObject> containers = new();

    // Start is called before the first frame update
    void Start()
    {
        //AddNewUIActionPart(action.Contains);
        //AddNewUIActionPart(action.Controllable);
        //AddNewUIActionPart(action.Spreads);
        //AddNewUIActionPart(action.Unlocks);
        //AddNewUITypePart(agentType.Key);
        //AddNewUITypePart(agentType.Wall);
        //AddNewUIAgent(agentType.Key);
    }

    private void RefreshDisplay()
    {
        foreach (GameObject container in containers.ToList())
        {
            Destroy(container);
        }

        int x = -2;
        int y = 2;
        float itemSlotCellSize = 150f;
        foreach (GameObject item in items)
        {
            GameObject obj = Instantiate(itemContainerPrefab, inventoryContainer.transform);
            containers.Add(obj);
            RectTransform itemSlotRectTransform = obj.GetComponent<RectTransform>();
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            item.GetComponent<RectTransform>().anchoredPosition = itemSlotRectTransform.anchoredPosition;
            item.transform.SetAsLastSibling(); //WHY UNITY WHY IS UI SORTING BY HIERARCHY WHY
            x++;
            if (x > 2)
            {
                x = -2;
                y--;
            }
        }
    }

    public void AddNewUIAgent(agentType type)
    {
        GameObject item = Instantiate(UIAgentPrefab, inventoryContainer.transform);
        item.GetComponent<UIAgent>().setType(type);
        items.Add(item);
        RefreshDisplay();
    }
    public void AddNewUITypePart(agentType type)
    {
        GameObject item = Instantiate(UITypePartPrefab, inventoryContainer.transform);
        item.GetComponent<TypePart>().SetType(type);
        items.Add(item);
        RefreshDisplay();
    }

    public void AddNewUIActionPart(action action)
    {
        GameObject item = Instantiate(UIActionPartPrefab, inventoryContainer.transform);
        item.GetComponent<ActionPart>().SetAction(action);
        items.Add(item);
        RefreshDisplay();
    }

    public void DeleteItem(GameObject item)
    {
        items.Remove(item);
        RefreshDisplay();
    }

    public void DeleteItems()
    {
        foreach (GameObject obj in items)
        {
            Destroy(obj);
        }
        items = new();
    }
}
