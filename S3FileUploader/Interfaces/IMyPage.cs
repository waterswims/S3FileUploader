using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace S3FileUploader
{
	public interface IMyPage
	{
		public Task ExecuteForFirstNavigation();
	}
}
