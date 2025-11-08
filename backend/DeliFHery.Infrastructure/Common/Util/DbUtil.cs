using System.Data.Common;

namespace DeliFHery.Infrastructure.Common.Util;

public class DbUtil
{
    
    public static void RegisterAdoProviders() {
        //DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", Microsoft.Data.SqlClient.SqlClientFactory.Instance);
         DbProviderFactories.RegisterFactory("MySqlConnector", MySqlConnector.MySqlConnectorFactory.Instance);
    }
}