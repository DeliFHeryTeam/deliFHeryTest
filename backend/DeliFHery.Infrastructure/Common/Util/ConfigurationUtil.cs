using DotNetEnv;
using Microsoft.Extensions.Configuration; /* in order to retrieve db relevant variables from .env */

namespace DeliFHery.Infrastructure.Common.Util;

public static class ConfigurationUtil
{
    private static IConfiguration? _configuration = null;
    /*Lazy initialization */
    public static IConfiguration GetConfiguration() {
        if (_configuration is null)
        {
            Env.Load("../../../../../.env");  
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();
            
            var dbServer = Environment.GetEnvironmentVariable("DB_SERVER") ?? throw new DirectoryNotFoundException("unable to find DB_SERVER-Variable in .env"); 
            var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? throw new DirectoryNotFoundException("unable to find DB_PORT-Variable in .env");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? throw new DirectoryNotFoundException("unable to find DB_NAME-Variable in .env");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? throw new DirectoryNotFoundException("unable to find DB_USER-Variable in .env");
            var dbPassword = Environment.GetEnvironmentVariable("DB_USER_PW") ?? throw new DirectoryNotFoundException("unable to find DB_USER_PW-Variable in .env");
            var connectionString = $"Server={dbServer};Port={dbPort};Database={dbName};User Id={dbUser};Password={dbPassword};SslMode=None;";
            var configRoot = (IConfigurationRoot)_configuration;
            configRoot.GetSection("ConnectionStrings")["DefaultConnection"] = connectionString;
        }

        return _configuration;
    }


    public static (string ConnectionString, string ProviderName) GetConnectionParameters(string connectionConfigName, string providerConfigName)
    {
        return GetConnectionParameters(GetConfiguration(), connectionConfigName, providerConfigName);
    }

    public static (string ConnectionString, string ProviderName) GetConnectionParameters(IConfiguration configuration, string connectionConfigName, string providerConfigName)
    {
        var connectionString = configuration.GetConnectionString(connectionConfigName);
        if (connectionString is null)
        {
            throw new ArgumentException($"Connection string with key '{connectionConfigName}' does not exist");
        }


        var providerName = configuration[providerConfigName];
        if (providerName is null)
        {
            throw new ArgumentException($"Configuration property '{providerConfigName}' does not exist");
        }

        return (connectionString, providerName);
    }
}