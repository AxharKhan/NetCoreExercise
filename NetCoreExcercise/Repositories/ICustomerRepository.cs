using Microsoft.AspNetCore.Mvc.Rendering;
using NetCoreExercise.Data;
using NetCoreExercise.Models;

namespace NetCoreExercise.Repositories
{
    public interface ICustomerRepository
    {
        public Task<IEnumerable<Customer>> GetAll();

        public Task<Customer> GetByID(int Id);

        public Task<Customer> Insert(Customer customer);

        public Task<Customer> Update(int Id, Customer customer);

        public Task Delete(int Id);

        public bool CustomerExists(int Id);

        #region Customer Type

        public Task<IEnumerable<SelectListItem>> GetAllTypesSelectList(int Id=0);
        public Task<IEnumerable<CustomerType>> GetAllTypes();
        public Task<CustomerType> InsertType(CustomerType customerType);

        #endregion
    }
}