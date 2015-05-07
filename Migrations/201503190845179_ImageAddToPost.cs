namespace NewsCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageAddToPost : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Post", "PostImage", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Post", "PostImage", c => c.Int(nullable: false));
        }
    }
}
