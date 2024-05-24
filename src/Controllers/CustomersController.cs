using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppliComptaApi.Models;
using AppliComptaApi.src.Models;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace AppliComptaApi.src.Controllers
{
    [Authorize]
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly AppliContext _context;

        public CustomersController(AppliContext context)
        {
            _context = context;
        }

     
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers()
        {
            var customers = await _context.Customers.ToListAsync();

            if (customers == null)
            {
                return NotFound();
            }

            var customerDTOs = customers.Select(customer => new CustomerDTO
            {
                Id = customer.Id,
                Number = customer.Number,
                Name = customer.Name,
                Address = customer.Address,
                PostalCode = customer.PostalCode,
                City = customer.City,
                TelephoneNumber = customer.TelephoneNumber,
                Email = customer.Email
            }).ToList();
                
            return customerDTOs;             
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomer(int id)
        {

            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            var customerDTO = new CustomerDTO
            {
                Id = customer.Id,
                Number = customer.Number,
                Name = customer.Name,
                Address = customer.Address,
                PostalCode = customer.PostalCode,
                City = customer.City,
                TelephoneNumber = customer.TelephoneNumber,
                Email = customer.Email
            };
            
            return customerDTO;
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerDTO customerDTO)
        {
            if (id != customerDTO.Id)
            {
                return BadRequest();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            customer.Number = customerDTO.Number;
            customer.Name = customerDTO.Name;
            customer.Address = customerDTO.Address;
            customer.PostalCode = customerDTO.PostalCode;
            customer.City = customerDTO.City;
            customer.TelephoneNumber = customerDTO.TelephoneNumber;
            customer.Email = customerDTO.Email;


            await _context.SaveChangesAsync();
            
            return NoContent();

        }


        
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(CustomerDTO customerDTO)
        {
            var customer = new Customer
            {
                Number = customerDTO.Number,
                Name = customerDTO.Name,
                Address = customerDTO.Address,
                PostalCode = customerDTO.PostalCode,
                City = customerDTO.City,
                TelephoneNumber = customerDTO.TelephoneNumber,
                Email = customerDTO.Email,
                
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCustomer),
                new { id = customer.Id },
                customer);
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        
    }
}
