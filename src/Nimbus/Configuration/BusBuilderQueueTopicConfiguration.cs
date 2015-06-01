using System;
using System.Linq;
using Nimbus.Configuration.Debug;
using Nimbus.Configuration.LargeMessages;
using Nimbus.Configuration.Settings;
using Nimbus.DependencyResolution;
using Nimbus.Extensions;
using Nimbus.Infrastructure.BrokeredMessageServices.Compression;
using Nimbus.Infrastructure.BrokeredMessageServices.Serialization;
using Nimbus.Infrastructure.Logging;
using Nimbus.Infrastructure.Routing;
using Nimbus.MessageContracts.Exceptions;
using Nimbus.Routing;

namespace Nimbus.Configuration
{
    public class BusBuilderQueueTopicConfiguration : INimbusConfiguration
	{
        internal DefaultMessageLockDurationSetting DefaultMessageLockDuration { get; set; }
        internal MaxDeliveryAttemptSetting MaxDeliveryAttempts { get; set; }
        internal AutoDeleteOnIdleSetting AutoDeleteOnIdle { get; set; }

		internal BusBuilderQueueTopicConfiguration()
        {
        }
    }
}