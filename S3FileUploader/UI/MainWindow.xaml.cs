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
		private List<Page> pages = new List<Page>();
		private int pageNumber = 0;
	
		public MainWindow()
		{
			InitializeComponent();

			this.DataContext = this;

			CredentialsPage credentialsPage = new CredentialsPage();
			credentialsPage.NewVerifiedCredentials += CredentialsPage_NewVerifiedCredentials;

			BucketSelectorPage bucketSelectorPage = new BucketSelectorPage();
			bucketSelectorPage.NewBucketSelected += BucketSelectorPage_NewBucketSelected;

			FileSelectorPage fileSelectorPage = new FileSelectorPage();

			pages.Add(credentialsPage);
			pages.Add(bucketSelectorPage);
			pages.Add(fileSelectorPage);

			this.mainFrame.Navigate(pages[0]);

			SetupUiForPageNumber();
		}

		#region Page Navigation

		private async void previousButton_Click(object sender, RoutedEventArgs e)
		{
			errorLabel.Content = "";
			pageNumber--;
			await PageSetup();
		}

		private async void nextButton_Click(object sender, RoutedEventArgs e)
		{
			errorLabel.Content = "";

			IRequiresVerification verificationPage = pages[pageNumber] as IRequiresVerification;
			if (verificationPage != null)
			{
				VerificationResult verificationResult = await verificationPage.VerifyContent();
				if (verificationResult.IsVerified)
				{
					await NavigateToNextPage();
				}
				else
				{
					errorLabel.Content = "*" + verificationResult.ErrorMessage;
				}
			}
			else
			{
				await NavigateToNextPage();
			}
		}

		private async Task NavigateToNextPage()
		{
			pageNumber++;
			await PageSetup();
		}

		private async Task PageSetup()
		{
			IMyPage myPage = pages[pageNumber] as IMyPage;
			if (myPage != null)
				await myPage.ExecuteForFirstNavigation();

			this.mainFrame.Navigate(pages[pageNumber]);
			SetupUiForPageNumber();
		}

		private void SetupUiForPageNumber()
		{
			previousButton.IsEnabled = pageNumber != 0;
			nextButton.IsEnabled = pageNumber != pages.Count - 1;
		}

		#endregion

		#region Selections Made Events

		private void CredentialsPage_NewVerifiedCredentials(object sender, Credentials e)
		{
			foreach (Page page in pages)
			{
				IRequiresCredentials requiresCredentials = page as IRequiresCredentials;
				if (requiresCredentials != null)
					requiresCredentials.Credentials = e;
			}
		}

		private void BucketSelectorPage_NewBucketSelected(object sender, Amazon.S3.Model.S3Bucket e)
		{
			foreach (Page page in pages)
			{
				IRequiresBucket requiresBucket = page as IRequiresBucket;
				if (requiresBucket != null)
					requiresBucket.BucketName = e.BucketName;
			}
		}

		#endregion
	}
}
