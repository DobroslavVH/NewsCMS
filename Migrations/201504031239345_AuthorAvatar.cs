namespace NewsCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuthorAvatar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuthorAvatar",
                c => new
                    {
                        AvatarID = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 255),
                        ContentType = c.String(maxLength: 100),
                        Content = c.Binary(),
                        FileType = c.Int(nullable: false),
                        AuthorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AvatarID)
                .ForeignKey("dbo.Author", t => t.AuthorID, cascadeDelete: true)
                .Index(t => t.AuthorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuthorAvatar", "AuthorID", "dbo.Author");
            DropIndex("dbo.AuthorAvatar", new[] { "AuthorID" });
            DropTable("dbo.AuthorAvatar");
        }
    }
}
