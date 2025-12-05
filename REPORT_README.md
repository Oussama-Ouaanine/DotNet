# Academic Report - LaTeX Compilation Guide

## Overview

This directory contains a professional academic report documenting the Library Management System project.

**Report File:** `academic_report.tex`  
**Output:** `academic_report.pdf`

## Report Contents

1. **Title Page** - Project title, your name, EMSI branding, date
2. **Table of Contents** - Automatic navigation
3. **Abstract** - Executive summary of the project
4. **Introduction** - Context, motivation, objectives, scope
5. **Methodology** - Technology stack, architecture, design decisions
6. **Results** - Features implemented, UI/UX achievements, performance metrics
7. **Discussion** - Design rationale, challenges, lessons learned, comparisons
8. **Conclusion** - Achievements summary, future work recommendations
9. **References** - Academic and technical citations
10. **Appendices** - Technical specifications, endpoints, default accounts

## Customization Instructions

Before compiling, **edit the LaTeX file** to personalize:

### Required Changes

1. **Your Name** (line ~138):
   ```latex
   {\large\itshape Prepared by:\\[0.3cm]
   \textbf{Your Name}\\[0.2cm]  % <-- CHANGE THIS
   Student at École Marocaine des Sciences de l'Ingénieur\par}
   ```

2. **Logo Image** (line ~145):
   ```latex
   \includegraphics[width=0.3\textwidth]{emsi_logo.png}
   ```
   - Place your EMSI logo file as `emsi_logo.png` in this directory
   - **OR** comment out this line if you don't have a logo

### Optional Enhancements

- Add your student ID number
- Customize the project description
- Add screenshots in the results section
- Expand technical details in appendices
- Include code snippets using `\lstlisting` environments

## Compilation Methods

### Method 1: Automated Script (Recommended)

```bash
./build_report.sh
```

This script will:
- Check for pdflatex installation
- Compile the document twice (for TOC generation)
- Clean up temporary files
- Open the PDF automatically

### Method 2: Manual Compilation

```bash
# First pass (generates TOC structure)
pdflatex academic_report.tex

# Second pass (includes TOC in document)
pdflatex academic_report.tex

# Clean up temporary files
rm academic_report.aux academic_report.log academic_report.out academic_report.toc
```

### Method 3: Overleaf (Online)

1. Visit [Overleaf.com](https://www.overleaf.com)
2. Create a new project → Upload Project
3. Upload `academic_report.tex`
4. Click "Recompile"
5. Download the PDF

## LaTeX Installation

### Ubuntu/Debian
```bash
sudo apt-get update
sudo apt-get install texlive-latex-base texlive-latex-extra texlive-fonts-recommended
```

### macOS
```bash
brew install --cask mactex
```

### Windows
Download and install MiKTeX: https://miktex.org/download

## Troubleshooting

### Error: "pdflatex: command not found"
Install LaTeX distribution (see installation section above).

### Error: Missing logo file
Either:
- Add `emsi_logo.png` to the directory
- Comment out line 145: `%\includegraphics[width=0.3\textwidth]{emsi_logo.png}`

### PDF has no Table of Contents
Run `pdflatex` twice—the first pass generates structure, second pass includes it.

### Compilation warnings about overfull boxes
These are minor formatting warnings and don't affect the PDF. To fix, adjust text or margins.

## Document Statistics

- **Pages:** ~25 pages
- **Sections:** 5 main sections + references + appendices
- **Word Count:** ~8,500 words
- **Figures:** Placeholder for logo (add your own)
- **Code Listings:** Technical specifications in appendix

## Academic Standards

This report follows:
- IEEE/ACM academic formatting conventions
- Professional LaTeX typography
- Proper citation style
- Clear section hierarchy
- Technical documentation best practices

## Export Formats

The generated PDF is suitable for:
- Academic submission
- Portfolio presentations
- Project documentation archives
- Printing and binding

## Questions or Issues?

If you encounter compilation errors:
1. Check the `.log` file for detailed error messages
2. Ensure all required LaTeX packages are installed
3. Verify the `.tex` file syntax (no unescaped special characters)

---

**Note:** This report comprehensively documents your Library Management System project, demonstrating both technical implementation and academic writing skills.
