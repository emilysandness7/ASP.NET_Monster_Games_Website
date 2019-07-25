using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MonsterGames.Models
{
  public class UserStat
  {
    public virtual int UserStatId { get; set; }
    public virtual string PlayerName { get; set; }

    //Monster Wants Candy stats
    [Display(Name="Times Played:")]
    public virtual int MWCTimesPlayed { get; set; }

    [Display(Name = "Highest Score:")]
    public virtual int MWCBestScore { get; set; }

    [Display(Name = "Lowest Score:")]
    public virtual int MWCWorstScore { get; set; }

    [Display(Name = "Average Score:")]
    public virtual int MWCAverageScore { get; set; }


    //Solo Pong stats
    [Display(Name = "Times Played:")]
    public virtual int SPTimesPlayed { get; set; }

    [Display(Name = "Highest Score:")]
    public virtual int SPBestScore { get; set; }

    [Display(Name = "Lowest Score:")]
    public virtual int SPWorstScore { get; set; }

    [Display(Name = "Average Score:")]
    public virtual int SPAverageScore { get; set; }

    public virtual List<Leaderboard> Scores { get; set; }
    public virtual Leaderboard Leaderboard { get; set; }
  }
}