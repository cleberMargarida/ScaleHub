#pragma warning disable CS8604

using ScaleHub.Core;
using ScaleHub.Core.Abstract;

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
            options(ScaleHubConfiguration.Singleton);
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
            options(ScaleHubConfiguration.Singleton, services.BuildServiceProvider());
            return AddScaleHub(services);
        }

        private static IServiceCollection AddScaleHub(IServiceCollection services)
        {
            services.AddSingleton<ISetup>(ScaleHubConfiguration.Singleton);
            services.AddSingleton(typeof(IScaleHub), ScaleHubConfiguration.Singleton.ScaleHub);
            services.AddHostedService<ScaleHubBackgroundService>();
            return services;
        }
    }
}
