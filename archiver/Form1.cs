using System.IO;
using System.IO.Compression;

namespace archiver
{
    public partial class compress : Form
    {
        // Supported file extensions
        private readonly string[] _supportedExtensions = { ".jpg", ".jpeg", ".png", ".pdf", ".docx", ".xlsx", ".xls" };

        // List to store file paths
        private List<string> _filePaths = new List<string>();

        // Store the selected output folder path
        private string _outputFolderPath = string.Empty;

        public compress()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Wire up button click events
            button2.Click += Button2_Click;
            button3.Click += Button3_Click;

            // Wire up search textbox event
            textBox4.TextChanged += TextBox4_TextChanged;

            // Set up context menu for list box
            SetupContextMenu();

            // Allow drag and drop
            this.AllowDrop = true;
            this.DragEnter += Home_DragEnter;
            this.DragDrop += Home_DragDrop;

            // Set default zip name
            textBox3.Text = $"archive_{DateTime.Now:yyyyMMdd}";
        }

        /// <summary>
        /// Handles the Add button click - opens file dialog to browse files
        /// </summary>
        private void button1_Click_1(object sender, EventArgs e)
        {
            BrowseAndAddFiles();
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
        private void Button3_Click(object? sender, EventArgs e)
        {
            CompressFiles();
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
            string searchText = textBox4.Text.Trim().ToLower();

            // Clear the listbox
            list_of_item_selected.Items.Clear();

            if (string.IsNullOrEmpty(searchText))
            {
                // Show all files if search is empty
                foreach (string filePath in _filePaths)
                {
                    list_of_item_selected.Items.Add(Path.GetFileName(filePath));
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
                    textBox2.Text = _outputFolderPath;
                }
            }
        }

        /// <summary>
        /// Opens a file dialog to browse and add files
        /// </summary>
        private void BrowseAndAddFiles()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Supported Files|*.jpg;*.jpeg;*.png;*.pdf;*.docx;*.xlsx;*.xls|" +
                                        "Images|*.jpg;*.jpeg;*.png|" +
                                        "PDF Files|*.pdf|" +
                                        "Word Documents|*.docx|" +
                                        "Excel Files|*.xlsx;*.xls|" +
                                        "All Files|*.*";
                openFileDialog.Multiselect = true;
                openFileDialog.Title = "Select files to archive";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in openFileDialog.FileNames)
                    {
                        AddFile(file);
                    }
                }
            }
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

            // Check file extension
            string extension = Path.GetExtension(filePath).ToLower();
            if (!IsValidFileType(extension))
            {
                MessageBox.Show($"Unsupported file type: {extension}\n\nSupported types: jpg, png, pdf, docx, xlsx",
                    "Invalid File Type", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check for duplicates
            if (_filePaths.Contains(filePath))
            {
                MessageBox.Show("This file is already in the list.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Add to list
            _filePaths.Add(filePath);

            // Refresh the filtered list
            FilterFileList();

            UpdateStatusText();
        }

        /// <summary>
        /// Validates if the file extension is supported
        /// </summary>
        private bool IsValidFileType(string extension)
        {
            return _supportedExtensions.Contains(extension.ToLower());
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
            if (_filePaths.Count > 0)
            {
                var result = MessageBox.Show("Are you sure you want to clear all items?", "Confirm Clear",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _filePaths.Clear();
                    list_of_item_selected.Items.Clear();
                    textBox4.Clear(); // Clear search text
                    UpdateStatusText();
                }
            }
        }

        /// <summary>
        /// Gets a sanitized ZIP file name from textBox3 or generates a default one
        /// </summary>
        private string GetZipFileName()
        {
            string zipName = textBox3.Text.Trim();

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

            // Ensure it has .zip extension
            if (!zipName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                zipName += ".zip";
            }

            return zipName;
        }

        /// <summary>
        /// Compresses files to ZIP format
        /// </summary>
        private void CompressFiles()
        {
            // Validate there are files to compress
            if (_filePaths.Count == 0)
            {
                MessageBox.Show("Please add files to compress.", "No Files", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                textBox2.Text = string.Empty;
                return;
            }

            // Get the ZIP file name from textBox3
            string zipFileName = GetZipFileName();
            string zipPath = Path.Combine(_outputFolderPath, zipFileName);

            // Check if file already exists
            if (File.Exists(zipPath))
            {
                var result = MessageBox.Show($"A file named '{zipFileName}' already exists.\n\nDo you want to overwrite it?",
                    "File Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            // Show progress form and compress
            using (ProgressForm progressForm = new ProgressForm())
            {
                progressForm.Show(this);
                this.Enabled = false;

                try
                {
                    bool success = CreateZipArchiveWithProgress(zipPath, progressForm);

                    if (success)
                    {
                        progressForm.SetComplete();
                        System.Threading.Thread.Sleep(500); // Brief pause to show completion
                        MessageBox.Show($"ZIP archive created successfully!\n\nLocation: {zipPath}",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (progressForm.CancelRequested)
                    {
                        // Delete partial file if cancelled
                        if (File.Exists(zipPath))
                        {
                            File.Delete(zipPath);
                        }
                        MessageBox.Show("Compression was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    // Delete partial file on error
                    if (File.Exists(zipPath))
                    {
                        try { File.Delete(zipPath); } catch { }
                    }
                    MessageBox.Show($"Error creating ZIP archive: {ex.Message}",
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

            using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                int totalFiles = _filePaths.Count;

                for (int i = 0; i < totalFiles; i++)
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

                        // Update progress
                        progressForm.UpdateProgress(entryName, i + 1, totalFiles);

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
            }

            return true;
        }

        /// <summary>
        /// Updates the status text in textBox1 with file count and total size
        /// </summary>
        private void UpdateStatusText()
        {
            if (_filePaths.Count > 0)
            {
                long totalSize = GetTotalFileSize();
                textBox1.Text = $"{_filePaths.Count} file(s) selected - Total: {FormatFileSize(totalSize)}";
            }
            else
            {
                textBox1.Text = string.Empty;
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

            ToolStripMenuItem clearAllItem = new ToolStripMenuItem("Clear All");
            clearAllItem.Click += (s, e) => ClearAllItems();

            ToolStripMenuItem openFileItem = new ToolStripMenuItem("Open File Location");
            openFileItem.Click += (s, e) => OpenFileLocation();

            ToolStripMenuItem clearSearchItem = new ToolStripMenuItem("Clear Search");
            clearSearchItem.Click += (s, e) => ClearSearch();

            contextMenu.Items.Add(removeItem);
            contextMenu.Items.Add(openFileItem);
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
            textBox4.Clear();
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
        /// Handles drag drop event for drag and drop functionality
        /// </summary>
        private void Home_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                string[]? files = (string[]?)e.Data.GetData(DataFormats.FileDrop);
                if (files != null)
                {
                    foreach (string file in files)
                    {
                        AddFile(file);
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
            foreach (string filePath in _filePaths)
            {
                if (File.Exists(filePath))
                {
                    totalSize += new FileInfo(filePath).Length;
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
            Form f2 = new home();
            f2.Show();
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
