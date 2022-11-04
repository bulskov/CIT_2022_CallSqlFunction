# CallSqlFunction

In the code you find 5 different examples. 

The first three examples use ADO, the difference is that the second illustrate how to use stored procedures in C#, 
and the third how to get access to ADO from Entity Framework.

The fourth example use Entity Framework(EF) to call the function and map into a domain model object called SearchResult. 
Since this object is not part of the database model primary keys do not make sense. 
To tell this to EF you use the HasNoKey (see the NorthwindContex class).

The final example call a function that do not return any result (void return type), and thus do not need to map the result to anything.

