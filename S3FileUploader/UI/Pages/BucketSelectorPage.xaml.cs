using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	/// Interaction logic for BucketSelectorPage.xaml
	/// </summary>
	public partial class BucketSelectorPage : Page, IRequiresCredentials, IMyPage, IRequiresVerification
	{
		public Credentials Credentials { get; set; }

		private readonly BucketManager bucketManager = new BucketManager();
		public ObservableCollection<S3Bucket> Buckets => bucketManager.Buckets;

		public event EventHandler<S3Bucket> NewBucketSelected;

		public BucketSelectorPage()
		{
			InitializeComponent();
			
			this.DataContext = this;
		}

		public async Task<VerificationResult> VerifyContent()
		{
			VerificationResult verificationResult = new VerificationResult();

			S3Bucket selectedBucket = bucketDataGrid.SelectedItem as S3Bucket;

			verificationResult.IsVerified = selectedBucket != null;
			if (verificationResult.IsVerified)
				NewBucketSelected?.Invoke(this, selectedBucket);
			else
				verificationResult.ErrorMessage = "No bucket selected";

			return verificationResult;
		}

		public async Task ExecuteForFirstNavigation()
		{
			await RefreshBuckets();
		}

		private async void refreshBucketsButton_Click(object sender, RoutedEventArgs e)
		{
			await RefreshBuckets();
		}

		private async Task RefreshBuckets()
		{
			await bucketManager.GetBucketList(Credentials);
		}
	}
}
