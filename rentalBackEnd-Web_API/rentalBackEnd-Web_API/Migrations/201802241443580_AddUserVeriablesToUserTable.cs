namespace rentalBackEnd_Web_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserVeriablesToUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "fullName", c => c.String());
            AddColumn("dbo.Users", "dateOfBirth", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "sex", c => c.String());
            AddColumn("dbo.Users", "profilePicture", c => c.Binary(true,maxLength: 16777215));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "profilePicture");
            DropColumn("dbo.Users", "sex");
            DropColumn("dbo.Users", "dateOfBirth");
            DropColumn("dbo.Users", "fullName");
        }
    }
}
