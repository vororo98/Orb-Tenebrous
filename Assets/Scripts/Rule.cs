using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum action //Should this be overloaded classes instead? Functions that do stuff with obj
{
    Null,
    Controllable,
    Move,
    Block,
    Seek,
    Flammable,
    Hot,
    Spreads,
    Contains,
    Locked,
    Unlocks,
    Pickup,
    Exit,
    Slashes,
    Slashable,
    Bestows, //TODO: bestows the following action or agentType as a word 
    Uses //TODO: An agent that uses another agent, like how use item in inventory works
}

public class Rule : MonoBehaviour, IDropHandler
{
    private agentType sub = agentType.Null; //The subject of a rule, who is doing
    private action verb = action.Null; //The verb of a rule, what is being done
    private agentType obj = agentType.Null; //The object of a rule, what is being done to
    [SerializeField] private RulePart subUI, verbUI, objUI;

    public void Start()
    {
        subUI.SetDragable(false);
        verbUI.SetDragable(false);
        objUI.SetDragable(false);
    }

    public Rule(agentType sub, action verb)
    {
        this.sub = sub;
        this.verb = verb;
    }

    public Rule(agentType sub, action verb, agentType obj)
    {
        this.sub = sub;
        this.verb = verb;
        this.obj = obj;
    }

    //TODO: make it one function
    //public ArrayList[] getRule()
    //{
    //    return new ArrayList[] {type, action};
    //}

    public agentType GetSub()
    {
        return subUI.GetType();
    }

    public void SetSub(agentType newType)
    {
        subUI.SetType(newType);
    }

    public action GetAction()
    {
        return verbUI.GetAction();
    }

    public void SetAction(action newAction)
    {
        verbUI.SetAction(newAction);
    }

    public agentType GetObj()
    {
        return objUI.GetType();
    }

    public void SetObj(agentType newType)
    {
        
        objUI.SetType(newType);
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("Dropped!");
    }
}
