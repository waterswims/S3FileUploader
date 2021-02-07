using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace S3FileUploader
{
	public class UploadManager
	{
		public ObservableCollection<MyFile> Files { get; set; }

		public event EventHandler<MyFile> FileUploadComplete;
		public event EventHandler<string> UploadError;

		public UploadManager()
		{
			Files = new ObservableCollection<MyFile>();
		}

		public async Task UploadFilesAsync(Credentials credentials, bool uploadFilesSequentially, string bucketName)
		{
			

			if (uploadFilesSequentially)
			{
				foreach (MyFile file in Files)
				{
					AmazonS3Client client = new AmazonS3Client(credentials.AccessKeyId, credentials.SecretAccessKey, Amazon.RegionEndpoint.EUWest2);

					TransferUtility transferUtility = new TransferUtility(client);

					Task<Tuple<string, MyFile>> task = UploadFile(transferUtility, file, bucketName);
					await task;

					if (!ProcessResult(task))
					{
						break;
					}
				}
			}
			else
			{
				AmazonS3Client client = new AmazonS3Client(credentials.AccessKeyId, credentials.SecretAccessKey, Amazon.RegionEndpoint.EUWest2);

				TransferUtility transferUtility = new TransferUtility(client);

				Task[] uploadTasks = new Task[Files.Count];

				int i = 0;

				foreach (MyFile file in Files)
				{
					uploadTasks[i] = UploadFile(transferUtility, file, bucketName);
					i++;
				}

				while (uploadTasks.Length != 0)
				{
					int completedTask = Task.WaitAny(uploadTasks);

					Task<Tuple<string, MyFile>> finishedTask = uploadTasks[completedTask] as Task<Tuple<string, MyFile>>;

					if (!ProcessResult(finishedTask))
					{
						break;
					}

					uploadTasks = uploadTasks.Except(new Task[] { uploadTasks[completedTask] }).ToArray();
				}
			}
		}

		private async Task<Tuple<string, MyFile>> UploadFile(TransferUtility transferUtility, MyFile file, string bucketName)
		{
			string errorString = "";

			try
			{
				file.Status = MyFile.CompletionStatus.Started;
				await transferUtility.UploadAsync(file.FullName, bucketName);
			}
			catch (Exception e)
			{
				errorString = "Error encountered on server";
			}

			return new Tuple<string, MyFile>(errorString, file);
		}

		private bool ProcessResult(Task<Tuple<string, MyFile>> task)
		{
			bool success = false;

			Tuple<string, MyFile> result = task.Result;

			if (string.IsNullOrEmpty(result.Item1))
			{
				result.Item2.Status = MyFile.CompletionStatus.Complete;
				FileUploadComplete?.Invoke(this, result.Item2);
				success = true;
			}
			else
			{
				result.Item2.Status = MyFile.CompletionStatus.Error;
				UploadError?.Invoke(this, result.Item1);
			}

			return success;
		}
	}
}
