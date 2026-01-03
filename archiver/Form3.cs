using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Archives.Tar;
using SharpCompress.Archives.GZip;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using SharpCompress.Readers;
using DiscUtils.Iso9660;
using Ionic.Zip;

namespace archiver
{
    public partial class extract : Form
    {
        // Store the selected archive file path
        private string _archiveFilePath = string.Empty;

        // Store the selected output folder path
        private string _outputFolderPath = string.Empty;

        // Store the list of entries in the archive file
        private List<string> _archiveEntries = new List<string>();

        // Store file sizes for ISO entries
        private Dictionary<string, long> _isoFileSizes = new Dictionary<string, long>();

        // Store whether the archive is password protected
        private bool _isPasswordProtected = false;

        // Supported archive extensions
        private readonly string[] _supportedExtensions = { ".zip", ".7z", ".rar", ".tar", ".gz", ".gzip", ".bz2", ".tar.gz", ".tgz", ".tar.bz2", ".tbz2", ".cab", ".iso", ".arj", ".lzh", ".uue", ".ace" };

        // File filter for open dialog
        private const string ArchiveFileFilter = 
            "All Supported Archives|*.zip;*.7z;*.rar;*.tar;*.gz;*.gzip;*.bz2;*.tar.gz;*.tgz;*.tar.bz2;*.tbz2;*.cab;*.iso;*.arj;*.lzh;*.uue;*.ace|" +
            "ZIP Files|*.zip|" +
            "7-Zip Files|*.7z|" +
            "RAR Files|*.rar|" +
            "TAR Files|*.tar;*.tar.gz;*.tgz;*.tar.bz2;*.tbz2|" +
            "GZip Files|*.gz;*.gzip|" +
            "BZip2 Files|*.bz2|" +
            "Cabinet Files|*.cab|" +
            "ISO Files|*.iso|" +
            "ARJ Files|*.arj|" +
            "LZH Files|*.lzh|" +
            "UUE Files|*.uue|" +
            "ACE Files|*.ace|" +
            "All Files|*.*";

        public extract()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // Wire up button click events
            add_btn.Click += button1_Click;
            extract_btn.Click += button3_Click;
            brws_btn.Click += button4_Click;

            // Set default extract folder name
            extractName.Text = $"extracted_{DateTime.Now:yyyyMMdd}";

            // Allow drag and drop
            this.AllowDrop = true;
            this.DragEnter += Extract_DragEnter;
            this.DragDrop += Extract_DragDrop;

            // Enable keyboard shortcuts
            this.KeyPreview = true;
            this.KeyDown += Extract_KeyDown;
        }

