namespace realtime_idea_space.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentPhoneNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IdeaModels", "CommentPhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.IdeaModels", "CommentPhoneNumber");
        }
    }
}
