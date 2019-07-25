using System.Web.Mvc;

namespace MonsterGames.Controllers
{
  public class HomeController : Controller
  {
    [AllowAnonymous]
    public ActionResult Index()
    {
      return View();
    }

    [AllowAnonymous]
    public ActionResult About()
    {
      return View();
    }

    [AllowAnonymous]
    public ActionResult Contact()
    {
      return View();
    }

    [AllowAnonymous]
    public ActionResult Changelog()
    {
      return View();
    }
  }
}