# API Documentation

Complete REST API documentation for Web Lab Database Assignment

## Base URL
```
https://localhost:7001/api
```

## Authentication
The To-Do List endpoints require user authentication through the Users controller.

---

## Students Endpoints

### Get All Students
```http
GET /students
```

**Response:**
```json
[
  {
    "id": 1,
    "rollNumber": "2024001",
    "firstName": "Ahmed",
    "lastName": "Hassan",
    "email": "ahmed.hassan@email.com",
    "phoneNumber": "03001234567",
    "dateOfBirth": "2004-05-15T00:00:00",
    "department": "Computer Science",
    "createdAt": "2024-05-23T10:30:00Z",
    "updatedAt": null
  }
]
```

### Get Student by ID
```http
GET /students/{id}
```

**Response:** Single student object

### Create Student
```http
POST /students
Content-Type: application/json

{
  "rollNumber": "2024004",
  "firstName": "Zainab",
  "lastName": "Ali",
  "email": "zainab.ali@email.com",
  "phoneNumber": "03001234570",
  "dateOfBirth": "2004-08-12",
  "department": "Engineering"
}
```

**Response:** 201 Created with student object

### Update Student
```http
PUT /students/{id}
Content-Type: application/json

{
  "id": 1,
  "rollNumber": "2024001",
  "firstName": "Ahmed",
  "lastName": "Hassan",
  "email": "ahmed.hassan@email.com",
  "phoneNumber": "03009876543",
  "dateOfBirth": "2004-05-15",
  "department": "Computer Science"
}
```

**Response:** 200 OK with updated student object

### Delete Student
```http
DELETE /students/{id}
```

**Response:** 200 OK with success message

---

## Contacts Endpoints

### Get All Contacts
```http
GET /contacts
```

### Get Contact by ID
```http
GET /contacts/{id}
```

### Create Contact
```http
POST /contacts
Content-Type: application/json

{
  "firstName": "Sarah",
  "lastName": "Ahmed",
  "email": "sarah.ahmed@email.com",
  "phoneNumber": "03101234567",
  "address": "123 Main Street",
  "city": "Karachi",
  "country": "Pakistan"
}
```

### Update Contact
```http
PUT /contacts/{id}
Content-Type: application/json

{
  "id": 1,
  "firstName": "Sarah",
  "lastName": "Ahmed",
  "email": "sarah.ahmed@email.com",
  "phoneNumber": "03101234567",
  "address": "456 Oak Avenue",
  "city": "Lahore",
  "country": "Pakistan"
}
```

### Delete Contact
```http
DELETE /contacts/{id}
```

---

## Users Endpoints

### Register New User
```http
POST /users/register
Content-Type: application/json

{
  "username": "johndoe",
  "email": "john@example.com",
  "password": "SecurePass123"
}
```

**Response:** 201 Created with user object

**Validation Rules:**
- Username: Required, unique
- Email: Required, valid email format, unique
- Password: Required, minimum 6 characters

### Login User
```http
POST /users/login
Content-Type: application/json

{
  "username": "johndoe",
  "password": "SecurePass123"
}
```

**Response:** 200 OK with user object containing ID
```json
{
  "id": 1,
  "username": "johndoe",
  "email": "john@example.com"
}
```

### Get User with TodoItems
```http
GET /users/{id}
```

**Response:** User object with array of todo items

---

## TodoItems Endpoints

### Get User's Todos
```http
GET /todoitems/user/{userId}
```

**Response:** Array of todo items for the user

### Get Todo by ID
```http
GET /todoitems/{id}
```

**Response:** Single todo item object

### Create Todo
```http
POST /todoitems
Content-Type: application/json

{
  "userId": 1,
  "title": "Complete project",
  "description": "Finish database assignment",
  "dueDate": "2024-05-30"
}
```

**Response:** 201 Created with todo item object

**Validation Rules:**
- userId: Must exist
- title: Required
- dueDate: Must be valid date

### Update Todo
```http
PUT /todoitems/{id}
Content-Type: application/json

{
  "title": "Complete project",
  "description": "Finish database assignment",
  "dueDate": "2024-05-30",
  "isCompleted": false
}
```

**Response:** 200 OK with updated todo item

### Delete Todo
```http
DELETE /todoitems/{id}
```

**Response:** 200 OK with success message

---

## Products Endpoints

### Get All Products
```http
GET /products
```

**Response:** Array of all products

### Get Product by ID
```http
GET /products/{id}
```

**Response:** Single product object

### Create Product
```http
POST /products
Content-Type: application/json

{
  "productName": "Laptop HP Pavilion",
  "sku": "HP-PAV-001",
  "description": "15-inch laptop with Intel i5",
  "price": 899.99,
  "quantityInStock": 20,
  "category": "Electronics",
  "supplier": "HP Inc."
}
```

**Response:** 201 Created with product object

**Validation Rules:**
- productName: Required
- sku: Required, unique
- price: Required, must be positive number
- quantityInStock: Required, must be non-negative

### Update Product
```http
PUT /products/{id}
Content-Type: application/json

{
  "id": 1,
  "productName": "Laptop HP Pavilion",
  "sku": "HP-PAV-001",
  "description": "15-inch laptop with Intel i5",
  "price": 799.99,
  "quantityInStock": 18,
  "category": "Electronics",
  "supplier": "HP Inc."
}
```

**Response:** 200 OK with updated product

### Delete Product
```http
DELETE /products/{id}
```

**Response:** 200 OK with success message

---

## Error Responses

### 400 Bad Request
```json
{
  "message": "Email already exists"
}
```

### 404 Not Found
```json
{
  "message": "Student not found"
}
```

### 500 Internal Server Error
```json
{
  "message": "An unexpected error occurred"
}
```

---

## Status Codes

| Code | Meaning |
|------|---------|
| 200 | OK - Request succeeded |
| 201 | Created - Resource created successfully |
| 400 | Bad Request - Invalid input or validation error |
| 404 | Not Found - Resource not found |
| 500 | Internal Server Error - Server error |

---

## Testing with Postman

1. Import collection from generated Swagger UI
2. API endpoint: `https://localhost:7001/swagger`
3. Test endpoints individually
4. Verify responses match expected formats

---

## Rate Limiting

No rate limiting implemented. For production, consider adding:
- Throttling per IP/user
- Request queuing
- Cache mechanisms

---

## Pagination (Future Enhancement)

Current implementation returns all records. For production, implement:
```http
GET /students?page=1&pageSize=10
```

---

**Last Updated:** May 2024
**Version:** 1.0
