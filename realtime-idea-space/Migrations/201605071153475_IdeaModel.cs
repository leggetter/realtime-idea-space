namespace realtime_idea_space.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdeaModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IdeaModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50),
                        Created = c.DateTime(nullable: false),
                        Description = c.String(nullable: false, maxLength: 2000),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.IdeaModels");
        }
    }
}
