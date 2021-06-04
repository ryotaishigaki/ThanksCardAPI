using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThanksCardAPI.Models;

namespace ThanksCardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThanksCardController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ThanksCardController(ApplicationContext context)
        {
            _context = context;
        }

        #region GetThanksCards
        // GET: api/ThanksCard
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThanksCard>>> GetThanksCards()
        {
            // Include を指定することで From, To (Userモデル) を同時に取得する。
            return await _context.ThanksCards
                                    .Include(ThanksCard => ThanksCard.From)
                                    .Include(ThanksCard => ThanksCard.To)
                                    .Include(ThanksCard => ThanksCard.ThanksCardTags)
                                        .ThenInclude(ThanksCardTag => ThanksCardTag.Tag)
                                    .ToListAsync();
        }
        #endregion

        #region GetNumberThanksCards
        // GET: api/ThanksCard/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ThanksCard>> GetNumberThanksCards(long id)
        {
            var card = await _context.ThanksCards.FindAsync(id);

            if (card == null)
            {
                return NotFound();
            }

            return card;
        }
        #endregion

        // POST api/ThanksCard
        [HttpPost]
        public async Task<ActionResult<ThanksCard>> Post([FromBody] ThanksCard thanksCard)
        {
            // From, To には既に存在しているユーザが入るため、更新の対象から外す。
            //_context.Users.Attach(thanksCard.From);
            //_context.Users.Attach(thanksCard.To);

            _context.ThanksCards.Add(thanksCard);
            await _context.SaveChangesAsync();
            // TODO: Error Handling
            return thanksCard;
        }

        #region DeleteThanksCards
        [HttpDelete("{id}")]
        public async Task<ActionResult<ThanksCard>> DeleteUser(long id)
        {
            var card = await _context.ThanksCards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            _context.ThanksCards.Remove(card);
            await _context.SaveChangesAsync();

            return card;
        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        #endregion
    }
}
