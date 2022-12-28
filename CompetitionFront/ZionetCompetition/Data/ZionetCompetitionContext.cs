using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZionetCompetition.Models;

namespace ZionetCompetition.Data
{
    public class ZionetCompetitionContext : DbContext
    {
        public ZionetCompetitionContext (DbContextOptions<ZionetCompetitionContext> options)
            : base(options)
        {
        }

        public DbSet<ZionetCompetition.Models.Users> Users { get; set; } = default!;
    }
}
