using System;


public interface Condition {
    bool Evaluate();
}

public class ConditionDoNothing : Condition
{
    public bool Evaluate()
    {
        return true;
    }
}

