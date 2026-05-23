# Quick Start Guide

Get the Web Lab Database Assignment up and running in 5 minutes!

## ⚡ Quick Setup

### Prerequisites Check
```bash
# Check .NET installation
dotnet --version

# Check SQL Server availability
sqlcmd -S (localdb)\mssqllocaldb -Q "SELECT @@VERSION"
```

### 1️⃣ Clone Repository
```bash
git clone https://github.com/abdulrafayau/WebLabDatabaseAssignment.git
cd WebLabDatabaseAssignment
```

### 2️⃣ Setup Database
```bash
# Option A: SQL Server Management Studio
# 1. Open SSMS
# 2. New Query
# 3. Open and run: Database/DatabaseSetup.sql

# Option B: Command Line
sqlcmd -S (localdb)\mssqllocaldb -i Database\DatabaseSetup.sql
```

### 3️⃣ Run Backend
```bash
cd Backend
dotnet restore
dotnet run
```
✅ API running at: `https://localhost:7001`

### 4️⃣ Run Frontend
```bash
# Open in browser
# Option A: Direct file
Frontend/index.html

# Option B: HTTP Server (from root)
python -m http.server 8000
# Visit: http://localhost:8000/Frontend/index.html
```

---

## 🎯 Features Quick Access

| Application | URL | Purpose |
|-------------|-----|---------|
| Home | `index.html` | Navigation hub |
| Students | `pages/student.html` | Student records |
| Contacts | `pages/contact.html` | Contact book |
| To-Do List | `pages/todo.html` | Task management (needs login) |
| Products | `pages/product.html` | Inventory tracking |

---

## 📝 Quick Test Data

### Student Test Record
- Roll: `2024004`
- Name: `John Doe`
- Email: `john@test.com`
- Dept: `Computer Science`

### Contact Test Record
- Name: `Jane Smith`
- Email: `jane@test.com`
- Phone: `03001234567`
- City: `Islamabad`

### To-Do Test User
- Username: `testuser`
- Email: `test@example.com`
- Password: `Test123`

### Product Test Item
- Name: `Test Laptop`
- SKU: `TEST-001`
- Price: `$1299.99`
- Stock: `5 units`

---

## 🔧 Troubleshooting

### Backend won't start?
```bash
# Clear cache
dotnet clean
# Restore
dotnet restore
# Run with verbose output
dotnet run --verbose
```

### Database error?
```bash
# Verify SQL Server
sqlcmd -S (localdb)\mssqllocaldb -Q "SELECT 1"

# Check connection string in Backend/appsettings.json
# Should be: (localdb)\mssqllocaldb
```

### CORS issues?
- Verify backend URL: `https://localhost:7001`
- Clear browser cache (Ctrl+Shift+Del)
- Check browser DevTools (F12) Network tab

---

## 📊 API Endpoints Summary

### Students
- `GET /api/students` - List all
- `POST /api/students` - Create
- `PUT /api/students/{id}` - Update
- `DELETE /api/students/{id}` - Delete

### Contacts
- `GET /api/contacts` - List all
- `POST /api/contacts` - Create
- `PUT /api/contacts/{id}` - Update
- `DELETE /api/contacts/{id}` - Delete

### Users (Auth)
- `POST /api/users/register` - Register
- `POST /api/users/login` - Login

### To-Do Items
- `GET /api/todoitems/user/{userId}` - Get user's tasks
- `POST /api/todoitems` - Create task
- `PUT /api/todoitems/{id}` - Update task
- `DELETE /api/todoitems/{id}` - Delete task

### Products
- `GET /api/products` - List all
- `POST /api/products` - Create
- `PUT /api/products/{id}` - Update
- `DELETE /api/products/{id}` - Delete

---

## 🚀 Common Tasks

### Add a Student
1. Go to Students page
2. Click "Add New Student"
3. Fill in details:
   - Roll Number: `2024005`
   - First Name: `Ali`
   - Email: `ali@test.com`
4. Click "Save Student"

### Add a Contact
1. Go to Contacts page
2. Click "Add New Contact"
3. Fill details and save

### Create To-Do List Task
1. Go to To-Do List page
2. Register new account or login
3. Click "Add Task"
4. Enter task details and due date
5. Save

### Manage Inventory
1. Go to Products page
2. View statistics
3. Add/Edit/Delete products
4. Track stock levels

---

## 📱 Mobile Access

The application is fully responsive!

- Open on mobile browser
- All features work the same
- Touch-friendly buttons
- Optimized layout for small screens

---

## 🔒 Security Tips

⚠️ **For Development Only**

1. Change database connection string in production
2. Use proper password hashing (currently uses simple hash for demo)
3. Implement JWT tokens for production auth
4. Enable HTTPS certificate validation
5. Add rate limiting
6. Validate all inputs server-side
7. Use parameterized queries (already implemented)

---

## 📚 Additional Resources

- [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [SQL Server Docs](https://docs.microsoft.com/sql/sql-server)
- [REST API Best Practices](https://restfulapi.net)

---

## ⏱️ Expected Setup Time

- ✅ Prerequisites: 5 min
- ✅ Clone repo: 1 min
- ✅ Database setup: 2 min
- ✅ Backend run: 1 min
- ✅ Frontend open: 1 min

**Total: ~10 minutes** ⚡

---

## 💡 Pro Tips

1. **Keep terminal open** - Helps debug backend issues
2. **Use F12 DevTools** - Check network requests
3. **Test CRUD** - Each button tests one operation
4. **Sample data** - Database includes test records
5. **Check logs** - Backend console shows requests

---

**Happy Coding! 🎉**

For detailed documentation, see [README.md](README.md) and [API_DOCUMENTATION.md](API_DOCUMENTATION.md)
