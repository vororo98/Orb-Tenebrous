using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;



public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab, skeletonPrefab, winText, UIAgent;
    [SerializeField] TileManager tileManager;
    [SerializeField] OrbManager orbManager;
    [SerializeField] InventoryManager inventoryManager;
    [SerializeField] ActionBarManager actionBarManager;
    List<Agent> agents = new();
    List<Rule> ruleSet = new();
    List<DeleteNextFrame> deleteList = new();//I shold name this something else, consider the structure of the cleanup callbacks

    Queue<int[,]> levels = new();

    //0 = empty, 1 = wall, 2 = player, 3 = Slime, 4 = Skeleton, 5 = Fire, 6 = chest, 7 = loot, 8 = door, 9 = exit, 10 = key, 11 = bestower, 12 = bestower 2, 13 = Sword
    private int[,] map1 = { 
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 6, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 1, 1, 1, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 2, 1, 0, 0, 3, 0, 0, 1 },
        { 1, 0, 0, 1, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 1, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 8, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 1, 0, 0, 0, 0, 0, 1 },
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
    };

    private int[,] tutorialMap1 = {
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        { 1, 0, 0, 1, 9, 1, 0, 0, 0, 1 },
        { 1, 0, 0, 1, 8, 1, 0, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 2, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 1, 1, 1, 1, 1, 1, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0,10, 1 },
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
    };

    private int[,] tutorialMap2 = {
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        { 1, 2, 0, 0, 1, 0, 0, 1, 1, 1 },
        { 1, 0, 0, 0, 1, 0, 0, 8, 9, 1 },
        { 1, 0, 0, 0, 1, 0, 0, 1, 1, 1 },
        { 1, 0, 0, 0, 1, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 1, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 1, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 1, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 1, 0, 0, 1 },
        { 1, 5, 0, 0, 0, 3, 1, 0,12, 1 },
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
    };

    private int[,] tutorialMap3 = {
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        { 1, 0, 0, 9, 0, 0, 1, 1, 1, 1 },
        { 1, 1, 1, 0, 1, 1, 1, 1, 1, 1 },
        { 1, 1, 1, 8, 1, 1, 1, 1, 1, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 2, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 0,13, 0, 0, 3, 0, 0, 0, 1 },
        { 1, 1, 1, 1, 1, 0, 1, 1, 0, 1 },
        { 1, 0, 0, 0, 1, 0, 1,11, 0, 1 },
        { 1,12, 0, 0, 0, 0, 1, 0, 0, 1 },
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
    };

    // Start is called before the first frame update
    void Start()
    {
        levels.Enqueue(tutorialMap2);
        levels.Enqueue(tutorialMap3);
        GenerateMap(tutorialMap1);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown) parseRules();

        if (Input.GetKeyDown("1"))
        {
            CreateAgent(agentType.Player, new Vector2(0, 0));
        }
        else if (Input.GetKeyDown("2"))
        {
            CreateAgent(agentType.Skeleton, new Vector2(0,0));
        }
        else if (Input.GetKeyDown("3"))
        {
            CreateAgent(agentType.Slime, new Vector2(0, 0));
        }
        else if (Input.GetKeyDown("4"))
        {
            CreateAgent(agentType.Fire, new Vector2(0, 0));
        }
        else if (Input.GetKeyDown("9"))
        {
            GenerateMap(tutorialMap2);
        }
        else if (Input.GetKeyDown("0"))
        {
            GenerateMap(tutorialMap3);
        }
    }

    public List<Rule> GetRules()
    {
        return ruleSet;
    }

    private void GenerateMap(int[,] map) //TODO: Consider changing it to char instead of numbers for ease of reading the map (with eyes, not with code)
    {
        DeleteAgents();
        int length = map.GetLength(0);
        int width = map.GetLength(1);
        tileManager.GenerateGridBySize(width, length);

        for(int y = 0; y < length; y++)
        {
            for(int x = 0; x < width; x++)
            {
                switch (map[length - y - 1,x]) //Reverse the order of the map
                {
                    case 1:
                        CreateAgent(agentType.Wall, new Vector2(x, y));
                        break;
                    case 2:
                        CreateAgent(agentType.Player, new Vector2(x, y));
                        break;
                    case 3:
                        CreateAgent(agentType.Slime, new Vector2(x, y));
                        break;
                    case 4:
                        CreateAgent(agentType.Skeleton, new Vector2(x, y));
                        break;
                    case 5:
                        CreateAgent(agentType.Fire, new Vector2(x, y));
                        break;
                    case 6:
                        CreateAgent(agentType.Chest, new Vector2(x, y));
                        break;
                    case 7:
                        CreateAgent(agentType.Loot, new Vector2(x, y));
                        break;
                    case 8:
                        CreateAgent(agentType.Door, new Vector2(x, y));
                        break;
                    case 9:
                        CreateAgent(agentType.Exit, new Vector2(x, y));
                        break;
                    case 10:
                        CreateAgent(agentType.Key, new Vector2(x, y));
                        break;
                    case 11:
                        CreateAgent(agentType.Bestower1, new Vector2(x, y));
                        break;
                    case 12:
                        CreateAgent(agentType.Bestower2, new Vector2(x, y));
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private Agent CreateAgent(agentType type, Vector2 pos)
    {
        GameObject newAgent = Instantiate(skeletonPrefab);
        Agent newAgentScript = newAgent.GetComponent<Agent>();
        Tile tile = tileManager.GetTileAtPosition(pos);
        agents.Add(newAgentScript);
        newAgentScript.setPos(pos);
        newAgentScript.setType(type);
        newAgent.transform.position = tile.transform.position;
        tile.AddAgent(newAgentScript);
        return newAgentScript;
    }

    private void DeleteAgent(Agent agent)
    {
        if (agent == null) return;
        if (agent.getFlags().Contains(action.Contains))
        {
            foreach (Rule rule in ruleSet) //TODO: this loop is unnecessary , agent should know what it contains
            {
                if (rule.GetSub() == agent.getType() && rule.GetAction() == action.Contains)
                {
                    CreateAgent(rule.GetObj(), agent.getPos());
                }
            }
        }

        agents.Remove(agent);
        tileManager.GetTileAtPosition(agent.getPos()).RemoveAgent(agent);
        Destroy(agent.gameObject);
    }

    private void DeleteAgents()
    {
        foreach (Agent agent in agents.ToList())
        {
            Destroy(agent.gameObject);
        }
        agents = new();
    }

    private void moveToTile(Agent agent, Vector2 direction)
    {
        Vector2 v2 = agent.getPos() + direction;
        Tile current = tileManager.GetTileAtPosition(agent.getPos());
        Tile target = tileManager.GetTileAtPosition(v2);
        agent.setFacingDir(direction);
        if(target != null && !checkForBlock(v2))
        {
            agent.setPos(v2);
            agent.gameObject.transform.position = target.transform.position;
            target.AddAgent(agent);
            current.RemoveAgent(agent);
        }
    }

    private bool checkForBlock(Vector2 position)
    {
        //foreach (Agent agent in agents) // Figure out how to store block status in Tile to remove this loop
        //{
        //    if (agent.getPos() == position)
        //    {
        //        foreach(Rule rule in ruleSet)
        //        {
        //            if(rule.GetSub() == agent.getType())
        //            {
        //                if (rule.GetAction() == action.Block) return true;
        //            }
        //        }
        //    }
        //}
        Tile target = tileManager.GetTileAtPosition(position);
        List<Agent> localAgents = target.GetLocalAgents();

        foreach (Agent agent in localAgents) // Better than prior, but if each Agent knows if it is blocking for this frame it would be better
        {
            foreach (Rule rule in ruleSet)
            {
                if (rule.GetSub() == agent.getType())
                {
                    if (rule.GetAction() == action.Block) return true;
                }
            }
        }

        return false;
    }

    private bool CheckForAdjacentType(agentType type, Vector2 position)
    {
        foreach (Agent agent in agents) // Figure out how to store agents in Tile to remove this loop
        {
            if(agent.getType() == type)
            {
                if (agent.getPos() == position || agent.getPos() == position + new Vector2(0, 1) || agent.getPos() == position + new Vector2(0, -1) || agent.getPos() == position + new Vector2(1, 0) || agent.getPos() == position + new Vector2(-1, 0))
                {
                    return true;
            }
            }
        }
        return false;
    }

    private void UnlockBehaviour(Agent Unlocker) //find all agents on this tile, remove them if they are locked, //TODO: make generic check for when things are on top of each other
    {
        agentType type = Unlocker.getType();
        Tile tile = tileManager.GetTileAtPosition(Unlocker.getPos());
        List<Agent> localAgents = tile.GetLocalAgents();

        foreach (Agent agent in localAgents.ToList())
        {
            List<action> flags = agent.getFlags();
            if (flags.Contains(action.Locked))
            {
                DeleteAgent(agent);
                DeleteAgent(Unlocker);
            }
        }
    }

    private void SlashBehaviour(Agent Slasher) //find all agents on this tile, remove them if they are locked, 
    {
        agentType type = Slasher.getType();
        Tile tile = tileManager.GetTileAtPosition(Slasher.getPos());
        List<Agent> localAgents = tile.GetLocalAgents();

        foreach (Agent agent in localAgents.ToList())
        {
            List<action> flags = agent.getFlags();
            if (flags.Contains(action.Slashable))
            {
                DeleteAgent(agent);
            }
        }
    }

    private void PickupBehaviour(Agent picker)
    {
        Tile tile = tileManager.GetTileAtPosition(picker.getPos());
        List<Agent> localAgents = tile.GetLocalAgents();

        foreach (Agent agent in localAgents.ToList())
        {
            List<action> flags = agent.getFlags();
            if (flags.Contains(action.Pickup))
            {
                inventoryManager.AddNewUIAgent(agent.getType());
                DeleteAgent(agent);
            }
        }
    }

    private void BestowBehaviour(Agent bestower, agentType bestowal)
    {
        Tile tile = tileManager.GetTileAtPosition(bestower.getPos());
        List<Agent> localAgents = tile.GetLocalAgents();

        foreach (Agent agent in localAgents.ToList())
        {
            List<action> flags = agent.getFlags();
            Debug.Log(agent.getType());
            if (agent.getType() == agentType.Player)
            {
                inventoryManager.AddNewUITypePart(bestowal);
                DeleteAgent(bestower);
            }
        }
    }

    private void ExitBehaviour(Agent exiter)
    {
        Tile tile = tileManager.GetTileAtPosition(exiter.getPos());
        List<Agent> localAgents = tile.GetLocalAgents();

        foreach (Agent agent in localAgents.ToList())
        {
            List<action> flags = agent.getFlags();
            if (flags.Contains(action.Exit))
            {
                if (levels.Count > 0)
                {
                    orbManager.DeleteRules();
                    inventoryManager.DeleteItems();
                    actionBarManager.DeleteAllEquipped();
                    if (levels.Count == 2) GenererateRulesTutorial2();
                    else if (levels.Count == 1) GenererateRulesTutorial3();
                    GenerateMap(levels.Dequeue());
                }
                else winText.SetActive(true);
            }
        }
    }

    private void GenererateRulesTutorial2()
    {
        orbManager.AddRule(agentType.Wall, action.Block);
        orbManager.AddRule(agentType.Exit, action.Exit);
        orbManager.AddRule(agentType.Door, action.Block);
        orbManager.AddRule(agentType.Door, action.Flammable);
        orbManager.AddRule(agentType.Door, action.Locked);
        orbManager.AddRule(agentType.Player, action.Block);
        orbManager.AddRule(agentType.Player, action.Controllable);
        orbManager.AddRule(agentType.SlimeTrail, action.Flammable);
        orbManager.AddRule(agentType.Slime, action.Spreads, agentType.SlimeTrail);
        orbManager.AddRule(agentType.Slime, action.Move);
        orbManager.AddRule(agentType.Fire, action.Hot);
        orbManager.AddRule(agentType.Bestower2, action.Bestows, agentType.Player);
    }

    private void GenererateRulesTutorial3()
    {
        orbManager.AddRule(agentType.Wall, action.Block);
        orbManager.AddRule(agentType.Exit, action.Exit);
        orbManager.AddRule(agentType.Door, action.Block);
        orbManager.AddRule(agentType.Door, action.Locked);
        orbManager.AddRule(agentType.Slime, action.Slashable);
        orbManager.AddRule(agentType.Slime, action.Contains, agentType.Slime);
        orbManager.AddRule(agentType.Bestower1, action.Bestows, agentType.Key);
        //orbManager.AddRule(agentType.Bestower1, action.Bestows, agentType.con);
        orbManager.AddRule(agentType.Player, action.Controllable);
        orbManager.AddRule(agentType.Sword, action.Slashes);
    }

    delegate void ChangeType();
    delegate bool DeleteNextFrame();

    private void parseRules() //TODO NEXT: Set the tick rate to happen once every player input. Then figure out tile system
    {
        ruleSet = orbManager.GetRuleSet();
        List<ChangeType> changeList = new();
        //Set a temp state in agents at the beginning and remove at the end
        foreach (Rule rule in ruleSet.ToList())
        {
            foreach (Agent agent in agents.ToList())
            {
                if(agent.getType() == rule.GetSub()) //TODO: do without this if statement
                {
                    switch(rule.GetAction())
                    {
                        case action.Controllable:
                            if (Input.GetKeyDown("a")) //left
                            {
                                moveToTile(agent, new Vector2(-1,0));
                            }
                            else if (Input.GetKeyDown("d")) //right
                            {
                                moveToTile(agent, new Vector2(1, 0));
                            }
                            else if (Input.GetKeyDown("s")) //down
                            {
                                moveToTile(agent, new Vector2(0, -1));
                            }
                            else if (Input.GetKeyDown("w")) //up
                            {
                                moveToTile(agent, new Vector2(0, 1));
                            }
                            else if (Input.GetKeyDown("return"))
                            {
                                Agent temp = CreateAgent(actionBarManager.getSlot(0), agent.getPos() + agent.getFacingDir());
                                int frameLimit = 1;
                                deleteList.Add(() =>
                                {
                                    if (frameLimit <= 0) { 
                                        DeleteAgent(temp);
                                        return true;
                                    }
                                    frameLimit--;
                                    return false;
                                });
                            }
                            PickupBehaviour(agent);
                            ExitBehaviour(agent);
                            break;
                        case action.Move:
                            moveToTile(agent, new Vector2(0, UnityEngine.Random.Range(-1,2)));
                            moveToTile(agent, new Vector2(UnityEngine.Random.Range(-1, 2), 0));
                            break;
                        case action.Spreads:
                            CreateAgent(rule.GetObj(), agent.getPos());
                            break;
                        case action.Flammable:
                            if (CheckForAdjacentType(agentType.Fire, agent.getPos())) changeList.Add(() => agent.setType(agentType.Fire)); //Should check for rule hot instead
                            break;
                        case action.Unlocks:
                            UnlockBehaviour(agent);
                            break;
                        case action.Slashes:
                            SlashBehaviour(agent);
                            break;
                        case action.Contains:
                            agent.addFlag(action.Contains);
                            break;
                        case action.Locked:
                            agent.addFlag(action.Locked);
                            break;
                        case action.Pickup:
                            agent.addFlag(action.Pickup);
                            break;
                        case action.Bestows:
                            BestowBehaviour(agent, rule.GetObj());
                            break;
                        case action.Slashable:
                            agent.addFlag(action.Slashable);
                            break;
                        case action.Exit:
                            agent.addFlag(action.Exit);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        foreach(Agent agent in agents)
        {
            agent.resetFlags();
        }

        foreach (ChangeType func in changeList)
        {
            func();
        }

        foreach (DeleteNextFrame func in deleteList.ToList())
        {
            if (func()) deleteList.Remove(func); //func returns true when it has finished removing obj
        }
    }
}


//IMPORTANT make the rule order (flag rules go first, then actions, then movement, then status)

//1. level design
//1. ART

//6. Moving camera
