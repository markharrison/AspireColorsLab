using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

//var colorsAPIUrl = builder.Configuration["ColorsAPIUrl"] ?? throw new InvalidOperationException("ColorsAPIUrl required in configuration");

var colorsAPIUrl = "https+http://colorsapi/colors/random";  

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapDefaultEndpoints();

app.MapRazorPages();

app.MapGet("/api/getcolor", async (IHttpClientFactory clientFactory) => {
    var client = clientFactory.CreateClient();
    var jsonString = await client.GetStringAsync(colorsAPIUrl); // Replace with your actual URL
    var colorObject = JsonSerializer.Deserialize<object>(jsonString);
    return Results.Ok(colorObject);
});

app.Run();
