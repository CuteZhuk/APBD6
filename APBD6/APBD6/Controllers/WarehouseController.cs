// namespace APBD6.Controllers
//
// using Microsoft.AspNetCore.Mvc;
// using System;
// using System.Data.SqlClient;
//     
//     [ApiController]
//     [Route("api/[controller]")]
//     public class WarehouseController : ControllerBase;
//     {
//         private readonly string _connectionString = "Your_Connection_String_Here";
//
//         [HttpPost]
//         public IActionResult AddProductToWarehouse([FromBody] ProductWarehouseRequest request)
//         {
//             if (request == null || request.IdProduct <= 0 || request.IdWarehouse <= 0 || request.Amount <= 0)
//             {
//                 return BadRequest("Invalid request data");
//             }
//
//             using (var connection = new SqlConnection(_connectionString))
//             {
//                 connection.Open();
//                 using (var transaction = connection.BeginTransaction())
//                 {
//                     try
//                     {
//                         // Step 1: Check if product exists
//                         var productExists = CheckIfProductExists(request.IdProduct, connection, transaction);
//                         if (!productExists)
//                         {
//                             return NotFound("Product not found");
//                         }
//
//                         // Step 2: Check if warehouse exists
//                         var warehouseExists = CheckIfWarehouseExists(request.IdWarehouse, connection, transaction);
//                         if (!warehouseExists)
//                         {
//                             return NotFound("Warehouse not found");
//                         }
//
//                         // Step 3: Check if there is a matching order
//                         var orderId = CheckIfOrderExists(request.IdProduct, request.Amount, request.CreatedAt, connection, transaction);
//                         if (orderId == null)
//                         {
//                             return BadRequest("No matching order found");
//                         }
//
//                         // Step 4: Check if order is already fulfilled
//                         var orderFulfilled = CheckIfOrderFulfilled(orderId.Value, connection, transaction);
//                         if (orderFulfilled)
//                         {
//                             return BadRequest("Order is already fulfilled");
//                         }
//
//                         // Step 5: Update FullfilledAt in Order table
//                         UpdateOrderFulfilledAt(orderId.Value, connection, transaction);
//
//                         // Step 6: Insert into Product_Warehouse table and return the new record ID
//                         var newProductWarehouseId = InsertProductWarehouseRecord(request, orderId.Value, connection, transaction);
//
//                         transaction.Commit();
//
//                         return Ok(new { ProductWarehouseId = newProductWarehouseId });
//                     }
//                     catch (Exception ex)
//                     {
//                         transaction.Rollback();
//                         return StatusCode(500, $"Internal server error: {ex.Message}");
//                     }
//                 }
//             }
//         }
//
//         private bool CheckIfProductExists(int productId, SqlConnection connection, SqlTransaction transaction)
//         {
//             var command = new SqlCommand("SELECT COUNT(1) FROM Product WHERE IdProduct = @IdProduct", connection, transaction);
//             command.Parameters.AddWithValue("@IdProduct", productId);
//             return (int)command.ExecuteScalar() > 0;
//         }
//
//         private bool CheckIfWarehouseExists(int warehouseId, SqlConnection connection, SqlTransaction transaction)
//         {
//             var command = new SqlCommand("SELECT COUNT(1) FROM Warehouse WHERE IdWarehouse = @IdWarehouse", connection, transaction);
//             command.Parameters.AddWithValue("@IdWarehouse", warehouseId);
//             return (int)command.ExecuteScalar() > 0;
//         }
//
//         private int? CheckIfOrderExists(int productId, int amount, DateTime createdAt, SqlConnection connection, SqlTransaction transaction)
//         {
//             var command = new SqlCommand("SELECT IdOrder FROM [Order] WHERE IdProduct = @IdProduct AND Amount = @Amount AND CreatedAt < @CreatedAt", connection, transaction);
//             command.Parameters.AddWithValue("@IdProduct", productId);
//             command.Parameters.AddWithValue("@Amount", amount);
//             command.Parameters.AddWithValue("@CreatedAt", createdAt);
//             var result = command.ExecuteScalar();
//             return result != DBNull.Value ? (int?)result : null;
//         }
//
//         private bool CheckIfOrderFulfilled(int orderId, SqlConnection connection, SqlTransaction transaction)
//         {
//             var command = new SqlCommand("SELECT COUNT(1) FROM Product_Warehouse WHERE IdOrder = @IdOrder", connection, transaction);
//             command.Parameters.AddWithValue("@IdOrder", orderId);
//             return (int)command.ExecuteScalar() > 0;
//         }
//
//         private void UpdateOrderFulfilledAt(int orderId, SqlConnection connection, SqlTransaction transaction)
//         {
//             var command = new SqlCommand("UPDATE [Order] SET FulfilledAt = @FulfilledAt WHERE IdOrder = @IdOrder", connection, transaction);
//             command.Parameters.AddWithValue("@FulfilledAt", DateTime.Now);
//             command.Parameters.AddWithValue("@IdOrder", orderId);
//             command.ExecuteNonQuery();
//         }
//
//         private int InsertProductWarehouseRecord(ProductWarehouseRequest request, int orderId, SqlConnection connection, SqlTransaction transaction)
//         {
//             var command = new SqlCommand(
//                 "INSERT INTO Product_Warehouse (IdProduct, IdWarehouse, IdOrder, Amount, Price, CreatedAt) " +
//                 "VALUES (@IdProduct, @IdWarehouse, @IdOrder, @Amount, (SELECT Price FROM Product WHERE IdProduct = @IdProduct) * @Amount, @CreatedAt);" +
//                 "SELECT CAST(scope_identity() AS int);", connection, transaction);
//             command.Parameters.AddWithValue("@IdProduct", request.IdProduct);
//             command.Parameters.AddWithValue("@IdWarehouse", request.IdWarehouse);
//             command.Parameters.AddWithValue("@IdOrder", orderId);
//             command.Parameters.AddWithValue("@Amount", request.Amount);
//             command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
//
//             return (int)command.ExecuteScalar();
//         }
//     }
