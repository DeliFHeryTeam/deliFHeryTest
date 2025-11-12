using System.Data.Common;

namespace DeliFHery.Infrastructure.Common.Ado;

public interface IConnectionFactory
{
    string ConnectionString { get; }
    string ProviderName { get; }
    Task<DbConnection> CreateConnectionAsync();
}