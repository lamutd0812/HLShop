namespace HLShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class erditOrderDetailTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "ProductName", c => c.String(nullable: false, maxLength: 256));
            AddColumn("dbo.OrderDetails", "ProductImage", c => c.String(maxLength: 256));
            AddColumn("dbo.OrderDetails", "ProductPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "ProductPrice");
            DropColumn("dbo.OrderDetails", "ProductImage");
            DropColumn("dbo.OrderDetails", "ProductName");
        }
    }
}
