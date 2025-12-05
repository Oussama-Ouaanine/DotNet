# 📋 Quick Reference Card - Library Management System

## 🚀 Start Application
```bash
cd /home/ergo/Desktop/dot-net/LibraryWebApp
dotnet run
# Open: http://localhost:5284
```

## 👤 Login Credentials

| Role   | Email                     | Password   |
|--------|---------------------------|------------|
| Admin  | admin@lumenlibrary.com    | admin123   |
| Client | maya@readers.com          | reader123  |
| Client | leo@readers.com           | reader123  |

## 📊 Key URLs

| Feature              | URL                           |
|----------------------|-------------------------------|
| Homepage             | http://localhost:5284         |
| Login                | /Account/Login                |
| Register             | /Account/Register             |
| Admin Dashboard      | /Admin/Index                  |
| Manage Books         | /Admin/Books                  |
| Manage Reservations  | /Admin/Bookings               |
| Client Home          | /Client/Index                 |
| Browse Catalog       | /Client/Browse                |
| Search Books         | /Client/Search                |
| My Bookings          | /Client/MyBookings            |

## 📖 Generate Academic Report PDF

### Method 1: Overleaf (Recommended)
1. Visit https://www.overleaf.com
2. New Project → Upload `academic_report.tex`
3. Click "Recompile" → Download PDF

### Method 2: Local (requires LaTeX)
```bash
# Install LaTeX first:
sudo apt-get install texlive-latex-base texlive-latex-extra texlive-fonts-recommended

# Then compile:
./build_report.sh
```

## 🛠️ Common Tasks

### Add a New Book (Admin)
1. Login as admin
2. Go to /Admin/Books
3. Click "Add new book"
4. Fill form + upload cover
5. Click "Publish title"

### Reserve a Book (Client)
1. Login as client
2. Browse or search for book
3. Click "Reserve now"
4. Wait for admin approval

### Approve Reservation (Admin)
1. Login as admin
2. Go to /Admin/Bookings
3. Find pending request
4. Set due date → Click "Approve"

### Mark Book Returned (Admin)
1. Go to /Admin/Bookings
2. Find approved booking
3. Click "Mark returned"

## 📁 Important Files

| File                      | Purpose                          |
|---------------------------|----------------------------------|
| academic_report.tex       | LaTeX source for report          |
| build_report.sh           | PDF compilation script           |
| PROJECT_OVERVIEW.md       | Comprehensive project guide      |
| REPORT_README.md          | Report compilation instructions  |
| QUICKSTART_REPORT.md      | Fast-start guide                 |

## 🎨 Design Tokens (CSS Variables)

Located in `wwwroot/css/site.css`:

- `--text-primary`: Main text color
- `--text-secondary`: Secondary text
- `--accent-primary`: Brand blue
- `--surface`: Card backgrounds
- `--spacing-*`: Spacing scale
- `--radius-*`: Border radius values

## 🔧 Troubleshooting

### Port Already in Use
```bash
fuser -k 5284/tcp
dotnet run
```

### Build Errors
```bash
dotnet clean
dotnet build
```

### Report Won't Compile
- Check LaTeX installation: `which pdflatex`
- Use Overleaf instead (no install needed)
- Remove logo line if missing: comment line 145

## 📞 Documentation

- Full overview: `PROJECT_OVERVIEW.md`
- Report guide: `REPORT_README.md`
- Quick start: `QUICKSTART_REPORT.md`
- This card: `REFERENCE_CARD.md`

---

**Need help?** Check the documentation files or the academic report for detailed explanations.
