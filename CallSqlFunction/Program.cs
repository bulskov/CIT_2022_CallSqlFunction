using CallSqlFunction;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var connectionString = "host=localhost;db=northwind;uid=bulskov;pwd=henrik";

UseAdo(connectionString);
UseAdoStoredProcedure(connectionString);
UseAdoViaEntityFramework(connectionString);
UseEntityFramework(connectionString);
UseEntityFrameworkToCallFunction(connectionString);


static void UseAdo(string connectionString)
{
    Console.WriteLine("Plain ADO");
    using var connection = new NpgsqlConnection(connectionString);
    connection.Open();

    using var cmd = new NpgsqlCommand("select * from search('%ab%')", connection);

    using var reader = cmd.ExecuteReader();

    while (reader.Read())
    {
        Console.WriteLine($"{reader.GetInt32(0)}, {reader.GetString(1)}");
    }
}

static void UseAdoStoredProcedure(string connectionString)
{
    Console.WriteLine("Plain ADO stored procedure");
    using var connection = new NpgsqlConnection(connectionString);
    connection.Open();

    using var cmd = new NpgsqlCommand("select * from search(@query)", connection);

    cmd.Parameters.AddWithValue("@query", "%ab%");

    using var reader = cmd.ExecuteReader();

    while (reader.Read())
    {
        Console.WriteLine($"{reader.GetInt32(0)}, {reader.GetString(1)}");
    }
}

static void UseAdoViaEntityFramework(string connectionString)
{
    Console.WriteLine("ADO from Entity Framework");
    using var ctx = new NorthwindContex(connectionString);
    using var connection = (NpgsqlConnection)ctx.Database.GetDbConnection();
    connection.Open();

    using var cmd = new NpgsqlCommand("select * from search('%ab%')", connection);

    using var reader = cmd.ExecuteReader();

    while (reader.Read())
    {
        Console.WriteLine($"{reader.GetInt32(0)}, {reader.GetString(1)}");
    }

}

static void UseEntityFramework(string connectionString)
{
    Console.WriteLine("Entity Framework");
    using var ctx = new NorthwindContex(connectionString);

    // You can use either FromSqlRaw or FromSqlInterpolated
    //var result = ctx.SearchResults.FromSqlRaw("select * from search({0})", "%ab%");
    var result = ctx.SearchResults.FromSqlInterpolated($"select * from search({"%ab%"})");

    foreach (var searchResult in result)
    {
        Console.WriteLine($"{searchResult.Id}, {searchResult.Name}");
    }

}



static void UseEntityFrameworkToCallFunction(string connectionString)
{
    Console.WriteLine("Call function from Entity Framework");
    using var ctx = new NorthwindContex(connectionString);

    int id = 101;
    string name = "testing";
    string description = "testing desc";

    ctx.Database.ExecuteSqlInterpolated($"select insertcategory({id},{name},{description})");

    var category = ctx.Categories.Find(id);

    Console.WriteLine("Newly inserted category:");
    Console.WriteLine($"Id={category.Id}, Name={category.Name}, Description={category.Description}");

    ctx.Categories.Remove(category);

    ctx.SaveChanges();
}


   