//<auto-generated>

using System.Diagnostics;
using GUI.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCors( options =>
{
    options.AddPolicy( "AllowSnake",
         o =>
         {
             o.AllowAnyOrigin();
//             o.WithOrigins( "http://snake.eng.utah.edu" );
         });
} );

var app = builder.Build();

app.UseCors( "AllowSnake" );

// Configure the HTTP request pipeline.
if ( app.Environment.IsDevelopment() )
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler( "/Error", createScopeForErrors: true );
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies( typeof( GUI.Client._Imports ).Assembly );

app.Run();