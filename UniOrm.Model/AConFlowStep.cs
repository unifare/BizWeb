using System;
using System.Collections.Generic;
using System.Text;
namespace UniOrm.Model
{
    public enum FlowStepType
    {
        GetData,
        Set,
        Declare,
        CallMethodAndStore, 
        CallMethod,
        Action,
        Function,
        Return,
        IF,
        Else,
        ElseIF,
        StaiticFile,
        Text,
        RazorText,
        RazorFile,
        TemplateText,
        TemplateFile,
        TemplateRazorText,
        ServerTransfer,
        Redirector,
        UrlRewriter
    }
    public enum ExcuteType
    {
        Syn,
        Asyn,
        Threading,

    }

    public enum GetValueProposal
    {
        Constant = 0,
        StepResourcePool = 1,
        FlowResourcePool = 2,
        AConCachePool = 3,
        CallMethod = 4,
        UnverDB = 5,
        DynamicProxy = 4,
        XML = 2001,
        Json = 2002,
        Mssql = 1001,
        Mysql = 1002,
        Sqlite = 1003,
        Postgre = 1004,
        Oracle = 1005,
        Http = 30001,
        TCP_IP = 30002,
        WebSocket = 30003
    }

    public enum StoreValueProposal
    {
        ComposityPool = 0,
        InstanceFlowPool,
        StepStaticResourcePool,
        StaticFlowResourcePool,
        SessionPool,
        ModulePool,
        AssemblyPool,
        CookiesPool,
        AConCachePool,
        AConStaticPool,
    }

    public class AConFlowStep
    {
        public AConFlowStep()
        {
            IsBuildIn = true;
            IsUsingCache = false;
            AddTime = DateTime.Now;
            IsUsingParentConnstring = true;
            Theme = "default";
        }
        public long Id { get; set; }
        public string AppName { get; set; } = "default";
        public bool IsUsingCache { get; set; }
        public bool? IsUsingAuth { get; set; } = false;
        public string UserName { get; set; }
        public string LoginUrl { get; set; } 
        public string UserRole { get; set; }
        public string Guid { get; set; }
        public string AComposityId { get; set; }
        public string VersionNum { get; set; }
        public int StepOrder { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OutPutText { get; set; }

        public string ModuleName { get; set; }
        public string Theme { get; set; }
        public FlowStepType FlowStepType { get; set; }
        public ExcuteType ExcuteType { get; set; }
        public GetValueProposal? GetValueProposal { get; set; }
        public string ReferenceDlls { get; set; }
        public string Connectionstring { get; set; }
        public DBType? DBType { get; set; }
        public bool? IsBuildIn { get; set; }
        public bool? IsUsingParentConnstring { get; set; }
        public StoreValueProposal? StoreValueProposal { get; set; }
        public string ProxyCode { get; set; }
        public string TypeLib { get; set; }
        public string TypeLibform { get; set; }
        public string TypeFullName { get; set; }
        public string DllDirName { get; set; }
        public string MethodName { get; set; }
        public string StorePoolKey { get; set; } 
        public string ArgNames { get; set; }
        public string InParamter1 { get; set; }
        public string InParamter2 { get; set; }
        public string InParamter3 { get; set; }
        public string InParamter4 { get; set; }
        public string InParamter5 { get; set; }
        public string InParamter6 { get; set; }
        public string InParamter7 { get; set; }
        public string InParamter8 { get; set; }
        public string InParamter9 { get; set; }
        public string InParamter10 { get; set; }
        public string InstanceName { get; set; }
        public string NextRuntimeID { get; set; }

        public DateTime? AddTime { get; set; }
    }
}
