#pragma warning disable CS8604

using ScaleHub.Core;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for registering ScaleHub services in the dependency injection container.
    /// </summary>
    public static class ServicesExtensions
    {
        /// <summary>
        /// Adds ScaleHub services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add ScaleHub services to.</param>
        /// <param name="options">An action to configure ScaleHub options.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddScaleHub(
              this IServiceCollection services
            , Action<ScaleHubConfiguration> options)
        {
            options(ScaleHubConfiguration.Default);
            return AddScaleHub(services);
        }

        /// <summary>
        /// Adds ScaleHub services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add ScaleHub services to.</param>
        /// <param name="options">An action to configure ScaleHub options, with access to the <see cref="IServiceProvider"/>.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddScaleHub(
              this IServiceCollection services
            , Action<ScaleHubConfiguration, IServiceProvider> options)
        {
            options(ScaleHubConfiguration.Default, services.BuildServiceProvider());
            return AddScaleHub(services);
        }

        private static IServiceCollection AddScaleHub(IServiceCollection services)
        {
            var scaleHub = Activator.CreateInstance(ScaleHubConfiguration.Default.ScaleHub);
            ScaleHubConfiguration.Default.Subscription(scaleHub as IChannel);
            services.AddSingleton(typeof(IScaleHub), scaleHub);
            services.AddHostedService<ScaleHubBackgroundService>();
            return services;
        }
    }
}
