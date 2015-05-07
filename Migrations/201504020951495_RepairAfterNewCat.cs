namespace NewsCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RepairAfterNewCat : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChildCategory", "ChildCategory_ChildCategoryID", "dbo.ChildCategory");
            DropForeignKey("dbo.Category", "ChildCategoryID", "dbo.ChildCategory");
            DropIndex("dbo.Category", new[] { "ChildCategoryID" });
            DropIndex("dbo.ChildCategory", new[] { "ChildCategory_ChildCategoryID" });
            DropColumn("dbo.Category", "ChildCategoryID");
            DropTable("dbo.ChildCategory");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ChildCategory",
                c => new
                    {
                        ChildCategoryID = c.Int(nullable: false, identity: true),
                        ChildCategoryName = c.String(),
                        ChildCategory_ChildCategoryID = c.Int(),
                    })
                .PrimaryKey(t => t.ChildCategoryID);
            
            AddColumn("dbo.Category", "ChildCategoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.ChildCategory", "ChildCategory_ChildCategoryID");
            CreateIndex("dbo.Category", "ChildCategoryID");
            AddForeignKey("dbo.Category", "ChildCategoryID", "dbo.ChildCategory", "ChildCategoryID", cascadeDelete: true);
            AddForeignKey("dbo.ChildCategory", "ChildCategory_ChildCategoryID", "dbo.ChildCategory", "ChildCategoryID");
        }
    }
}
