using System.Drawing.Imaging;
using System.Text;

namespace VotVImageConverter
{
	public partial class ConverterForm : Form
	{
		public ConverterForm()
		{
			InitializeComponent();
		}

		private void BtnSelectDir_Click(object sender, EventArgs e)
		{
			using var dialog = new FolderBrowserDialog();
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				txtSelectedDir.Text = dialog.SelectedPath;
			}
		}

		private void BtnConvert_Click(object sender, EventArgs e)
		{
			var selectedDirectory = txtSelectedDir.Text;

			// Validate if the directory path is empty or not
			if (!Directory.Exists(selectedDirectory))
			{
				MessageBox.Show("The selected directory is not valid. Please select a valid directory.");
				return;
			}

			// Path to check in Appdata\Local
			string appDataFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VotV", "Assets");

			// Check if the VotV\Assets directory exists
			if (!Directory.Exists(appDataFolderPath))
			{
				MessageBox.Show("The required directory structure was not found. Please ensure VotV is installed or that you are using the latest version of this program.");
				return;
			}

			// List of required subfolders
			string[] requiredSubfolders = [
				Path.Combine(appDataFolderPath, "flags"),
				Path.Combine(appDataFolderPath, "paintings", "h"),
				Path.Combine(appDataFolderPath, "paintings", "s"),
				Path.Combine(appDataFolderPath, "paintings", "t"),
				Path.Combine(appDataFolderPath, "paintings", "v"),
				Path.Combine(appDataFolderPath, "posters"),
				Path.Combine(appDataFolderPath, "rugs"),
				Path.Combine(appDataFolderPath, "stickers")
			];

			// Check for the existence of each required subfolder
			foreach (string subfolder in requiredSubfolders)
			{
				if (!Directory.Exists(subfolder))
				{
					MessageBox.Show($"The folder '{subfolder}' is missing. Please ensure that you are using the latest version of this program.");
					return;
				}
			}

			// If all checks pass
			MessageBox.Show("The directory structure is valid. Processing.");

			// Process each image file
			ProcessImages(selectedDirectory, requiredSubfolders);

			MessageBox.Show("Processing complete, a list of images that failed will be outputted to the selected directory.");
		}

