//using Microsoft.AspNetCore.Http.HttpResults;
//using webapi_learning.Properties;

//var builder = WebApplication.CreateBuilder(args);

//var app = builder.Build();


//app.UseHttpsRedirection();
//var shirts = new List<Shirt>()
//{
//    new Shirt { Id = 1, Model = "Red" },
//    new Shirt { Id = 2, Model = "Blavk "}
//};

//// shirts
//app.MapGet("/shirts", () =>
//{
//    return shirts.ToList();
//});

//// shirts/id
//app.MapGet("/shirts/{id:int}", (int id) =>
//{
//    return shirts.Where(_ => _.Id == id).ToList();
//});

//// add shirt
//app.MapPost("/shirts", (Shirt shirt) =>
//{
//    if (shirt != null)
//    {
//        shirts.Add(shirt);

//        return Results.NoContent();
//    }
//    else
//    {
//        return Results.BadRequest();
//    }
//});

//// update
//app.MapPut("shirts/{id}", async (int id, Shirt shirt) =>
//{
//    Shirt? tmpshirt = shirts.Find(_ => _.Id == id);
//    if (tmpshirt != null)
//    {
//        tmpshirt.Model = shirt.Model;
//    }

//    return Results.NoContent();
//});

//// delete 
//app.MapDelete("/shirts/{id:int}", (int id) =>
//{
//    var shirt = shirts.Find(shirts => shirts.Id == id);
//    if (shirt != null) shirts.Remove(shirt);
//    return Results.NoContent();
//});

//app.Run();