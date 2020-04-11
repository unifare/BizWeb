using FluentMigrator;
using OauthMngPlugin.Model;
using System;
using System.Collections.Generic;
using System.Text;
using UniOrm;
using UniOrm.Common;

namespace UniOrm.UserWebPlugin
{
    [UniMigration("UserWebPlugin.mig")]
    public class UserWebDBInit : UniOrm.Common.DBMIgrateBase
    {
        public override void Up()
        {

            //Create.Table(WholeTableName(nameof(Shoper)))
            //    .WithColumn(nameof(Shoper.Id)).AsInt64().PrimaryKey().Identity()
            //    .WithColumn(nameof(Shoper.Guid)).AsString(200).Nullable()
            //     .WithColumn(nameof(Shoper.ShopName)).AsString(600).Nullable()
            //    .WithColumn(nameof(Shoper.Email)).AsString(200).Nullable()
            //     .WithColumn(nameof(Shoper.PhoneNumber)).AsString(100).Nullable()
            //    .WithColumn(nameof(Shoper.Telephone)).AsString(50).Nullable()
            //    .WithColumn(nameof(Shoper.UserID)).AsString(100).Nullable()
            //    .WithColumn(nameof(Shoper.AddTime)).AsDateTime().Nullable()
            //   // .WithColumn(nameof(Shoper.LastIP)).AsString(50).Nullable()
            //    .WithColumn(nameof(Shoper.IsEnable)).AsBoolean() 
                ;
        }

        public override void Down()
        {
           // Delete.Table(WholeTableName(nameof(Shoper)));

        }
    }


     
         
}