		private void ProcessImages(string selectedDirectory, string[] subfolders)
		{
			string[] imageFiles = Directory.GetFiles(selectedDirectory, "*.*", SearchOption.TopDirectoryOnly)
										   .Where(file => file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
														  file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
														  file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
														  file.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
										   .ToArray();

			// Dictionary to keep track of failed images due to overwriting
			var imageStatus = new Dictionary<string, Dictionary<string, bool>>();

			// Process groups
			ProcessGroup(imageFiles, [subfolders[0], subfolders[2]], image => CropImage(image, 1, 1), imageStatus);
			ProcessGroup(imageFiles, [subfolders[1]], image => CropImage(image, 5, 4), imageStatus);
			ProcessGroup(imageFiles, [subfolders[3]], image => CropImage(image, 49, 64), imageStatus);
			ProcessGroup(imageFiles, [subfolders[4]], image => CropImage(image, 58, 93), imageStatus);
			ProcessGroup(imageFiles, [subfolders[5], subfolders[6], subfolders[7]], ExtendToSquare, imageStatus);

			// Generate the failure report
			GenerateFailureReport(selectedDirectory, subfolders, imageStatus);
		}

		private static void ProcessGroup(string[] imageFiles, string[] subfolders, Func<Image, Bitmap> cropFunc, Dictionary<string, Dictionary<string, bool>> imageStatus)
		{
			foreach (string imagePath in imageFiles)
			{
				string fileName = Path.GetFileName(imagePath);

				// Initialize the status dictionary for this image
				if (!imageStatus.ContainsKey(fileName))
				{
					imageStatus[fileName] = [];
				}
				try
				{
					using Image image = Image.FromFile(imagePath);

					// Determine the file name without extension
					string fileNamewithoutExtension = Path.GetFileNameWithoutExtension(imagePath);
					string extension = Path.GetExtension(imagePath).ToLower();

					// Apply cropping based on the folder
					using Bitmap croppedImage = cropFunc(image);

					string targetFileName = extension == ".png" ? fileNamewithoutExtension + ".png" : string.Concat(fileNamewithoutExtension, extension.AsSpan(1), ".png");
					foreach (string subfolder in subfolders)
					{
						// Construct the output file path
						string targetPath = Path.Combine(subfolder, targetFileName);

						// Check if the file already exists
						if (File.Exists(targetPath))
						{
							imageStatus[fileName][subfolder] = false; // failed
							continue; // Skip this file if it would overwrite an existing file
						} else
						{
							imageStatus[fileName][subfolder] = true; // succeeded
							croppedImage.Save(targetPath, ImageFormat.Png); // Save the cropped image as PNG
						}

					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Error processing image {imagePath}: {ex.Message}");
				}
			}
		}

		private static Bitmap CropImage(Image image, int targetWidthRatio, int targetHeightRatio)
		{
			int sourceWidth = image.Width;
			int sourceHeight = image.Height;

			float sourceAspect = (float)sourceWidth / sourceHeight;
			float targetAspect = (float)targetWidthRatio / targetHeightRatio;

			int cropWidth, cropHeight;

			if (sourceAspect > targetAspect)
			{
				// Source is wider than target, crop the width
				cropWidth = (int)(sourceHeight * targetAspect);
				cropHeight = sourceHeight;
			} else
			{
				// Source is taller than target, crop the height
				cropWidth = sourceWidth;
				cropHeight = (int)(sourceWidth / targetAspect);
			}

			int cropX = (sourceWidth - cropWidth) / 2;
			int cropY = (sourceHeight - cropHeight) / 2;

			Bitmap croppedImage = new(cropWidth, cropHeight);
			using (Graphics g = Graphics.FromImage(croppedImage))
			{
				g.DrawImage(image, new Rectangle(0, 0, cropWidth, cropHeight), cropX, cropY, cropWidth, cropHeight, GraphicsUnit.Pixel);
			}

			return croppedImage;
		}

		private static Bitmap ExtendToSquare(Image image)
		{
			int sourceWidth = image.Width;
			int sourceHeight = image.Height;
			int targetSize = Math.Max(sourceWidth, sourceHeight);

			Bitmap paddedImage = new(targetSize, targetSize);
			using (Graphics g = Graphics.FromImage(paddedImage))
			{
				g.Clear(Color.Transparent);
				g.DrawImage(image, new Rectangle((targetSize - sourceWidth) / 2, (targetSize - sourceHeight) / 2, sourceWidth, sourceHeight));
			}

			return paddedImage;
		}

		private static void GenerateFailureReport(string outputDirectory, string[] subfolders, Dictionary<string, Dictionary<string, bool>> imageStatus)
		{
			if (imageStatus.Count is 0 || imageStatus.Values.All(status => status.Values.All(v => v)))
				return;

			StringBuilder sb = new();

			// Find the longest filename for dynamic padding
			int longestFilenameLength = imageStatus.Keys.Max(name => name.Length);
			int paddingLength = longestFilenameLength + 5;

			// Add headers
			sb.Append("Image Name".PadRight(paddingLength));
			foreach (var subfolder in subfolders)
			{
				sb.Append(Path.GetFileName(subfolder).PadRight(10));
			}
			sb.AppendLine();

			// Add each image's status
			foreach (var image in imageStatus)
			{
				// Check if all statuses are true (passed)
				bool allPassed = subfolders.All(subfolder =>
					image.Value.TryGetValue(subfolder, out bool passed) && passed);

				if (allPassed)
					continue;

				sb.Append(image.Key.PadRight(paddingLength)); // Image name

				foreach (var subfolder in subfolders)
				{
					if (image.Value.TryGetValue(subfolder, out bool passed))
					{
						sb.Append((passed ? "" : "X").PadRight(10)); // "X" for failed, empty for passed
					} else
					{
						sb.Append("X".PadRight(10)); // If not processed, assume failure
					}
				}
				sb.AppendLine();
			}

			// Write to file
			string outputPath = Path.Combine(outputDirectory, "failed.txt");
			File.WriteAllText(outputPath, sb.ToString());
		}

	}
}
