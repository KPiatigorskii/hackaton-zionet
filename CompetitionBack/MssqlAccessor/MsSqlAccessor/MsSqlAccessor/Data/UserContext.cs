using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MsSqlAccessor.Models;

namespace MsSqlAccessor.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options): base(options) { }

		public DbSet<User> user { get; set; }

	}
}
