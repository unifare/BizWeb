using System;
using FluentMigrator;
using UniNote.Web.Model;
namespace UniNote.DBMigration
{
    [Migration(10000)]
    public class MigrationVersion1 : Migration
    {
        public override void Up()
        {

            Create.Table(nameof(pigcms_adma))
                .WithColumn(nameof(pigcms_adma.id)).AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(pigcms_adma.info)).AsString(200)
                .WithColumn(nameof(pigcms_adma.title)).AsString(300)
                ;
            Create.Table(nameof(LocalUser))
              .WithColumn(nameof(LocalUser.Id)).AsInt32().PrimaryKey().Identity()
              .WithColumn(nameof(LocalUser.UserName)).AsString(200)
              .WithColumn(nameof(LocalUser.Password)).AsString(300)
              .WithColumn(nameof(LocalUser.LatestToken)).AsString(600)
              .WithColumn(nameof(LocalUser.LatestRefreshToken)).AsString(500)
              .WithColumn(nameof(LocalUser.AddTime)).AsDateTime();
            ;
        }

        public override void Down()
        {
            Delete.Table(nameof(pigcms_adma));
            Delete.Table(nameof(LocalUser));
        }
    }
}
