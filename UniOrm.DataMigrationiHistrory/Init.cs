using FluentMigrator;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniOrm.Common;

namespace UniOrm.DataMigrationiHistrory
{
    [Migration(1)]
    public class Init : DBMIgrateBase
    {

        public override void Up()
        {

            Create.Table(WholeTableName("SystemACon"))
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("AppName").AsString(200)
                .WithColumn("AppDiscription").AsString(300)
                .WithColumn("AppConfigs").AsString().Nullable()
                .WithColumn("VersionNum").AsString(100).Nullable()
                .WithColumn("UpdateUrl").AsString(600).Nullable()
                .WithColumn("UpdateParamter").AsString(600).Nullable()
                .WithColumn("Author").AsString(100).Nullable()
                .WithColumn("LastRunTime").AsDateTime().Nullable()
                 .WithColumn("CreateTime").AsDateTime().Nullable()
                ;
            Create.Table(WholeTableName("SystemRegitionInfo"))
              .WithColumn("Id").AsInt64().PrimaryKey().Identity()
              .WithColumn("AuthorizeWay").AsString(200)
              .WithColumn("VersionNum").AsString(100).Nullable()
              .WithColumn("MachineCode").AsString(100).Nullable()
              .WithColumn("RigesterCode").AsString(2000).Nullable()
              .WithColumn("RigesterUrl").AsString(1000).Nullable()
              .WithColumn("RigesterFilePath").AsString(1000).Nullable()
              .WithColumn("ClientName").AsString(1000).Nullable()
              .WithColumn("TryNumber").AsInt32().Nullable().WithDefaultValue(0)
              .WithColumn("TryEndTime").AsDateTime().Nullable()
              .WithColumn("LastRunTime").AsDateTime().Nullable()
               .WithColumn("CreateTime").AsDateTime().Nullable()
              ;

            Create.Table(WholeTableName("FieldInfoDefinition"))
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("BelongTypeId").AsInt64().Nullable()
                .WithColumn("Name").AsString(200)
                .WithColumn("IsPublic").AsBoolean().WithDefaultValue(true)
                .WithColumn("IsPrivate").AsBoolean().WithDefaultValue(false)
                .WithColumn("IsSpecialName").AsBoolean().WithDefaultValue(false)
                .WithColumn("FieldType").AsString(400).Nullable()
             .WithColumn("MemberType").AsInt32().Nullable()
             .WithColumn("VersionNum").AsString(100).Nullable()
             .WithColumn("AddTime").AsDateTime().Nullable();
            ;

            Create.Table(WholeTableName("ProperityDefinition"))
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("BelongTypeId").AsInt64().Nullable()
               .WithColumn("Name").AsString(400)
               .WithColumn("IsPublic").AsBoolean().WithDefaultValue(true)
               .WithColumn("IsPrivate").AsBoolean().WithDefaultValue(false)
               .WithColumn("IsSpecialName").AsBoolean().WithDefaultValue(false)
               .WithColumn("CanWrite").AsBoolean().WithDefaultValue(true)
               .WithColumn("CanRead").AsBoolean().WithDefaultValue(true)
               .WithColumn("ProperityType").AsString(400).Nullable()
               .WithColumn("MemberType").AsInt32().Nullable()
               .WithColumn("VersionNum").AsString(100).Nullable()
               .WithColumn("AddTime").AsDateTime().Nullable();
            ;

            Create.Table(WholeTableName("MethodDefinition"))
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("BelongTypeId").AsInt64().Nullable()
               .WithColumn("Name").AsString(400)
                     .WithColumn("FullName").AsString(400)
                     .WithColumn("IsStatic").AsBoolean().Nullable().WithDefaultValue(true)
                   .WithColumn("IsConstructor").AsBoolean().WithDefaultValue(false).Nullable()
               .WithColumn("IsPublic").AsBoolean().WithDefaultValue(true)
               .WithColumn("IsPrivate").AsBoolean().WithDefaultValue(false).Nullable()
               .WithColumn("ReturnType").AsString(400).WithDefaultValue(string.Empty).Nullable()
               .WithColumn("ParameterInfo").AsString(400).Nullable()
               .WithColumn("MemberType").AsInt32().Nullable()
               .WithColumn("VersionNum").AsString(100).Nullable()
               .WithColumn("AddTime").AsDateTime().Nullable();
            ;
            Create.Table(WholeTableName("TypeDefinition"))
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("AliName").AsString(400)
               .WithColumn("ClassName").AsString(400)
               .WithColumn("GenericParameterAttributes").AsInt32().Nullable()
               .WithColumn("IsAbstract").AsBoolean().WithDefaultValue(true)
               .WithColumn("IsVisible").AsBoolean().WithDefaultValue(true)
               .WithColumn("IsNotPublic").AsBoolean().WithDefaultValue(true)
               .WithColumn("Attributes").AsInt32().Nullable()
               .WithColumn("IsNestedFamORAssem").AsBoolean().WithDefaultValue(true)
               .WithColumn("IsAutoLayout").AsBoolean().WithDefaultValue(true)
               .WithColumn("GUID").AsString(80).WithDefaultValue(Guid.NewGuid().ToString("N"))
               .WithColumn("IsValueType").AsBoolean().WithDefaultValue(false)
               .WithColumn("IsInterface").AsBoolean().WithDefaultValue(false)
               .WithColumn("IsGenericType").AsBoolean().WithDefaultValue(true)
               .WithColumn("IsArray").AsBoolean().WithDefaultValue(false)
               .WithColumn("IsSpecialName").AsBoolean().WithDefaultValue(false)
               .WithColumn("IsEnum").AsBoolean().WithDefaultValue(false)
               .WithColumn("IsClass").AsBoolean().WithDefaultValue(false)
               .WithColumn("IsSealed").AsBoolean().WithDefaultValue(false)
               .WithColumn("IsPublic").AsBoolean().WithDefaultValue(true)
               .WithColumn("IsPrivate").AsBoolean().WithDefaultValue(false)
               .WithColumn("Namespace").AsString(400).WithDefaultValue(false).Nullable()
               .WithColumn("AssemblyQualifiedName").AsString(400).Nullable()
               .WithColumn("FullName").AsString(700).Nullable()
                .WithColumn("VersionNum").AsString(100).Nullable()
               .WithColumn("AddTime").AsDateTime().Nullable();
            ;
            Create.Table(WholeTableName("ClassACon"))
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("VersionNum").AsString(100).Nullable()
                 .WithColumn("Name").AsString(700).Nullable()
                .WithColumn("Description").AsString(700).Nullable()
                .WithColumn("IsVirtual").AsBoolean().Nullable().WithDefaultValue(false)
                .WithColumn("Guid").AsString(80).WithDefaultValue(Guid.NewGuid().ToString("N"))
               .WithColumn("TypeDefinitionId").AsInt64().Nullable()
               .WithColumn("AddTime").AsDateTime().Nullable();
            ;

            Create.Table(WholeTableName("AssemblyACon"))
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("NameSpace").AsString(600).Nullable()
                 .WithColumn("Name").AsString(700).Nullable()
                 .WithColumn("Description").AsString(700).Nullable()
                .WithColumn("DllPath").AsString(1200).Nullable()
                  .WithColumn("VersionNum").AsString(100).Nullable()
                .WithColumn("Guid").AsString(80).WithDefaultValue(Guid.NewGuid().ToString("N"))
                .WithColumn("IsVirtual").AsBoolean().Nullable().WithDefaultValue(false)
                .WithColumn("AddTime").AsDateTime().Nullable();
            ;

             IfDatabase("mysql").Create.Table(WholeTableName("AConFlowStep"))
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("Guid").AsString(70).Nullable()
                  .WithColumn("AppName").AsString(200).WithDefaultValue("default")
                 .WithColumn("ModuleName").AsString(300).Nullable()
                 .WithColumn("Theme").AsString(200).Nullable()
                 .WithColumn("AComposityId").AsString(80)
                  .WithColumn("IsUsingParentConnstring").AsBoolean().Nullable()
                  .WithColumn("IsBuildIn").AsBoolean().Nullable()
                 .WithColumn("IsUsingCache").AsBoolean().WithDefaultValue(true)
                .WithColumn("DBType").AsInt32().Nullable()
                .WithColumn("Connectionstring").AsString(700).Nullable()
                 .WithColumn("StepOrder").AsInt32()
                .WithColumn("Name").AsString(700).Nullable()
                .WithColumn("Description").AsString(700).Nullable()
                .WithColumn("VersionNum").AsString(100).Nullable()
               .WithColumn("FlowStepType").AsInt32().Nullable()
                .WithColumn("ProxyCode").AsCustom("text").Nullable()
                 .WithColumn("OutPutText").AsCustom("text").Nullable()
                .WithColumn("ReferenceDlls").AsString(300).Nullable()
                .WithColumn("StoreValueProposal").AsInt32().Nullable()
            .WithColumn("ExcuteType").AsInt32()
               .WithColumn("GetValueProposal").AsInt32().Nullable()
                .WithColumn("TypeLib").AsString(100).Nullable()
                 .WithColumn("TypeLibform").AsString(100).Nullable()
                  .WithColumn("TypeFullName").AsString(400).Nullable()
                   .WithColumn("DllDirName").AsString(100).Nullable()
                     .WithColumn("MethodName").AsString(100).Nullable()
                   .WithColumn("StorePoolKey").AsString(100).Nullable()
                    .WithColumn("ArgNames").AsString(100).Nullable()
                    .WithColumn("InParamter1").AsString(100).Nullable()
                   .WithColumn("InParamter2").AsString(100).Nullable()
                    .WithColumn("InParamter3").AsString(100).Nullable()
                   .WithColumn("InParamter4").AsString(100).Nullable()
                    .WithColumn("InParamter5").AsString(100).Nullable()
                   .WithColumn("InParamter6").AsString(100).Nullable()
                    .WithColumn("InParamter7").AsString(100).Nullable()
                   .WithColumn("InParamter8").AsString(100).Nullable()
                .WithColumn("InParamter9").AsString(100).Nullable()
               .WithColumn("InParamter10").AsString(100).Nullable()
                .WithColumn("InstanceName").AsString(100).Nullable()
                 .WithColumn("NextRuntimeID").AsString(100).Nullable()
               .WithColumn("AddTime").AsDateTime().Nullable();

            IfDatabase("SqlServer", "Postgres","sqlite").
                    Create.Table(WholeTableName("AConFlowStep"))
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("Guid").AsString(70).Nullable()
                  .WithColumn("AppName").AsString(200).WithDefaultValue("default")
                 .WithColumn("ModuleName").AsString(300).Nullable()
                 .WithColumn("Theme").AsString(200).Nullable()
                 .WithColumn("AComposityId").AsString(80)
                  .WithColumn("IsUsingParentConnstring").AsBoolean().Nullable()
                  .WithColumn("IsBuildIn").AsBoolean().Nullable()
                 .WithColumn("IsUsingCache").AsBoolean().WithDefaultValue(true)
                .WithColumn("DBType").AsInt32().Nullable()
                .WithColumn("Connectionstring").AsString(700).Nullable()
                 .WithColumn("StepOrder").AsInt32()
                .WithColumn("Name").AsString(700).Nullable()
                .WithColumn("Description").AsString(700).Nullable()
                .WithColumn("VersionNum").AsString(100).Nullable()
               .WithColumn("FlowStepType").AsInt32().Nullable()
                .WithColumn("ProxyCode").AsCustom("ntext").Nullable()
                 .WithColumn("OutPutText").AsCustom("ntext").Nullable()
                .WithColumn("ReferenceDlls").AsCustom("ntext").Nullable()
                .WithColumn("StoreValueProposal").AsInt32().Nullable()
            .WithColumn("ExcuteType").AsInt32()
               .WithColumn("GetValueProposal").AsInt32().Nullable()
                .WithColumn("TypeLib").AsString(100).Nullable()
                 .WithColumn("TypeLibform").AsString(100).Nullable()
                  .WithColumn("TypeFullName").AsString(400).Nullable()
                   .WithColumn("DllDirName").AsString(100).Nullable()
                     .WithColumn("MethodName").AsString(100).Nullable()
                   .WithColumn("StorePoolKey").AsString(100).Nullable()
                    .WithColumn("ArgNames").AsString(100).Nullable()
                    .WithColumn("InParamter1").AsString(100).Nullable()
                   .WithColumn("InParamter2").AsString(100).Nullable()
                    .WithColumn("InParamter3").AsString(100).Nullable()
                   .WithColumn("InParamter4").AsString(100).Nullable()
                    .WithColumn("InParamter5").AsString(100).Nullable()
                   .WithColumn("InParamter6").AsString(100).Nullable()
                    .WithColumn("InParamter7").AsString(100).Nullable()
                   .WithColumn("InParamter8").AsString(100).Nullable()
                .WithColumn("InParamter9").AsString(100).Nullable()
               .WithColumn("InParamter10").AsString(100).Nullable()
                .WithColumn("InstanceName").AsString(100).Nullable()
                 .WithColumn("NextRuntimeID").AsString(100).Nullable()
               .WithColumn("AddTime").AsDateTime().Nullable();
            ;
            //IfDatabase("SqlServer").Alter.Table(WholeTableName("AConFlowStep"))
            //     .AlterColumn("ProxyCode").AsCustom("nvarchar(max)").Nullable()
            //     .AlterColumn("OutPutText").AsCustom("nvarchar(max)").Nullable(),

           Create.Table(WholeTableName("ComposeTemplate"))
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Name").AsString(700).Nullable()
                .WithColumn("StepIds").AsString(700).Nullable()
                .WithColumn("Isenable").AsBoolean().Nullable()
                .WithColumn("AddTime").AsDateTime().Nullable();
            ;


            Create.Table(WholeTableName("AConStateModule"))
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Name").AsString(700).Nullable()
                .WithColumn("Description").AsString(700).Nullable()
                .WithColumn("VersionNum").AsString(100).Nullable()
                .WithColumn("AddTime").AsDateTime().Nullable();
            ;


            Create.Table(WholeTableName("ComposeEntity"))
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("Guid").AsString(70).Nullable()
                .WithColumn("Name").AsString(700).Nullable()
                 .WithColumn("AppName").AsString(200).WithDefaultValue("default")
                 .WithColumn("Theme").AsString(200).Nullable()
                .WithColumn("RunMode").AsInt32()
                  .WithColumn("IsBuildIn").AsBoolean().Nullable()
                .WithColumn("IsUsingParentConnstring").AsBoolean().Nullable()
                .WithColumn("AppType").AsString(70).Nullable()
                .WithColumn("Connectionstring").AsString(700).Nullable()
               .WithColumn("TrigeMethod").AsString(100).Nullable()
                .WithColumn("TrigeType").AsString(100).Nullable()
                .WithColumn("Description").AsString(700).Nullable()
                    .WithColumn("Templateid").AsString(700).Nullable()
                //.WithColumn("RegisterGuid").AsString(80).WithDefaultValue(Guid.NewGuid().ToString("N"))
                .WithColumn("CalStackMD5").AsString(100).Nullable()
                .WithColumn("IsStatic").AsBoolean().Nullable().WithDefaultValue(true)
                .WithColumn("TransferParamterLib").AsString(300).Nullable()
                .WithColumn("TransferParamterPlantform").AsString(300).Nullable()
                .WithColumn("TransferParamterType").AsString(300).Nullable()
                .WithColumn("TransferParamterMethod").AsString(300).Nullable()
                .WithColumn("TransferParamterInArguments").AsString(300).Nullable()
                .WithColumn("RequsetEnviromentId").AsInt64().Nullable()
                .WithColumn("VersionNum").AsString(100).Nullable()
                .WithColumn("AddTime").AsDateTime().Nullable();
            ;

            Create.Table(WholeTableName("StepParamter"))
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Name").AsString(700).Nullable()
                 .WithColumn("ModuleId").AsInt64().Nullable()
                .WithColumn("FlowID").AsInt64().Nullable()
                .WithColumn("VersionNum").AsString(100).Nullable()
                 .WithColumn("GetValueWayID").AsInt64().Nullable()
               .WithColumn("AddTime").AsDateTime().Nullable();
            ;


            Create.Table(WholeTableName("ResouceInfo"))
             .WithColumn("Id").AsInt64().PrimaryKey().Identity()
              .WithColumn("FlowId").AsInt64().Nullable()
              .WithColumn("ModuleId").AsInt64().Nullable()
             .WithColumn("ComposeEntityId").AsInt64().Nullable()
             .WithColumn("KeyName").AsString(100)
            .WithColumn("Value").AsString(8000)
          .WithColumn("SerialObject").AsString(8000)
               .WithColumn("VersionNum").AsString(100).Nullable()
             .WithColumn("AddTime").AsDateTime().Nullable();
            ;



            Create.Table(WholeTableName("TrigerRuleInfo"))
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("VersionNum").AsString(100).Nullable()
                 .WithColumn("AppName").AsString(200).WithDefaultValue("default")
                .WithColumn("RuleName").AsString(100).Nullable()
                 .WithColumn("HttpMethod").AsString(20).Nullable().WithDefaultValue("Get")
                .WithColumn("ComposityId").AsString(70).Nullable()
                .WithColumn("Rule").AsString(100).Nullable()
                .WithColumn("IsEnable").AsBoolean().Nullable().WithDefaultValue(true)
                .WithColumn("AddTime").AsDateTime().Nullable();
            ;

            var s = new
            {
                Guid = "D506C9B6-AC0C-421A-BD6F-BD94A92DA418",
                AppName = "default",
                AddTime = DateTime.Now,
                RunMode = 0,
                IsBuildIn = true,
                Theme = "default",
                IsUsingParentConnstring = true,
                Connectionstring = "sys_default",
                AppType = "aspnetcore",
                Description = "defaultweb",
                TrigeType = "urlreg",
                Name = "defaultweb",

            };

            Insert.IntoTable(WholeTableName("ComposeEntity")).Row(s);
            var ste = new
            {
                Guid = "3E3F7118-8282-4476-A903-7400336B9A9F",
                AddTime = DateTime.Now,
                AppName = "default",
                IsBuildIn = true,
                FlowStepType = 0,
                ExcuteType = 0,
                GetValueProposal = 5,
                IsUsingParentConnstring = true,
                AComposityId = s.Guid,
                Description = "defaultweb_Setp1",
                StepOrder = 0,
                InParamter1 = "select * from  " + WholeTableName("TrigerRuleInfo"),
                StorePoolKey = "allrules",
                //StoreValueType = typeof(List<ComposeEntity>).FullName
            };
            Insert.IntoTable(WholeTableName("AConFlowStep")).Row(ste);

            //var ste2 = new
            //{
            //    Guid = "38E13CBA-4456-43B7-B3B3-A7611F738C16",
            //    AddTime = DateTime.Now,
            //    AppName = "default",
            //    IsBuildIn = true,
            //    FlowStepType = 4,
            //    ExcuteType = 0,
            //    GetValueProposal = 4,
            //    IsUsingParentConnstring = true,
            //    AComposityId = s.Guid,
            //    IsUsingCache = false,
            //    Description = "defaultweb_Setp2",
            //    StepOrder = 1,
            //    TypeLib = "BasicPlugin.dll",
            //    MethodName = "GetRequestUrl",
            //    TypeFullName = "BasicPlugin.HttpUtility",
            //    StorePoolKey = "url",
            //    ArgNames = "__actioncontext",
            //};
            //Insert.IntoTable(WholeTableName("AConFlowStep")).Row(ste2);
            //var ste33 = new
            //{
            //    Guid = "1d9F",
            //    AddTime = DateTime.Now,
            //    IsBuildIn = true,
            //    FlowStepType = 2,
            //    AppName = "default",
            //    ExcuteType = 0,
            //    GetValueProposal = 5,
            //    IsUsingParentConnstring = true,
            //    AComposityId = s.Guid,
            //    StepOrder = 2,
            //    ProxyCode = "BasicPlugin.QueryGeneratorcs.test( \"3 \",\"4\")",
            //    TypeLib = "BasicPlugin.dll",
            //    StorePoolKey = "",
            //    //StoreValueType = typeof(List<ComposeEntity>).FullName
            //};
            //Insert.IntoTable(WholeTableName("AConFlowStep")).Row(ste33);
            var ste3 = new
            {
                Guid = "BBF6B4BC-AEDA-4607-83ED-406E8BB67351",
                AddTime = DateTime.Now,
                IsBuildIn = true,
                FlowStepType = 4,
                AppName = "default",
                ExcuteType = 0,
                GetValueProposal = 4,
                IsUsingCache = false,
                IsUsingParentConnstring = true,
                AComposityId = s.Guid,
                Description = "defaultweb_Setp2",
                StepOrder = 2,
                TypeLib = "BasicPlugin.dll",
                MethodName = "GetIsTriger",
                TypeFullName = "BasicPlugin.HttpUtility",
                ArgNames = "allrules,__httpcontext",
                StorePoolKey = "_NextRunTimeKey",
            };
            Insert.IntoTable(WholeTableName("AConFlowStep")).Row(ste3);


            Insert.IntoTable(WholeTableName("TrigerRuleInfo")).Row(new { RuleName = "default", HttpMethod = "GET", Rule = @"\/test", ComposityId = "1111" });
            Insert.IntoTable(WholeTableName("TrigerRuleInfo")).Row(new { RuleName = "default", HttpMethod = "GET", Rule = @"^\/$", ComposityId = "_fact" });
            Insert.IntoTable(WholeTableName("TrigerRuleInfo")).Row(new { RuleName = "pay", HttpMethod = "GET", Rule = @"\/pay", ComposityId = "1e1cb869-2512-8dfc-81ee-ed3150836182" });
            Insert.IntoTable(WholeTableName("TrigerRuleInfo")).Row(new { RuleName = "hellorazor", HttpMethod = "GET", Rule = @"\/r", ComposityId = "e984ff96-c112-7133-3c00-df09f7338338" });
            var s_fact = new
            {
                Guid = "_fact",
                Theme = "default",
                AddTime = DateTime.Now,
                IsBuildIn = false,
                AppName = "default",
                IsUsingParentConnstring = true,
                Connectionstring = "sys_default",
                RunMode = 0,
                AppType = "aspnetcore",
                Description = "defaultweb_fact",
                TrigeType = "urlreg",
                Name = "defaultweb_fact",

            };
            Insert.IntoTable(WholeTableName("ComposeEntity")).Row(s_fact);
            var ste_fact = new
            {
                Guid = "_factS1",
                AppName = "default",
                AddTime = DateTime.Now,
                IsBuildIn = true,
                FlowStepType = 2,
                ExcuteType = 0,
                GetValueProposal = 5,
                IsUsingParentConnstring = true,
                AComposityId = s_fact.Guid,
                StepOrder = 2,
                ProxyCode = "\"hello\"",
                TypeLib = "BasicPlugin.dll",
                StorePoolKey = "restext",
                //StoreValueType = typeof(List<ComposeEntity>).FullName
            };
            Insert.IntoTable(WholeTableName("AConFlowStep")).Row(ste_fact);
            var ste_fact1 = new
            {
                Guid = "_factS2",
                AddTime = DateTime.Now,
                AppName = "default",
                IsBuildIn = true,
                FlowStepType = 4,
                ExcuteType = 0,
                GetValueProposal = 4,
                IsUsingParentConnstring = true,

                AComposityId = s_fact.Guid,
                Description = "defaultweb_facSetp2",
                StepOrder = 3,
                TypeLib = "BasicPlugin.dll",
                MethodName = "ResponseText",
                TypeFullName = "BasicPlugin.HttpUtility",
                ArgNames = "__actioncontext,restext",
                StorePoolKey = "",
            };
            Insert.IntoTable(WholeTableName("AConFlowStep")).Row(ste_fact1);


            var s_pay = new
            {
                Guid = "1e1cb869-2512-8dfc-81ee-ed3150836182",

                AddTime = DateTime.Now,
                IsBuildIn = false,
                AppName = "default",
                IsUsingParentConnstring = true,
                Connectionstring = "sys_default",
                RunMode = 0,
                AppType = "aspnetcore",
                Description = "paycom",
                Theme = "default",
                Name = "paycom",

            };

            Insert.IntoTable(WholeTableName("ComposeEntity")).Row(s_pay);

            var ste_s_pay1 = new
            {
                Guid = "d6f55a47-0b6f-b30e-d7cc-724c24705bd4",
                AddTime = DateTime.Now,
                AppName = "default",
                IsBuildIn = false,
                FlowStepType = 2,
                ExcuteType = 0,
                GetValueProposal = 4,
                IsUsingParentConnstring = true,
                ProxyCode = @"new { Body=""APP支付描述信息"",
Subject = ""APP支付测试"",
                TotalAmount = ""0.01"",
                OutTradeNo = System.DateTime.Now.ToString(""yyyyMMddHHmmssfff""),
                ProductCode = ""FAST_INSTANT_TRADE_PAY"",
                NotifyUrl = ""http://localhost:6008/alipay/pagepayreturn"",
                ReturnUrl = ""http://ocalhost:6008/notify/alipay/pagepay""
            }",
                AComposityId = s_pay.Guid,
                Description = "pay_step1",
                StepOrder = 0,
                Theme = "default",
                StorePoolKey = "model",
            };
            Insert.IntoTable(WholeTableName("AConFlowStep")).Row(ste_s_pay1);

