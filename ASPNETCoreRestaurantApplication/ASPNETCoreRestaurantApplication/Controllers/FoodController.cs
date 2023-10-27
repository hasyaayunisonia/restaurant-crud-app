using ASPNETCoreRestaurantApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASPNETCoreRestaurantApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly FoodContext _dbContext;

        public FoodController(FoodContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("getAllFoods")]
        public IActionResult GetAllFoods()
        {
            try
            {
                // Ambil semua makanan dari database beserta informasi transaksi dan pelanggan terkait
                List<Food> foods = _dbContext.Foods
                    .Include(f => f.Transactions)          // Menyertakan properti navigasi Transactions
                        .ThenInclude(t => t.Customer)      // Menyertakan properti navigasi Customer dalam setiap Transaction
                    .ToList();

                if (foods.Count > 0)
                {
                    return Ok(foods);
                }
                else
                {
                    return NotFound("No foods found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("getFoodById/{id}")]
        public IActionResult GetFoodById(int id)
        {
            try
            {
                // Ambil makanan berdasarkan ID beserta informasi transaksi dan pelanggan terkait
                var food = _dbContext.Foods
                    .Include(f => f.Transactions)          // Menyertakan properti navigasi Transactions
                        .ThenInclude(t => t.Customer)      // Menyertakan properti navigasi Customer dalam setiap Transaction
                    .FirstOrDefault(f => f.food_id == id);

                if (food == null)
                {
                    return NotFound("Food not found");
                }

                return Ok(food);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost]
        [Route("addFood")]
        public IActionResult AddFood([FromBody] Food newFood)
        {
            try
            {
                _dbContext.Foods.Add(newFood);
                _dbContext.SaveChanges();

                return CreatedAtAction(nameof(GetFoodById), new { id = newFood.food_id }, newFood);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("updateFood/{id}")]
        public IActionResult UpdateFood(int id, [FromBody] Food updatedFood)
        {
            try
            {
                var existingFood = _dbContext.Foods.Find(id);

                if (existingFood == null)
                {
                    return NotFound("Food not found");
                }

                existingFood.name = updatedFood.name;
                existingFood.description = updatedFood.description;
                existingFood.price = updatedFood.price;
                existingFood.category = updatedFood.category;

                _dbContext.SaveChanges();

                return CreatedAtAction(nameof(GetFoodById), new { id = existingFood.food_id }, existingFood);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("deleteFood/{id}")]
        public IActionResult DeleteFood(int id)
        {
            try
            {
                var existingFood = _dbContext.Foods.Find(id);

                if (existingFood == null)
                {
                    return NotFound("Food not found");
                }

                _dbContext.Foods.Remove(existingFood);
                _dbContext.SaveChanges();

                return Ok("Food deleted successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
