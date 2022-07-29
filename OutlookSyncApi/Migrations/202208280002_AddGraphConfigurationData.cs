using FluentMigrator;
using OutlookSyncApi.Models;

[Migration(202208280004)]
public class AddGraphConfigurationData_202208280002 : Migration
{
    public override void Down()
    {
       Delete.FromTable("GraphConfiguration")
            .Row(new GraphConfiguration
            {
                Id = 1
            });
    }

    public override void Up()
    {
        Execute.Sql("SET IDENTITY_INSERT [dbo].[GraphConfiguration] ON");

        Insert.IntoTable("GraphConfiguration")
             .Row(new GraphConfiguration
             {
                 Id = 1,
                 Username = "ldoglioli@80v3lh.onmicrosoft.com",
                 Password = "Luchito312086._",
                 ClientId = "d92eafd0-b1f1-4218-8012-43b3b55a6e51",
                 TenantId = "5d6392ad - 0fd1 - 4f66 - a689 - d6e5c542d719",
                 Authority = "https://login.microsoftonline.com/5d6392ad-0fd1-4f66-a689-d6e5c542d719/",
                 Scopes = "user.read user.read.all offline_access calendars.ReadWrite mailboxsettings.read application.read.all"
             });

        Execute.Sql("SET IDENTITY_INSERT [dbo].[GraphConfiguration] OFF");
    }
}