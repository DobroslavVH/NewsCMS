namespace NewsCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostImage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostImage",
                c => new
                    {
                        PostImageID = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 255),
                        ContentType = c.String(maxLength: 100),
                        Content = c.Binary(),
                        FileType = c.Int(nullable: false),
                        PostID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PostImageID)
                .ForeignKey("dbo.Post", t => t.PostID, cascadeDelete: true)
                .Index(t => t.PostID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostImage", "PostID", "dbo.Post");
            DropIndex("dbo.PostImage", new[] { "PostID" });
            DropTable("dbo.PostImage");
        }
    }
}
