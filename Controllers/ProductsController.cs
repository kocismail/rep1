using System;
using System.Threading.Tasks;
using DeepHist.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DeepHist.Controllers
{
    [Route("api/products")]
    //  [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult> GetValues()
        {
            var values = await _productService.GetList();
            return Ok(values);
        }

        // GET api/values/5
        [HttpGet("{categoryId}")]
        public async Task<ActionResult> GetByCategory(int categoryId)
        {
            var values = await _productService.GetByCategory(categoryId);
            return Ok(values);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
