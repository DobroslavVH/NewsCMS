namespace NewsCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubCategoryImage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubCategoryImage",
                c => new
                    {
                        SubCategoryImageID = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 255),
                        ContentType = c.String(maxLength: 100),
                        Content = c.Binary(),
                        FileType = c.Int(nullable: false),
                        SubCategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SubCategoryImageID)
                .ForeignKey("dbo.SubCategory", t => t.SubCategoryID, cascadeDelete: true)
                .Index(t => t.SubCategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubCategoryImage", "SubCategoryID", "dbo.SubCategory");
            DropIndex("dbo.SubCategoryImage", new[] { "SubCategoryID" });
            DropTable("dbo.SubCategoryImage");
        }
    }
}
