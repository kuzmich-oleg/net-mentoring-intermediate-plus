## What diagram types do you have on your current project?
So far, I've seen the next types:
System Context, Container, Sequence, Use Case, State, MindMap, Class

## What are the pros and cons of UML diagrams?

Pros:
- Visual represantation helps to simplify complex things.
- Depending on the diagram type, it helps to understand system architecture, provide high level system/process overview, give detailed view of concrete interaction within the system.
- Such artifacts can be used as solution documentation.
- Can be used by develops, BAs, stackholders and other team members.
- Diagrams are a good choice to propose changes or new features and explain them to others. For example, show diagram on the meeting to discuss proposed solution.
- Help to plan system's parts interaction before implementing them.
- In case of diagrams creation before implementation, it might help to reveal gaps in requirements/specification and clarify them in advance.
- Widely supported by differerent tools.
- Can be used as an input for prompts. 

Cons:
- Require at least minimal knowledge base to be able to read and create diagrams.
- It time consuming to always have them in actual state to avoid misunderstanding.
- At projects where requirement change frequently, might cause time loss 

## What is the difference between a structural diagram and a behavioral diagram in UML?
Structural diagram shows static view (structure) of the system (components, classes, relationships) while behavioral diagram shows dynamic interaction of the system parts over time.

The key difference is that Structural diagram shows what are the system parts, when behavioral diagram shows what happens with them when system is running. 

## Name the relationship types in a use case diagram

### Association between actor and use case.
Solid lines that show all available cases for the actor and all involved actors for the specific use case

### Generalization of an actor
Arrow between actors. Is similar to inheritance in .Net. It means that one actor (descendant) can inherit all the use case from another one (ancestor). But descendant should have at least 1 use case that is not associated with the ancestor. Arrow points at ancestor

### Extend between two use cases
Dashed arrow with << extend >> label pointing from Extenging use case to the Extended one (Base). 
- Extenging use case is dependent on Extended use case and can't be used without it.
- Also Extending use case is usually optional and applies under certain conditions.
- Extended (base) is meaningful without Extending.

### Include between two use cases
Dashed arrow with << include >> label pointing from Base use case to the Included one. Means that Included use case is a part of Base one.
- Main reason is to reuse common actions or simplify complex behaviour.
- Included use case is always mandatory.
- Base use case is incomplete without Included one.

### Generalization of a use case
Solid arrow with empty triangle arrowhead pointing to the parent. Similar to Generalization of an actor, i.e. child use case inherits the characteristics (meaning + behaviour) of the parent.

## What is the sequence diagram?
It's a diagram that shows objects interaction over time and the order in which the interaction happens for a single use case using vertical lines (per object) and horizontal arrows (per message). In other words, it shows how different parts of the system work in the "sequence"