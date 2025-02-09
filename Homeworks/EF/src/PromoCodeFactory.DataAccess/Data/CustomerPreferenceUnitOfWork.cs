using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Repositories;

namespace PromoCodeFactory.DataAccess.Data
{
    public class CustomerPreferenceUnitOfWork : IUnitOfWork
    {
        private readonly IGenericRepository<CustomerPreference> _customerPreferenceRepository;
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly EfDbContext _context;

        public CustomerPreferenceUnitOfWork(EfDbContext context)
        {

            _context = context;
            _customerRepository = new GenericRepository<Customer>(context);
            _customerPreferenceRepository = new GenericRepository<CustomerPreference>(context);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        IGenericRepository<CustomerPreference> IUnitOfWork.CustomerPreferenceRepository()
        {
            return _customerPreferenceRepository;
        }

        IGenericRepository<Customer> IUnitOfWork.CustomerRepository()
        {
            return _customerRepository;
        }
    }
}
