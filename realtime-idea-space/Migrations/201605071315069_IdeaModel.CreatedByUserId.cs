namespace realtime_idea_space.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdeaModelCreatedByUserId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IdeaModels", "CreatedByUserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.IdeaModels", "CreatedByUserId");
        }
    }
}
