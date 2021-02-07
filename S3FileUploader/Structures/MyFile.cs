using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace S3FileUploader
{
	public class MyFile: INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private readonly FileInfo fileInfo;

		public string Name => fileInfo.Name;
		public long Length => fileInfo.Length;
		public string FullName => fileInfo.FullName;

		private CompletionStatus status = CompletionStatus.NotStarted;
		public CompletionStatus Status
		{
			get
			{
				return status;
			}
			set
			{
				status = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Status)));
			}
		}

		public enum CompletionStatus
		{
			NotStarted,
			Started,
			Complete,
			Error
		}

		public MyFile(FileInfo fileInfo)
		{
			this.fileInfo = fileInfo;
		}
	}
}
