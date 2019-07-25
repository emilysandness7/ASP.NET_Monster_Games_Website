using MonsterGames.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MonsterGames.Controllers
{
  public class LeaderboardsController : Controller
  {
    private MonsterGamesDB db = new MonsterGamesDB();

    /*
     * LEADERBOARD VIEWS
     */

    //GET: Leaderboards/Index
    [AllowAnonymous]
    public ActionResult Index()
    {
      return View(db.Leaderboards.OrderByDescending(x => x.Score));
    }

    //GET: /Leaderboards/PlayerLb
    public ActionResult PlayerLb()
    {
      return View(db.Leaderboards.Where(x => x.PlayerName.Equals(User.Identity.Name))
                                 .OrderByDescending(x => x.Score));
    }

    // GET: LeaderBoards/MWCLeaderboard
    [AllowAnonymous]
    public ActionResult MWCLeaderboard()
    {
      return View(db.Leaderboards.Where(l => l.GameName.Equals(ApplicationConstants.GAME_MWC))
                                 .OrderByDescending(l => l.Score));
    }

    // GET: LeaderBoards/SPLeaderboard
    [AllowAnonymous]
    public ActionResult SPLeaderboard()
    {
      return View(db.Leaderboards.Where(l => l.GameName.Equals(ApplicationConstants.GAME_SP))
                                 .OrderByDescending(l => l.Score));
    }

    /*
     * DETAIL VIEWS
     */

    // GET: Leaderboards/MWCDetails/5
    [AllowAnonymous]
    public ActionResult MWCDetails(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Leaderboard leaderboard = db.Leaderboards.Find(id);
      if (leaderboard == null)
      {
        return HttpNotFound();
      }
      /*
       * The following code that is used to calculate a user's stats was originally from 
       * Kyle Aardal's project. During Lab 2 I was having trouble calculating the user
       * stats and was unable to finish the user stats. So after reviewing his project I 
       * adapted parts of his user stat calculator for my project. Credit goes to him.
       */
      List<int> MWCScores = new List<int>();
      UserStat stats = new UserStat();

      stats.PlayerName = leaderboard.PlayerName;
      stats.Scores = db.Leaderboards.Where(s => s.PlayerName.Equals(stats.PlayerName))
                                    .Where(s => s.GameName.Equals(ApplicationConstants.GAME_MWC)).ToList();

      foreach (Leaderboard score in stats.Scores)
      {
        MWCScores.Add(score.Score);
      }

      stats.MWCAverageScore = (int)MWCScores.Average();
      stats.MWCBestScore = MWCScores.Max();
      stats.MWCTimesPlayed = MWCScores.Count();
      stats.MWCWorstScore = MWCScores.Min();

      return View(stats);
    }

    // GET: Leaderboards/SPDetails/5
    [AllowAnonymous]
    public ActionResult SPDetails(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Leaderboard leaderboard = db.Leaderboards.Find(id);
      if (leaderboard == null)
      {
        return HttpNotFound();
      }
      /*
       * The following code that is used to calculate a user's stats was originally from 
       * Kyle Aardal's project. During Lab 2 I was having trouble calculating the user
       * stats and was unable to finish the user stats. So after reviewing his project I 
       * adapted parts of his user stat calculator for my project. Credit goes to him.
       */
      List<int> SPScores = new List<int>();
      UserStat stats = new UserStat();

      stats.PlayerName = leaderboard.PlayerName;
      stats.Scores = db.Leaderboards.Where(s => s.PlayerName.Equals(stats.PlayerName))
                                    .Where(s => s.GameName.Equals(ApplicationConstants.GAME_SP)).ToList();

      foreach (Leaderboard score in stats.Scores)
      {
        SPScores.Add(score.Score);
      }

      stats.SPAverageScore = (int)SPScores.Average();
      stats.SPBestScore = SPScores.Max();
      stats.SPTimesPlayed = SPScores.Count();
      stats.SPWorstScore = SPScores.Min();

      return View(stats);
    }

    /*
     * CREATE METHOD
     */

    // POST: Leaderboards/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    //allowanonymous is on so that guest will be redirected to the GuestScore page
    public ActionResult Create([Bind(Include = "GameName,Score,PlayerName")] Leaderboard leaderboard)
    {
      //if player is not logged do not save their score and redirect to GuestScore page
      if (leaderboard.PlayerName == null || User.Identity.Name.Length == 0)
      {
        return RedirectToAction("GuestScore", "Games");
      }

      if (ModelState.IsValid)
      {
        leaderboard.ScoreDate = DateTime.Now;

        db.Leaderboards.Add(leaderboard);
        db.SaveChanges();

        //determines which leaderboard to redirect to after create
        string redirectAction = "";
        if (leaderboard.GameName.Equals(ApplicationConstants.GAME_MWC))
        {
          redirectAction = ApplicationConstants.LB_MWC;
        }
        else
        {
          redirectAction = ApplicationConstants.LB_SP;
        }
        return RedirectToAction(redirectAction, "Leaderboards");
      }
      //sad path
      return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    }

    /*
     * DELETE METHODS
     */

    // GET: Leaderboards/Delete/5
    public ActionResult Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Leaderboard leaderboard = db.Leaderboards.Find(id);
      if (leaderboard == null)
      {
        return HttpNotFound();
      }
      if (!User.IsInRole(ApplicationConstants.Admin))
      {
        if (leaderboard.PlayerName == null || !leaderboard.PlayerName.Equals(User.Identity.Name))
        {
          return HttpNotFound();
        }
      }
      return View(leaderboard);
    }

    // POST: Leaderboards/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      Leaderboard leaderboard = db.Leaderboards.Find(id);

      if (!User.IsInRole(ApplicationConstants.Admin))
      {
        if (leaderboard.PlayerName == null || !leaderboard.PlayerName.Equals(User.Identity.Name))
        {
          return HttpNotFound();
        }
      }
      db.Leaderboards.Remove(leaderboard);
      db.SaveChanges();

      //determines which leaderboard to redirect to after create
      string redirectAction = "";
      if (leaderboard.GameName.Equals(ApplicationConstants.GAME_MWC))
      {
        redirectAction = ApplicationConstants.LB_MWC;
      }
      else
      {
        redirectAction = ApplicationConstants.LB_SP;
      }
      return RedirectToAction(redirectAction);
    }

    // CONTROLLER DISPOSER
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        db.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
