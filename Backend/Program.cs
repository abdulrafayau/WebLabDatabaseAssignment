using Microsoft.EntityFrameworkCore;
using WebLabAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Add services
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.MapControllers();

// Run migrations/ensure database exists and seed on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Students.Any())
    {
        db.Students.AddRange(
            new WebLabAPI.Models.Student { RollNumber = "2024001", FirstName = "Ahmed", LastName = "Hassan", Email = "ahmed.hassan@email.com", PhoneNumber = "03001234567", DateOfBirth = new DateTime(2004, 5, 15), Department = "Computer Science", CreatedAt = DateTime.UtcNow },
            new WebLabAPI.Models.Student { RollNumber = "2024002", FirstName = "Fatima", LastName = "Khan", Email = "fatima.khan@email.com", PhoneNumber = "03001234568", DateOfBirth = new DateTime(2004, 6, 20), Department = "Engineering", CreatedAt = DateTime.UtcNow },
            new WebLabAPI.Models.Student { RollNumber = "2024003", FirstName = "Ali", LastName = "Ahmed", Email = "ali.ahmed@email.com", PhoneNumber = "03001234569", DateOfBirth = new DateTime(2004, 7, 10), Department = "Business", CreatedAt = DateTime.UtcNow }
        );
    }

    if (!db.Contacts.Any())
    {
        db.Contacts.AddRange(
            new WebLabAPI.Models.Contact { FirstName = "Muhammad", LastName = "Rizvi", Email = "rizvi@email.com", PhoneNumber = "03101234567", Address = "123 Main St", City = "Karachi", Country = "Pakistan", CreatedAt = DateTime.UtcNow },
            new WebLabAPI.Models.Contact { FirstName = "Ayesha", LastName = "Malik", Email = "ayesha@email.com", PhoneNumber = "03201234567", Address = "456 Oak Ave", City = "Lahore", Country = "Pakistan", CreatedAt = DateTime.UtcNow },
            new WebLabAPI.Models.Contact { FirstName = "Hassan", LastName = "Ali", Email = "hassan.ali@email.com", PhoneNumber = "03301234567", Address = "789 Pine Rd", City = "Islamabad", Country = "Pakistan", CreatedAt = DateTime.UtcNow }
        );
    }

    if (!db.Products.Any())
    {
        db.Products.AddRange(
            new WebLabAPI.Models.Product { ProductName = "Laptop Dell XPS 13", Sku = "DEL-XPS-001", Description = "High-performance laptop with Intel Core i7", Price = 1299.99m, QuantityInStock = 15, Category = "Electronics", Supplier = "Dell Inc.", CreatedAt = DateTime.UtcNow },
            new WebLabAPI.Models.Product { ProductName = "Wireless Mouse", Sku = "LOG-MOUSE-001", Description = "Bluetooth wireless mouse", Price = 29.99m, QuantityInStock = 50, Category = "Electronics", Supplier = "Logitech", CreatedAt = DateTime.UtcNow },
            new WebLabAPI.Models.Product { ProductName = "USB-C Hub", Sku = "ANK-HUB-001", Description = "7-in-1 USB-C Hub with HDMI", Price = 49.99m, QuantityInStock = 25, Category = "Electronics", Supplier = "Anker", CreatedAt = DateTime.UtcNow },
            new WebLabAPI.Models.Product { ProductName = "Programming Book", Sku = "BK-PROG-001", Description = "Clean Code: A Handbook of Agile Software Craftsmanship", Price = 39.99m, QuantityInStock = 20, Category = "Books", Supplier = "Prentice Hall", CreatedAt = DateTime.UtcNow },
            new WebLabAPI.Models.Product { ProductName = "Desk Lamp LED", Sku = "PHI-LAMP-001", Description = "Adjustable LED desk lamp with USB charging", Price = 24.99m, QuantityInStock = 30, Category = "Home & Garden", Supplier = "Philips", CreatedAt = DateTime.UtcNow }
        );
    }

    db.SaveChanges();
}

app.Run();
