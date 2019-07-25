using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MonsterGames.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MonsterGames.Controllers
{
  [Authorize(Roles = ApplicationConstants.Admin)]
  public class ManageLbController : Controller
  {
    private MonsterGamesDB db = new MonsterGamesDB();
    private ApplicationDbContext appDb = new ApplicationDbContext();

    // GET: ManageLb
    public ActionResult Index()
    {
      return View();
    }

    //GET: ManageLb/ManageAdmins
    public ActionResult ManageAdmins()
    {
      var users = appDb.Users.ToList();
      return View(users);
    }

    /*
     * METHODS FOR DELETING ALL SCORES FOR A SINGLE PLAYER
     */

    //GET: ManageLb/DelPlayerScores
    //get players for the dropdown list, only displays names that are on the leaderboard
    public ActionResult DelPlayerScores()
    {
      /*
       * The following code in this method is from:
       * http://forums.asp.net/t/1817879.aspx?how+to+bind+dropdown+in+mvc+razor
       * This code is used to get unique values in the dropdown list.
       */
      var temp = db.Leaderboards.Select(x => x.PlayerName).Distinct();
      List<SelectListItem> items = new List<SelectListItem>();

      foreach (var t in temp)
      {
        SelectListItem s = new SelectListItem();
        s.Text = t.ToString();
        s.Value = t.ToString();
        items.Add(s);
      }
      ViewBag.PlayerName = items;
      return View();
    }

    // POST: ManageLb/DelPlayerScoresSelect?playerName=name
    [HttpPost, ActionName("DelPlayerScoresSelect")]
    [ValidateAntiForgeryToken]
    public ActionResult DelPlayerScoresSelectConfirmed(string playerName)
    {
      var players = db.Leaderboards.Where(r => r.PlayerName.Equals(playerName));

      db.Leaderboards.RemoveRange(players);
      db.SaveChanges();

      return RedirectToAction("Index");
    }

    /*
     * METHODS FOR DELETING ALL SCORES FOR A SINGLE LEADERBOARD
     */

    //GET: ManageLb/DelLbScores
    public ActionResult DelLbScores()
    {
      /*
       * The following code in this method is from:
       * http://forums.asp.net/t/1817879.aspx?how+to+bind+dropdown+in+mvc+razor
       * This code is used to get unique values in the dropdown list.
       */
      var temp = db.Leaderboards.Select(x => x.GameName).Distinct();
      List<SelectListItem> items = new List<SelectListItem>();

      foreach (var t in temp)
      {
        SelectListItem s = new SelectListItem();
        s.Text = t.ToString();
        s.Value = t.ToString();
        items.Add(s);
      }
      ViewBag.GameName = items;
      return View();
    }

    // POST: ManageLb/DelLbScoresSelect?gameName=name
    [HttpPost, ActionName("DelLbScoresSelect")]
    [ValidateAntiForgeryToken]
    public ActionResult DelLbScoresSelectConfirmed(string gameName)
    {
      var games = db.Leaderboards.Where(r => r.GameName.Equals(gameName));

      db.Leaderboards.RemoveRange(games);
      db.SaveChanges();

      return RedirectToAction("Index");
    }

    /*
     * METHODS FOR DELETING ALL SCORES FOR ALL LEADERBOARDS
     */

    //GET: ManageLb/DelLbAllScores
    public ActionResult DelLbAllScores()
    {
      return View();
    }

    // POST: ManageLb/DelLbAllScoresSelect
    [HttpPost, ActionName("DelLbAllScoresSelect")]
    [ValidateAntiForgeryToken]
    public ActionResult DelLbAllScoresSelectConfirmed()
    {
      var games = db.Leaderboards.Where(r => r.GameName.Equals(ApplicationConstants.GAME_MWC));
      var games2 = db.Leaderboards.Where(r => r.GameName.Equals(ApplicationConstants.GAME_SP));

      db.Leaderboards.RemoveRange(games);
      db.Leaderboards.RemoveRange(games2);
      db.SaveChanges();

      return RedirectToAction("Index");
    }

    /*
     * METHODS FOR PROMOTING/DEMOTING ADMIN 
     */

    //GET: ManageLb/ManageAdmins/PromoteAdmin/playerName
    public ActionResult PromoteAdmin(string playerName)
    {
      var userManager = new UserManager<ApplicationUser>
        (new UserStore<ApplicationUser>(new ApplicationDbContext()));
      userManager.AddToRole(playerName, ApplicationConstants.Admin);

      return RedirectToAction("ManageAdmins");
    }

    //GET: ManageLb/ManageAdmins/RevokeAdmin/playerName
    public ActionResult RevokeAdmin(string playerName)
    {
      var userManager = new UserManager<ApplicationUser>
        (new UserStore<ApplicationUser>(new ApplicationDbContext()));
      userManager.RemoveFromRole(playerName, ApplicationConstants.Admin);

      return RedirectToAction("ManageAdmins");
    }

    //controller cleanup
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        db.Dispose(); 
        appDb.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
