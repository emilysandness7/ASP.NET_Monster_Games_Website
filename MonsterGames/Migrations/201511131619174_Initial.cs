namespace MonsterGames.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Leaderboards",
                c => new
                    {
                        LeaderboardId = c.Int(nullable: false, identity: true),
                        GameName = c.String(),
                        Score = c.Int(nullable: false),
                        PlayerName = c.String(),
                        ScoreDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LeaderboardId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Leaderboards");
        }
    }
}
