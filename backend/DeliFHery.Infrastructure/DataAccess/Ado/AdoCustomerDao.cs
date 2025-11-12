using System.Data;
using DeliFHery.Application.Interfaces.DataAccess;
using DeliFHery.Domain;
using DeliFHery.Infrastructure.Common.Ado;

namespace DeliFHery.Infrastructure.DataAccess.Ado;

public class AdoCustomerDao(IConnectionFactory connectionFactory) : ICustomerDao
{
    private readonly AdoTemplate _template = new AdoTemplate(connectionFactory);

    private Customer MapRowToCustomer(IDataRecord row)
    {
        return new Customer(
            id: (int)row["participant_id"],
            userName: (string)row["username"],
            authId: (int)row["authId"]
        );
    }

    public async Task<IEnumerable<Customer>> FindAllAsync()
    {
        return await _template.QueryAsync("select * from customer", MapRowToCustomer);
    }

    public async Task<Customer?> FindByIdAsync(int id)
    {
        return await _template.QueryByIdAsync("select * from customer where id=@id",
            MapRowToCustomer,
            new QueryParameter("participant_id", id));
    }

    public async Task<bool> UpdateAsync(Customer customer)
    {
        Task<int> result = _template.ExecuteAsync("update customer set username=@un, authId=@aid where id = @id ",
            new QueryParameter("un", customer.UserName),
            new QueryParameter("aid", customer.AuthId),
            new QueryParameter("id", customer.Id));
        return await result == 1;
    }
}