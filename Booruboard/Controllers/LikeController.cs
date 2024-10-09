using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Booruboard.Models;

namespace Booruboard.Controllers
{
    [Authorize]
    public class LikeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public LikeController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> LikePost(int postId)
        {
            var userId = _userManager.GetUserId(User);

            var existingLike = _context.Likes.FirstOrDefault(l => l.UserId == userId && l.PostId == postId);

            if (existingLike == null)
            {
                var like = new Like
                {
                    UserId = userId,
                    PostId = postId
                };
                _context.Likes.Add(like);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
