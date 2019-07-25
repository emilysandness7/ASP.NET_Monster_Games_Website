using System;
using System.ComponentModel.DataAnnotations;

namespace MonsterGames.Models
{
  public class Leaderboard
  {
    public virtual int LeaderboardId { get; set; }

    public virtual string GameName { get; set; }

    [Range(0,1000000)]
    public virtual int Score { get; set; }

    [Display(Name="Player Email")]
    public virtual string PlayerName { get; set; }

    [Display(Name="Score Date")]
    public virtual DateTime ScoreDate { get; set; }
  }
}