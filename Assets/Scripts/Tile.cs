using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject highlight;
    

    private List<Agent> Agents = new();

    public void Init(bool isOffset)
    {
        //spriteRenderer.color = isOffset ? offsetColor : baseColor;
        spriteRenderer.color = baseColor;
    }

    public List<Agent> GetLocalAgents()
    {
        return Agents;
    }

    public void AddAgent(Agent agent)
    {
        Agents.Add(agent);
    }

    public void RemoveAgent(Agent agent)
    {
        Agents.Remove(agent);
    }

    void OnMouseEnter()
    {
        highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        highlight.SetActive(false);
    }
}
