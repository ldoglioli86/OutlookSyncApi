using FluentMigrator;

[Migration(202208280001)]
public class CreateGraphConfigurationTable_202208280001 : Migration
{
    public override void Down()
    {
        Delete.Table("GraphConfiguration");
    }
    public override void Up()
    {
        Create.Table("GraphConfiguration")
            .WithColumn("Id").AsInt64().Identity().NotNullable().PrimaryKey()
            .WithColumn("Username").AsString(100).NotNullable()
            .WithColumn("Password").AsString(100).NotNullable()
            .WithColumn("Authority").AsString(100).NotNullable()
            .WithColumn("ClientId").AsString(1000).NotNullable()
            .WithColumn("TenantId").AsString(50).NotNullable()
            .WithColumn("scopes").AsString(100).NotNullable();
    }
}