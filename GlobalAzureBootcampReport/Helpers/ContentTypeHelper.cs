using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalAzureBootcampReport.Helpers
{
	public class ContentTypeHelper
	{
		public static string GetContentType(string filename) {
			if (!filename.Contains(".")) {
				return "application/octet-stream";
			}
			var extension = filename.Split('.').Last();
			switch (extension) {
				case "jpg":
				case "jpeg":
					return "image/jpeg";
				case "png":
					return "image/png";
				case "gif":
					return "image/gif";
				default:
					return "application/octet-stream";
			}
		}
	}
}
