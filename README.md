# 📚 Lumen Library Management System 

<div align="center">
  <p>A modern, full-featured library management web application built with ASP.NET Core MVC.</p>
  
  [![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
  [![C#](https://img.shields.io/badge/C%23-12-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
  [![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)](https://getbootstrap.com/)
</div>

---

## ✨ Features

- 👤 **Dual-Role Architecture**: Dedicated portals for **Administrators** and **Library Members**.
- 📖 **Comprehensive Catalog**: Beautiful book catalog with cover image uploads.
- ✅ **Dynamic Reservation Workflow**: Real-world reservation lifecycle (`Pending` ➔ `Approved/Refused` ➔ `Completed`).
- 🎨 **Premium UI/UX**: Apple-inspired, minimalist aesthetic with generous whitespace and smooth animations.
- 📱 **Fully Responsive**: Flawless experience across desktop, tablet, and mobile devices.
- 🔐 **Authentication**: Secure session-based authentication and role-based access control.
- 🔍 **Search & Filters**: Instantly find books by title, author, or category.

## 🛠️ Tech Stack

- **Framework**: `ASP.NET Core 8.0 MVC`
- **Language**: `C# 12`
- **Frontend**: `Razor Pages`, `HTML5`, `Bootstrap 5.3` + Custom CSS properties
- **Architecture**: Service-oriented architecture with Dependency Injection

## 🚀 Getting Started

Follow these instructions to get a copy of the project up and running on your local machine.

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later.

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/Oussama-Ouaanine/DotNet.git
   ```
2. Navigate to the project directory:
   ```bash
   cd DotNet/LibraryWebApp
   ```
3. Build the application:
   ```bash
   dotnet build
   ```
4. Run the application:
   ```bash
   dotnet run
   ```
5. Open your browser and navigate to `http://localhost:5284` (or the port specified in your console).

## 🔑 Default Accounts

The application comes pre-configured with the following user accounts for immediate testing:

**Administrator Account:**
- **Email:** `admin@lumenlibrary.com`
- **Password:** `admin123`

**Client Account:**
- **Email:** `maya@readers.com`
- **Password:** `reader123`

## 🎨 Design Philosophy

This project prioritizes **User Experience** and clean aesthetics, heavily inspired by premium brands like Apple and Airbnb. It utilizes:
- **System Font Stacks** for native performance and feel.
- **Subtle Shadows & Depth** for modern component rendering.
- **Restrained Color Palette** (neutrals with a distinct accent color).
- **Mobile-first Methodology** ensuring accessibility everywhere.

## 📈 Future Enhancements

- Integration with **Entity Framework Core** and a persistent database (SQL Server/MongoDB).
- Advanced search features with Elasticsearch.
- Real-time email notifications for reservation updates.

---
<div align="center">
  <b>Built with ❤️ by <a href="https://github.com/Oussama-Ouaanine">Oussama Ouaanine</a></b>
</div>
