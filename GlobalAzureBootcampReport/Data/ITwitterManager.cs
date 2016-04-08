using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalAzureBootcampReport.Data
{
	public interface ITwitterManager
	{
		Task Connect();
		void Pause();
		void Resume();
	}
}
