          Group 8 assigniment 

# LibraryWebAPI - RESTful Library Management System

## 📌 Project Overview
A secure ASP.NET Core Web API for library operations with optional frontend integration. Built by Team 5 for Event-Driven Programming 

## 👥 Team Members & Roles
| Name               | ID       | Contribution                     |
|--------------------|----------|----------------------------------|
| Kalkidan Ambaw     | 1501289  | Books/Borrowers API              |
| Hana Solomon       | 1501252  | Database Schema Design          |
| Rahel Mekonen      | 1501427  | Loans API                       |
| Yonas Tilahun      | 1501577  | JWT Authentication System       |
| Maramawit Zeleke   | 1501334  | Frontend Implementation         |

## 🚀 Key Features
- **JWT Authentication**: Secure user registration/login
- **Full CRUD Operations**:
  - Books management (title, author, copies)
  - Borrowers management
  - Loan transactions with due dates
- **Overdue Tracking**: Special endpoint for overdue loans
- **Minimal Frontend**: HTML/JS interface with Tailwind CSS

  Default credentioal:
  username:kalkidan
  password:kalkidan123

  username:admin
  password:admin123

## 🛠️ Tech Stack
**Backend:
- ASP.NET Core 8.0 Web API
- Entity Framework Core (Code First)
- SQL Server LocalDB
- JWT Bearer Authentication
- error handlinng

**Frontend (Bonus):
- HTML5/CSS3/ES6+
- Tailwind CSS via CDN

## 🔗 API Endpoints
### Authentication
- `POST /api/Auth/register` - User registration
- `POST /api/Auth/login` - JWT token generation

### Books
- `GET /api/Books` - List all books
- `POST /api/Books` - Add new book (Auth)
- `GET /api/Books/{id}` - Get single book
- `PUT /api/Books/{id}` - Update book (Auth)
- `DELETE /api/Books/{id}` - Remove book (Auth)

### Borrowers
- `GET /api/Borrowers` - List borrowers
- `POST /api/Borrowers` - Add borrower (Auth)

### Loans
- `POST /api/Loans` - Issue book (Auth)
- `POST /api/Loans/returns` - Return book (Auth)
- `GET /api/Loans/overdue` - List overdue loans (Auth)

## 🔒 Security
- JWT Token Authentication
- ASP.NET Core Identity
- Role-based Authorization (if implemented)
- CORS configured for frontend access

## 🚦 Getting Started
1. Clone repo
2. Configure connection string in `appsettings.json` 
3. Run migrations: `dotnet ef database update`
4. Launch API: `dotnet run`
5. Access Swagger docs at `/swagger`

## 📦 Optional Frontend
1. Open `index.html` in browser
2. Configure API base URL in `app.js`


finally the project are work only to vissual studio so run the program see the full project i mean see the front-end part 
but you can see the only back-end change in launchSettings.json file the index.html part are changed to swwager.

Source
1.chatgpt
2.geminia

## 📅 Submission
Submitted: June 2, 2025  
Course: Event-Driven Programming  
