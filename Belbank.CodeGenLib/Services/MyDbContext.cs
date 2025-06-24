
using Belbank.CodeGenLib.Models;
using Microsoft.EntityFrameworkCore;
namespace Belbank.CodeGenLib.Services;


public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

    public DbSet<ColumnInfo> ColumnInfos { get; set; }
}