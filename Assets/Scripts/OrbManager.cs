using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject ruleContainer;
    [SerializeField] GameObject rulePrefab;
    List<GameObject> ruleSet = new();
    private int offset = -400;

    void Start()
    {
        AddRule(agentType.Door, action.Locked);
        AddRule(agentType.Door, action.Block);
        AddRule(agentType.Key, action.Unlocks);
        AddRule(agentType.Wall, action.Block);
        AddRule(agentType.Key, action.Pickup);
        AddRule(agentType.Exit, action.Exit);
        AddRule(agentType.Player, action.Controllable);
        //AddRule(agentType.Loot, action.Pickup);
        //AddRule(agentType.Chest, action.Locked);
        //AddRule(agentType.Chest, action.Contains, agentType.Loot);


        // AddRule(agentType.Player, action.Block);

        // AddRule(agentType.Skeleton, action.Move);

        //AddRule(agentType.Slime, action.Move);
        //AddRule(agentType.Slime, action.Spreads, agentType.SlimeTrail);
        //AddRule(agentType.SlimeTrail, action.Flammable);
        //AddRule(agentType.Fire, action.Hot);

        // AddRule(new Rule(agentType.Player, action.Spreads, agentType.Player));
        // AddRule(new Rule(agentType.Slime, action.Seek, agentType.Fire)); //TODO
    }

    public List<Rule> GetRuleSet() //TODO: GameManager should only need to get new ruleset whenever it changes
    {
        List<Rule> rules = new();
        foreach(GameObject obj in ruleSet)
        {
            rules.Add(obj.GetComponent<Rule>());
        }
        return rules;
    }

    public void AddRule(agentType sub, action verb)
    {
        GameObject newRule = Instantiate(rulePrefab, ruleContainer.transform);
        offset += 100;
        newRule.transform.position -= new Vector3(0,offset,0);

        Rule rule = newRule.GetComponent<Rule>();
        rule.SetSub(sub);
        rule.SetAction(verb);

        ruleSet.Add(newRule);
    }

    public void AddRule(agentType sub, action verb, agentType obj)
    {
        GameObject newRule = Instantiate(rulePrefab, ruleContainer.transform);
        offset += 100;
        newRule.transform.position -= new Vector3(0, offset, 0);

        Rule rule = newRule.GetComponent<Rule>();
        rule.SetSub(sub);
        rule.SetAction(verb);
        rule.SetObj(obj);

        ruleSet.Add(newRule);
    }

    

    public void DeleteRules()
    {
        foreach (GameObject obj in ruleSet)
        {
            Destroy(obj);
        }
        offset = -400;
        ruleSet = new();
    }
}
