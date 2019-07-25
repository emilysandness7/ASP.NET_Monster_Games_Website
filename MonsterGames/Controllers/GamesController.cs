using System.Web.Mvc;

namespace MonsterGames.Controllers
{
    public class GamesController : Controller
    {
        // GET: Games/MonsterWantsCandy
      [AllowAnonymous]
        public ActionResult MonsterWantsCandy()
        {
          return View();
        }

        // GET: Games/SoloPong
      [AllowAnonymous]
        public ActionResult SoloPong()
        {
          return View();
        }

      //GET: Games/GuestScore
      [AllowAnonymous]
        public ActionResult GuestScore()
        {
          return View();
        }
    }
}