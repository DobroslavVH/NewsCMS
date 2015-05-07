namespace NewsCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryImage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryImage",
                c => new
                    {
                        CategoryImageID = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 255),
                        ContentType = c.String(maxLength: 100),
                        Content = c.Binary(),
                        FileType = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryImageID)
                .ForeignKey("dbo.Category", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CategoryImage", "CategoryID", "dbo.Category");
            DropIndex("dbo.CategoryImage", new[] { "CategoryID" });
            DropTable("dbo.CategoryImage");
        }
    }
}
