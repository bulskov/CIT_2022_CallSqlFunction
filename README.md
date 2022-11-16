# CallSqlFunction

In the code you find 7 different examples. 

The first three examples use ADO, the difference is that the second illustrate how to use stored procedures in C#, 
and the third how to get access to ADO from Entity Framework.

The fourth example use Entity Framework(EF) to call the function and map into a domain model object called SearchResult. 
Since this object is not part of the database model primary keys do not make sense. 
To tell this to EF you use the HasNoKey (see the NorthwindContex class).

The fifth example call a function that do not return any result (void return type), and thus do not need to map the result to anything.

The final two examples call a function that return a scalar (single value), and create mapping may be overkill. The last wrap this functionality into an extension method so the usage is easier.


 
         
Assume that the following functions is present in the DB:
         
    CREATE OR REPLACE FUNCTION search (pattern VARCHAR) 
        RETURNS TABLE (
            p_id INT,
	        p_name VARCHAR
    ) 
    AS $$
    BEGIN
        RETURN QUERY SELECT
            productid,
	        productname
        FROM
            products
        WHERE
            productname LIKE pattern;
    END; $$ 
             
    LANGUAGE 'plpgsql';
         

    CREATE OR REPLACE FUNCTION insertcategory(id int, name varchar, description varchar)
        RETURNS void AS $$
    BEGIN
        insert into categories values(id, name, desc);
    END
    $$  
    LANGUAGE 'plpgsql';

    CREATE OR REPLACE FUNCTION "public"."count_products"()
    RETURNS "pg_catalog"."int4" AS $$
    BEGIN
        RETURN(select count(*)::int from products);
    END
    $$
    LANGUAGE 'plpgsql';
        