            var ste_s_pay2 = new
            {
                Guid = "af89bbba-dd09-711d-a88f-b794a6be63ae",
                AddTime = DateTime.Now,
                AppName = "default",
                IsBuildIn = false,
                FlowStepType = 4,
                ExcuteType = 0,
                GetValueProposal = 4,
                IsUsingParentConnstring = true,
                Description = "ste_s_pay_step2",
                StepOrder = 1,
                TypeLib = "AlipayPlugin.dll",
                MethodName = "PagePay",
                TypeFullName = "AlipayPlugin.AliDevelop",
                ArgNames = "model",
                AComposityId = s_pay.Guid,
                StorePoolKey = "resonse",
                Theme = "default",
            };
            Insert.IntoTable(WholeTableName("AConFlowStep")).Row(ste_s_pay2);



            var ste_s_pay3 = new
            {
                Guid = "0769f3fe-579b-c1c3-a4a2-bc10000f9d31",
                AddTime = DateTime.Now,
                AppName = "default",
                IsBuildIn = false,
                FlowStepType = 4,
                ExcuteType = 0,
                GetValueProposal = 4,
                IsUsingParentConnstring = true,
                Description = "ste_s_pay_step3",
                StepOrder = 2,
                TypeLib = "BasicPlugin.dll",
                MethodName = "ResponseText",
                TypeFullName = "BasicPlugin.HttpUtility",
                ArgNames = "__actioncontext,resonse",
                AComposityId = s_pay.Guid,
                Theme = "default",
            };