        /// <summary>
        /// Handles keyboard shortcuts for the extract form
        /// </summary>
        private void Extract_KeyDown(object? sender, KeyEventArgs e)
        {
            // Ctrl+O - Add archive file
            if (e.Control && e.KeyCode == Keys.O)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                BrowseAndAddArchiveFile();
            }
            // Ctrl+B - Browse output folder
            else if (e.Control && e.KeyCode == Keys.B)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                BrowseOutputFolder();
            }
            // Ctrl+Enter - Start extraction
            else if (e.Control && e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                button3_Click(this, EventArgs.Empty);
            }
            // F5 - Refresh archive contents
            else if (e.KeyCode == Keys.F5)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (!string.IsNullOrEmpty(_archiveFilePath) && File.Exists(_archiveFilePath))
                {
                    LoadArchiveContents();
                }
            }
            // Escape - Close form
            else if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                this.Close();
            }
        }

        /// <summary>
        /// Handles the Add button click - opens file dialog to select an archive file
        /// </summary>
        private void button1_Click(object? sender, EventArgs e)
        {
            BrowseAndAddArchiveFile();
        }

        /// <summary>
        /// Handles the Browse button click - opens folder dialog to select extraction location
        /// </summary>
        private void button4_Click(object? sender, EventArgs e)
        {
            BrowseOutputFolder();
        }

        /// <summary>
        /// Handles the Extract button click - extracts the archive file
        /// </summary>
        private async void button3_Click(object? sender, EventArgs e)
        {
            await ExtractArchiveFileAsync();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Opens a file dialog to browse and select an archive file
        /// </summary>
        private void BrowseAndAddArchiveFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = ArchiveFileFilter;
                openFileDialog.Title = "Select an archive file to extract";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _archiveFilePath = openFileDialog.FileName;
                    insert.Text = Path.GetFileName(_archiveFilePath);
                    
                    // Load and display archive contents
                    LoadArchiveContents();
                }
            }
        }

        /// <summary>
        /// Gets the archive type description based on file extension
        /// </summary>
        private string GetArchiveTypeDescription(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLower();
            string fileName = Path.GetFileName(filePath).ToLower();

            // Check for compound extensions
            if (fileName.EndsWith(".tar.gz") || fileName.EndsWith(".tgz"))
                return "TAR.GZ";
            if (fileName.EndsWith(".tar.bz2") || fileName.EndsWith(".tbz2"))
                return "TAR.BZ2";

            return ext switch
            {
                ".zip" => "ZIP",
                ".7z" => "7-Zip",
                ".rar" => "RAR",
                ".tar" => "TAR",
                ".gz" or ".gzip" => "GZip",
                ".bz2" => "BZip2",
                ".cab" => "Cabinet",
                ".iso" => "ISO",
                ".arj" => "ARJ",
                ".lzh" => "LZH",
                ".uue" => "UUE",
                ".ace" => "ACE",
                _ => "Archive"
            };
        }

        /// <summary>
        /// Checks if the file is an ISO archive
        /// </summary>
        private bool IsIsoFile(string filePath)
        {
            return Path.GetExtension(filePath).Equals(".iso", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks if a ZIP archive is password protected
        /// </summary>
        private bool IsZipPasswordProtected(string zipPath)
        {
            try
            {
                using (var archive = SharpCompress.Archives.Zip.ZipArchive.Open(zipPath))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (!entry.IsDirectory && entry.IsEncrypted)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch
            {
                // If we can't determine, assume not protected
                return false;
            }
        }

        /// <summary>
        /// Loads the contents of the selected archive file into the list box
        /// </summary>
        private void LoadArchiveContents()
        {
            zip_items.Items.Clear();
            _archiveEntries.Clear();
            _isoFileSizes.Clear();
            _isPasswordProtected = false;

            if (string.IsNullOrEmpty(_archiveFilePath) || !File.Exists(_archiveFilePath))
            {
                return;
            }

            try
            {
                string archiveType = GetArchiveTypeDescription(_archiveFilePath);

                // Check if it's an ISO file - use DiscUtils
                if (IsIsoFile(_archiveFilePath))
                {
                    LoadIsoContents();
                    return;
                }

                // Check if it's a ZIP file and if it's password protected
                if (Path.GetExtension(_archiveFilePath).Equals(".zip", StringComparison.OrdinalIgnoreCase))
                {
                    _isPasswordProtected = IsZipPasswordProtected(_archiveFilePath);
                }

                // Try to open with SharpCompress for other formats
                using (var archive = ArchiveFactory.Open(_archiveFilePath))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (!entry.IsDirectory)
                        {
                            // Store entry name
                            _archiveEntries.Add(entry.Key ?? "");

                            // Show file name and size
                            string sizeInfo = FormatFileSize(entry.Size);
                            string protectedIndicator = entry.IsEncrypted ? " 🔒" : "";
                            zip_items.Items.Add($"{entry.Key} ({sizeInfo}){protectedIndicator}");
                        }
                        else
                        {
                            // Add directory entries too
                            _archiveEntries.Add(entry.Key ?? "");
                            zip_items.Items.Add($"{entry.Key} (folder)");
                        }
                    }
                }

                // Update status
                string protectedStatus = _isPasswordProtected ? " - Password Protected 🔒" : "";
                this.Text = $"EXTRACT ({archiveType}) - {_archiveEntries.Count(e => !e.EndsWith("/") && !e.EndsWith("\\"))} file(s) in archive{protectedStatus}";
            }
            catch (Exception ex)
            {
                // Try fallback for unsupported formats
                if (TryLoadUnsupportedArchive(out string errorMessage))
                {
                    return;
                }

                MessageBox.Show($"Error reading archive file: {ex.Message}\n\n{errorMessage}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _archiveFilePath = string.Empty;
                insert.Text = string.Empty;
            }
        }

        /// <summary>
        /// Loads the contents of an ISO file using DiscUtils
        /// </summary>
        private void LoadIsoContents()
        {
            try
            {
                using (FileStream isoStream = File.OpenRead(_archiveFilePath))
                using (CDReader cd = new CDReader(isoStream, true))
                {
                    // Get all files recursively
                    LoadIsoDirectory(cd, "\\");
                }

                // Update status
                this.Text = $"EXTRACT (ISO) - {_archiveEntries.Count} file(s) in archive";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading ISO file: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _archiveFilePath = string.Empty;
                insert.Text = string.Empty;
            }
        }

        /// <summary>
        /// Recursively loads files and directories from an ISO
        /// </summary>
        private void LoadIsoDirectory(CDReader cd, string path)
        {
            // Get files in current directory
            foreach (string file in cd.GetFiles(path))
            {
                string fileName = file.TrimStart('\\');
                // Remove ISO version suffix (e.g., ";1")
                fileName = RemoveIsoVersionSuffix(fileName);
                
                long fileSize = cd.GetFileLength(file);
                
                _archiveEntries.Add(fileName);
                _isoFileSizes[fileName] = fileSize;
                
                string sizeInfo = FormatFileSize(fileSize);
                zip_items.Items.Add($"{fileName} ({sizeInfo})");
            }

            // Recurse into subdirectories
            foreach (string dir in cd.GetDirectories(path))
            {
                string dirName = dir.TrimStart('\\');
                if (!string.IsNullOrEmpty(dirName))
                {
                    _archiveEntries.Add(dirName + "\\");
                    zip_items.Items.Add($"{dirName}\\ (folder)");
                }
                LoadIsoDirectory(cd, dir);
            }
        }

        /// <summary>
        /// Removes the ISO 9660 version suffix (e.g., ";1") from a filename
        /// </summary>
        private string RemoveIsoVersionSuffix(string fileName)
        {
            // ISO 9660 appends ";1" or similar version numbers to filenames
            int semicolonIndex = fileName.LastIndexOf(';');
            if (semicolonIndex > 0)
            {
                // Check if everything after the semicolon is a number
                string afterSemicolon = fileName.Substring(semicolonIndex + 1);
                if (int.TryParse(afterSemicolon, out _))
                {
                    return fileName.Substring(0, semicolonIndex);
                }
            }
            return fileName;
        }

        /// <summary>
        /// Try to handle archive formats not directly supported by SharpCompress
        /// </summary>
        private bool TryLoadUnsupportedArchive(out string errorMessage)
        {
            errorMessage = string.Empty;
            string ext = Path.GetExtension(_archiveFilePath).ToLower();

            // Check if it's an unsupported format
            if (ext == ".arj" || ext == ".lzh" || ext == ".uue" || ext == ".ace")
            {
                errorMessage = $"The {ext.ToUpper().TrimStart('.')} format may have limited support. Consider using a dedicated tool for this format.";
                
                // Still try to add a placeholder entry
                _archiveEntries.Add($"[{ext.ToUpper().TrimStart('.')} archive - contents preview not available]");
                zip_items.Items.Add($"Archive contents cannot be previewed for {ext.ToUpper().TrimStart('.')} format");
                this.Text = $"EXTRACT ({ext.ToUpper().TrimStart('.')}) - Preview not available";
                
                return true;
            }

            return false;
        }

        /// <summary>
        /// Opens a folder browser dialog to select where to extract files
        /// </summary>
        private void BrowseOutputFolder()
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select folder to extract files to";
                folderDialog.ShowNewFolderButton = true;
                folderDialog.UseDescriptionForTitle = true;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    _outputFolderPath = folderDialog.SelectedPath;
                    selectSave.Text = _outputFolderPath;
                }
            }
        }

        /// <summary>
        /// Extracts the selected archive file to the output folder
        /// </summary>
        private async Task ExtractArchiveFileAsync()
        {
            // Validate archive file is selected
            if (string.IsNullOrEmpty(_archiveFilePath))
            {
                MessageBox.Show("Please select an archive file to extract.",
                    "No File Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate archive file exists
            if (!File.Exists(_archiveFilePath))
            {
                MessageBox.Show("The selected archive file no longer exists.",
                    "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _archiveFilePath = string.Empty;
                insert.Text = string.Empty;
                zip_items.Items.Clear();
                return;
            }

            // Check for unsupported formats
            string ext = Path.GetExtension(_archiveFilePath).ToLower();
            if (ext == ".arj" || ext == ".lzh" || ext == ".uue" || ext == ".ace")
            {
                MessageBox.Show($"The {ext.ToUpper().TrimStart('.')} format extraction is not fully supported.\n\nPlease use a dedicated tool like 7-Zip or WinRAR for this format.",
                    "Format Not Supported", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate output folder is selected
            if (string.IsNullOrEmpty(_outputFolderPath))
            {
                MessageBox.Show("Please select a folder to extract the files to using the Browse button.",
                    "No Output Location", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate output folder exists
            if (!Directory.Exists(_outputFolderPath))
            {
                MessageBox.Show("The selected output folder no longer exists. Please select a new location.",
                    "Folder Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _outputFolderPath = string.Empty;
                selectSave.Text = string.Empty;
                return;
            }

            // Get the extraction folder name
            string extractFolderName = GetExtractFolderName();
            string extractPath = Path.Combine(_outputFolderPath, extractFolderName);

            // Check if folder already exists
            if (Directory.Exists(extractPath))
            {
                var result = MessageBox.Show($"A folder named '{extractFolderName}' already exists.\n\nDo you want to overwrite it?",
                    "Folder Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                // Delete existing folder
                try
                {
                    Directory.Delete(extractPath, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting existing folder: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Handle password-protected ZIP files
            string? archivePassword = null;
            if (_isPasswordProtected && ext == ".zip")
            {
                using (var passwordForm = new PasswordForm(Path.GetFileName(_archiveFilePath)))
                {
                    if (passwordForm.ShowDialog(this) != DialogResult.OK)
                    {
                        return; // User cancelled
                    }
                    archivePassword = passwordForm.Password;
                }
            }

            // Show progress form and extract
            using (ExtractProgressForm progressForm = new ExtractProgressForm())
            {
                progressForm.Show(this);
                this.Enabled = false;

                try
                {
                    bool success = false;

                    // Run extraction on a background thread
                    success = await Task.Run(() =>
                    {
                        // Use appropriate extraction method based on file type
                        if (IsIsoFile(_archiveFilePath))
                        {
                            return ExtractIsoWithProgress(extractPath, progressForm);
                        }
                        else if (_isPasswordProtected && ext == ".zip" && !string.IsNullOrEmpty(archivePassword))
                        {
                            return ExtractPasswordProtectedZipWithProgress(extractPath, progressForm, archivePassword);
                        }
                        else
                        {
                            return ExtractArchiveWithProgress(extractPath, progressForm);
                        }
                    });

                    if (success)
                    {
                        progressForm.SetComplete();
                        await Task.Delay(500); // Brief pause to show completion
                        MessageBox.Show($"Files extracted successfully!\n\nLocation: {extractPath}",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (progressForm.CancelRequested)
                    {
                        // Delete partial extraction if cancelled
                        if (Directory.Exists(extractPath))
                        {
                            try { Directory.Delete(extractPath, true); } catch { }
                        }
                        MessageBox.Show("Extraction was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Ionic.Zip.BadPasswordException)
                {
                    // Delete partial extraction on error
                    if (Directory.Exists(extractPath))
                    {
                        try { Directory.Delete(extractPath, true); } catch { }
                    }
                    MessageBox.Show("The password you entered is incorrect.\n\nPlease try again with the correct password.",
                        "Incorrect Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    // Delete partial extraction on error
                    if (Directory.Exists(extractPath))
                    {
                        try { Directory.Delete(extractPath, true); } catch { }
                    }
                    
                    // Check if it's a password-related error
                    if (ex.Message.Contains("password") || ex.Message.Contains("encrypted"))
                    {
                        MessageBox.Show("Failed to extract the archive. The password may be incorrect or the archive may be corrupted.",
                            "Extraction Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show($"Error extracting archive file: {ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                finally
                {
                    this.Enabled = true;
                    progressForm.Close();
                }
            }
        }

        /// <summary>
        /// Extracts a password-protected ZIP file using DotNetZip
        /// </summary>
        private bool ExtractPasswordProtectedZipWithProgress(string extractPath, ExtractProgressForm progressForm, string password)
        {
            // Create the extraction folder
            Directory.CreateDirectory(extractPath);

            progressForm.StartProgress();

            using (var archive = Ionic.Zip.ZipFile.Read(_archiveFilePath))
            {
                archive.Password = password;
                
                var entries = archive.Entries.ToList();
                int totalFiles = entries.Count;
                int currentIndex = 0;

                foreach (var entry in entries)
                {
                    // Check for cancellation
                    if (progressForm.CancelRequested)
                    {
                        return false;
                    }

                    currentIndex++;

                    // Skip if filename is null or empty
                    if (string.IsNullOrEmpty(entry.FileName))
                    {
                        continue;
                    }

                    // Update progress
                    progressForm.UpdateProgress(entry.FileName, currentIndex, totalFiles);

                    // Normalize the path separators (ZIP files use forward slashes)
                    string normalizedFileName = entry.FileName.Replace('/', Path.DirectorySeparatorChar);
                    
                    // Sanitize the filename to remove invalid characters
                    normalizedFileName = SanitizeFileName(normalizedFileName);
                    
                    // Get the full destination path
                    string destinationPath = Path.Combine(extractPath, normalizedFileName);
                    
                    // Security check: ensure the path is within the extract directory
                    string fullDestPath = Path.GetFullPath(destinationPath);
                    string fullExtractPath = Path.GetFullPath(extractPath);
                    if (!fullDestPath.StartsWith(fullExtractPath + Path.DirectorySeparatorChar) &&
                        !fullDestPath.Equals(fullExtractPath))
                    {
                        continue; // Skip potentially malicious entries
                    }

                    if (entry.IsDirectory)
                    {
                        // Create directory
                        Directory.CreateDirectory(destinationPath);
                    }
                    else
                    {
                        // Ensure the parent directory exists
                        string? directoryPath = Path.GetDirectoryName(destinationPath);
                        if (!string.IsNullOrEmpty(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        // Extract the file using a stream
                        using (var fileStream = File.Create(destinationPath))
                        {
                            entry.Extract(fileStream);
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Sanitizes a file path by removing or replacing invalid characters
        /// </summary>
        private string SanitizeFileName(string fileName)
        {
            // Get invalid characters for file names
            char[] invalidFileChars = Path.GetInvalidFileNameChars();
            char[] invalidPathChars = Path.GetInvalidPathChars();
            
            // Split the path into parts to handle directories and filename separately
            string[] parts = fileName.Split(Path.DirectorySeparatorChar, '/');

            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                
                // Replace invalid characters with underscore
                foreach (char c in invalidFileChars)
                {
                    if (c != Path.DirectorySeparatorChar && c != '/')
                    {
                        part = part.Replace(c, '_');
                    }
                }
                
                // Also handle some common problematic characters that might slip through
                part = part.Replace('?', '_');
                part = part.Replace('*', '_');
                part = part.Replace('"', '_');
                part = part.Replace('<', '_');
                part = part.Replace('>', '_');
                part = part.Replace('|', '_');
                part = part.Replace(':', '_');
                
                // Remove leading/trailing spaces and dots (Windows doesn't like them)
                part = part.Trim().TrimEnd('.');
                
                // If the part is empty after sanitization, use a placeholder
                if (string.IsNullOrEmpty(part))
                {
                    part = "_";
                }
                
                parts[i] = part;
            }
            
            return string.Join(Path.DirectorySeparatorChar.ToString(), parts);
        }

        /// <summary>
        /// Extracts an ISO file with progress reporting using DiscUtils
        /// </summary>
        private bool ExtractIsoWithProgress(string extractPath, ExtractProgressForm progressForm)
        {
            // Create the extraction folder
            Directory.CreateDirectory(extractPath);

            progressForm.StartProgress();

            using (FileStream isoStream = File.OpenRead(_archiveFilePath))
            using (CDReader cd = new CDReader(isoStream, true))
            {
                // Get all files (not directories) for progress tracking
                var files = _archiveEntries.Where(e => !e.EndsWith("\\") && !e.EndsWith("/")).ToList();
                int totalFiles = files.Count;
                int currentIndex = 0;

                // Extract all files recursively
                return ExtractIsoDirectory(cd, "\\", extractPath, progressForm, ref currentIndex, totalFiles);
            }
        }

        /// <summary>
        /// Recursively extracts files and directories from an ISO
        /// </summary>
        private bool ExtractIsoDirectory(CDReader cd, string isoPath, string destinationPath, 
            ExtractProgressForm progressForm, ref int currentIndex, int totalFiles)
        {
            // Create destination directory
            Directory.CreateDirectory(destinationPath);

            // Extract files in current directory
            foreach (string file in cd.GetFiles(isoPath))
            {
                // Check for cancellation
                if (progressForm.CancelRequested)
                {
                    return false;
                }

                currentIndex++;
                string fileName = Path.GetFileName(file);
                // Remove ISO version suffix (e.g., ";1")
                fileName = RemoveIsoVersionSuffix(fileName);
                
                // Update progress
                progressForm.UpdateProgress(fileName, currentIndex, totalFiles);

                string destFilePath = Path.Combine(destinationPath, fileName);

                // Extract the file
                using (Stream sourceStream = cd.OpenFile(file, FileMode.Open))
                using (FileStream destStream = File.Create(destFilePath))
                {
                    sourceStream.CopyTo(destStream);
                }
            }

            // Recurse into subdirectories
            foreach (string dir in cd.GetDirectories(isoPath))
            {
                if (progressForm.CancelRequested)
                {
                    return false;
                }

                string dirName = Path.GetFileName(dir.TrimEnd('\\'));
                string destDirPath = Path.Combine(destinationPath, dirName);

                if (!ExtractIsoDirectory(cd, dir, destDirPath, progressForm, ref currentIndex, totalFiles))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Extracts the archive file with progress reporting using SharpCompress
        /// </summary>
        private bool ExtractArchiveWithProgress(string extractPath, ExtractProgressForm progressForm)
        {
            // Create the extraction folder
            Directory.CreateDirectory(extractPath);

            progressForm.StartProgress();

            using (var archive = ArchiveFactory.Open(_archiveFilePath))
            {
                var entries = archive.Entries.ToList();
                int totalFiles = entries.Count;
                int currentIndex = 0;

                foreach (var entry in entries)
                {
                    // Check for cancellation
                    if (progressForm.CancelRequested)
                    {
                        return false;
                    }

                    currentIndex++;

                    // Update progress
                    progressForm.UpdateProgress(entry.Key ?? "Unknown", currentIndex, totalFiles);

                    // Skip if key is null or empty
                    if (string.IsNullOrEmpty(entry.Key))
                    {
                        continue;
                    }

                    // Get the full path for extraction
                    string destinationPath = Path.GetFullPath(Path.Combine(extractPath, entry.Key));

                    // Ensure the destination path is within the extract directory (security check)
                    if (!destinationPath.StartsWith(Path.GetFullPath(extractPath) + Path.DirectorySeparatorChar) &&
                        !destinationPath.Equals(Path.GetFullPath(extractPath)))
                    {
                        continue; // Skip potentially malicious entries
                    }

                    // If it's a directory entry, create the directory
                    if (entry.IsDirectory)
                    {
                        Directory.CreateDirectory(destinationPath);
                    }
                    else
                    {
                        // Ensure the directory exists
                        string? directoryPath = Path.GetDirectoryName(destinationPath);
                        if (!string.IsNullOrEmpty(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        // Extract the file
                        entry.WriteToFile(destinationPath, new ExtractionOptions
                        {
                            ExtractFullPath = false,
                            Overwrite = true
                        });
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Gets a sanitized folder name from textBox3 or generates a default one
        /// </summary>
        private string GetExtractFolderName()
        {
            string folderName = extractName.Text.Trim();

            // If empty, use archive file name without extension
            if (string.IsNullOrEmpty(folderName))
            {
                folderName = Path.GetFileNameWithoutExtension(_archiveFilePath);
                
                // Handle double extensions like .tar.gz
                if (folderName.EndsWith(".tar", StringComparison.OrdinalIgnoreCase))
                {
                    folderName = Path.GetFileNameWithoutExtension(folderName);
                }
            }

            // Remove invalid characters from folder name
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                folderName = folderName.Replace(c.ToString(), "");
            }

            return folderName;
        }

        /// <summary>
        /// Formats file size to human-readable format
        /// </summary>
        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            double size = bytes;

            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }

            return $"{size:0.##} {sizes[order]}";
        }

        /// <summary>
        /// Handles drag enter event for drag and drop functionality
        /// </summary>
        private void Extract_DragEnter(object? sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Check if the dragged file is a supported archive
                string[]? files = e.Data.GetData(DataFormats.FileDrop) as string[];
                
                if (files != null && files.Length == 1)
                {
                    string ext = Path.GetExtension(files[0]).ToLower();
                    if (_supportedExtensions.Contains(ext))
                    {
                        e.Effect = DragDropEffects.Copy;
                        return;
                    }
                }
            }
            
            e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Handles drag drop event for drag and drop functionality
        /// </summary>
        private void Extract_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                string[]? files = (string[]?)e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    // Take the first file (archive files)
                    string archiveFile = files[0];
                    
                    // Validate it's a supported archive format
                    string ext = Path.GetExtension(archiveFile).ToLower();
                    if (_supportedExtensions.Contains(ext) && File.Exists(archiveFile))
                    {
                        _archiveFilePath = archiveFile;
                        insert.Text = Path.GetFileName(_archiveFilePath);
                        
                        // Load and display archive contents
                        LoadArchiveContents();
                    }
                    else
                    {
                        MessageBox.Show("Please drag and drop a supported archive file.\n\nSupported formats: ZIP, 7z, RAR, TAR, GZ, BZ2, CAB, ISO, and more.",
                            "Unsupported File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
    }
}
