using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Data
{
    public interface IUnitOfWork
    {
        void SaveChanges();
        IGenericRepository<CustomerPreference> CustomerPreferenceRepository();
        IGenericRepository<Customer> CustomerRepository();

    }
}
