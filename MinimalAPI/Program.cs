using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);
// builder.WebHost.UseUrls("http://localhost:5050");

var app = builder.Build();

// save list of todos (stored in memory, no db yet)
var todos = new List<Todo>();

// get all todos
app.MapGet("/todos", () => todos);

// CREATE new todo item, task of type Todo
app.MapPost("/todos", (Todo task) => 
{
    // add created todo to list of todos
    todos.Add(task);

    // return response
    return TypedResults.Created("/todos/{id}", task);
});

// READ todo item
// includes route parameter
app.MapGet("/todos/{id}", Results<Ok<Todo>, NotFound> (int id) => 
{
    var targetTodo = todos.SingleOrDefault(t => id == t.Id);
    return targetTodo is null
        ? TypedResults.NotFound()
        : TypedResults.Ok(targetTodo);
}
);



// DELETE - delete all todos
app.MapDelete("/todos/{id}", (int id) => 
{
    todos.RemoveAll(t => id == t.Id);
    return TypedResults.NoContent();
});


app.Run();



// type to represent todos in application

// add other records for items in application
public record Todo(int Id, string Name, DateTime DueDate, bool IsCompleted){}