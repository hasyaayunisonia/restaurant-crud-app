using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ASPNETCoreRestaurantApplication.Models;
using Newtonsoft.Json;
using ASPNETCoreRestaurantApplication.Dto.Response;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCoreRestaurantApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly RestaurantContext _dbContext;

        public CustomerController(RestaurantContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("getAllCustomers")]
        public IActionResult GetAllCustomers()
        {
            try
            {
                // Ambil semua pelanggan dari database beserta informasi transaksi dan makanan terkait
                List<Customer> customers = _dbContext.Customers
                    .Include(c => c.Transactions)      // Menyertakan properti navigasi Transactions
                        .ThenInclude(t => t.Food)      // Menyertakan properti navigasi Food dalam setiap Transaction
                    .ToList();

                if (customers.Count > 0)
                {
                    return Ok(customers);
                }
                else
                {
                    return NotFound("No customers found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpPost]
        public async Task<ActionResult<Customer>> Post(Customer customer)
        {
            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.customer_id}, customer);
        }

        [HttpPost]
        [Route("addCustomer")]
        public IActionResult AddCustomer([FromBody] Customer newCustomer)
        {
            try
            {
                // Validasi input atau lakukan operasi lain yang diperlukan

                // Tambahkan pelanggan baru ke konteks dan simpan perubahan ke database
                _dbContext.Customers.Add(newCustomer);
                _dbContext.SaveChanges();

                // Setelah menyimpan, buat URI untuk pelanggan baru
                var uri = Url.Action("GetCustomerById", new { id = newCustomer.customer_id });

                // Return CreatedAtAction dengan URI lokasi baru
                return CreatedAtAction(nameof(GetCustomerById), new { id = newCustomer.customer_id }, newCustomer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("getCustomerById/{id}")]
        public IActionResult GetCustomerById(int id)
        {
            try
            {
                // Ambil pelanggan berdasarkan ID beserta informasi transaksi terkait
                var customer = _dbContext.Customers
                    .Include(c => c.Transactions)
                    .ThenInclude(t => t.Food)
                    .FirstOrDefault(c => c.customer_id == id);


                if (customer == null)
                {
                    return NotFound("Customer not found");
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpDelete]
        [Route("deleteCustomer/{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            try
            {
                // Cari pelanggan berdasarkan ID
                var customer = _dbContext.Customers.Find(id);

                if (customer == null)
                {
                    return NotFound("Customer not found");
                }

                // Hapus pelanggan dari konteks dan simpan perubahan ke database
                _dbContext.Customers.Remove(customer);
                _dbContext.SaveChanges();

                return Ok("Customer deleted successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("updateCustomer/{id}")]
        public IActionResult UpdateCustomer(int id, [FromBody] Customer updatedCustomer)
        {
            try
            {
                // Cari pelanggan berdasarkan ID
                var existingCustomer = _dbContext.Customers.Find(id);

                if (existingCustomer == null)
                {
                    return NotFound("Customer not found");
                }

                // Update propertinya dengan data yang baru
                existingCustomer.first_name = updatedCustomer.first_name;
                existingCustomer.last_name = updatedCustomer.last_name;
                existingCustomer.email = updatedCustomer.email;
                existingCustomer.phone_number = updatedCustomer.phone_number;

                // Simpan perubahan ke database
                _dbContext.SaveChanges();


                // Kembalikan respons CreatedAtAction bersama dengan data yang diupdate
                return CreatedAtAction(nameof(GetCustomerById), new { id = existingCustomer.customer_id}, existingCustomer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
