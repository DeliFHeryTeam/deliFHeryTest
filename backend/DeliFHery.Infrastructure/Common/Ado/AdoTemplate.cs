using System.Data.Common;

namespace DeliFHery.Infrastructure.Common.Ado;

public class AdoTemplate(IConnectionFactory connectionFactory)
{
    private readonly IConnectionFactory _connectionFactory = connectionFactory;

    private void AddParameters(DbCommand command, params IEnumerable<QueryParameter> parameters)
    {
        foreach (var p in parameters)
        {
            DbParameter dbParam = command.CreateParameter(); 
            dbParam.ParameterName = p.Name;
            dbParam.Value = p.Value;
            command.Parameters.Add(dbParam);
        }
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, RowMapper<T> rowMapper, params IEnumerable<QueryParameter> parameters)
    {
        await using DbConnection connection = await connectionFactory.CreateConnectionAsync();

        await using DbCommand command = connection.CreateCommand(); // oder dbFactory.CreateCommand()
        command.CommandText = sql;
        AddParameters(command, parameters);

        await using DbDataReader reader = await command.ExecuteReaderAsync();

        IList<T> entities = new List<T>();
        while (await reader.ReadAsync())
        {
            entities.Add(rowMapper(reader));
        }
        return entities;
    }

    public async Task<T?> QueryByIdAsync<T>(string sql, RowMapper<T> rowMapper, params IEnumerable<QueryParameter> parameters)
    {
        IEnumerable<T> result = await QueryAsync(sql, rowMapper, parameters);
        return result.SingleOrDefault();
    }

    public async Task<int> ExecuteAsync (string sql, params IEnumerable<QueryParameter> parameters)
    {
        await using DbConnection connection = await connectionFactory.CreateConnectionAsync();

        await using DbCommand command = connection.CreateCommand(); // oder dbFactory.CreateCommand()
        command.CommandText = sql;
        AddParameters(command, parameters);

        return await command.ExecuteNonQueryAsync();
    }
}