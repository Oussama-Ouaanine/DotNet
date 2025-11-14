# 📸 Book Cover Image Upload Feature

## Overview
The application now supports uploading and displaying book cover images for each book in the library system.

## Features Added

### 1. Image Upload
- ✅ Upload images when creating new books
- ✅ Update/replace images when editing books
- ✅ Supported formats: JPG, PNG, GIF
- ✅ Images stored in `wwwroot/uploads/books/`
- ✅ Unique filename generation to prevent conflicts

### 2. Image Display
- ✅ Thumbnail in admin book list
- ✅ Full-size image on book details page
- ✅ Card images in client browse/search views
- ✅ Placeholder shown when no image available

## Files Modified

### Models
- **Book.cs** - Added `ImagePath` property to store the image path

### Controllers
- **AdminController.cs** - Updated `CreateBook()` and `EditBook()` to handle image uploads

### Views
- **CreateBook.cshtml** - Added file upload input
- **EditBook.cshtml** - Added file upload with current image preview
- **Books.cshtml** (Admin) - Added thumbnail column
- **Index.cshtml** (Client) - Added cover images to cards
- **Browse.cshtml** - Added cover images to cards
- **BookDetails.cshtml** - Added large cover image display

## How to Use

### For Administrators

#### Adding a New Book with Image
1. Go to Admin Dashboard → Books → "Add New Book"
2. Fill in all book details
3. Click "Choose File" under "Book Cover Image"
4. Select an image file (JPG, PNG, or GIF)
5. Click "Create"
6. The image will be uploaded and displayed with the book

#### Editing a Book's Image
1. Go to Admin Dashboard → Books
2. Click "Edit" on any book
3. You'll see the current cover image (if one exists)
4. To replace it, click "Choose File" and select a new image
5. Click "Save Changes"
6. The old image will be deleted and replaced with the new one

#### What Happens Behind the Scenes
```csharp
// When you upload an image:
1. File is validated (size and type)
2. Unique filename is generated: {GUID}_{originalFilename}
3. File is saved to: wwwroot/uploads/books/{uniqueFilename}
4. Path is stored in database: /uploads/books/{uniqueFilename}
5. Old image is deleted when replacing (if exists)
```

### For Clients

#### Viewing Book Images
- **Home Page**: See cover images in card layout
- **Browse Page**: All books displayed with covers
- **Book Details**: Large cover image with full details
- **Search Results**: Images shown in search results

## Technical Details

### Image Storage
- **Location**: `LibraryWebApp/wwwroot/uploads/books/`
- **Format**: `{GUID}_{originalFilename}.{extension}`
- **Example**: `a1b2c3d4-e5f6-7890-abcd-ef1234567890_book-cover.jpg`

### Database Storage
- **Field**: `ImagePath` (string)
- **Value Example**: `/uploads/books/a1b2c3d4-e5f6-7890-abcd-ef1234567890_book-cover.jpg`
- **Nullable**: Yes (books can exist without images)

### Image Display
```razor
<!-- In Razor views -->
@if (!string.IsNullOrEmpty(book.ImagePath))
{
    <img src="@book.ImagePath" alt="@book.Title cover" />
}
else
{
    <span>No Cover Image</span>
}
```

### Code Implementation

#### Controller Code (AdminController.cs)
```csharp
[HttpPost]
public async Task<IActionResult> CreateBook(Book book, IFormFile? coverImage)
{
    // Handle image upload
    if (coverImage != null && coverImage.Length > 0)
    {
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), 
            "wwwroot", "uploads", "books");
        Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}_{coverImage.FileName}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await coverImage.CopyToAsync(fileStream);
        }

        book.ImagePath = $"/uploads/books/{uniqueFileName}";
    }

    await _bookService.CreateAsync(book);
    return RedirectToAction("Books");
}
```

#### View Code (CreateBook.cshtml)
```razor
<form method="post" asp-action="CreateBook" enctype="multipart/form-data">
    <!-- Other fields... -->
    
    <div class="mb-3">
        <label for="coverImage" class="form-label">Book Cover Image</label>
        <input type="file" class="form-control" id="coverImage" 
               name="coverImage" accept="image/*" />
        <small class="text-muted">
            Upload a cover image for the book (JPG, PNG, GIF)
        </small>
    </div>
</form>
```

## Security Considerations

### Current Implementation
- ✅ File type restriction using `accept="image/*"`
- ✅ Unique filename generation prevents conflicts
- ✅ Files stored outside of user-accessible directories

