### What are your steps to start designing database?

Lets assume that I already know the DB type, for example from SDD.

In this case the first step would be to get familiar with the functional requirements. It's needed to predefine key business models and get understanding how they interact. What are the common query scenarios, models attributes, validation rules (e.g. max length, nullability, etc.)

After defining key models, the next step is to draw entities relation diagram. It will allow to visualize relations between the entities and describe their attributes. Also the diagram will be useful during stucture discussions with the team.

It's also important to take into account non functional requirements, for example audit log requirements, how to delete data (soft vs hard), performance requirements.
After gathering all this data together, I can add details to entities relation diagram.

With detailed diagram I'll procced with DB creation. This step includes:
 - tables definition and their relations
 - indexes set up 

### When can we say that our database is modeled correctly?

When it has the next qualities:
 - it represents all business models and supports all use cases of the system it was designed for, including data validation
 - meets performance and security requirements
 - data integrity
 - structure is normilized (e.g. no data duplicates and redundancy)
 - schema can be easily extended


### What is a Data Access Layer (DAL), and how does it simplify database interactions?

It's a layer between Database and Business Logic Layer that contains logic of interaction with DB (connection, mapping, data quering and manipulation, error handling, retries, etc.).

This additional layer helps to isolate Business Logic Layer from implementation details of interaction with Database. Business only knows that data is stored somewhere and doesn't worry about the details, using interfaces for interaction. This approach allows to change data related logic or even DB type without changing Business  logic

### You need to implement a new service for a customer. How would you select database (SQL or NoSQL)?

Need to go over the next system requirements:
 - business use cases
 - performance requirements
 - data models and their relations
 - required scalability
 - complience and security requirements

SQL indicators:
 - strict complience and security requirement
 - complex structure
 - required strong consistency
 - transactions over multiple tables
 - complex analytics queries

NoSQL indicators:
 - non structured ot semi structured data
 - requires horizontal scalability
 - schema is flexible and frequently changing over time
 - simple queries
 - content management