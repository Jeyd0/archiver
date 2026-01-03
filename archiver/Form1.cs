using System.IO;
using SharpCompress.Archives;
using SharpCompress.Archives.Tar;
using SharpCompress.Archives.GZip;
using SharpCompress.Common;
using SharpCompress.Compressors.BZip2;
using SharpCompress.Compressors.LZMA;
using SharpCompress.Writers;
using System.IO.Compression;
using DiscUtils.Iso9660;
using Ionic.Zip;

namespace archiver
{
    public partial class compress : Form
    {
        // List to store file paths
        private List<string> _filePaths = new List<string>();
        
        // List to store folder paths
        private List<string> _folderPaths = new List<string>();

        // Store the selected output folder path
        private string _outputFolderPath = string.Empty;

        // Store the selected archive format
        private string _selectedArchiveFormat = "Select type";

        public compress()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Wire up button click events
            clear_btn.Click += Button2_Click;
            compress_btn.Click += Button3_Click;

            // Wire up search textbox event
            search.TextChanged += TextBox4_TextChanged;

            // Wire up combobox event
            type.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;

            // Set up context menu for list box
            SetupContextMenu();

            // Allow drag and drop
            this.AllowDrop = true;
            this.DragEnter += Home_DragEnter;
            this.DragDrop += Home_DragDrop;

            // Set default zip name
            zipname.Text = $"archive_{DateTime.Now:yyyyMMdd}";

            // Set default selection for combobox
            type.SelectedIndex = 0; // Default to Zip

            // Initially hide password controls (will be shown when ZIP is selected)
            UpdatePasswordVisibility();

            // Enable keyboard shortcuts
            this.KeyPreview = true;
            this.KeyDown += Compress_KeyDown;
        }

