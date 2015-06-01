using System;
using System.IO;
using Nimbus.Configuration.Debug;
using Nimbus.Configuration.LargeMessages;
using Nimbus.Configuration.Settings;
using Nimbus.DependencyResolution;
using Nimbus.Routing;
using Nimbus.MessageContracts;

namespace Nimbus.Configuration
{
    public static class BusBuilderConfigurationExtensions
    {
        public static BusBuilderConfiguration WithConnectionString(this BusBuilderConfiguration configuration, string connectionString)
        {
            configuration.ConnectionString = new ConnectionStringSetting {Value = connectionString};
            return configuration;
        }

        public static BusBuilderConfiguration WithConnectionStringFromFile(this BusBuilderConfiguration configuration, string filename)
        {
            var connectionString = File.ReadAllText(filename).Trim();
            return configuration.WithConnectionString(connectionString);
        }

        /// <summary>
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="applicationName">
        ///     This should be unique for your application (e.g. Foo.exe) but should be the same across
        ///     all instances of your application.
        /// </param>
        /// <param name="instanceName">
        ///     This should be unique across ALL instances of your application. Use your hostname if you're
        ///     stuck.
        /// </param>
        /// <returns></returns>
        public static BusBuilderConfiguration WithNames(this BusBuilderConfiguration configuration, string applicationName, string instanceName)
        {
            configuration.ApplicationName = new ApplicationNameSetting {Value = applicationName};
            configuration.InstanceName = new InstanceNameSetting {Value = instanceName};
            return configuration;
        }

        public static BusBuilderConfiguration WithTypesFrom(this BusBuilderConfiguration configuration, ITypeProvider typeProvider)
        {
            configuration.TypeProvider = typeProvider;
            return configuration;
        }

        public static BusBuilderConfiguration WithDependencyResolver(this BusBuilderConfiguration configuration, IDependencyResolver dependencyResolver)
        {
            configuration.DependencyResolver = dependencyResolver;
            return configuration;
        }

        public static BusBuilderConfiguration WithLargeMessageStorage(this BusBuilderConfiguration configuration, Action<LargeMessageStorageConfiguration> configurationAction)
        {
            configurationAction(configuration.LargeMessageStorageConfiguration);
            return configuration;
        }

        public static BusBuilderConfiguration WithRouter(this BusBuilderConfiguration configuration, IRouter router)
        {
            configuration.Router = router;
            return configuration;
        }

        public static BusBuilderConfiguration WithServerConnectionCount(this BusBuilderConfiguration configuration, int serverConnectionCount)
        {
            configuration.ServerConnectionCount = new ServerConnectionCountSetting {Value = serverConnectionCount};
            return configuration;
        }

        public static BusBuilderConfiguration WithDefaultTimeout(this BusBuilderConfiguration configuration, TimeSpan defaultTimeout)
        {
            configuration.DefaultTimeout = new DefaultTimeoutSetting {Value = defaultTimeout};
            return configuration;
        }

        public static BusBuilderConfiguration WithDefaultMessageLockDuration(this BusBuilderConfiguration configuration, TimeSpan defaultLockDuration)
        {
            configuration.DefaultMessageLockDuration = new DefaultMessageLockDurationSetting {Value = defaultLockDuration};
            return configuration;
        }

        public static BusBuilderConfiguration WithDefaultConcurrentHandlerLimit(this BusBuilderConfiguration configuration, int defaultConcurrentHandlerLimit)
        {
            configuration.DefaultConcurrentHandlerLimit = new ConcurrentHandlerLimitSetting {Value = defaultConcurrentHandlerLimit};
            return configuration;
        }

        public static BusBuilderConfiguration WithMaximumThreadPoolThreads(this BusBuilderConfiguration configuration, int maximumThreadPoolThreads)
        {
            configuration.MaximumThreadPoolThreads = new MaximumThreadPoolThreadsSetting {Value = maximumThreadPoolThreads};
            return configuration;
        }

        public static BusBuilderConfiguration WithMinimumThreadPoolThreads(this BusBuilderConfiguration configuration, int minimumThreadPoolThreads)
        {
            configuration.MinimumThreadPoolThreads = new MinimumThreadPoolThreadsSetting {Value = minimumThreadPoolThreads};
            return configuration;
        }

        public static BusBuilderConfiguration WithMaxDeliveryAttempts(this BusBuilderConfiguration configuration, int maxDeliveryAttempts)
        {
            configuration.MaxDeliveryAttempts = new MaxDeliveryAttemptSetting {Value = maxDeliveryAttempts};
            return configuration;
        }

        public static BusBuilderConfiguration WithDefaultMessageTimeToLive(this BusBuilderConfiguration configuration, TimeSpan timeToLive)
        {
            configuration.DefaultMessageTimeToLive = new DefaultMessageTimeToLiveSetting { Value = timeToLive };
            return configuration;
        }

