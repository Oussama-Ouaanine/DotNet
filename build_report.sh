#!/bin/bash

# Academic Report Compilation Script
# Compiles the LaTeX report into PDF

echo "=========================================="
echo "Library Management System - Report Builder"
echo "=========================================="
echo ""

# Check if pdflatex is installed
if ! command -v pdflatex &> /dev/null; then
    echo "ERROR: pdflatex is not installed."
    echo ""
    echo "To install on Ubuntu/Debian:"
    echo "  sudo apt-get update"
    echo "  sudo apt-get install texlive-latex-base texlive-latex-extra texlive-fonts-recommended"
    echo ""
    echo "To install on macOS:"
    echo "  brew install --cask mactex"
    echo ""
    echo "To install on Windows:"
    echo "  Download and install MiKTeX from https://miktex.org/download"
    exit 1
fi

echo "✓ pdflatex found"
echo ""

# Compile the report (run twice for table of contents)
echo "Compiling report (pass 1/2)..."
pdflatex -interaction=nonstopmode academic_report.tex > /dev/null 2>&1

echo "Compiling report (pass 2/2)..."
pdflatex -interaction=nonstopmode academic_report.tex > /dev/null 2>&1

# Check if PDF was created
if [ -f "academic_report.pdf" ]; then
    echo ""
    echo "=========================================="
    echo "✓ SUCCESS: academic_report.pdf created"
    echo "=========================================="
    echo ""
    echo "File size: $(du -h academic_report.pdf | cut -f1)"
    echo "Location: $(pwd)/academic_report.pdf"
    echo ""
    
    # Clean up auxiliary files
    echo "Cleaning up temporary files..."
    rm -f academic_report.aux academic_report.log academic_report.out academic_report.toc
    echo "✓ Cleanup complete"
    echo ""
    
    # Open PDF if viewer is available
    if command -v xdg-open &> /dev/null; then
        echo "Opening PDF..."
        xdg-open academic_report.pdf &
    elif command -v open &> /dev/null; then
        echo "Opening PDF..."
        open academic_report.pdf
    fi
else
    echo ""
    echo "=========================================="
    echo "✗ ERROR: PDF compilation failed"
    echo "=========================================="
    echo ""
    echo "Check academic_report.log for details"
    exit 1
fi
