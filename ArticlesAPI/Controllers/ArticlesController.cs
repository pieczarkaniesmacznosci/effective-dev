using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArticlesAPI.Controllers
{
    class Result
    {
        static int i =0;

        public int Value
        {
            get 
            {
                i++;
                return i; 
            }
        }

    }

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        static bool isOk = true;
        public ArticlesController()
        {
            result = new Result();
        }
        private Result result;

        [HttpGet]
        public ActionResult Index()
        {
            isOk = !isOk;
            return Ok();
        }

        // GET: ArticlesController/Create
        [HttpGet]
        public ActionResult<int> Create()
        {
            if (isOk)
            {
                return Created("article", result.Value);
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
