using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Models.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly ApplicationDbContext _context;

        public RestaurantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region GeneralQueries

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        // Only return success if at least one row was changed
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        #endregion

        #region CustomersQueries

        public async Task<Customer[]> GetAllCustomersAsync()
        {
            IQueryable<Customer> query = _context.Customers;

            return await query.ToArrayAsync();
        }

        //GET Customers with SPROC
        public async Task<Customer[]> GetAllCustomersSprocAsync()
        {
            return await _context.Customers.FromSql("exec GET_CUSTOMERS").ToArrayAsync();
        }

        //GET Customer by ID with SPROC
        public async Task<Customer> GetCustomerByIdSprocAsync(int id)
        {
            var param = new SqlParameter("CustomerId", id);

            return await _context.Customers.FromSql("exec GET_CUSTOMERS_BY_ID @CustomerId", param).FirstAsync();
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            IQueryable<Customer> query = _context.Customers;

            query = query.Where(q => q.CustomerId == id);

            return await query.FirstOrDefaultAsync();
        }

        #endregion
    }
}
