using Microsoft.EntityFrameworkCore;
using OtoKMaui.Entity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtoKMaui.Data
{
    internal class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<UserTb1> UserTb1Entities { get; set; }
    }
}
