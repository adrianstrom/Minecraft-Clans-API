using ClansGrpcService.Services;
using Database.Models;
using Database.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClansGrpcService
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClansController : ControllerBase
    {
        private IClanService _clanService;
        public ClansController(IClanService clanService) 
        {
            _clanService = clanService;
        }

        // GET: api/clans
        [HttpGet]
        public async Task<IEnumerable<Clan>> Get()
        {
            //return await _clanRepository.GetClans();
            return null;
        }

        // GET api/<ValuesController>/5
        [HttpGet("{name}")]
        public async Task<Clan?> Get(string name)
        {
            //return await _clanRepository.GetClan(name, true, true, true);
            return null;
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
