using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using prsCapstone.Model;

namespace prsCapstone.Data
{
    public class prsCapstoneContext : DbContext
    {
        public prsCapstoneContext (DbContextOptions<prsCapstoneContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Vendor> Vendors { get; set; } = default!;
        public DbSet<Request> Requests { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<RequestLine> RequestLines { get; set; } = default!;
    }
}
