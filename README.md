# Action

### Overview
`Action` managing actions of unit to ensure that everything happen in a rightway, in other words `Action` can create a pipeline and constraint for any action is being do.

`Action` help to reduce the code need to be implemented when new action is added to game, no need to use many lines of code checkout the condition before the action can be invoked.

`Action` help to make an action pipeline easier, every action will have an order in pipeline and it is very easy to handle an action or adding new action
### Using
An action can be declared by derived `IAction` then implement `Invoke` method which is using to declare the action.

### Figure
In my game, I would like my unit only can cast a spell at a moment (not free cast)
