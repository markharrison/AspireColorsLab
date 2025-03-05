using ColorsAPI;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRedisOutputCache("outputcache");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

List<ColorsItem> colors = new List<ColorsItem>
{
    new ColorsItem { Name = "Red", Hexcode = "#FF0000" },
    new ColorsItem { Name = "Yellow", Hexcode = "#FFFF00" },
    new ColorsItem { Name = "Black", Hexcode = "#000000" },
    new ColorsItem { Name = "Green", Hexcode = "#008000" },
    new ColorsItem { Name = "Blue", Hexcode = "#0000FF" },
    new ColorsItem { Name = "Purple", Hexcode = "#800080" },
    new ColorsItem { Name = "Pink", Hexcode = "#FFC0CB" },
    new ColorsItem { Name = "Cyan", Hexcode = "#00FFFF" }
};

app.UseOutputCache();

app.MapDefaultEndpoints();

app.MapGet("/colors", () =>
{
    return Results.Ok(colors);
});

// add route to insert new color
app.MapPost("/colors", (ColorsItem color) =>
{ // validate hexcode 
    if (string.IsNullOrEmpty(color.Hexcode) || color.Hexcode.Length != 7 || color.Hexcode[0] != '#')
    {
        return Results.BadRequest("Invalid Hexcode");
    }

    colors.Add(color);
    return Results.Created($"/colors/{color.Name}", color);
});

// add route for random color 
app.MapGet("/colors/random", () =>
{
    var random = new Random();
    var randomColor = colors[random.Next(colors.Count)];

    if (randomColor.Name.ToLower() == "orange")
    {
        throw new Exception("We do not like orange");
    }

    return Results.Ok(randomColor);
})
.CacheOutput(policy =>
        {
            policy.Expire(TimeSpan.FromSeconds(10));
        }
    );

app.Run();

