using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using Fasterflect; 
using UniOrm.Model;
using UniOrm.Core;
using UniOrm.Common;

namespace UniOrm.Application
{
    public class ComposeBuilder
    { 
        public ComposeBuilder( )
        {
            
        }

        public ComposeEntity NewComposy(string name, string discription = null)
        {
            var guid = Guid.NewGuid().ToString();
            var cc = new ComposeEntity()
            {
                AddTime = DateTime.Now,
                AppType = "aspnetcore",
                Description = discription,
                IsBuildIn = false,
                TrigeType = "urlreg",
                Connectionstring = "sys_default",
                IsUsingParentConnstring = true,
                Name = name,
                Guid = guid
            }; 
            var reint = DB.UniClient.Insertable (cc).ExecuteCommand();
            return DB.UniClient.Queryable<ComposeEntity>().Where(p => p.Guid == guid).First(); 
        }

        public AConFlowStep AddMathodStep(bool isBuildIn, string comguidi, string dllName, string typeName, string methodInfoName,
           string returnValueName = null, List<string> Inparas = null)
        {
            var methodInfo = APP.GetMethodFromConfig(isBuildIn, dllName, typeName, methodInfoName);
            return AddMathodStep(comguidi, methodInfo, returnValueName, Inparas);
        }

        public AConFlowStep AddMathodStep(string comguidi, MethodInfo methodInfo,
            string returnValueName = null, List<string> Inparas = null)
        {
            var all = methodInfo.GetParameters();
            string arags = string.Empty;
            for (var i = 0; i < all.Count(); i++)
            {
                if (all[i].IsIn)
                {
                    arags += all[i].Name + ",";
                }
            }
            arags = arags.Trim(',');
            var guid = Guid.NewGuid().ToString();
            var refsName = methodInfo.ReflectedType.Assembly.GetReferencedAssemblies();
            string refsAll = string.Empty;
            for (var i = 0; i < refsName.Count(); i++)
            {
                refsAll += refsName[i].Name + ",";
            }
            refsAll = refsAll.Trim(',');

            var cc = new AConFlowStep()
            {
                AddTime = DateTime.Now,
                AComposityId = comguidi,
                FlowStepType = FlowStepType.CallMethod,
                ArgNames = arags,
                Connectionstring = "sys_default",
                IsUsingParentConnstring = true,
                Name = guid,
                Guid = guid,
                ExcuteType = ExcuteType.Syn,
                MethodName = methodInfo.Name,
                ReferenceDlls = refsAll,
                TypeLib = methodInfo.ReflectedType.Assembly.FullName,
                TypeFullName = methodInfo.ReflectedType.FullName,
                StorePoolKey = returnValueName,
                StoreValueProposal = StoreValueProposal.ComposityPool
            };
            if (Inparas != null)
            {
                for (var i = 0; i < 9 && i < Inparas.Count; i++)
                {
                    cc.SetPropertyValue("InParamter" + (i + 1), Inparas[i]);
                }
            }

            var allmax = DB.UniClient.Queryable<AConFlowStep>().Where(p => p.AComposityId == comguidi).ToList();
            var maxOrder = 0;
            if (allmax != null)
            {
                maxOrder = allmax.Max(p => p.StepOrder) + 1;
            }
            cc.StepOrder = maxOrder;
            var reint = DB.UniClient.Insertable(cc).ExecuteCommand();
            return DB.UniClient.Queryable<AConFlowStep>().Where(p => p.Guid == guid).First();

        }



    }



}
