namespace NewsCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplyNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Post", "CreateAt", c => c.DateTime());
            AlterColumn("dbo.Post", "UpdateAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Post", "UpdateAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Post", "CreateAt", c => c.DateTime(nullable: false));
        }
    }
}
