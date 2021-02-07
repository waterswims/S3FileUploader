using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace S3FileUploader
{
	/// <summary>
	/// Interaction logic for FileSelectorPage.xaml
	/// </summary>
	public partial class FileSelectorPage : Page, IRequiresBucket, IRequiresCredentials, IMyPage
	{
		public Credentials Credentials { get; set; }
		public string BucketName { get; set; }
		public ObservableCollection<MyFile> Files => uploadManager.Files;

		private UploadManager uploadManager = new UploadManager();

		public FileSelectorPage()
		{
			InitializeComponent();

			this.DataContext = this;

			this.uploadManager.FileUploadComplete += UploadManager_FileUploadComplete;
			this.uploadManager.UploadError += UploadManager_UploadError;
		}

		#region Upload Events

		private void UploadManager_UploadError(object sender, string e)
		{
			errorLabel.Content = "*" + e;
			EnableControls();
		}

		private void UploadManager_FileUploadComplete(object sender, MyFile e)
		{
			// Do Nothing
		}

		#endregion

		#region UI Events

		private void chooseFilesButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Multiselect = true;
			openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			if (openFileDialog.ShowDialog() == true)
			{
				Files.Clear();

				foreach (string fileName in openFileDialog.FileNames)
				{
					FileInfo file = new FileInfo(fileName);
					MyFile myFile = new MyFile(file);
					Files.Add(myFile);
				}
			}
		}

		private async void uploadFilesButton_Click(object sender, RoutedEventArgs e)
		{
			DisableControls();
			errorLabel.Content = "";

			await uploadManager.UploadFilesAsync(Credentials, sequentialCheckBox.IsChecked.Value, BucketName);
		}

		#endregion

		private void DisableControls()
		{
			chooseFilesButton.IsEnabled = false;
			sequentialCheckBox.IsEnabled = false;
			uploadFilesButton.IsEnabled = false;
		}

		private void EnableControls()
		{
			chooseFilesButton.IsEnabled = true;
			sequentialCheckBox.IsEnabled = true;
			uploadFilesButton.IsEnabled = true;
		}

		public async Task ExecuteForFirstNavigation()
		{
			// Do Nothing
		}
	}
}
