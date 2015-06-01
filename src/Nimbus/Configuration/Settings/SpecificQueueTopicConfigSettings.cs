using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimbus.Configuration.Settings
{
	class SpecificQueueTopicConfigSettings : Setting<Dictionary<Type, BusBuilderQueueTopicConfiguration>>
	{
		BusBuilderQueueTopicConfiguration QueueTopicConfig { get; set; }

		public SpecificQueueTopicConfigSettings()
		{


			Value = new Dictionary<Type, BusBuilderQueueTopicConfiguration>();
		}
	}
}
