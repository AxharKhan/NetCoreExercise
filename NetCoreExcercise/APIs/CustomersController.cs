using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreExercise.Models;
using NetCoreExercise.Repositories;

namespace NetCoreExercise.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customers;

        public CustomersController(ICustomerRepository customers)
        {
            _customers = customers;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return Ok(await _customers.GetAll());
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _customers.GetByID(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/UpdateCustomer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromForm] Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            try
            {
                customer.LastUpdated = DateTime.UtcNow;
                await _customers.Update(id, customer);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_customers.CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }

            return Ok(customer);
        }

        // POST: api/Customers/AddCustomer
        [HttpPost]
        public async Task<ActionResult<Customer>> AddCustomer([FromForm] Customer customer)
        {
            customer = await _customers.Insert(customer);

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/DeleteCustomer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                await _customers.Delete(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

    }
}
