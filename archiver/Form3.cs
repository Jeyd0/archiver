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
            button1.Click += button1_Click;
            button3.Click += button3_Click;
            button4.Click += button4_Click;

            // Set default extract folder name
            textBox3.Text = $"extracted_{DateTime.Now:yyyyMMdd}";
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
        private void button3_Click(object? sender, EventArgs e)
        {
            ExtractArchiveFile();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form f2 = new home();
            f2.Show();
            this.Hide();
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
                    textBox1.Text = Path.GetFileName(_archiveFilePath);
                    
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
        /// Loads the contents of the selected archive file into the list box
        /// </summary>
        private void LoadArchiveContents()
        {
            zip_items.Items.Clear();
            _archiveEntries.Clear();

            if (string.IsNullOrEmpty(_archiveFilePath) || !File.Exists(_archiveFilePath))
            {
                return;
            }

            try
            {
                string archiveType = GetArchiveTypeDescription(_archiveFilePath);

                // Try to open with SharpCompress
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
                            zip_items.Items.Add($"{entry.Key} ({sizeInfo})");
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
                this.Text = $"EXTRACT ({archiveType}) - {_archiveEntries.Count(e => !e.EndsWith("/") && !e.EndsWith("\\"))} file(s) in archive";
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
                textBox1.Text = string.Empty;
            }
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
                    textBox2.Text = _outputFolderPath;
                }
            }
        }

        /// <summary>
        /// Extracts the selected archive file to the output folder
        /// </summary>
        private void ExtractArchiveFile()
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
                textBox1.Text = string.Empty;
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
                textBox2.Text = string.Empty;
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

            // Show progress form and extract
            using (ExtractProgressForm progressForm = new ExtractProgressForm())
            {
                progressForm.Show(this);
                this.Enabled = false;

                try
                {
                    bool success = ExtractArchiveWithProgress(extractPath, progressForm);

                    if (success)
                    {
                        progressForm.SetComplete();
                        System.Threading.Thread.Sleep(500); // Brief pause to show completion
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
                catch (Exception ex)
                {
                    // Delete partial extraction on error
                    if (Directory.Exists(extractPath))
                    {
                        try { Directory.Delete(extractPath, true); } catch { }
                    }
                    MessageBox.Show($"Error extracting archive file: {ex.Message}",
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

                    // Process UI events to keep the form responsive
                    Application.DoEvents();
                }
            }

            return true;
        }

        /// <summary>
        /// Gets a sanitized folder name from textBox3 or generates a default one
        /// </summary>
        private string GetExtractFolderName()
        {
            string folderName = textBox3.Text.Trim();

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
    }
}
