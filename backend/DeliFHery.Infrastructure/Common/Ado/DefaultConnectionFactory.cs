using System.Data.Common;
using DeliFHery.Infrastructure.Common.Util;
using Microsoft.Extensions.Configuration;

namespace DeliFHery.Infrastructure.Common.Ado;

public class DefaultConnectionFactory : IConnectionFactory
{
    private readonly DbProviderFactory _dbProviderFactory;
    

    public static IConnectionFactory FromConfiguration(IConfiguration configuration, string connectionConfigName, string providerConfigName)
    {
        (string connectionString, string providerName) =
            ConfigurationUtil.GetConnectionParameters(configuration, connectionConfigName, providerConfigName);
        return new DefaultConnectionFactory(connectionString, providerName);
    }

    public DefaultConnectionFactory(string connectionString, string providerName)
    {
        this.ConnectionString = connectionString;
        this.ProviderName = providerName;

        DbUtil.RegisterAdoProviders();
        this._dbProviderFactory = DbProviderFactories.GetFactory(providerName);
    }

    public string ConnectionString { get; }

    public string ProviderName { get; }

    public async Task<DbConnection> CreateConnectionAsync()
    {
   
        DbConnection? connection = _dbProviderFactory.CreateConnection();
        if (connection is null) throw new Exception("Shit, no connection free");
        
        connection.ConnectionString = ConnectionString;  
        await connection.OpenAsync(); 
    
        return connection;
    }
}