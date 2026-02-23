using Application.DTOs;
using Application.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SearchController : ControllerBase
    {
        private readonly ISearchRepository service;

        public SearchController(ISearchRepository searchRepository)
        {
            this.service = searchRepository;
        }

        [HttpGet]
        public async Task<IActionResult> SearchAsync([FromQuery] SearchParameterModel model)
        {
            var results = await service.SearchAsync(model);

            return Ok(results);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> SearchPagedAsync([FromQuery] SearchParameterModel model, int pageNum, int pageSize)
        {
            var results = await service.SearchPagedAsync(model, pageNum, pageSize);

            return Ok(results);
        }
    }
}
