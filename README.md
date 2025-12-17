# Archiver

A simple and user-friendly Windows desktop application for compressing files into ZIP archives. Built with .NET 8.0 and Windows Forms.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)
![Platform](https://img.shields.io/badge/Platform-Windows-0078D6)
![License](https://img.shields.io/badge/License-MIT-green)

## Features

- **File Selection**: Browse and select multiple files using a file dialog
- **Drag & Drop**: Simply drag files onto the application window to add them
- **Search/Filter**: Quickly filter your file list with the built-in search functionality
- **Custom Archive Names**: Specify your own ZIP file name or use auto-generated names
- **Progress Tracking**: Visual progress bar with time estimation during compression
- **Duplicate Handling**: Automatically handles duplicate file names in archives
- **Context Menu**: Right-click options for removing items, opening file locations, and more

### Supported File Types

| Type | Extensions |
|------|------------|
| Images | `.jpg`, `.jpeg`, `.png` |
| Documents | `.pdf`, `.docx` |
| Spreadsheets | `.xlsx`, `.xls` |

## Screenshots

*Application main window with file list and compression controls*

## Installation

### Prerequisites

- **Operating System**: Windows 10/11 (64-bit recommended)
- **.NET 8.0 Runtime**: Required to run the application
  - Download from: [.NET 8.0 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)

### Option 1: Download Pre-built Release (Recommended)

1. Go to the [Releases](../../releases) page
2. Download the latest `archiver.zip` file
3. Extract to your preferred location
4. Run `archiver.exe`

### Option 2: Build from Source

#### Requirements

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 (17.8+) or VS Code with C# extension

#### Build Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/archiver.git
   cd archiver
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the application**
   ```bash
   dotnet build --configuration Release
   ```

4. **Run the application**
   ```bash
   dotnet run --project archiver/archiver.csproj
   ```

#### Publish as Standalone Executable

To create a self-contained executable that doesn't require .NET runtime:

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

The output will be in `archiver/bin/Release/net8.0-windows/win-x64/publish/`

## Usage

### Adding Files

1. **Using the Add Button**: Click "Add" to open the file browser and select files
2. **Drag and Drop**: Drag files directly onto the application window

### Creating an Archive

1. Add files using either method above
2. Click "Browse" to select the output folder for your ZIP file
3. (Optional) Enter a custom name in the archive name field
4. Click "Compress" to create the ZIP archive
5. Monitor progress in the progress dialog

### Managing Files

- **Remove a file**: Right-click on a file and select "Remove Selected"
- **Clear all files**: Click "Clear all" or right-click and select "Clear All"
- **Search files**: Type in the search box to filter the file list
- **Open file location**: Right-click and select "Open File Location"

## Project Structure

```
archiver/
├── archiver.sln           # Solution file
├── README.md              # This file
└── archiver/
    ├── archiver.csproj    # Project file
    ├── Program.cs         # Application entry point
    ├── Form1.cs           # Main form logic
    ├── Form1.Designer.cs  # Main form UI designer
    ├── Form1.resx         # Main form resources
    ├── ProgressForm.cs    # Progress dialog
    └── ProgressForm.resx  # Progress dialog resources
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Built with [.NET 8.0](https://dotnet.microsoft.com/)
- Windows Forms for the user interface
- System.IO.Compression for ZIP functionality
