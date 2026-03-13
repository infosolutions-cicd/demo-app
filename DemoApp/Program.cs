using DemoApp.Components;
using MimeKit;
using System.Data.SqlClient;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

var yo = new MimeKit.Encodings.Base64Decoder();

var secret = "PassWord124!";
Console.WriteLine($"The secret is: {secret}");

var apiKey = app.Configuration.GetValue<string>("APIKEY");
Console.WriteLine("The API key is: " + apiKey);

BadSql("test");
void BadSql(string userInput)
{
    using var conn = new SqlConnection(app.Configuration.GetConnectionString("DefaultConnection"));
    conn.Open();
    // Vulnerable: concatenating untrusted input into SQL
    var cmd = new SqlCommand("SELECT * FROM Users WHERE Name = '" + userInput + "'", conn);
    using var reader = cmd.ExecuteReader();
    while (reader.Read()) { /*...*/ }
}

var HelloWorld = "Hello, World!";

Console.WriteLine(HelloWorld);
GetSumOfNumbers(10);
void GetSumOfNumbers(int n)
{
    int sUm = 0;
    for (int i = 1; i <= n; i++)
    {
        sUm += i;
    }
    Console.WriteLine($"The sum of the first {n} numbers is: {sUm}");
}


app.Run();
