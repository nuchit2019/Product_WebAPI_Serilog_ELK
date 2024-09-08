using Dapper;
using ProductWebAPISerilog.Models;
using System.Data;

namespace ProductWebAPISerilog.Repository
{
    public class ProductRepository
    {
        private readonly IDbConnection _dbConnection;

        public ProductRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            const string sql = "SELECT * FROM Products";
            return await _dbConnection.QueryAsync<Product>(sql);
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            const string sql = "SELECT * FROM Products WHERE Id = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<Product>(sql, new { Id = id });
        }

        public async Task AddProductAsync(Product product)
        {
            const string sql = "INSERT INTO Products (Name, Price) VALUES (@Name, @Price)";
            await _dbConnection.ExecuteAsync(sql, product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            const string sql = "UPDATE Products SET Name = @Name, Price = @Price WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, product);
        }

        public async Task DeleteProductAsync(int id)
        {
            const string sql = "DELETE FROM Products WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
