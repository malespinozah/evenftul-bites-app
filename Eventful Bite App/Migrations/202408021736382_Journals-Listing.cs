namespace Eventful_Bite_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JournalsListing : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Journals",
                c => new
                    {
                        JournalId = c.Int(nullable: false, identity: true),
                        EventName = c.String(nullable: false),
                        RestaurantName = c.String(nullable: false),
                        JournalTitle = c.String(),
                        EntryDate = c.DateTime(nullable: false),
                        JournalDescription = c.String(),
                    })
                .PrimaryKey(t => t.JournalId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Journals");
        }
    }
}
