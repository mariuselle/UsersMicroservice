namespace Users.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteTasks : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Tasks");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