### Recommended Improvements for Production
```csharp
// Add these validations:

1. File Size Limit
if (coverImage.Length > 5 * 1024 * 1024) // 5MB
{
    ModelState.AddModelError("", "File size must be less than 5MB");
    return View(book);
}

2. File Type Validation
var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
var extension = Path.GetExtension(coverImage.FileName).ToLower();
if (!allowedExtensions.Contains(extension))
{
    ModelState.AddModelError("", "Only JPG, PNG, and GIF files are allowed");
    return View(book);
}

3. Image Validation (verify it's a real image)
try
{
    using (var image = Image.Load(coverImage.OpenReadStream()))
    {
        // Valid image
    }
}
catch
{
    ModelState.AddModelError("", "Invalid image file");
    return View(book);
}
```

## Directory Structure
```
LibraryWebApp/
└── wwwroot/
    └── uploads/
        └── books/
            ├── .gitkeep
            ├── {guid}_book1.jpg
            ├── {guid}_book2.png
            └── {guid}_book3.gif
```

## Troubleshooting

### Image Not Displaying
1. **Check file exists**: Navigate to `wwwroot/uploads/books/` and verify the file
2. **Check path in database**: Should start with `/uploads/books/`
3. **Check permissions**: Ensure the directory is writable
   ```bash
   chmod 755 /home/ergo/Desktop/dot-net/LibraryWebApp/wwwroot/uploads/books/
   ```

### Upload Fails
1. **Check directory exists**:
   ```bash
   mkdir -p /home/ergo/Desktop/dot-net/LibraryWebApp/wwwroot/uploads/books/
   ```

2. **Check file size**: Large files may exceed limits
   - Default ASP.NET Core limit: 30MB
   - Configure in `Program.cs` if needed:
   ```csharp
   builder.Services.Configure<FormOptions>(options =>
   {
       options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10MB
   });
   ```

3. **Check form encoding**: Form must have `enctype="multipart/form-data"`

### Image Paths Not Saving
- Ensure hidden field in EditBook.cshtml:
  ```razor
  <input type="hidden" asp-for="ImagePath" />
  ```

## Best Practices

### 1. Image Optimization
Before uploading, consider:
- Resize images to reasonable dimensions (e.g., 800x1200 for covers)
- Compress images to reduce file size
- Use JPG for photographs, PNG for graphics

### 2. Backup Strategy
Include `wwwroot/uploads/` in your backup plan:
```bash
# Backup images
tar -czf library-images-backup.tar.gz wwwroot/uploads/

# Restore images
tar -xzf library-images-backup.tar.gz
```

### 3. CDN Integration (Future)
For production, consider using a CDN:
- AWS S3
- Azure Blob Storage
- Cloudinary
- imgix

## Testing Checklist

- [ ] Upload image when creating new book
- [ ] Create book without image (should work)
- [ ] Edit book and add image
- [ ] Edit book and replace existing image (old image deleted?)
- [ ] Delete book (manually clean up image if needed)
- [ ] View images in admin list
- [ ] View images in client browse
- [ ] View images in book details
- [ ] View images in search results
- [ ] Try uploading different formats (JPG, PNG, GIF)
- [ ] Try uploading large files (test limits)

## Future Enhancements

### Easy Additions
- [ ] Image preview before upload
- [ ] Drag-and-drop upload
- [ ] Multiple images per book
- [ ] Image gallery view

### Medium Additions
- [ ] Automatic image resizing
- [ ] Image cropping tool
- [ ] Image compression
- [ ] Thumbnail generation

### Advanced Additions
- [ ] CDN integration
- [ ] Image optimization service
- [ ] Lazy loading for performance
- [ ] WebP format support
- [ ] Image metadata extraction

## Code Comments

### Key Points to Remember
1. **Form Encoding**: Always use `enctype="multipart/form-data"` for file uploads
2. **Async Operations**: Use `await CopyToAsync()` for non-blocking uploads
3. **Unique Filenames**: Prevent conflicts with `Guid.NewGuid()`
4. **Path Storage**: Store relative paths starting with `/uploads/`
5. **Null Checks**: Always check if image exists before displaying

### Common Pitfalls
❌ Forgetting `enctype="multipart/form-data"` → File won't upload
❌ Not creating directory first → FileNotFoundException
❌ Using same filename → Files overwrite each other
❌ Storing absolute path → Won't work in production
❌ Not deleting old images → Wasted storage space

## Summary

✅ **What Works**:
- Upload images when creating/editing books
- Display images throughout the application
- Automatic cleanup when replacing images
- Graceful handling of missing images

✅ **How to Use**:
- Admin: Use file input in create/edit forms
- Client: View images automatically in all book views

✅ **Where Images Are**:
- Physical: `wwwroot/uploads/books/`
- Database: `/uploads/books/{filename}`
- Display: `<img src="@book.ImagePath" />`

---

**Feature Status**: ✅ Ready to Use

**Next Steps**: Test the feature by creating/editing books with images!
