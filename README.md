Image Text Extraction Windows Forms .Net8 - Eng Mage Ali
 

Overview
The project aims to extract text from images using Optical Character Recognition (OCR). It leverages the Tesseract library to process images and convert them into editable text. The user interface is designed with a modern look, featuring controls to select images, extract text, and display results.

Key Components

1. Namespaces and Libraries:
   - System.Drawing: Provides access to basic graphics functionality, used for image manipulation.
   - System.Windows.Forms: Allows you to create Windows-based applications that take full advantage of the rich user interface features available in the Windows operating system.
   - Tesseract: The library used for OCR. It processes the images and extracts text.
   - System.IO: Used for file and stream operations.

2. Form1 Class:
   This is the main form of your application, inheriting from `Form`.

#Constructor
- Form1(): Initializes the form, sets up its properties, and calls `InitializeModernUI()` to set up the user interface.

UI Initialization (`InitializeModernUI` Method)
This method sets up the modern UI layout using a `TableLayoutPanel`, organizing controls for user interaction.

1. Form Properties:
   - Title, size, background color, and font style for the form.

2. Layout Panel:
   - A `TableLayoutPanel` is used to create a flexible grid layout.
   - It is configured with several rows and columns to hold various controls.

3. Controls:
   - Labels: Instructions for the user and a loading label.
   - ComboBox: For selecting the language of the text in the image.
   - PictureBox: To display the selected image.
   - RichTextBox: To show the extracted text.
   - Buttons: 
     - "Select Image": Opens a file dialog for image selection.
     - "Extract Text": Initiates text extraction from the selected image.
     - "Copy Text": Copies extracted text to the clipboard.

Button Hover Effects
- The `ApplyButtonHoverEffect` method adds visual feedback when the user interacts with buttons. It changes the button color on hover and click.

Event Handlers

1. Image Selection (`btnSelectImage_Click`):
   - Opens a file dialog to allow the user to select an image file.
   - Loads the selected image into a `Bitmap` and displays it in the `PictureBox`.

2. Text Extraction (`btnExtractText_Click`):
   - Checks if an image is selected; if not, it shows an error message.
   - Displays a loading label while processing.
   - Uses Tesseract to extract text based on the selected language:
     - Creates a memory stream for the image.
     - Initializes the Tesseract engine with the appropriate language.
     - Processes the image and retrieves extracted text.
   - Displays the extracted text in the `RichTextBox`, with center alignment.

3. Copy to Clipboard (`button1_Click`):
   - Copies the text from the `RichTextBox` to the clipboard if there’s any text.

UI Layout
- The layout is carefully designed with controls spanning multiple columns to ensure a good visual structure.
- The design uses colors that match the application's theme and provides a pleasant user experience.

Resource Management
- Proper handling of resources is done when disposing of images to prevent memory leaks.
- `using` statements ensure that resources are automatically disposed of after use.

