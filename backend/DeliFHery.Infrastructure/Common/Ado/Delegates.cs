using System.Data;

namespace DeliFHery.Infrastructure.Common.Ado;

public delegate T RowMapper<T>(IDataRecord row);
    