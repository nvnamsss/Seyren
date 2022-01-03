using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityNode : TheNode
{
    public string Name {get;}
    public string Icon { get; }
    public int Level { get; set; }
    public int MaxLevel { get; set; }
    public AbilityNode(string name, string icon, int level, int maxLevel) {
        Name = name;
        Icon = icon;
        Level = level;
        MaxLevel = maxLevel;
    }

    public override string ToString()
    {
        return Name;
    }
}


public class CharacterLevelCondition : Condition
{
    private int level;
    private int required;
    public CharacterLevelCondition(int level, int required) {
        this.level = level;
        this.required = required;
    }
    public bool Evaluate()
    {
        return level >= required;
    }
}