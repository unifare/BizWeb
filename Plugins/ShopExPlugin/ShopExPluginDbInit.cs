using UniOrm.Common;
using UniOrm.ShopExPlugin.Model;

namespace UniOrm.ShopExPlugin
{
    [UniMigration("ShopExPlugin.mig")]
    public class ShopExPluginDbInit : DBMIgrateBase
    {
        public override void Up()
        {

            Create.Table(WholeTableName(nameof(Shoper)))
                .WithColumn(nameof(Shoper.Id)).AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(Shoper.Guid)).AsString(100).Nullable()
                 .WithColumn(nameof(Shoper.ShopName)).AsString(600).Nullable()
                .WithColumn(nameof(Shoper.Email)).AsString(200).Nullable()
                 .WithColumn(nameof(Shoper.PhoneNumber)).AsString(100).Nullable()
                .WithColumn(nameof(Shoper.Telephone)).AsString(50).Nullable()
                .WithColumn(nameof(Shoper.UserID)).AsString(100).Nullable()
                .WithColumn(nameof(Shoper.AddTime)).AsDateTime().Nullable()
                // .WithColumn(nameof(Shoper.LastIP)).AsString(50).Nullable()
                .WithColumn(nameof(Shoper.IsEnable)).AsBoolean()
                ;

            Create.Table(WholeTableName(nameof(OrderBill)))
             .WithColumn(nameof(OrderBill.Id)).AsInt32().PrimaryKey().Identity()
             .WithColumn(nameof(OrderBill.Guid)).AsString(100).Nullable()
              .WithColumn(nameof(OrderBill.AuditNum)).AsInt32().Nullable()
             .WithColumn(nameof(OrderBill.AuditPrice)).AsDecimal(10, 2).Nullable()
              .WithColumn(nameof(OrderBill.BeginTime)).AsDateTime().Nullable()
             .WithColumn(nameof(OrderBill.BizType)).AsString(50).Nullable()
             .WithColumn(nameof(OrderBill.CostomerEmail)).AsString(200).Nullable()
             .WithColumn(nameof(OrderBill.CostomerHandtel)).AsString(100).Nullable()
             .WithColumn(nameof(OrderBill.CostomerName)).AsString(200).Nullable()
             .WithColumn(nameof(OrderBill.CreateBy)).AsString(200).Nullable()
              .WithColumn(nameof(OrderBill.CreateDate)).AsDateTime().Nullable()
             .WithColumn(nameof(OrderBill.CstIDNumber)).AsString(100).Nullable()
             .WithColumn(nameof(OrderBill.DayNum)).AsInt32().Nullable()
             .WithColumn(nameof(OrderBill.EndTime)).AsDateTime().Nullable()
             .WithColumn(nameof(OrderBill.GoType)).AsString(10).Nullable()
             .WithColumn(nameof(OrderBill.GoType_bus_Price)).AsDecimal(10, 2).Nullable()
              .WithColumn(nameof(OrderBill.GoType_Zijia_Adress)).AsString(300).Nullable()
             .WithColumn(nameof(OrderBill.GrowingValues)).AsInt32().Nullable()
             .WithColumn(nameof(OrderBill.IsDayCal)).AsBoolean().Nullable()
             .WithColumn(nameof(OrderBill.IsEnable)).AsBoolean().Nullable()
             .WithColumn(nameof(OrderBill.IsOnlyOrder)).AsBoolean().Nullable()
             .WithColumn(nameof(OrderBill.IsRefund)).AsBoolean().Nullable()
              .WithColumn(nameof(OrderBill.IsSales)).AsBoolean().Nullable()
             .WithColumn(nameof(OrderBill.IsUsedPoint)).AsBoolean().Nullable()
             .WithColumn(nameof(OrderBill.JID)).AsString(100).Nullable()
             .WithColumn(nameof(OrderBill.JiheAdress)).AsString(1000).Nullable()
              .WithColumn(nameof(OrderBill.JiheID)).AsInt32().Nullable()
             .WithColumn(nameof(OrderBill.KidNum)).AsInt32().Nullable()
              .WithColumn(nameof(OrderBill.KidPrice)).AsDecimal(10, 2).Nullable()
             .WithColumn(nameof(OrderBill.LastCheckOutTime)).AsDateTime().Nullable()
             .WithColumn(nameof(OrderBill.LineType)).AsString(50).Nullable()
             .WithColumn(nameof(OrderBill.Memo)).AsString(700).Nullable()
              .WithColumn(nameof(OrderBill.OrderID)).AsString(80).Nullable()
             .WithColumn(nameof(OrderBill.OrderState)).AsString(50).Nullable()
              .WithColumn(nameof(OrderBill.OrderTime)).AsDateTime().Nullable()
             .WithColumn(nameof(OrderBill.OrderTotalPrice)).AsDecimal(10, 2).Nullable()
             .WithColumn(nameof(OrderBill.OrderType)).AsString(100).Nullable()
             .WithColumn(nameof(OrderBill.PayedFee)).AsDecimal(10, 2).Nullable()
              .WithColumn(nameof(OrderBill.PayedTime)).AsDateTime().Nullable()
             .WithColumn(nameof(OrderBill.PointsNeeded)).AsInt32().Nullable()
              .WithColumn(nameof(OrderBill.PointsReturned)).AsInt32().Nullable()
             .WithColumn(nameof(OrderBill.PointToMoney)).AsDecimal(10, 2).Nullable()
             .WithColumn(nameof(OrderBill.qdSource)).AsString(200).Nullable()
             .WithColumn(nameof(OrderBill.ReferorGuid)).AsString(100).Nullable()
             .WithColumn(nameof(OrderBill.ReferorWxid)).AsString(80).Nullable()
             .WithColumn(nameof(OrderBill.RefundAfterFee)).AsDecimal(10, 2).Nullable()
              .WithColumn(nameof(OrderBill.RefundDate)).AsDateTime().Nullable()
             .WithColumn(nameof(OrderBill.RefundType)).AsString(50).Nullable()
             .WithColumn(nameof(OrderBill.RefundValue)).AsDecimal(10, 2).Nullable()
             .WithColumn(nameof(OrderBill.SalersCode)).AsString(100).Nullable()
             // .WithColumn(nameof(Shoper.LastIP)).AsString(50).Nullable()
             .WithColumn(nameof(OrderBill.SalesID)).AsInt32().Nullable()
             .WithColumn(nameof(OrderBill.SharePricePerUnit)).AsDecimal(10, 2).Nullable()
             .WithColumn(nameof(OrderBill.TaocanNum)).AsInt32().Nullable()
             .WithColumn(nameof(OrderBill.TaocanPrice)).AsDecimal(10,2).Nullable()
             .WithColumn(nameof(OrderBill.TotalPrice)).AsDecimal(10, 2).Nullable()
             .WithColumn(nameof(OrderBill.Transaction_id)).AsString(150).Nullable()
             .WithColumn(nameof(OrderBill.UJID)).AsString(150).Nullable()
             .WithColumn(nameof(OrderBill.WeixinID)).AsString(150).Nullable() 
             ;

