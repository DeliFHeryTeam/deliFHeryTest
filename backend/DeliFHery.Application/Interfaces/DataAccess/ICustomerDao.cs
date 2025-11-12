using DeliFHery.Domain;

namespace DeliFHery.Application.Interfaces.DataAccess;

public interface ICustomerDao
{
    Task<IEnumerable<Customer>> FindAllAsync ();
    Task<Customer?> FindByIdAsync(int id);
    Task<bool> UpdateAsync(Customer customer);
}