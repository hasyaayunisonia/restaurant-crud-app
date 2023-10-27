using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNETCoreRestaurantApplication.Models;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCoreRestaurantApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionContext _dbContext;

        public TransactionController(TransactionContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("getAllTransactions")]
        public IActionResult GetAllTransactions()
        {
            try
            {
                var transactions = _dbContext.Transactions
                    .Include(t => t.Customer)  // Menyertakan properti navigasi Customer
                    .Include(t => t.Food)      // Menyertakan properti navigasi Food
                    .ToList();

                if (transactions.Count > 0)
                {
                    return Ok(transactions);
                }
                else
                {
                    return NotFound("No transactions found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("addTransaction")]
        public IActionResult AddTransaction([FromBody] Transaction newTransaction)
        {
            try
            {
                // Validasi input atau operasi lain yang diperlukan

                // Tambahkan transaksi baru ke konteks dan simpan perubahan ke database
                _dbContext.Transactions.Add(newTransaction);
                _dbContext.SaveChanges();

                // Kembalikan respons CreatedAtAction bersama dengan data yang baru
                return CreatedAtAction(nameof(GetTransactionById), new { id = newTransaction.transaction_id }, newTransaction);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("getTransactionById/{id}")]
        public IActionResult GetTransactionById(int id)
        {
            try
            {
                var transaction = _dbContext.Transactions
                    .Include(t => t.Customer)  // Menyertakan properti navigasi Customer
                    .Include(t => t.Food)      // Menyertakan properti navigasi Food
                    .FirstOrDefault(t => t.transaction_id == id);

                if (transaction == null)
                {
                    return NotFound("Transaction not found");
                }

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpDelete]
        [Route("deleteTransaction/{id}")]
        public IActionResult DeleteTransaction(int id)
        {
            try
            {
                var existingTransaction = _dbContext.Transactions.Find(id);

                if (existingTransaction == null)
                {
                    return NotFound("Transaction not found");
                }

                _dbContext.Transactions.Remove(existingTransaction);
                _dbContext.SaveChanges();

                return Ok("Transaction deleted successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("updateTransaction/{id}")]
        public IActionResult UpdateTransaction(int id, [FromBody] Transaction updatedTransaction)
        {
            try
            {
                var existingTransaction = _dbContext.Transactions.Find(id);

                if (existingTransaction == null)
                {
                    return NotFound("Transaction not found");
                }

                // Update properties of the existing transaction
                existingTransaction.customer_id = updatedTransaction.customer_id;
                existingTransaction.food_id = updatedTransaction.food_id;
                existingTransaction.amount = updatedTransaction.amount;
                existingTransaction.order_date = updatedTransaction.order_date;

                _dbContext.SaveChanges();

                // Return CreatedAtAction response along with the updated data
                return CreatedAtAction(nameof(GetTransactionById), new { id = existingTransaction.transaction_id }, existingTransaction);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
