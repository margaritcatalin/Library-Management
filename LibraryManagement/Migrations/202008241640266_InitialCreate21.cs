namespace LibraryManagement.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate21 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Categories", new[] { "ParentCategory_Id" });
            AlterColumn("dbo.Categories", "ParentCategory_Id", c => c.Int());
            CreateIndex("dbo.Categories", "ParentCategory_Id");
        }

        public override void Down()
        {
            DropIndex("dbo.Categories", new[] { "ParentCategory_Id" });
            AlterColumn("dbo.Categories", "ParentCategory_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Categories", "ParentCategory_Id");
        }
    }
}