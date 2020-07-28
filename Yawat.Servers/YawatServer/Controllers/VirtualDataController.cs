namespace YawatServer.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using VirtualData;

    [ApiController]
    [Route("vdata")]
    public class VirtualDataController : ControllerBase
    {
        [HttpGet]
        public GeneratorResult Get([FromQuery] int first, [FromQuery] int take, [FromQuery] string name, [FromQuery] string group, [FromQuery] string order, [FromQuery] string dir)
        {
            var generator = new Generator(1000000, 500);
            if (!string.IsNullOrWhiteSpace(order))
            {
                generator.Order = order;
            }

            if (!string.IsNullOrWhiteSpace(dir))
            {
                generator.Dir = dir;
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                generator.NameStartsWith(name);
            }

            if (!string.IsNullOrWhiteSpace(group))
            {
                generator.GroupStartsWith(group);
            }

            return generator.GetAsObject(first, take);
        }
    }
}
