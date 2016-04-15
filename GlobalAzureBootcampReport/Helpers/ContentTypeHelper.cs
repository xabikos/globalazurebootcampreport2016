using System.Linq;

namespace GlobalAzureBootcampReport.Helpers {
	public class ContentTypeHelper {
		/// <summary>
		/// Helper method that returns the correct content type for the supplied <paramref name="filename"/>
		/// </summary>
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
