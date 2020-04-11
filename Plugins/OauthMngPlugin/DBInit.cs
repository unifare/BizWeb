using FluentMigrator;
using OauthMngPlugin.Model;
using System;
using System.Collections.Generic;
using System.Text;
using UniOrm;
using UniOrm.Common;

namespace OauthMngPlugin
{
    [UniMigration("OauthMngPlugin.mig")]
    public class DbInit : UniOrm.Common.DBMIgrateBase
    {
        public override void Up()
        {

            Create.Table(WholeTableName(nameof(OauthUser)))
                .WithColumn(nameof(OauthUser.Id)).AsInt64().PrimaryKey().Identity()
                .WithColumn(nameof(OauthUser.Guid)).AsString(200).Nullable()
                 .WithColumn(nameof(OauthUser.Name)).AsString(200).Nullable()
                .WithColumn(nameof(OauthUser.Email)).AsString(200).Nullable()
                 .WithColumn(nameof(OauthUser.UserName)).AsString(100).Nullable()
                .WithColumn(nameof(OauthUser.PhoneNumber)).AsString(50).Nullable()
                .WithColumn(nameof(OauthUser.Age)).AsInt32().Nullable()
                .WithColumn(nameof(OauthUser.AddTime)).AsDateTime().Nullable()
                .WithColumn(nameof(OauthUser.LastIP)).AsString(50).Nullable()
                .WithColumn(nameof(OauthUser.IsEnable)).AsBoolean() 
                ;
        }

        public override void Down()
        {
            Delete.Table(WholeTableName(nameof(OauthUser)));

        }
    }


     
         
}
