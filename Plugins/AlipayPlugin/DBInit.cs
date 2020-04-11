using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;
using UniOrm;
using UniOrm.Common;

namespace AlipayPlugin
{
    [UniMigration("AlipayPlugin.mig" )]
    public class Init : DBMIgrateBase
    {
        public override void Up()
        {

            Create.Table( WholeTableName( "pigcms_adma3"))
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("info").AsString(200)
                .WithColumn("title").AsString(300)
                ;

        }

        public override void Down()
        {
            Delete.Table(WholeTableName("pigcms_adma3"));

        }
    }


     
         
}
