using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetCoreExercise.Data;
using NetCoreExercise.Models;

namespace NetCoreExercise.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        protected readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Customer Types

        public async Task<IEnumerable<CustomerType>> GetAllTypes() => await _context.CustomerTypes.ToListAsync();

        public async Task<IEnumerable<SelectListItem>> GetAllTypesSelectList(int Id=0)
        {
            return await _context.CustomerTypes.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected= x.Id==Id
            }).ToListAsync();
        }

        public async Task<CustomerType> InsertType(CustomerType customerType)
        {
            _context.CustomerTypes.Add(customerType);
            await _context.SaveChangesAsync();

            return customerType;
        }

        #endregion

        public bool CustomerExists(int Id)
        {
            return (_context.Customers?.Any(e => e.Id == Id)).GetValueOrDefault();
        }

        public async Task Delete(int Id)
        {
            var customer = await _context.Customers.FindAsync(Id);

            if (customer == null)
            {
                throw new Exception("NotFound");
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Customer>> GetAll() => await _context.Customers.Include(x=>x.CustomerType).ToListAsync();


        public async Task<Customer> GetByID(int Id) => await _context.Customers.FindAsync(Id);

        public async Task<Customer> Insert(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        public async Task<Customer> Update(int Id, Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException dbException)
            {
                throw dbException;
            }
            return customer;
        }

    }
}
