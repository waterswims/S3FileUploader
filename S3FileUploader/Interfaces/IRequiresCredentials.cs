using System;
using System.Collections.Generic;
using System.Text;

namespace S3FileUploader
{
	public interface IRequiresCredentials
	{
		public Credentials Credentials { set; }
	}
}
