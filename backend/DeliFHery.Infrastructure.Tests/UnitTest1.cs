using DeliFHery.Application.Interfaces.DataAccess;
using DeliFHery.Domain;
using DeliFHery.Infrastructure.Common.Ado;
using DeliFHery.Infrastructure.Common.Util;
using DeliFHery.Infrastructure.DataAccess.Ado;
using Microsoft.Extensions.Configuration;

namespace DeliFHery.Infrastructure.Tests;

public class UnitTest1
{

    private IConnectionFactory _factory = null;

    private IConnectionFactory getFactory()
    {
        if (_factory is null)
        {
            _factory =  DefaultConnectionFactory.FromConfiguration( ConfigurationUtil.GetConfiguration(),
                "DefaultConnection", "DefaultProvider");
        }

        return _factory;
    }

    [Fact]
    public void ConfigurationUtil_FindsConfiguration()
    {
        IConfiguration configuration;
        configuration = ConfigurationUtil.GetConfiguration();
        Assert.NotNull(configuration);
    }

    [Fact]
    public void DefaultConnectionFactory_CreatesInstance()
    {
        IConnectionFactory factory;
        IConfiguration configuration;
        configuration = ConfigurationUtil.GetConfiguration();
        factory = DefaultConnectionFactory.FromConfiguration(configuration,
            "DefaultConnection", "DefaultProvider");
        Assert.NotNull(factory);
    }

    [Fact]
    public async Task CustomerDaoReturns_AllCustomers()
    {
        var factory = getFactory();
        ICustomerDao cDao = new AdoCustomerDao(factory);

        IEnumerable<Customer> customers = await cDao.FindAllAsync();

        Assert.NotNull(customers);
        Assert.NotEmpty(customers);
    }


}