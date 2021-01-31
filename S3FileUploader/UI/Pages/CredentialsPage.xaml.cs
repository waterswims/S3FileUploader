using System;
using System.Collections.Generic;
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
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;

namespace S3FileUploader
{
	/// <summary>
	/// Interaction logic for CredentialsPage.xaml
	/// </summary>
	public partial class CredentialsPage : Page, IRequiresVerification, IMyPage
	{
		public event EventHandler<Credentials> NewVerifiedCredentials;

		public CredentialsPage()
		{
			InitializeComponent();
		}

		public async Task<VerificationResult> VerifyContent()
		{
			VerificationResult verificationResult = new VerificationResult();

			Credentials credentials = new Credentials();
			credentials.SecretAccessKey = secretAccessKeyTextBox.Text;
			credentials.AccessKeyId = accessKeyIdTextBox.Text;

			AmazonIdentityManagementServiceClient client = new AmazonIdentityManagementServiceClient(credentials.AccessKeyId, credentials.SecretAccessKey, Amazon.RegionEndpoint.EUWest2);

			try
			{
				GetUserResponse response = await client.GetUserAsync();

				verificationResult.IsVerified = string.IsNullOrEmpty(response.User?.UserName);

				if (verificationResult.IsVerified)
					NewVerifiedCredentials?.Invoke(this, credentials);
				else
					verificationResult.ErrorMessage = "Failed to verify credentials";
			}
			catch
			{
				verificationResult.IsVerified = false;
				verificationResult.ErrorMessage = "Failed to verify credentials";
			}
			
			return verificationResult;
		}

		public async Task ExecuteForFirstNavigation()
		{
			// Do nothing
		}
	}
}
