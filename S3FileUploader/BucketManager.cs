using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace S3FileUploader
{
	public class BucketManager
	{
		public ObservableCollection<S3Bucket> Buckets { get; set; }

		public BucketManager()
		{
			Buckets = new ObservableCollection<S3Bucket>();
		}

		public async Task GetBucketList(string awsAccessKeyId, string secretAccessKey)
		{
			Buckets.Clear();

			AmazonS3Client client = new AmazonS3Client(awsAccessKeyId, secretAccessKey, Amazon.RegionEndpoint.EUWest2);

			ListBucketsResponse response = await client.ListBucketsAsync();
			foreach (S3Bucket bucket in response.Buckets)
			{
				Buckets.Add(bucket);
			}
		}
	}
}