        /// <summary>
        /// Handles keyboard shortcuts for the compress form
        /// </summary>
        private void Compress_KeyDown(object? sender, KeyEventArgs e)
        {
            // Ctrl+O - Add files
            if (e.Control && e.KeyCode == Keys.O)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                button1_Click_1(this, EventArgs.Empty);
            }
            // Ctrl+F - Focus search box
            else if (e.Control && e.KeyCode == Keys.F)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                search.Focus();
                search.SelectAll();
            }
            // Delete - Remove selected item
            else if (e.KeyCode == Keys.Delete && list_of_item_selected.Focused)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                RemoveSelectedItem();
            }
            // Escape - Clear search or close if search is empty
            else if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (!string.IsNullOrEmpty(search.Text))
                {
                    search.Clear();
                    list_of_item_selected.Focus();
                }
                else
                {
                    this.Close();
                }
            }
            // Ctrl+L - Clear all items
            else if (e.Control && e.KeyCode == Keys.L)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                ClearAllItems();
            }
            // Ctrl+B - Browse output folder
            else if (e.Control && e.KeyCode == Keys.B)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                BrowseOutputFolder();
            }
            // Ctrl+Enter - Start compression
            else if (e.Control && e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                Button3_Click(this, EventArgs.Empty);
            }
            // Ctrl+Shift+F - Add folder
            else if (e.Control && e.Shift && e.KeyCode == Keys.F)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                BrowseAndAddFolder();
            }
        }

        /// <summary>
        /// Handles the combobox selection changed event - updates the selected archive format
        /// </summary>
        private void ComboBox1_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (type.SelectedIndex >= 0)
            {
                _selectedArchiveFormat = type.SelectedItem?.ToString() ?? "Zip";
                UpdateArchiveNameExtension();
                UpdatePasswordVisibility();
            }
        }

        /// <summary>
        /// Updates the archive name extension based on selected format
        /// </summary>
        private void UpdateArchiveNameExtension()
        {
            string currentName = zipname.Text.Trim();

            // Remove existing extension if present
            string[] knownExtensions = { ".zip", ".tar", ".iso", ".gz", ".bz2", ".lz" };
            foreach (string ext in knownExtensions)
            {
                if (currentName.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                {
                    currentName = currentName.Substring(0, currentName.Length - ext.Length);
                    break;
                }
            }

            // The extension will be added when compressing, so just keep the base name
            zipname.Text = currentName;
        }

        /// <summary>
        /// Gets the file extension for the selected archive format
        /// </summary>
        private string GetArchiveExtension()
        {
            return _selectedArchiveFormat.ToLower() switch
            {
                "zip" => ".zip",
                "tar" => ".tar",
                "iso" => ".iso",
                "gzip" => ".gz",
                "bzip2" => ".bz2",
                "lzip" => ".lz",
                _ => ".zip"
            };
        }

        /// <summary>
        /// Updates the visibility of password controls based on selected format
        /// Password is only supported for ZIP format
        /// </summary>
        private void UpdatePasswordVisibility()
        {
            bool isZip = _selectedArchiveFormat.Equals("Zip", StringComparison.OrdinalIgnoreCase);
            password.Visible = isZip;
            label1.Visible = isZip;
            showpass.Visible = isZip;

            // Clear password and reset show password checkbox when switching to non-ZIP format
            if (!isZip)
            {
                password.Text = string.Empty;
                showpass.Checked = false;
            }
        }

        /// <summary>
        /// Handles the Add button click - opens file dialog to browse files
        /// </summary>
        private async void button1_Click_1(object sender, EventArgs e)
        {
            await BrowseAndAddFilesAsync();
        }

        /// <summary>
        /// Handles the Clear all button click - clears all items from the list
        /// </summary>
        private void Button2_Click(object? sender, EventArgs e)
        {
            ClearAllItems();
        }

        /// <summary>
        /// Handles the Compress button click - compresses files to ZIP format
        /// </summary>
        private async void Button3_Click(object? sender, EventArgs e)
        {
            await CompressFilesAsync();
        }

        /// <summary>
        /// Handles the Browse button click - opens folder dialog to select save location
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            BrowseOutputFolder();
        }

        /// <summary>
        /// Handles the search textbox text changed event - filters the file list
        /// </summary>
        private void TextBox4_TextChanged(object? sender, EventArgs e)
        {
            FilterFileList();
        }

        /// <summary>
        /// Filters the file list based on search text
        /// </summary>
        private void FilterFileList()
        {
            string searchText = search.Text.Trim().ToLower();

            // Clear the listbox
            list_of_item_selected.Items.Clear();

            if (string.IsNullOrEmpty(searchText))
            {
                // Show all files if search is empty
                foreach (string filePath in _filePaths)
                {
                    list_of_item_selected.Items.Add(Path.GetFileName(filePath));
                }
                
                // Show all folders
                foreach (string folderPath in _folderPaths)
                {
                    list_of_item_selected.Items.Add($"?? {Path.GetFileName(folderPath)}");
                }
            }
            else
            {
                // Show only files matching the search text
                foreach (string filePath in _filePaths)
                {
                    string fileName = Path.GetFileName(filePath).ToLower();
                    if (fileName.Contains(searchText))
                    {
                        list_of_item_selected.Items.Add(Path.GetFileName(filePath));
                    }
                }
                
                // Show only folders matching the search text
                foreach (string folderPath in _folderPaths)
                {
                    string folderName = Path.GetFileName(folderPath).ToLower();
                    if (folderName.Contains(searchText))
                    {
                        list_of_item_selected.Items.Add($"?? {Path.GetFileName(folderPath)}");
                    }
                }
            }
        }

        /// <summary>
        /// Gets the actual file index from the filtered list selection
        /// </summary>
        private int GetActualFileIndex(int selectedIndex)
        {
            if (selectedIndex < 0 || selectedIndex >= list_of_item_selected.Items.Count)
                return -1;

            string selectedFileName = list_of_item_selected.Items[selectedIndex].ToString() ?? "";

            // Find the matching file in _filePaths
            for (int i = 0; i < _filePaths.Count; i++)
            {
                if (Path.GetFileName(_filePaths[i]) == selectedFileName)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Opens a folder browser dialog to select where to save the archive
        /// </summary>
        private void BrowseOutputFolder()
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select folder to save the archive";
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
        /// Opens a file dialog to browse and add files
        /// </summary>
        private async Task BrowseAndAddFilesAsync()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "All Files|*.*";
                openFileDialog.Multiselect = true;
                openFileDialog.Title = "Select files to archive";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string[] selectedFiles = openFileDialog.FileNames;

                    // Show progress form for multiple files
                    if (selectedFiles.Length > 1)
                    {
                        await AddFilesWithProgressAsync(selectedFiles);
                    }
                    else
                    {
                        // Single file - add directly without progress dialog
                        foreach (string file in selectedFiles)
                        {
                            AddFile(file);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Opens a folder browser dialog to browse and add a folder
        /// </summary>
        private void BrowseAndAddFolder()
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select folder to add to archive";
                folderDialog.ShowNewFolderButton = false;
                folderDialog.UseDescriptionForTitle = true;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string folderPath = folderDialog.SelectedPath;
                    AddFolder(folderPath);
                }
            }
        }

        /// <summary>
        /// Adds a folder to the list
        /// </summary>
        private void AddFolder(string folderPath)
        {
            // Check if folder exists
            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show($"Folder not found: {folderPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check for duplicates
            if (_folderPaths.Contains(folderPath))
            {
                MessageBox.Show("This folder is already in the list.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Add to folder list
            _folderPaths.Add(folderPath);
            
            // Add to display list with a folder indicator
            list_of_item_selected.Items.Add($"?? {Path.GetFileName(folderPath)}");

            UpdateStatusText();
        }

        /// <summary>
        /// Adds multiple files with a progress dialog
        /// </summary>
        private async Task AddFilesWithProgressAsync(string[] files)
        {
            using (AddFilesProgressForm progressForm = new AddFilesProgressForm())
            {
                progressForm.Show(this);
                this.Enabled = false;

                int addedCount = 0;
                int skippedCount = 0;

                try
                {
                    progressForm.StartProgress();
                    int totalFiles = files.Length;

                    // Run file adding on a background thread
                    await Task.Run(() =>
                    {
                        for (int i = 0; i < totalFiles; i++)
                        {
                            // Check for cancellation
                            if (progressForm.CancelRequested)
                            {
                                break;
                            }

                            string filePath = files[i];
                            string fileName = Path.GetFileName(filePath);

                            // Update progress
                            progressForm.UpdateProgress(fileName, i + 1, totalFiles);

                            // Try to add the file
                            bool added = false;
                            this.Invoke(() =>
                            {
                                added = AddFileSilent(filePath);
                            });

                            if (added)
                            {
                                addedCount++;
                            }
                            else
                            {
                                skippedCount++;
                            }
                        }
                    });

                    if (!progressForm.CancelRequested)
                    {
                        progressForm.SetComplete(addedCount, skippedCount);
                        await Task.Delay(500); // Brief pause to show completion

                        if (addedCount > 0)
                        {
                            MessageBox.Show($"Successfully added {addedCount} file(s).\n{skippedCount} file(s) were skipped (duplicates).",
                                "Files Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No files were added. All files were duplicates.",
                                "No Files Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Adding files was cancelled.\n\n{addedCount} file(s) were added before cancellation.",
                            "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding files: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Enabled = true;
                    progressForm.Close();

                    // Refresh the filtered list and update status
                    FilterFileList();
                    UpdateStatusText();
                }
            }
        }

        /// <summary>
        /// Adds a file silently without showing message boxes (for batch adding)
        /// </summary>
        /// <returns>True if file was added, false if skipped</returns>
        private bool AddFileSilent(string filePath)
        {
            // Check if file exists
            if (!File.Exists(filePath))
            {
                return false;
            }

            // Check for duplicates
            if (_filePaths.Contains(filePath))
            {
                return false;
            }

            // Add to list (no file type restriction - accept all files)
            _filePaths.Add(filePath);
            return true;
        }

        /// <summary>
        /// Adds a file to the list if it's valid
        /// </summary>
        private void AddFile(string filePath)
        {
            // Check if file exists
            if (!File.Exists(filePath))
            {
                MessageBox.Show($"File not found: {filePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check for duplicates
            if (_filePaths.Contains(filePath))
            {
                MessageBox.Show("This file is already in the list.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Add to list (no file type restriction - accept all files)
            _filePaths.Add(filePath);

            // Refresh the filtered list
            FilterFileList();

            UpdateStatusText();
        }

        /// <summary>
        /// Removes the selected item from the list
        /// </summary>
        private void RemoveSelectedItem()
        {
            if (list_of_item_selected.SelectedIndex >= 0)
            {
                int actualIndex = GetActualFileIndex(list_of_item_selected.SelectedIndex);
                if (actualIndex >= 0)
                {
                    _filePaths.RemoveAt(actualIndex);
                    FilterFileList();
                    UpdateStatusText();
                }
            }
        }

        /// <summary>
        /// Clears all items from the list
        /// </summary>
        private void ClearAllItems()
        {
            if (_filePaths.Count > 0 || _folderPaths.Count > 0)
            {
                var result = MessageBox.Show("Are you sure you want to clear all items?", "Confirm Clear",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _filePaths.Clear();
                    _folderPaths.Clear();
                    list_of_item_selected.Items.Clear();
                    search.Clear(); // Clear search text
                    UpdateStatusText();
                }
            }
        }

        /// <summary>
        /// Gets a sanitized archive file name from textBox3 or generates a default one
        /// </summary>
        private string GetZipFileName()
        {
            string zipName = zipname.Text.Trim();

            // If empty, generate default name
            if (string.IsNullOrEmpty(zipName))
            {
                zipName = $"archive_{DateTime.Now:yyyyMMdd_HHmmss}";
            }

            // Remove invalid characters from filename
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                zipName = zipName.Replace(c.ToString(), "");
            }

            // Remove any existing archive extensions
            string[] knownExtensions = { ".zip", ".tar", ".iso", ".gz", ".bz2", ".lz" };
            foreach (string ext in knownExtensions)
            {
                if (zipName.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                {
                    zipName = zipName.Substring(0, zipName.Length - ext.Length);
                    break;
                }
            }

            // Add the appropriate extension based on selected format
            zipName += GetArchiveExtension();

            return zipName;
        }

        /// <summary>
        /// Compresses files to the selected archive format
        /// </summary>
        private async Task CompressFilesAsync()
        {
            // Validate there are files or folders to compress
            if (_filePaths.Count == 0 && _folderPaths.Count == 0)
            {
                MessageBox.Show("Please add files or folders to compress.", "No Files or Folders", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate output folder is selected
            if (string.IsNullOrEmpty(_outputFolderPath))
            {
                MessageBox.Show("Please select a folder to save the archive using the Browse button.",
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

            // Get the archive file name from textBox3
            string archiveFileName = GetZipFileName();
            string archivePath = Path.Combine(_outputFolderPath, archiveFileName);

            // Check if file already exists
            if (File.Exists(archivePath))
            {
                var result = MessageBox.Show($"A file named '{archiveFileName}' already exists.\n\nDo you want to overwrite it?",
                    "File Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            // Validate format-specific limitations
            string format = _selectedArchiveFormat.ToLower();
            if ((format == "gzip" || format == "bzip2" || format == "lzip") && (_filePaths.Count + _folderPaths.Count) > 1)
            {
                MessageBox.Show($"{_selectedArchiveFormat} format can only compress a single file.\n\nPlease select only one file or use ZIP/TAR format for multiple files/folders.",
                    $"{_selectedArchiveFormat} Limitation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Show progress form and compress
            using (ProgressForm progressForm = new ProgressForm())
            {
                progressForm.Show(this);
                this.Enabled = false;

                try
                {
                    bool success = false;

                    // Run compression on a background thread
                    success = await Task.Run(() =>
                    {
                        switch (format)
                        {
                            case "zip":
                                return CreateZipArchiveWithProgress(archivePath, progressForm);
                            case "tar":
                                return CreateTarArchiveWithProgress(archivePath, progressForm);
                            case "gzip":
                                return CreateGZipArchiveWithProgress(archivePath, progressForm);
                            case "bzip2":
                                return CreateBZip2ArchiveWithProgress(archivePath, progressForm);
                            case "lzip":
                                return CreateLZipArchiveWithProgress(archivePath, progressForm);
                            case "iso":
                                return CreateIsoArchiveWithProgress(archivePath, progressForm);
                            default:
                                this.Invoke(() => MessageBox.Show($"The {_selectedArchiveFormat} format is not supported.",
                                    "Format Not Supported", MessageBoxButtons.OK, MessageBoxIcon.Information));
                                return false;
                        }
                    });

                    if (success)
                    {
                        progressForm.SetComplete();
                        await Task.Delay(500); // Brief pause to show completion
                        MessageBox.Show($"{_selectedArchiveFormat.ToUpper()} archive created successfully!\n\nLocation: {archivePath}",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clear added files and folders after successful compression
                        _filePaths.Clear();
                        _folderPaths.Clear();
                        list_of_item_selected.Items.Clear();
                        search.Clear();
                        
                        // Clear textboxes and reset state
                        zipname.Text = $"archive_{DateTime.Now:yyyyMMdd}"; // Reset to default name pattern
                        selectSave.Clear();
                        password.Clear();
                        _outputFolderPath = string.Empty;
                        
                        UpdateStatusText();
                    }
                    else if (progressForm.CancelRequested)
                    {
                        // Delete partial file if cancelled
                        if (File.Exists(archivePath))
                        {
                            File.Delete(archivePath);
                        }
                        MessageBox.Show("Compression was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    // Delete partial file on error
                    if (File.Exists(archivePath))
                    {
                        try { File.Delete(archivePath); } catch { }
                    }
                    MessageBox.Show($"Error creating archive: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Enabled = true;
                    progressForm.Close();
                }
            }
        }

        /// <summary>
        /// Creates a ZIP archive from the selected files with progress reporting
        /// </summary>
        private bool CreateZipArchiveWithProgress(string zipPath, ProgressForm progressForm)
        {
            // Delete existing file if it exists
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }

            // Track entry names to handle duplicates
            HashSet<string> usedEntryNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            progressForm.StartProgress();

            // Get password if provided
            string? archivePassword = string.IsNullOrEmpty(password.Text) ? null : password.Text;

            // Use DotNetZip if password is provided, otherwise use System.IO.Compression
            if (!string.IsNullOrEmpty(archivePassword))
            {
                return CreatePasswordProtectedZipWithProgress(zipPath, progressForm, archivePassword, usedEntryNames);
            }

            // Use standard System.IO.Compression for non-password protected archives
            using (System.IO.Compression.ZipArchive archive = System.IO.Compression.ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                // Count total files including files in folders
                int totalFiles = _filePaths.Count;
                foreach (string folderPath in _folderPaths)
                {
                    totalFiles += Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories).Length;
                }

                int currentFileIndex = 0;

                // Add individual files
                for (int i = 0; i < _filePaths.Count; i++)
                {
                    // Check for cancellation
                    if (progressForm.CancelRequested)
                    {
                        return false;
                    }

                    string filePath = _filePaths[i];

                    if (File.Exists(filePath))
                    {
                        string entryName = Path.GetFileName(filePath);
                        currentFileIndex++;

                        // Update progress
                        progressForm.UpdateProgress(entryName, currentFileIndex, totalFiles);

                        // Handle duplicate file names in archive
                        string originalName = Path.GetFileNameWithoutExtension(entryName);
                        string extension = Path.GetExtension(entryName);
                        int counter = 1;

                        while (usedEntryNames.Contains(entryName))
                        {
                            entryName = $"{originalName}_{counter}{extension}";
                            counter++;
                        }

                        usedEntryNames.Add(entryName);
                        archive.CreateEntryFromFile(filePath, entryName, CompressionLevel.Optimal);
                    }
                }

                // Add folders recursively
                foreach (string folderPath in _folderPaths)
                {
                    if (progressForm.CancelRequested)
                    {
                        return false;
                    }

                    string folderName = Path.GetFileName(folderPath);
                    string[] files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);

                    foreach (string file in files)
                    {
                        if (progressForm.CancelRequested)
                        {
                            return false;
                        }

                        currentFileIndex++;

                        // Get relative path within the folder
                        string relativePath = Path.GetRelativePath(folderPath, file);
                        string entryName = Path.Combine(folderName, relativePath);

                        // Update progress
                        progressForm.UpdateProgress(entryName, currentFileIndex, totalFiles);

                        // Normalize path separators for ZIP
                        entryName = entryName.Replace(Path.DirectorySeparatorChar, '/');

                        archive.CreateEntryFromFile(file, entryName, CompressionLevel.Optimal);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Creates a password-protected ZIP archive using DotNetZip (Ionic.Zip)
        /// </summary>
        private bool CreatePasswordProtectedZipWithProgress(string zipPath, ProgressForm progressForm,
            string archivePassword, HashSet<string> usedEntryNames)
        {
            using (var archive = new Ionic.Zip.ZipFile())
            {
                // Set password for the archive
                archive.Password = archivePassword;
                archive.Encryption = EncryptionAlgorithm.WinZipAes256;

                // Count total files including files in folders
                int totalFiles = _filePaths.Count;
                foreach (string folderPath in _folderPaths)
                {
                    totalFiles += Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories).Length;
                }

                int currentFileIndex = 0;

                // Add individual files
                for (int i = 0; i < _filePaths.Count; i++)
                {
                    // Check for cancellation
                    if (progressForm.CancelRequested)
                    {
                        return false;
                    }

                    string filePath = _filePaths[i];

                    if (File.Exists(filePath))
                    {
                        string entryName = Path.GetFileName(filePath);
                        currentFileIndex++;

                        // Update progress
                        progressForm.UpdateProgress(entryName, currentFileIndex, totalFiles);

                        // Handle duplicate file names in archive
                        string originalName = Path.GetFileNameWithoutExtension(entryName);
                        string extension = Path.GetExtension(entryName);
                        int counter = 1;

                        while (usedEntryNames.Contains(entryName))
                        {
                            entryName = $"{originalName}_{counter}{extension}";
                            counter++;
                        }

                        usedEntryNames.Add(entryName);

                        // Add file to archive
                        var entry = archive.AddFile(filePath, "");

                        // Rename entry if needed for duplicate handling
                        if (entryName != Path.GetFileName(filePath))
                        {
                            entry.FileName = entryName;
                        }
                    }
                }

                // Add folders recursively
                foreach (string folderPath in _folderPaths)
                {
                    if (progressForm.CancelRequested)
                    {
                        return false;
                    }

                    string folderName = Path.GetFileName(folderPath);
                    
                    // Add entire directory
                    archive.AddDirectory(folderPath, folderName);
                    
                    // Update progress for each file in the folder
                    string[] files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
                    foreach (string file in files)
                    {
                        currentFileIndex++;
                        string relativePath = Path.GetRelativePath(folderPath, file);
                        string entryName = Path.Combine(folderName, relativePath);
                        progressForm.UpdateProgress(entryName, currentFileIndex, totalFiles);
                    }
                }

                // Save the archive
                archive.Save(zipPath);
            }

            return true;
        }

        /// <summary>
        /// Creates a TAR archive from the selected files with progress reporting
        /// </summary>
        private bool CreateTarArchiveWithProgress(string tarPath, ProgressForm progressForm)
        {
            // Delete existing file if it exists
            if (File.Exists(tarPath))
            {
                File.Delete(tarPath);
            }

            // Track entry names to handle duplicates
            HashSet<string> usedEntryNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            progressForm.StartProgress();

            using (var archive = TarArchive.Create())
            {
                // Count total files including files in folders
                int totalFiles = _filePaths.Count;
                foreach (string folderPath in _folderPaths)
                {
                    totalFiles += Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories).Length;
                }

                int currentFileIndex = 0;

                // Add individual files
                for (int i = 0; i < _filePaths.Count; i++)
                {
                    // Check for cancellation
                    if (progressForm.CancelRequested)
                    {
                        return false;
                    }

                    string filePath = _filePaths[i];

                    if (File.Exists(filePath))
                    {
                        string entryName = Path.GetFileName(filePath);
                        currentFileIndex++;

                        // Update progress
                        progressForm.UpdateProgress(entryName, currentFileIndex, totalFiles);

                        // Handle duplicate file names in archive
                        string originalName = Path.GetFileNameWithoutExtension(entryName);
                        string extension = Path.GetExtension(entryName);
                        int counter = 1;

                        while (usedEntryNames.Contains(entryName))
                        {
                            entryName = $"{originalName}_{counter}{extension}";
                            counter++;
                        }

                        usedEntryNames.Add(entryName);
                        archive.AddEntry(entryName, filePath);
                    }
                }

                // Add folders recursively
                foreach (string folderPath in _folderPaths)
                {
                    if (progressForm.CancelRequested)
                    {
                        return false;
                    }

                    string folderName = Path.GetFileName(folderPath);
                    string[] files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);

                    foreach (string file in files)
                    {
                        if (progressForm.CancelRequested)
                        {
                            return false;
                        }

                        currentFileIndex++;

                        // Get relative path within the folder
                        string relativePath = Path.GetRelativePath(folderPath, file);
                        string entryName = Path.Combine(folderName, relativePath);

                        // Update progress
                        progressForm.UpdateProgress(entryName, currentFileIndex, totalFiles);

                        // Normalize path separators for TAR (use forward slashes)
                        entryName = entryName.Replace(Path.DirectorySeparatorChar, '/');

                        archive.AddEntry(entryName, file);
                    }
                }

                // Save the archive
                using (var fileStream = File.Create(tarPath))
                {
                    archive.SaveTo(fileStream, new WriterOptions(CompressionType.None));
                }
            }

            return true;
        }

        /// <summary>
        /// Creates a GZip archive from the selected files with progress reporting
        /// Note: GZip can only compress a single file
        /// </summary>
        private bool CreateGZipArchiveWithProgress(string gzipPath, ProgressForm progressForm)
        {
            // Delete existing file if it exists
            if (File.Exists(gzipPath))
            {
                File.Delete(gzipPath);
            }

            progressForm.StartProgress();

            string filePath = _filePaths[0];

            if (!File.Exists(filePath))
            {
                return false;
            }

            string fileName = Path.GetFileName(filePath);
            progressForm.UpdateProgress(fileName, 1, 1);

            // Check for cancellation
            if (progressForm.CancelRequested)
            {
                return false;
            }

            using (FileStream originalFileStream = File.OpenRead(filePath))
            using (FileStream compressedFileStream = File.Create(gzipPath))
            using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionLevel.Optimal))
            {
                originalFileStream.CopyTo(compressionStream);
            }

            return true;
        }

        /// <summary>
        /// Creates a BZip2 archive from the selected files with progress reporting
        /// Note: BZip2 can only compress a single file
        /// </summary>
        private bool CreateBZip2ArchiveWithProgress(string bzip2Path, ProgressForm progressForm)
        {
            // Delete existing file if it exists
            if (File.Exists(bzip2Path))
            {
                File.Delete(bzip2Path);
            }

            progressForm.StartProgress();

            string filePath = _filePaths[0];

            if (!File.Exists(filePath))
            {
                return false;
            }

            string fileName = Path.GetFileName(filePath);
            progressForm.UpdateProgress(fileName, 1, 1);

            // Check for cancellation
            if (progressForm.CancelRequested)
            {
                return false;
            }

            using (FileStream originalFileStream = File.OpenRead(filePath))
            using (FileStream compressedFileStream = File.Create(bzip2Path))
            using (BZip2Stream compressionStream = new BZip2Stream(compressedFileStream, SharpCompress.Compressors.CompressionMode.Compress, false))
            {
                originalFileStream.CopyTo(compressionStream);
            }

            return true;
        }

        /// <summary>
        /// Creates an LZip archive from the selected files with progress reporting
        /// Note: LZip can only compress a single file
        /// </summary>
        private bool CreateLZipArchiveWithProgress(string lzipPath, ProgressForm progressForm)
        {
            // Delete existing file if it exists
            if (File.Exists(lzipPath))
            {
                File.Delete(lzipPath);
            }

            progressForm.StartProgress();

            string filePath = _filePaths[0];

            if (!File.Exists(filePath))
            {
                return false;
            }

            string fileName = Path.GetFileName(filePath);
            progressForm.UpdateProgress(fileName, 1, 1);

            // Check for cancellation
            if (progressForm.CancelRequested)
            {
                return false;
            }

            using (FileStream originalFileStream = File.OpenRead(filePath))
            using (FileStream compressedFileStream = File.Create(lzipPath))
            using (LZipStream compressionStream = new LZipStream(compressedFileStream, SharpCompress.Compressors.CompressionMode.Compress))
            {
                originalFileStream.CopyTo(compressionStream);
            }

            return true;
        }

        /// <summary>
        /// Creates an ISO archive from the selected files with progress reporting
        /// </summary>
        private bool CreateIsoArchiveWithProgress(string isoPath, ProgressForm progressForm)
        {
            // Delete existing file if it exists
            if (File.Exists(isoPath))
            {
                File.Delete(isoPath);
            }

            // Track entry names to handle duplicates
            HashSet<string> usedEntryNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            progressForm.StartProgress();

            CDBuilder builder = new CDBuilder();
            builder.UseJoliet = true;
            builder.VolumeIdentifier = "ARCHIVE";

            // Count total files including files in folders
            int totalFiles = _filePaths.Count;
            foreach (string folderPath in _folderPaths)
            {
                totalFiles += Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories).Length;
            }

            int currentFileIndex = 0;

            // Add individual files
            for (int i = 0; i < _filePaths.Count; i++)
            {
                // Check for cancellation
                if (progressForm.CancelRequested)
                {
                    return false;
                }

                string filePath = _filePaths[i];

                if (File.Exists(filePath))
                {
                    string entryName = Path.GetFileName(filePath);
                    currentFileIndex++;

                    // Update progress
                    progressForm.UpdateProgress(entryName, currentFileIndex, totalFiles);

                    // Handle duplicate file names in archive
                    string originalName = Path.GetFileNameWithoutExtension(entryName);
                    string extension = Path.GetExtension(entryName);
                    int counter = 1;

                    while (usedEntryNames.Contains(entryName))
                    {
                        entryName = $"{originalName}_{counter}{extension}";
                        counter++;
                    }

                    usedEntryNames.Add(entryName);
                    builder.AddFile(entryName, filePath);
                }
            }

            // Add folders recursively
            foreach (string folderPath in _folderPaths)
            {
                if (progressForm.CancelRequested)
                {
                    return false;
                }

                string folderName = Path.GetFileName(folderPath);
                string[] files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    if (progressForm.CancelRequested)
                    {
                        return false;
                    }

                    currentFileIndex++;

                    // Get relative path within the folder
                    string relativePath = Path.GetRelativePath(folderPath, file);
                    string entryName = Path.Combine(folderName, relativePath);

                    // Update progress
                    progressForm.UpdateProgress(entryName, currentFileIndex, totalFiles);

                    // Normalize path separators for ISO (use backslashes)
                    entryName = entryName.Replace('/', '\\');

                    builder.AddFile(entryName, file);
                }
            }

            // Build and save the ISO
            builder.Build(isoPath);

            return true;
        }

        /// <summary>
        /// Updates the status text in textBox1 with file count and total size
        /// </summary>
        private void UpdateStatusText()
        {
            int totalItems = _filePaths.Count + _folderPaths.Count;
            
            if (totalItems > 0)
            {
                long totalSize = GetTotalFileSize();
                string itemText = totalItems == 1 ? "item" : "items";
                insertfile.Text = $"{totalItems} {itemText} selected - Total: {FormatFileSize(totalSize)}";
            }
            else
            {
                insertfile.Text = string.Empty;
            }
        }

        /// <summary>
        /// Sets up the context menu for the list box
        /// </summary>
        private void SetupContextMenu()
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            ToolStripMenuItem removeItem = new ToolStripMenuItem("Remove Selected");
            removeItem.Click += (s, e) => RemoveSelectedItem();
            
            ToolStripMenuItem addFolderItem = new ToolStripMenuItem("Add Folder");
            addFolderItem.Click += (s, e) => BrowseAndAddFolder();

            ToolStripMenuItem clearAllItem = new ToolStripMenuItem("Clear All");
            clearAllItem.Click += (s, e) => ClearAllItems();

            ToolStripMenuItem openFileItem = new ToolStripMenuItem("Open File Location");
            openFileItem.Click += (s, e) => OpenFileLocation();

            ToolStripMenuItem clearSearchItem = new ToolStripMenuItem("Clear Search");
            clearSearchItem.Click += (s, e) => ClearSearch();

            contextMenu.Items.Add(removeItem);
            contextMenu.Items.Add(openFileItem);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(addFolderItem);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(clearSearchItem);
            contextMenu.Items.Add(clearAllItem);

            list_of_item_selected.ContextMenuStrip = contextMenu;
        }

        /// <summary>
        /// Clears the search textbox and shows all files
        /// </summary>
        private void ClearSearch()
        {
            search.Clear();
        }

        /// <summary>
        /// Opens the file location in Windows Explorer
        /// </summary>
        private void OpenFileLocation()
        {
            if (list_of_item_selected.SelectedIndex >= 0)
            {
                int actualIndex = GetActualFileIndex(list_of_item_selected.SelectedIndex);
                if (actualIndex >= 0)
                {
                    string filePath = _filePaths[actualIndex];
                    if (File.Exists(filePath))
                    {
                        System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{filePath}\"");
                    }
                }
            }
        }

        /// <summary>
        /// Handles drag enter event for drag and drop functionality
        /// </summary>
        private void Home_DragEnter(object? sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// Handles the Home_DragDrop event for drag and drop functionality
        /// </summary>
        private async void Home_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                string[]? items = (string[]?)e.Data.GetData(DataFormats.FileDrop);
                if (items != null)
                {
                    // Separate files and folders
                    List<string> files = new List<string>();
                    List<string> folders = new List<string>();

                    foreach (string item in items)
                    {
                        if (Directory.Exists(item))
                        {
                            folders.Add(item);
                        }
                        else if (File.Exists(item))
                        {
                            files.Add(item);
                        }
                    }

                    // Add folders
                    foreach (string folder in folders)
                    {
                        AddFolder(folder);
                    }

                    // Add files
                    if (files.Count > 1)
                    {
                        await AddFilesWithProgressAsync(files.ToArray());
                    }
                    else if (files.Count == 1)
                    {
                        AddFile(files[0]);
                    }
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Show file details when an item is selected
            if (list_of_item_selected.SelectedIndex >= 0)
            {
                int actualIndex = GetActualFileIndex(list_of_item_selected.SelectedIndex);
                if (actualIndex >= 0)
                {
                    string filePath = _filePaths[actualIndex];
                    if (File.Exists(filePath))
                    {
                        var fileInfo = new FileInfo(filePath);
                        this.Text = $"HOME - {fileInfo.Name} ({FormatFileSize(fileInfo.Length)})";
                    }
                }
            }
            else
            {
                this.Text = "HOME";
            }
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
        /// Gets the total size of all files in the list
        /// </summary>
        public long GetTotalFileSize()
        {
            long totalSize = 0;
            
            // Add size of individual files
            foreach (string filePath in _filePaths)
            {
                if (File.Exists(filePath))
                {
                    totalSize += new FileInfo(filePath).Length;
                }
            }
            
            // Add size of files in folders
            foreach (string folderPath in _folderPaths)
            {
                if (Directory.Exists(folderPath))
                {
                    string[] files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
                    foreach (string file in files)
                    {
                        if (File.Exists(file))
                        {
                            totalSize += new FileInfo(file).Length;
                        }
                    }
                }
            }
            
            return totalSize;
        }

        /// <summary>
        /// Gets the list of all file paths
        /// </summary>
        public List<string> GetFilePaths()
        {
            return new List<string>(_filePaths);
        }

        /// <summary>
        /// Gets the count of files in the list
        /// </summary>
        public int GetFileCount()
        {
            return _filePaths.Count;
        }

        /// <summary>
        /// Gets the selected output folder path
        /// </summary>
        public string GetOutputFolderPath()
        {
            return _outputFolderPath;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void compress_btn_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void password_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the show password checkbox changed event - toggles password visibility
        /// </summary>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (showpass.Checked == true)
            {
                password.UseSystemPasswordChar = false;
            }
            else
            {
                password.UseSystemPasswordChar = true;
            }
        }

        private void zipname_TextChanged(object sender, EventArgs e)
        {

        }

        private void type_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
