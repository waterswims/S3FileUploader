using System;
using System.Collections.Generic;
using System.Text;

namespace S3FileUploader
{
	public interface IRequiresBucket
	{
		public string BucketName { get; set; }
	}
}
