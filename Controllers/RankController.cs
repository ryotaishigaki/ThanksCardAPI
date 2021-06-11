using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanksCardAPI.Models;

namespace ThanksCardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RankController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public RankController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rank>>> GetRanks()
        {
            // ThanksCard を受け取った回数をカウントしてリストで返す
            return await _context.ThanksCards
                .Include(ThanksCard => ThanksCard.To)
                .GroupBy(x => x.To.Name)
                .Select(t => new Rank { Name = t.Key, Count = t.Count() })
                .ToListAsync();
        }
        
        //new
        [HttpGet("From")]
        public async Task<ActionResult<IEnumerable<Rank>>> GetFromRanks()
        {
            // ThanksCard を送った回数をカウントしてリストで返す
            return await _context.ThanksCards
                .Include(ThanksCard => ThanksCard.From)
                .GroupBy(x => x.From.Name)
                .Select(t => new Rank { Name = t.Key, Count = t.Count() })
                .ToListAsync();
        }
    }
}