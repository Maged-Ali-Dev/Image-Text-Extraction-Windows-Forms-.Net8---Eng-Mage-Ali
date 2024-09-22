using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Tesseract;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace ImageTextExtraction
{
    public partial class Form1 : Form
    {
        private Bitmap selectedImage;
        private Label marqueeLabel;
        private ComboBox languageComboBox;

        public Form1()
        {
            InitializeComponent();
            
            InitializeModernUI();
        }


        
        // Declare the loadingLabel globally (if not already declared)
        private Label loadingLabel;

        private void InitializeModernUI()
        {
            // Set form properties
            this.Text = "Image Text Extraction Windows Forms .Net8 - Eng Mage Ali";
            this.Size = new Size(800, 600);
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10);

            // Initialize layout panel
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 7, // Adjusted row count to accommodate loadingLabel
                Padding = new Padding(15),
                BackColor = Color.White
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Row for instruction label
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40)); // Row for picture box
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Row for buttons
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40)); // Row for rich text box
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Row for "Copy Text" button
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Row for language combo box
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Row for loading label

            // Create and configure the instruction label
            var instructionLabel = new Label
            {
                Text = "For best results, please choose an image with clear text.. Senior .Net Software Engineer Maged Ali",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12, FontStyle.Italic),
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Initialize the ComboBox (language selection)
            languageComboBox = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10),
                BackColor = Color.WhiteSmoke
            };
            languageComboBox.Items.AddRange(new string[] { "English", "Arabic", "French", "Spanish" });
            languageComboBox.SelectedIndex = 0;

            // PictureBox to display the selected image
            pictureBox1 = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.LightGray
            };

            // RichTextBox to display extracted text
            richTextBox1 = new RichTextBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Multiline = true,
                BackColor = Color.WhiteSmoke
            };

            // Button to select an image
            btnSelectImage = new Button
            {
                Text = "Select Image",
                Size = new Size(150, 50),
                Dock = DockStyle.Fill,
                BackColor = ColorTranslator.FromHtml("#624E88"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSelectImage.FlatAppearance.BorderSize = 0;
            btnSelectImage.Click += btnSelectImage_Click;
            ApplyButtonHoverEffect(btnSelectImage);

            // Button to extract text
            btnExtractText = new Button
            {
                Text = "Extract Text",
                Size = new Size(150, 50),
                Dock = DockStyle.Fill,
                BackColor = ColorTranslator.FromHtml("#624E88"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExtractText.FlatAppearance.BorderSize = 0;
            btnExtractText.Click += btnExtractText_Click;
            ApplyButtonHoverEffect(btnExtractText);

            // Button to copy text to clipboard
            button1 = new Button
            {
                Text = "Copy Text",
                Size = new Size(150, 50),
                Dock = DockStyle.Fill,
                BackColor = ColorTranslator.FromHtml("#624E88"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            button1.FlatAppearance.BorderSize = 0;
            button1.Click += button1_Click;
            ApplyButtonHoverEffect(button1);

            // Initialize the loading label
            loadingLabel = new Label
            {
                Text = "Loading...",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#624E88"),
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false // Initially hidden
            };

            // Add controls to the layout
            layout.Controls.Add(instructionLabel, 0, 0);
            layout.SetColumnSpan(instructionLabel, 2); // Span across 2 columns for the label

            layout.Controls.Add(pictureBox1, 0, 1);
            layout.SetColumnSpan(pictureBox1, 2); // Span across 2 columns for the picture box

            layout.Controls.Add(btnSelectImage, 0, 2);
            layout.Controls.Add(btnExtractText, 1, 2);

            layout.Controls.Add(richTextBox1, 0, 3);
            layout.SetColumnSpan(richTextBox1, 2); // Span across 2 columns for the rich text box

            layout.Controls.Add(button1, 0, 4);
            layout.SetColumnSpan(button1, 2); // Span across 2 columns to center the "Copy Text" button

            layout.Controls.Add(languageComboBox, 0, 5);
            layout.SetColumnSpan(languageComboBox, 2); // Span across 2 columns for the ComboBox

            layout.Controls.Add(loadingLabel, 0, 6); // Add the loadingLabel
            layout.SetColumnSpan(loadingLabel, 2); // Span across 2 columns for the loading label

            // Add the layout to the form
            this.Controls.Add(layout);
        }






        // Add hover effect to buttons
        private void ApplyButtonHoverEffect(Button button)
        {
            button.MouseEnter += (s, e) => button.BackColor = ColorTranslator.FromHtml("#564076");
            button.MouseLeave += (s, e) => button.BackColor = ColorTranslator.FromHtml("#624E88");
            button.MouseDown += (s, e) => button.BackColor = ColorTranslator.FromHtml("#463562");
            button.MouseUp += (s, e) => button.BackColor = ColorTranslator.FromHtml("#624E88");
        }

        private async void btnExtractText_Click(object sender, EventArgs e)
        {
            if (selectedImage == null)
            {
                MessageBox.Show("Please select an image first.", "Error");
                return;
            }
            loadingLabel.Visible = true;
            richTextBox1.Clear(); // Clear previous text

            // Get the selected language
         string selectedLanguage = languageComboBox.SelectedItem.ToString() switch
{
    "Arabic" => "ara",
    "French" => "fra",
    "Spanish" => "spa",
    _ => "eng" // Default to English
};


            try
            {
                await Task.Run(() =>
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        // Save selected image to memory stream
                        selectedImage.Save(memoryStream, ImageFormat.Png);
                        byte[] imageBytes = memoryStream.ToArray();

                        string tessDataPath = Path.Combine(Application.StartupPath, "tessdata");

                        // Initialize Tesseract engine with selected language
                        using (var ocrEngine = new TesseractEngine(tessDataPath, selectedLanguage, EngineMode.Default))
                        {
                            using (var pixImage = Pix.LoadFromMemory(imageBytes))
                            {
                                // Process the image to extract text
                                using (var page = ocrEngine.Process(pixImage))
                                {
                                    string extractedText = page.GetText();

                                    // Update UI on the main thread
                                    Invoke((MethodInvoker)delegate
                                    {
                                        if (string.IsNullOrWhiteSpace(extractedText))
                                        {
                                            MessageBox.Show("No text extracted", "Error");
                                        }
                                        else
                                        {
                                            richTextBox1.Text = extractedText;

                                            // Center align the text in the RichTextBox
                                            richTextBox1.SelectAll();
                                            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
                                            richTextBox1.DeselectAll();
                                        }
                                    });
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                // Hide loading label on the UI thread
                Invoke((MethodInvoker)delegate
                {
                    loadingLabel.Visible = false;
                });
            }
        }






        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                    openFileDialog.Title = "Select an Image File";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Dispose of the previous image if it exists
                        if (selectedImage != null)
                        {
                            selectedImage.Dispose(); // Dispose to free resources
                        }

                        string imagePath = openFileDialog.FileName;

                        // Load and clone the image to avoid cross-thread issues
                        using (var originalImage = new Bitmap(imagePath))
                        {
                            selectedImage = new Bitmap(originalImage); // Clone the image
                        }

                        pictureBox1.Image = new Bitmap(selectedImage); // Use a clone in the PictureBox
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(richTextBox1.Text))
            {
                Clipboard.SetText(richTextBox1.Text);
                MessageBox.Show("Text copied to clipboard!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("There is no text to copy.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Controls
        private PictureBox pictureBox1;
        private RichTextBox richTextBox1;
        private Button btnSelectImage;
        private Button btnExtractText;
        private Button button1;
    }
}
