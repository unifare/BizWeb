using FluentMigrator;
using UniOrm.DistributeSystemPlugin.Model;
using System;
using System.Collections.Generic;
using System.Text;
using UniOrm;
using UniOrm.Common;

namespace UniOrm.DistributeSystemPlugin
{
    [UniMigration("UniOrm.DistributeSystemPlugin.mig")]
    public class DbInitAgent : UniOrm.Common.DBMIgrateBase
    {
        public override void Up()
        {

            Create.Table(WholeTableName(nameof(AgentUser)))
                .WithColumn(nameof(AgentUser.Id)).AsInt64().PrimaryKey().Identity()
                .WithColumn(nameof(AgentUser.Guid)).AsString(100).Nullable()
                .WithColumn(nameof(AgentUser.Address)).AsString(1000).Nullable()
                .WithColumn(nameof(AgentUser.AddTime)).AsDateTime().Nullable()
                .WithColumn(nameof(AgentUser.Age)).AsInt32().Nullable().WithDefaultValue(0)
                .WithColumn(nameof(AgentUser.BankCardNo)).AsString(80).Nullable()
                .WithColumn(nameof(AgentUser.BankName)).AsString(500).Nullable()
                .WithColumn(nameof(AgentUser.BankOwnerName)).AsString(80).Nullable()
                .WithColumn(nameof(AgentUser.City)).AsString(380).Nullable()
                .WithColumn(nameof(AgentUser.Email)).AsString(380).Nullable()
                .WithColumn(nameof(AgentUser.FeeAccountNO)).AsString(180).Nullable()
                .WithColumn(nameof(AgentUser.IdentityNo)).AsString(180).Nullable()
                .WithColumn(nameof(AgentUser.IsEnable)).AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn(nameof(AgentUser.LastScanTime)).AsDateTime().Nullable()
                .WithColumn(nameof(AgentUser.LiveLocation)).AsString(300).Nullable()
                .WithColumn(nameof(AgentUser.Memo)).AsString(600).Nullable()
                .WithColumn(nameof(AgentUser.NewSubcribeTimes)).AsDateTime().Nullable()
                .WithColumn(nameof(AgentUser.NoSubcribeTimes)).AsDateTime().Nullable()
                .WithColumn(nameof(AgentUser.PhoneNumer)).AsString(100).Nullable()
                .WithColumn(nameof(AgentUser.Profession)).AsString(100).Nullable()
                .WithColumn(nameof(AgentUser.ProxyLevel)).AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn(nameof(AgentUser.ProxyOpenId)).AsString(100).NotNullable().WithDefaultValue(0)
                .WithColumn(nameof(AgentUser.ProxyRealNameStatus)).AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn(nameof(AgentUser.ProxyStatus)).AsString(100).Nullable()
                .WithColumn(nameof(AgentUser.ProxyType)).AsString(100).Nullable()
                .WithColumn(nameof(AgentUser.RegTime)).AsDateTime().Nullable()
                .WithColumn(nameof(AgentUser.SalesCode)).AsString(100).Nullable()
                .WithColumn(nameof(AgentUser.SalesCodeAli)).AsString(100).Nullable()
                .WithColumn(nameof(AgentUser.Sex)).AsBoolean().Nullable()
                .WithColumn(nameof(AgentUser.SubcribePeopleNum)).AsInt32().Nullable()
                .WithColumn(nameof(AgentUser.UserId)).AsString(80).Nullable()
                .WithColumn(nameof(AgentUser.WechatOpenId)).AsString(80).Nullable()
                .WithColumn(nameof(AgentUser.WeChatQrcode)).AsString(80).Nullable() 
                ;

            Create.Table(WholeTableName(nameof(ProxyPolicy)))
                 .WithColumn(nameof(ProxyPolicy.Id)).AsInt64().PrimaryKey().Identity()
                 .WithColumn(nameof(ProxyPolicy.Guid)).AsString(100).Nullable()
                 .WithColumn(nameof(ProxyPolicy.CreateDate)).AsDateTime().Nullable()
                 .WithColumn(nameof(ProxyPolicy.IsDisabled)).AsBoolean().NotNullable().WithDefaultValue(false)
                 .WithColumn(nameof(ProxyPolicy.PriceForBill)).AsDecimal(10, 2).Nullable()
                  .WithColumn(nameof(ProxyPolicy.ProductGuid)).AsString(100).NotNullable()
                  .WithColumn(nameof(ProxyPolicy.ProxyLevel)).AsInt32( ).Nullable()
                 .WithColumn(nameof(ProxyPolicy.ProxyType)).AsInt32().Nullable()
                 .WithColumn(nameof(ProxyPolicy.RateForPrice)).AsDecimal(10, 2).Nullable()
                 .WithColumn(nameof(ProxyPolicy.RateType)).AsInt32( ).Nullable()
                 .WithColumn(nameof(ProxyPolicy.TaocanId)).AsInt32().Nullable()
                ;

            //Create.Table(WholeTableName(nameof(ProxyPolicy)))
            //    .WithColumn(nameof(ProxyPolicy.Id)).AsInt64().PrimaryKey().Identity()
            //    .WithColumn(nameof(ProxyPolicy.Guid)).AsString(100).Nullable()
            //    .WithColumn(nameof(ProxyPolicy.CreateDate)).AsDateTime().Nullable()
            //    .WithColumn(nameof(ProxyPolicy.IsDisabled)).AsBoolean().NotNullable().WithDefaultValue(false)
            //    .WithColumn(nameof(ProxyPolicy.PriceForBill)).AsDecimal(10, 2).Nullable()
            //     .WithColumn(nameof(ProxyPolicy.ProductGuid)).AsString(100).NotNullable()
            //     .WithColumn(nameof(ProxyPolicy.ProxyLevel)).AsInt32().Nullable()
            //    .WithColumn(nameof(ProxyPolicy.ProxyType)).AsInt32().Nullable()
            //    .WithColumn(nameof(ProxyPolicy.RateForPrice)).AsDecimal(10, 2).Nullable()
            //    .WithColumn(nameof(ProxyPolicy.RateType)).AsInt32().Nullable()
            //    .WithColumn(nameof(ProxyPolicy.TaocanId)).AsInt32().Nullable()
               ;

            Create.Table(WholeTableName(nameof(ProxyFeeDetail)))
              .WithColumn(nameof(ProxyFeeDetail.Id)).AsInt64().PrimaryKey().Identity()
              .WithColumn(nameof(ProxyFeeDetail.ApplyID)).AsString(100).Nullable()
              .WithColumn(nameof(ProxyFeeDetail.ApplyTime)).AsDateTime().Nullable()
              .WithColumn(nameof(ProxyFeeDetail.CheckedoutFee)).AsDecimal(10, 2).Nullable() 
              .WithColumn(nameof(ProxyFeeDetail.CheckedTime)).AsDateTime( ).Nullable()
               .WithColumn(nameof(ProxyFeeDetail.CountedFee)).AsDecimal(10, 2).Nullable()
               .WithColumn(nameof(ProxyFeeDetail.CreateTime)).AsDateTime().Nullable()
              .WithColumn(nameof(ProxyFeeDetail.FirstSalescode)).AsString(100).Nullable()
              .WithColumn(nameof(ProxyFeeDetail.IsCancel)).AsBoolean( ).Nullable().WithDefaultValue(false)
              .WithColumn(nameof(ProxyFeeDetail.IsChecked)).AsBoolean().Nullable().WithDefaultValue(false)
               .WithColumn(nameof(ProxyFeeDetail.IsInFronzen)).AsBoolean().Nullable().WithDefaultValue(false)
              .WithColumn(nameof(ProxyFeeDetail.IsRefonded)).AsBoolean().Nullable().WithDefaultValue(false)
               .WithColumn(nameof(ProxyFeeDetail.LockFanProxySalesCode)).AsString(100).Nullable() 
              .WithColumn(nameof(ProxyFeeDetail.OrderID)).AsString(100).Nullable() 
              .WithColumn(nameof(ProxyFeeDetail.OrderTime)).AsDateTime().Nullable()
               .WithColumn(nameof(ProxyFeeDetail.PayedTime)).AsString(100).Nullable()
              .WithColumn(nameof(ProxyFeeDetail.PriceForBill)).AsDecimal(10, 2).Nullable()
              .WithColumn(nameof(ProxyFeeDetail.ProxyFrozenDays)).AsInt32().Nullable()
              .WithColumn(nameof(ProxyFeeDetail.ProxyLevel)).AsInt32().Nullable()
              .WithColumn(nameof(ProxyFeeDetail.ProxyType)).AsInt32().Nullable()
              .WithColumn(nameof(ProxyFeeDetail.RateForPrice)).AsDecimal(10, 2).Nullable()
              .WithColumn(nameof(ProxyFeeDetail.RateType)).AsInt32().Nullable()
              .WithColumn(nameof(ProxyFeeDetail.SalesCode)).AsString(100).NotNullable()
              .WithColumn(nameof(ProxyFeeDetail.TotalPrice)).AsDecimal(10, 2).Nullable()
              .WithColumn(nameof(ProxyFeeDetail.Ujid)).AsString(100). Nullable()
              .WithColumn(nameof(ProxyFeeDetail.UnCheckedoutFee)).AsDecimal(10, 2).Nullable()
              .WithColumn(nameof(ProxyFeeDetail.UpdateTime)).AsDateTime( ).Nullable() 
             ;
        }

        public override void Down()
        {
            // Delete.Table(WholeTableName(nameof(Shoper)));

        }
    }




}
