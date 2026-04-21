var builder = WebApplication.CreateBuilder(args);

// 🔹 Add MVC
builder.Services.AddControllersWithViews();

// 🔹 HttpClient (for calling API)
builder.Services.AddHttpClient();
builder.Services.AddSession();

var app = builder.Build();

// 🔹 Error Handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// 🔹 Middleware Pipeline
app.UseHttpsRedirection();

// ✅ Use static files (IMPORTANT)
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
// 🔐 (Add when using authentication)
app.UseAuthentication();   // optional now, required later
app.UseAuthorization();

// 🔹 Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();