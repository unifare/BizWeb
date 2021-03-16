using FluentMigrator; 
using System;
using System.Collections.Generic;
using System.Text;
using UniOrm;
using UniOrm.Common;
using UniOrm.Model;

namespace LocalMemberShip
{
    [UniMigration("LocalMemberShip.mig")]
    public class DbInit : UniOrm.Common.DBMIgrateBase
    {
        public override void Up()
        {

     
        }

        public override void Down()
        {
            //Delete.Table(WholeTableName(nameof(OauthUser)));

        }
    }


     
         
}
