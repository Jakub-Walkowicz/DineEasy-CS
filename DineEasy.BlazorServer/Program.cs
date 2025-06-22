using DineEasy.Application.Interfaces;
using DineEasy.Application.Services;
using DineEasy.BlazorServer.Components;
using DineEasy.BlazorServer.Services;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Radzen
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddRadzenComponents();
builder.Services.AddScoped<AuthStateService, AuthStateService>();
builder.Services.AddTransient<AuthTokenHandler>();
builder.Services.AddScoped<IReservationApiClient, ReservationApiClient>();
builder.Services.AddHttpClient<IReservationApiClient, ReservationApiClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5049/"); 
});
// Usuń poprzednią rejestrację
// builder.Services.AddScoped<IReservationApiClient, ReservationApiClient>();
builder.Services.AddHttpClient<IAuthApiClient, AuthApiClient>(client =>
    {
        client.BaseAddress = new Uri("http://localhost:5049"); 
    })
    .AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<ITableApiClient, TableApiClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5049"); 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();