            Insert.IntoTable(WholeTableName("AConFlowStep")).Row(ste_s_pay3);

            var s_razor = new
            {
                Guid = "e984ff96-c112-7133-3c00-df09f7338338",

                AddTime = DateTime.Now,
                IsBuildIn = false,
                AppName = "default",
                IsUsingParentConnstring = true,
                Connectionstring = "sys_default",
                RunMode = 0,
                AppType = "aspnetcore",
                Description = "razor",
                Name = "razor",
                Theme = "default",
            };

            Insert.IntoTable(WholeTableName("ComposeEntity")).Row(s_razor);


            var ste_razor1 = new
            {
                Guid = "75b50d33-ccfd-f398-e395-3e2df84550f7",
                AddTime = DateTime.Now,
                AppName = "default",
                IsBuildIn = false,
                FlowStepType = 2,
                ExcuteType = 0,
                GetValueProposal = 4,
                IsUsingParentConnstring = true,
                ProxyCode = @"new {Name=""ttttttttttttttttttt <span style =\""color:red\"" >tttttt</span>tttt""}",
                AComposityId = s_razor.Guid,
                Description = "ste_razor1",
                StepOrder = 0,
                Name = "makemodel",
                StorePoolKey = "model",
                Theme = "default",
            };
            Insert.IntoTable(WholeTableName("AConFlowStep")).Row(ste_razor1);

