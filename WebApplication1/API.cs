using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    [Route("api/[controller]")]
    [ApiController]
    public class API(BloggingContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<Blog> Get()
        {
            var blog = new Blog { Url = "http://blogs.msdn.com/adonet" };
            await context.Blogs.AddAsync(blog);
            await context.SaveChangesAsync();
            return blog;
        }
    }
}
