using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
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
	public partial class FileSelectorPage : Page
	{
		public ObservableCollection<FileInfo> Files { get; set; } = new ObservableCollection<FileInfo>();

		public FileSelectorPage()
		{
			InitializeComponent();

			this.DataContext = this;
		}

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
					Files.Add(file);
				}
			}
		}
	}
}
