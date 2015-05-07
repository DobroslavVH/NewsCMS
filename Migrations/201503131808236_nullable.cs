namespace NewsCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Author", "CreateAt", c => c.DateTime());
            AlterColumn("dbo.Author", "UpdateAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Author", "UpdateAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Author", "CreateAt", c => c.DateTime(nullable: false));
        }
    }
}
