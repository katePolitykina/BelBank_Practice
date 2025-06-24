
using DataTypesConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
namespace DataTypesConsoleApp.Services;


public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

    public DbSet<ColumnInfo> ColumnInfos { get; set; }
}