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

        public DbSet<prsCapstone.Model.User> Users { get; set; } = default!;
        public DbSet<prsCapstone.Model.Vendor> Vendors { get; set; } = default!;
        public DbSet<prsCapstone.Model.Request> Requests { get; set; } = default!;
        public DbSet<prsCapstone.Model.Product> Products { get; set; } = default!;
        public DbSet<prsCapstone.Model.RequestLine> RequestLines { get; set; } = default!;
    }
}