            Create.Table(WholeTableName(nameof(OrderBillItem)))
                .WithColumn(nameof(OrderBillItem.Id)).AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(OrderBillItem.Guid)).AsString(100).Nullable()
                 .WithColumn(nameof(OrderBillItem.AddTime)).AsDateTime().Nullable()
                .WithColumn(nameof(OrderBillItem.ItemPrice)).AsDecimal(10, 2).Nullable()
                 .WithColumn(nameof(OrderBillItem.ItemQuatity)).AsDouble().Nullable()
                .WithColumn(nameof(OrderBillItem.OrderGuid)).AsString(100).Nullable()
                .WithColumn(nameof(OrderBillItem.ProductId)).AsString(100).Nullable() 
                ;
            IfDatabase("mysql").Create.Table(WholeTableName(nameof(PhotoProduct)))
                .WithColumn(nameof(PhotoProduct.Id)).AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(PhotoProduct.Guid)).AsString(100).Nullable()
                 .WithColumn(nameof(PhotoProduct.AddTime)).AsDateTime().Nullable()
                .WithColumn(nameof(PhotoProduct.BeginTime)).AsDateTime().Nullable()
                .WithColumn(nameof(PhotoProduct.EndTime)).AsDateTime().Nullable()
                .WithColumn(nameof(PhotoProduct.PhoteDate)).AsString(100).Nullable()
                .WithColumn(nameof(PhotoProduct.PhoteDescribe)).AsCustom("text").Nullable()
                .WithColumn(nameof(PhotoProduct.PhotoShoperName)).AsString(500).Nullable()
                .WithColumn(nameof(PhotoProduct.Place)).AsString(1000).Nullable()
                .WithColumn(nameof(PhotoProduct.Price)).AsDecimal(10, 2).Nullable()
                .WithColumn(nameof(PhotoProduct.Unit)).AsString(100).Nullable()
                ;
            IfDatabase("SqlServer", "Postgres", "sqlite").Create.Table(WholeTableName(nameof(PhotoProduct)))
                .WithColumn(nameof(PhotoProduct.Id)).AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(PhotoProduct.Guid)).AsString(100).Nullable()
                 .WithColumn(nameof(PhotoProduct.AddTime)).AsDateTime().Nullable()
                .WithColumn(nameof(PhotoProduct.BeginTime)).AsDateTime( ).Nullable()
                .WithColumn(nameof(PhotoProduct.EndTime)).AsDateTime().Nullable()
                .WithColumn(nameof(PhotoProduct.PhoteDate)).AsString(100).Nullable()
                .WithColumn(nameof(PhotoProduct.PhoteDescribe)).AsCustom("ntext").Nullable()
                .WithColumn(nameof(PhotoProduct.PhotoShoperName)).AsString(500).Nullable()
                .WithColumn(nameof(PhotoProduct.Place)).AsString(1000).Nullable()
                .WithColumn(nameof(PhotoProduct.Price)).AsDecimal(10,2).Nullable()
                .WithColumn(nameof(PhotoProduct.Unit)).AsString(100).Nullable() 
                ;

            Create.Table(WholeTableName(nameof(ProductType)))
              .WithColumn(nameof(ProductType.Id)).AsInt32().PrimaryKey().Identity()
              .WithColumn(nameof(ProductType.Guid)).AsString(100).Nullable()
               .WithColumn(nameof(ProductType.AddTime)).AsDateTime().Nullable()
              .WithColumn(nameof(ProductType.IsShow)).AsBoolean().Nullable().WithDefaultValue(true)
              .WithColumn(nameof(ProductType.IsSystem)).AsBoolean().Nullable().WithDefaultValue(false)
              .WithColumn(nameof(ProductType.TypeName)).AsString(100).Nullable()
              .WithColumn(nameof(ProductType.TypeParentID)).AsString(100).Nullable() 
              ;

            IfDatabase("mysql").Create.Table(WholeTableName(nameof(ShopProduct)))
             .WithColumn(nameof(ShopProduct.Id)).AsInt32().PrimaryKey().Identity()
             .WithColumn(nameof(ShopProduct.Guid)).AsString(100).Nullable()
              .WithColumn(nameof(ShopProduct.AddTime)).AsDateTime().Nullable()
             .WithColumn(nameof(ShopProduct.PhoneNumber)).AsString(1100).Nullable().WithDefaultValue(true)
             .WithColumn(nameof(ShopProduct.PrdouctContent)).AsCustom("text").Nullable()
             .WithColumn(nameof(ShopProduct.Price)).AsDecimal(10, 2).Nullable()
             .WithColumn(nameof(ShopProduct.ProductName)).AsString(200).Nullable()
               .WithColumn(nameof(ShopProduct.ProductTitle)).AsString(700).Nullable()
             .WithColumn(nameof(ShopProduct.ProductTypeID)).AsString(80).Nullable()
             .WithColumn(nameof(ShopProduct.ShopGuid)).AsString(100).Nullable()
               .WithColumn(nameof(ShopProduct.Telephone)).AsString(80).Nullable()
             ;
            IfDatabase("SqlServer", "Postgres", "sqlite").Create.Table(WholeTableName(nameof(ShopProduct)))
              .WithColumn(nameof(ShopProduct.Id)).AsInt32().PrimaryKey().Identity()
              .WithColumn(nameof(ShopProduct.Guid)).AsString(100).Nullable()
               .WithColumn(nameof(ShopProduct.AddTime)).AsDateTime().Nullable()
              .WithColumn(nameof(ShopProduct.PhoneNumber)).AsString(1100).Nullable().WithDefaultValue(true)
              .WithColumn(nameof(ShopProduct.PrdouctContent)).AsCustom("ntext").Nullable()
              .WithColumn(nameof(ShopProduct.Price)).AsDecimal(10, 2).Nullable()
              .WithColumn(nameof(ShopProduct.ProductName)).AsString(200).Nullable()
                .WithColumn(nameof(ShopProduct.ProductTitle)).AsString(700).Nullable()
              .WithColumn(nameof(ShopProduct.ProductTypeID)).AsString(80).Nullable()
              .WithColumn(nameof(ShopProduct.ShopGuid)).AsString(100).Nullable()
                .WithColumn(nameof(ShopProduct.Telephone)).AsString(80).Nullable() 
              ;
            Create.Table(WholeTableName(nameof(ShopProductTag)))
                  .WithColumn(nameof(ShopProductTag.Id)).AsInt32().PrimaryKey().Identity()
                  .WithColumn(nameof(ShopProductTag.ShopGuid)).AsString(100).Nullable()
                   .WithColumn(nameof(ShopProductTag.TagGuid)).AsString(100).Nullable() 
                  ;
            Create.Table(WholeTableName(nameof(Tags)))
                  .WithColumn(nameof(Tags.Id)).AsInt32().PrimaryKey().Identity()
                  .WithColumn(nameof(Tags.Guid)).AsString(100).Nullable()
                   .WithColumn(nameof(Tags.TagName)).AsString(400).Nullable()
                  ;

        }

        public override void Down()
        {
            Delete.Table(WholeTableName(nameof(Shoper)));

        }
    }




}