            var ste_razor2 = new
            {
                Guid = "364f2237-87de-068e-957f-388e65aab63e",
                AddTime = DateTime.Now,
                AppName = "default",
                IsBuildIn = false,
                FlowStepType = 13,
                ExcuteType = 0,
                GetValueProposal = 4,
                IsUsingParentConnstring = true,
                ProxyCode = @"@{ 
Page.Session(""key"",""sdfsdfd"");
            }
Hello, @Model.Item.Name .Welcome to RazorLight repository

<br/>
@Page.Session(""key"")",
                AComposityId = s_razor.Guid,
                Description = "ste_razor2",
                StepOrder = 1,
                Name = "getresult",
                ArgNames = "model",
                StorePoolKey = "restext",
                Theme = "default",
            };
            Insert.IntoTable(WholeTableName("AConFlowStep")).Row(ste_razor2);


            var ste_razor3 = new
            {
                Guid = "3bfb7e30-ad3d-db7e-0dd4-65f84fd9364d",
                AddTime = DateTime.Now,
                AppName = "default",
                IsBuildIn = true,
                FlowStepType = 4,
                ExcuteType = 0,
                GetValueProposal = 4,
                IsUsingParentConnstring = true,
                Description = "ste_razor3",
                StepOrder = 2,
                TypeLib = "BasicPlugin.dll",
                MethodName = "ResponseText",
                TypeFullName = "BasicPlugin.HttpUtility",
                ArgNames = "__actioncontext,restext",
                Name = "reponse",
                AComposityId = s_razor.Guid,
                Theme = "default",
            };

            Insert.IntoTable(WholeTableName("AConFlowStep")).Row(ste_razor3);

            //var aGateWay_fact = new
            //{
            //    Guid = "9CBCEE00-2AB2-40D3-8406-4A1B923432cc",
            //    AddTime = DateTime.Now,
            //    AComposityId = s_fact.Guid,
            //    AConFlowStepId = ste_fact.Guid,
            //    InParamter1 = "select * from TrigerRuleInfo",
            //    StorePoolKeys = "url",
            //    PoolKey1 = "__actioncontext",
            //    //StorePoolsKeys = "allrules",
            //    //StoreValueType = typeof(ComposeEntity).FullName
            //};
            //Insert.IntoTable("AConGetWay").Row(aGateWay);
            Create.Table(WholeTableName("AdminUser"))
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Guid").AsString(50).Nullable()
                 .WithColumn("UserName").AsString(100).Nullable()
                .WithColumn("Password").AsString(100).Nullable()
                .WithColumn("IsDisable").AsBoolean().NotNullable()
               .WithColumn("AddTime").AsDateTime().Nullable();
            ;
            var password = "admin";
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                var strResult = BitConverter.ToString(result);
                password = strResult.Replace("-", "");

            }
            var addAdminUser = new
            {
                Guid = "BBF6B4BC-AECA-4699-831D-4A6E8BB6CS51",
                AddTime = DateTime.Now,
                UserName = "admin",
                Password = password,
                IsDisable = false,
            };
            Insert.IntoTable(WholeTableName("AdminUser")).Row(addAdminUser);


            Create.Table(WholeTableName("DefaultUser"))
             .WithColumn("Id").AsInt64().PrimaryKey().Identity()
              .WithColumn("Guid").AsString(50).Nullable()
               .WithColumn("UserName").AsString(100).Nullable()
              .WithColumn("Password").AsString(100).Nullable()
              .WithColumn("IsDisable").AsBoolean().NotNullable()
             .WithColumn("AddTime").AsDateTime().Nullable();
            ;
            var defaultUserpassword = "123";
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(defaultUserpassword));
                var strResult = BitConverter.ToString(result);
                defaultUserpassword = strResult.Replace("-", "");

            }
            var addDefaultUser = new
            {
                Guid = "BBF6B4BC-AECA-4699-831D-4A6E8BB6CS51",
                AddTime = DateTime.Now,
                UserName = "user",
                Password = defaultUserpassword,
                IsDisable = false,
            };
            Insert.IntoTable(WholeTableName("DefaultUser")).Row(addDefaultUser);

            Create.Table(WholeTableName("SystemDictionary"))
               .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("KeyName").AsString(450).Nullable()
                 .WithColumn("Value").AsString(900).Nullable()
                .WithColumn("SystemDictionarytype").AsInt32()
                .WithColumn("IsSystem").AsBoolean().NotNullable()
               .WithColumn("AddTime").AsDateTime().Nullable();
            ;

            var SystemDictionary1 = new
            {
                KeyName = "security",
                Value = "security",
                IsSystem = true,
                SystemDictionarytype = 0,
                AddTime = DateTime.Now,
            };
            Insert.IntoTable(WholeTableName("SystemDictionary")).Row(SystemDictionary1);
        }

        public override void Down()
        {
            Delete.Table(WholeTableName("SystemACon"));
            Delete.Table(WholeTableName("SystemRegitionInfo"));
            Delete.Table(WholeTableName("FieldInfoDefinition"));
            Delete.Table(WholeTableName("ProperityDefinition"));
            Delete.Table(WholeTableName("MethodDefinition"));
            Delete.Table(WholeTableName("TypeDefinition"));
            Delete.Table(WholeTableName("ClassACon"));
            Delete.Table(WholeTableName("AssemblyACon"));
            Delete.Table(WholeTableName("AConStateModule"));
            Delete.Table(WholeTableName("AuthorizeInfo"));
            Delete.Table(WholeTableName("AConStateFlow"));
            Delete.Table(WholeTableName("AConFlowStep"));
            Delete.Table(WholeTableName("GetWay"));
            Delete.Table(WholeTableName("StepParamter"));
            Delete.Table(WholeTableName("TrigerRuleInfo"));
            Delete.Table(WholeTableName("ComposeEntity"));
            Delete.Table(WholeTableName("SystemRegistionInfo"));
            Delete.Table(WholeTableName("AdminUser"));
            Delete.Table(WholeTableName("DefaultUser"));
            Delete.Table(WholeTableName("SystemDictionary"));
        }
    }
}
