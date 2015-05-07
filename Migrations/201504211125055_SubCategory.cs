namespace NewsCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubCategory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubCategory",
                c => new
                    {
                        SubCategoryID = c.Int(nullable: false, identity: true),
                        SubCategoryName = c.String(),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SubCategoryID)
                .ForeignKey("dbo.Category", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubCategory", "CategoryID", "dbo.Category");
            DropIndex("dbo.SubCategory", new[] { "CategoryID" });
            DropTable("dbo.SubCategory");
        }
    }
}
