using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly BucketManager bucketManager = new BucketManager();
		public ObservableCollection<S3Bucket> Buckets => bucketManager.Buckets;

		public MainWindow()
		{
			InitializeComponent();

			this.DataContext = this;
		}

		private async void refreshBucketsButton_Click(object sender, RoutedEventArgs e)
		{
			await bucketManager.GetBucketList(accessKeyIdTextBox.Text, secretAccessKeyTextBox.Text);
		}
	}
}
