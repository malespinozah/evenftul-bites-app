namespace Eventful_Bite_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class restaurantbranchestable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        BranchId = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        Review = c.String(),
                        Location = c.String(),
                        Address = c.String(),
                        RestaurantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BranchId)
                .ForeignKey("dbo.Restaurants", t => t.RestaurantId, cascadeDelete: true)
                .Index(t => t.RestaurantId);
            
            CreateTable(
                "dbo.Restaurants",
                c => new
                    {
                        RestaurantId = c.Int(nullable: false, identity: true),
                        RestaurantName = c.String(),
                        RestaurantType = c.String(),
                        Cuisine = c.String(),
                        Budget = c.String(),
                    })
                .PrimaryKey(t => t.RestaurantId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Branches", "RestaurantId", "dbo.Restaurants");
            DropIndex("dbo.Branches", new[] { "RestaurantId" });
            DropTable("dbo.Restaurants");
            DropTable("dbo.Branches");
        }
    }
}
