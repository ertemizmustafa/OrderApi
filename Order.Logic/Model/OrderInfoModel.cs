using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Order.Logic.Model
{
    public class OrderInfoModel
    {
        public int Id { get; }
        public int BrandId { get; set; }
        public decimal Price { get; set; }
        public string StoreName { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Status { get; set; }

        public static IEnumerable<string> GetMatchOrderByFields(string orderBy)
        {
            return !string.IsNullOrEmpty(orderBy) ? orderBy.Split(",").Where(x => TypeDescriptor.GetProperties(typeof(OrderInfoModel)).Find(x, false) != null).Select(x => TypeDescriptor.GetProperties(typeof(OrderInfoModel)).Find(x, false).Name) : new List<string>();
        }
    }
}
