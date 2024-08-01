using Microsoft.EntityFrameworkCore;

namespace WebApplication1;

public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseOracle("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=XE)));User Id=system;Password=superSecret12345;");
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
}