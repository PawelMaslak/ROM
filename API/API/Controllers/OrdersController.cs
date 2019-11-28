using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using API.Models.Context;
using Remotion.Linq.Clauses;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetOrders()
        {
            var result = (from a in _context.Orders
                          join b in _context.Customers on a.CustomerId equals b.CustomerId
                          select new
                          {
                              a.OrderId,
                              a.OrderNo,
                              Customer = b.Name,
                              a.PaymentMethod,
                              a.Total
                          }).ToListAsync();

            //Query for returning OrderedItems List as well!
            //var properResult = _context.Orders
            //    .Join(_context.Customers, p => p.CustomerId, pc => pc.CustomerId, (p, pc) => new {p, pc})
            //    .Join(_context.OrderDetails, ppc => ppc.p.OrderId, c => c.OrderId, (ppc, c) => new {ppc, c})
            //    .Select(m => new
            //    {
            //        m.ppc.p.OrderId,
            //        Name = m.ppc.pc.Name,
            //        m.ppc.p.OrderItems
            //    }).ToListAsync();


            return await result;
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await (from a in _context.Orders
                               where a.OrderId == id

                               select new
                               {
                                   a.OrderId,
                                   a.OrderNo,
                                   a.CustomerId,
                                   a.PaymentMethod,
                                   a.Total,
                                   a.DeletedOrderItemsIds
                               }).FirstAsync();

            var items = await (from a in _context.OrderDetails
                               join b in _context.Items on a.ItemId equals b.ItemId
                               where a.OrderId == id

                               select new
                               {
                                   a.OrderDetailId,
                                   a.OrderId,
                                   a.ItemId,
                                   a.Quantity,
                                   ItemName = b.Name,
                                   b.Price,
                                   Total = a.Quantity * b.Price
                               }).ToListAsync();

            return Ok(new
            {
                Order = order,
                OrderItems = items
            });
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            try
            {
                //POST
                if (order.OrderId == 0)
                {
                    //Order Table Insert:
                    _context.Orders.Add(order);
                    //Order Detail Table Insert:

                    foreach (var orderItem in order.OrderItems)
                    {
                        _context.OrderDetails.Add(orderItem);
                    }
                }
                //PUT
                else
                {
                    _context.Entry(order).State = EntityState.Modified;

                    foreach (var orderItem in order.OrderItems)
                    {
                        if (orderItem.OrderDetailId == 0)
                        {
                            _context.OrderDetails.Add(orderItem);
                        }
                        else
                        {
                            _context.Entry(orderItem).State = EntityState.Modified;
                        }

                    }
                }

                if (order.DeletedOrderItemsIds != null)
                {
                    //DELETE OrderItems
                    foreach (var id in order.DeletedOrderItemsIds)
                    {
                        OrderDetail item = await _context.OrderDetails.FindAsync(id);

                        _context.OrderDetails.Remove(item);
                    }
                }


                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return order;
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
