namespace realtime_idea_space.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdeaCommentIdeaModelId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.IdeaComments", "IdeaModel_Id", "dbo.IdeaModels");
            DropIndex("dbo.IdeaComments", new[] { "IdeaModel_Id" });
            RenameColumn(table: "dbo.IdeaComments", name: "IdeaModel_Id", newName: "IdeaModelId");
            AlterColumn("dbo.IdeaComments", "IdeaModelId", c => c.Guid(nullable: false));
            CreateIndex("dbo.IdeaComments", "IdeaModelId");
            AddForeignKey("dbo.IdeaComments", "IdeaModelId", "dbo.IdeaModels", "Id", cascadeDelete: true);
            DropColumn("dbo.IdeaComments", "CommentForIdeaId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IdeaComments", "CommentForIdeaId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.IdeaComments", "IdeaModelId", "dbo.IdeaModels");
            DropIndex("dbo.IdeaComments", new[] { "IdeaModelId" });
            AlterColumn("dbo.IdeaComments", "IdeaModelId", c => c.Guid());
            RenameColumn(table: "dbo.IdeaComments", name: "IdeaModelId", newName: "IdeaModel_Id");
            CreateIndex("dbo.IdeaComments", "IdeaModel_Id");
            AddForeignKey("dbo.IdeaComments", "IdeaModel_Id", "dbo.IdeaModels", "Id");
        }
    }
}