        public static BusBuilderConfiguration WithAutoDeleteOnIdle(this BusBuilderConfiguration configuration, TimeSpan autoDeleteOnIdle)
        {
            configuration.AutoDeleteOnIdle = new AutoDeleteOnIdleSetting { Value = autoDeleteOnIdle };
            return configuration;
        }

        public static BusBuilderConfiguration WithEnableDeadLetteringOnMessageExpiration(this BusBuilderConfiguration configuration, bool enableDeadLettering)
        {
            configuration.EnableDeadLetteringOnMessageExpiration = new EnableDeadLetteringOnMessageExpirationSetting { Value = enableDeadLettering };
            return configuration;
        }

        public static BusBuilderConfiguration WithHeartbeatInterval(this BusBuilderConfiguration configuration, TimeSpan heartbeatInterval)
        {
            configuration.HeartbeatInterval = new HeartbeatIntervalSetting { Value = heartbeatInterval };
            return configuration;
        }

        public static BusBuilderConfiguration WithLogger(this BusBuilderConfiguration configuration, ILogger logger)
        {
            configuration.Logger = logger;
            return configuration;
        }

        public static BusBuilderConfiguration WithGlobalInboundInterceptorTypes(this BusBuilderConfiguration configuration, params Type[] interceptorTypes)
        {
            configuration.GlobalInboundInterceptorTypes = new GlobalInboundInterceptorTypesSetting {Value = interceptorTypes};
            return configuration;
        }

        public static BusBuilderConfiguration WithGlobalOutboundInterceptorTypes(this BusBuilderConfiguration configuration, params Type[] interceptorTypes)
        {
            configuration.GlobalOutboundInterceptorTypes = new GlobalOutboundInterceptorTypesSetting {Value = interceptorTypes};
            return configuration;
        }

        public static BusBuilderConfiguration WithDebugOptions(this BusBuilderConfiguration configuration,
                                                               Func<BusBuilderDebuggingConfiguration, BusBuilderDebuggingConfiguration> debugConfiguration)
        {
            debugConfiguration(configuration.Debugging);
            return configuration;
        }

		class Test : IBusCommand
		{

		}

		private void test()
		{
			var bus = new BusBuilder().Configure()
				.WithNames("BusTester", Environment.MachineName)
				.WithConnectionString("")
				.WithDefaultTimeout(TimeSpan.FromSeconds(10))
				.WithSpecificQueueConfig<Test>((m) => new BusBuilderQueueTopicConfiguration()
					.WithMaxDeliveryAttempts(m, 100))
				//.WithLogger(new ConsoleLogger())
				.Build();
		}

		public static BusBuilderConfiguration WithSpecificQueueConfig<TMsg>(this BusBuilderConfiguration configuration,
																			Func<TMsg, BusBuilderQueueTopicConfiguration,
																			BusBuilderQueueTopicConfiguration> queueTopicConfigBuilder)
			where TMsg : Nimbus.MessageContracts.IBusCommand, Nimbus.MessageContracts.IBusEvent
		{
			Type t = typeof(TMsg);

			configuration.SpecificQueueTopicConfig.Value.Add(t, );

			return configuration;
		}


		public static BusBuilderQueueTopicConfiguration WithAutoDeleteOnIdle<TMsg>(this BusBuilderQueueTopicConfiguration configuration,
																				   TMsg msg, TimeSpan autoDeleteOnIdle)
			where TMsg : Nimbus.MessageContracts.IBusCommand, Nimbus.MessageContracts.IBusEvent
		{
			configuration.AutoDeleteOnIdle = new AutoDeleteOnIdleSetting { Value = autoDeleteOnIdle };
			return configuration;
		}

		public static BusBuilderQueueTopicConfiguration WithMaxDeliveryAttempts<TMsg>(this BusBuilderQueueTopicConfiguration configuration,
																				   TMsg msg, int maxDeliveryAttempts)
			where TMsg : Nimbus.MessageContracts.IBusCommand, Nimbus.MessageContracts.IBusEvent
		{
			configuration.MaxDeliveryAttempts = new MaxDeliveryAttemptSetting { Value = maxDeliveryAttempts };
			return configuration;
		}

		public static BusBuilderQueueTopicConfiguration WithDefaultMessageLockDuration<TMsg>(this BusBuilderQueueTopicConfiguration configuration,
																				   TMsg msg, TimeSpan defaultLockDuration)
			where TMsg : Nimbus.MessageContracts.IBusCommand, Nimbus.MessageContracts.IBusEvent
		{
			configuration.DefaultMessageLockDuration = new DefaultMessageLockDurationSetting { Value = defaultLockDuration };
			return configuration;
		}
    }
}