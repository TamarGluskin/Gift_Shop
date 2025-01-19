using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using GiftShopManager.Models;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;

namespace GiftShopManager.Controllers
{

    [RoutePrefix("api/products")]
    public class ProductsController:ApiController
    {
        private readonly string _connectionString;
        public ProductsController()
        {
        
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        [HttpGet]
        [Route("getall")]
        public IHttpActionResult GetAllProducts()
        {
            var products = new List<ProductItem>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("GetAllProducts", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new ProductItem()
                        {
                            ProductCode = Convert.ToInt32(reader["ProductCode"]),
                            ProductName = reader["ProductName"].ToString(),
                            Description = reader["ProductDescription"].ToString(),
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            ImagePath = reader["ImagePath"].ToString(),

                        });
                    }
                }
            }

            return Ok(products);
        }


        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddProduct([FromBody] ProductItem product)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("AddProduct", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                if (!string.IsNullOrEmpty(product.Image))
                {
                    byte[] imageBytes = Convert.FromBase64String(product.Image);
                    var uniqueFileName = $"{Guid.NewGuid()}.jpg";
                    var baseDirectory = Path.Combine("C:\\Users\\user1\\source\\repos\\GiftShopManager\\GiftShopManager", "wwwroot", "images");
                    if (!Directory.Exists(baseDirectory))
                    {
                        Directory.CreateDirectory(baseDirectory);
                    }  
                    var filePath = Path.Combine(baseDirectory, uniqueFileName);
                    System.IO.File.WriteAllBytes(filePath, imageBytes);
                    product.ImagePath = $"wwwroot/images/{uniqueFileName}";
                }
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@ProductDescription", product.Description);
                command.Parameters.AddWithValue("@StartDate", product.StartDate);
                if (!string.IsNullOrEmpty(product.ImagePath))
                {
                    command.Parameters.AddWithValue("@ImagePath", product.ImagePath);
                }
                else
                {
                    command.Parameters.AddWithValue("@ImagePath", DBNull.Value); 
                }

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                    return Ok("Product added successfully.");
                else
                    return BadRequest("Failed to add product.");
            }
                
        }




        [HttpPut]
        [Route("update")]
        public IHttpActionResult UpdateProduct([FromBody] ProductItem product)
        {
            if (product == null)
                return BadRequest("Product data is missing.");

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UpdateProduct", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@ProductCode", product.ProductCode);
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@ProductDescription", product.Description);
                command.Parameters.AddWithValue("@StartDate", product.StartDate);
                if (!string.IsNullOrEmpty(product.Image))
                {
                    byte[] imageBytes = Convert.FromBase64String(product.Image);
                    var uniqueFileName = $"{Guid.NewGuid()}.jpg";
                    var baseDirectory = Path.Combine("C:\\Users\\user1\\source\\repos\\GiftShopManager\\GiftShopManager", "wwwroot", "images");
                    if (!Directory.Exists(baseDirectory))
                    {
                        Directory.CreateDirectory(baseDirectory);
                    }
                    var filePath = Path.Combine(baseDirectory, uniqueFileName);
                    System.IO.File.WriteAllBytes(filePath, imageBytes);
                    product.ImagePath = $"wwwroot/images/{uniqueFileName}";
                }
                if (!string.IsNullOrEmpty(product.ImagePath))
                {
                    command.Parameters.AddWithValue("@ImagePath", product.ImagePath);
                }
                else
                {
                    command.Parameters.AddWithValue("@ImagePath", DBNull.Value);
                }
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                    return Ok("Product updated successfully.");
                else
                    return NotFound();
            }
        }

        [HttpDelete]
        [Route("delete/{productCode}")]
        public IHttpActionResult DeleteProduct(int productCode)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DeleteProduct", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@ProductCode", productCode);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                    return Ok("Product deleted successfully.");
                else
                    return NotFound();
            }
        }


        [HttpGet]
        [Route("search")]
        public IHttpActionResult SearchProduct(string query)
        {
            var products = new List<ProductItem>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SearchProducts", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@SearchTerm", query);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new ProductItem()
                        {
                            ProductCode = Convert.ToInt32(reader["ProductCode"]),
                            ProductName = reader["ProductName"].ToString(),
                            Description = reader["ProductDescription"].ToString(),
                            StartDate = Convert.ToDateTime(reader["StartDate"])
                        });
                    }
                }
            }

            return Ok(products);
        }
    }

}
