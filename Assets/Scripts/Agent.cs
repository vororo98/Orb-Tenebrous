using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum agentType
{
    Null,
    Player,
    Skeleton,
    Slime,
    SlimeTrail,
    Wall,
    Fire,
    Chest,
    Loot,
    Key,
    Door,
    Exit,
    Sword,
    Bestower1,
    Bestower2
}

public class Agent : MonoBehaviour
{
    private Vector2 pos;
    private Vector2 facingDir = new Vector2(0,-1);
    private SpriteRenderer renderer;
    private List<action> flags = new();
    //private List<agentType> containedTypes = new();
    [SerializeField] private agentType type;

    void Start()
    {
        
    }

    public void setPos(Vector2 newPos)
    {
        pos = newPos;
    }

    public Vector2 getPos()
    {
        return pos;
    }

    public void setFacingDir(Vector2 dir)
    {
        facingDir = dir;
    }

    public Vector2 getFacingDir()
    {
        return facingDir;
    }

    public void setType(agentType newType)
    {
        renderer = GetComponent<SpriteRenderer>();
        type = newType;

        switch (newType)
        {
            case agentType.Player:
                renderer.color = Color.yellow;
                break;
            case agentType.Skeleton:
                renderer.color = Color.white;
                break;
            case agentType.Slime:
                renderer.color = Color.green;
                renderer.sortingOrder = 1;
                break;
            case agentType.SlimeTrail:
                renderer.color = Color.blue;
                renderer.sortingOrder = 0;
                break;
            case agentType.Wall:
                renderer.color = Color.grey;
                break;
            case agentType.Fire:
                renderer.color = Color.red;
                break;
            case agentType.Chest:
                renderer.color = Color.cyan;
                break;
            case agentType.Loot:
                renderer.color = Color.magenta;
                break;
            case agentType.Key:
                renderer.color = Color.yellow;
                break;
            case agentType.Door:
                renderer.color = new Color (230f, 0f, 50f, 255f);
                break;
            case agentType.Exit:
                renderer.color = Color.white;
                break;
            case agentType.Bestower1:
                renderer.color = Color.white; //new Color(128f, 117f, 60f, 0f);
                break;
            case agentType.Bestower2:
                renderer.color = Color.white; //new Color(128f, 117f, 60f, 0f);
                break;
            case agentType.Sword:
                renderer.color = new Color(158f, 200f, 200f, 0f);
                break;
            default:
                break;
        }
    }

    public void addFlag(action newFlag)
    {
        flags.Add(newFlag);
    }

    public List<action> getFlags()
    {
        return flags;
    }

    public void resetFlags()
    {
        flags = new();
    }

    //public void addContained(agentType newContained)
    //{
    //    containedTypes.Add(newContained);
    //}

    //public List<agentType> getContainedTypes()
    //{
    //    return containedTypes;
    //}

    //public void resetContainedTypes()
    //{
    //    containedTypes = new();
    //}

    public agentType getType()
    {
        return type;
    }
}
