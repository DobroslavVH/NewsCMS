namespace NewsCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplyNull_1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Category", "CreateAt", c => c.DateTime());
            AlterColumn("dbo.Category", "UpdateAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Category", "UpdateAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Category", "CreateAt", c => c.DateTime(nullable: false));
        }
    }
}
