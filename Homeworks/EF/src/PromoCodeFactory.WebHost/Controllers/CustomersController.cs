using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CustomersController(IGenericRepository<Customer> repository, IUnitOfWork unitOfWork)
        {
            _customerRepository = repository;
            _unitOfWork = unitOfWork;   
        }

        /// <summary>
        /// Получить всех клиентов
        /// </summary>
        /// <returns>Список клиентов</returns>
        [HttpGet]
        public async Task<ActionResult<IList<CustomerShortResponse>>> GetCustomersAsync()
        {
            var customers = _customerRepository.Get()
                .Select(x => new CustomerShortResponse
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email
                })
                .ToList();
            return Ok(customers);
        }

        /// <summary>
        /// Получить клиента по Guid
        /// </summary>
        /// <param name="id">Guid клиента</param>
        /// <returns>Клиент</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var customer = _customerRepository
                .Get(x => x.Id == id)
                .Select(x => new CustomerResponse
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    CustomerPreferences = x.CustomerPreferences
                        .Select(cp => new CustomerPreferenceResponse 
                        { 
                            Id = cp.PreferenceId, 
                            Name = cp.Preference.Name 
                        })
                        .ToList(),

                    PromoCodes = x.PromoCodes
                        .Select(x => new PromoCodeShortResponse 
                        { 
                            Id = x.Id,
                            Code = x.Code,
                            ServiceInfo = x.ServiceInfo,
                            BeginDate = x.BeginDate.ToString(),
                            EndDate = x.EndDate.ToString(),
                            PartnerName = x.PartnerName,
                        })
                        .ToList()
                })
                .FirstOrDefault();
            return Ok(customer);
        }

        /// <summary>
        /// Создать нового клиента
        /// </summary>
        /// <param name="request">Модель для создания клиента</param>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var newCustomer = new Customer
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            };

            var preferences = request.PreferenceIds
                .Select(x => new CustomerPreference
                {
                    Customer = newCustomer,
                    PreferenceId = x
                })
                .ToList();

            _unitOfWork.CustomerRepository().Create(newCustomer);

            foreach (var preference in preferences)
            {
                _unitOfWork.CustomerPreferenceRepository().Create(preference);
            }

            _unitOfWork.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Редактировать существующего клиента
        /// </summary>
        /// <param name="id">Guid клиента</param>
        /// <param name="request">Модель для редактирования существующего клиента</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var customer = _unitOfWork.CustomerRepository()
                .Get(x => x.Id == id)
                .FirstOrDefault();

            if (customer == null) return NotFound();

            var updatedCustomer = new Customer
            {
                Id = customer.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            };

            _unitOfWork.CustomerRepository().Update(updatedCustomer);

            var existingCustomerPreferences = _unitOfWork.CustomerPreferenceRepository().Get(x => x.CustomerId == id);

            foreach (var preference in existingCustomerPreferences)
                _unitOfWork.CustomerPreferenceRepository().Remove(preference);

            var updatedCustomerPreferences = request.PreferenceIds.Select(x => new CustomerPreference
            {
                CustomerId = id,
                PreferenceId = x
            });

            foreach (var preference in updatedCustomerPreferences)
                _unitOfWork.CustomerPreferenceRepository().Create(preference);

            _unitOfWork.SaveChanges();
            return Ok();
        }


        /// <summary>
        /// Удалить клиента
        /// </summary>
        /// <param name="id">Guid клиента</param>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = _unitOfWork.CustomerRepository()
                .Get(x => x.Id == id)
                .FirstOrDefault();

            if (customer == null) return NotFound();

            var customerPreferences = customer.CustomerPreferences.ToList();

            _unitOfWork.CustomerRepository().Remove(customer);

            foreach (var preference in customerPreferences)
            {
                _unitOfWork.CustomerPreferenceRepository().Remove(preference);
            }
            _unitOfWork.SaveChanges();
            return Ok();
        }
    }
}