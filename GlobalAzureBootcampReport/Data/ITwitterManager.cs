using System.Threading.Tasks;

namespace GlobalAzureBootcampReport.Data {

	/// <summary>
	/// Contract interface for managing connection to Twitter
	/// </summary>
	public interface ITwitterManager {
		/// <summary>
		/// Connects to the default stream
		Task Connect();

		/// <summary>
		/// Pauses the connection to the default stream
		/// </summary>
		void Pause();

		/// <summary>
		/// Resumes the connection to the default stream
		/// </summary>
		void Resume();
	}
}
