using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using UniOrm;
using SqlSugar;
using UniOrm.Common;

namespace UniOrm.Model.DataService
{
    public class SysDatabaseService : ISysDatabaseService
    {
        ISqlSugarClient Db;
        public const string LoggerName = "CodeService";  
        public bool IsOpenSessionEveryTime { get; set; } 
        public SysDatabaseService(SqlSugar.ISqlSugarClient db )
        {
            Db = db;

        }

       
        public SystemACon GetSystemACon()
        { 
            return Db.Queryable<SystemACon>().First(); 
            //}
        }
        public int InsertCode<T>(T objcode) where T : class, new()
        { 
            return  Db.Insertable<T>(objcode).ExecuteCommand();
        
        }

        public TypeDefinition GetTypeDefinition(string typeName)
        { 
            var q = Db.Queryable<TypeDefinition>().Where( p=>p.AliName ==  typeName);
            var typeds = q.ToList ();
            if (typeds.Count() == 0)
            {
                throw new Exception("TypeDefinition ali name " + typeName + " is shown more than twice. ");
            }
            else if (typeds.Count() > 1)
            {
                throw new Exception("TypeDefinition ali name " + typeName + " is shown more than twice. ");
            }
            else
            {
                return typeds.ToList()[0];
            }
        }

        public AdminUser GetAdminUser(string username, string password)
        { 
            var typeds = Db.Queryable<AdminUser>().Where(p => p.UserName == username && p.Password == password).ToList();
            if (typeds.Count() == 0)
            {
                Logger.LogError(LoggerName, "AdminUser   name " + username + " is not found ", new Exception("AdminUser   name " + username + " is shown more than twice. "));
                return null;
            }
            else if (typeds.Count() > 1)
            {
                Logger.LogError(LoggerName, "AdminUser   name " + username + " is shown more than twice. ", new Exception("AdminUser   name " + username + " is shown more than twice. "));
                return null;
            }
            else
            {
                return typeds.ToList()[0];
            }
        }
        public DefaultUser GetDefaultUser(string username, string password)
        { 
            var typeds = Db.Queryable<DefaultUser>().Where(p => p.UserName == username && p.Password == password);
            if (typeds.Count() == 0)
            {
                Logger.LogError(LoggerName, "DefaultUser   name " + username + " is not found ", new Exception("DefaultUser   name " + username + " is shown more than twice. "));
                return null;
            }
            else if (typeds.Count() > 1)
            {
                Logger.LogError(LoggerName, "DefaultUser   name " + username + " is shown more than twice. ", new Exception("DefaultUser   name " + username + " is shown more than twice. "));
                return null;
            }
            else
            {
                return typeds.ToList()[0];
            }
        }
        public List<AConFlowStep> GetAConStateSteps(string stepflowid)
        { 
            var oneobject = new List<AConFlowStep>(); ;
            var basequery = Db.Queryable< AConFlowStep>( );

            if (stepflowid != null)
            {
                basequery.Where(p=>p.AComposityId == stepflowid);

            }

            oneobject = basequery.ToList ();
            if (oneobject == null)
            {
                oneobject = new List<AConFlowStep>();
            }
            return oneobject;
        }

        public List<ComposeEntity> GetConposity(string id, string name = null)
        { 
            var oneobject = new List<ComposeEntity>();
            var basequery = Db.Queryable<ComposeEntity>( );
            if (id != null)
            {
                basequery.Where(p=>p.Guid== id);

            }
            if (name != null)
            {
                basequery.Where(nameof(ComposeEntity.Name), name);

            }
            oneobject = basequery.ToList ();
            if (oneobject == null)
            {
                oneobject = new List<ComposeEntity>();
            }
            return oneobject;
        }

        public QueryResult GetSimpleCodePage<T>(object simplequery, int pageindex, int pagesize) where T : class, new()
        {
            //dbFactory.EFCore<pigcms_adma>().CreateDefaultInstance();
            var basequery = Db.Queryable<T>();
            if (simplequery != null)
            {
                List<IConditionalModel> conModels = new List<IConditionalModel>();
                var t = simplequery.GetType();
                foreach (var pi in t.GetProperties())
                {
                    conModels.Add(new ConditionalModel() { FieldName = pi.Name, ConditionalType = ConditionalType.Equal, FieldValue = pi.GetValue(simplequery).ToSafeString() });
                }
                if (simplequery != null)
                {
                    basequery = basequery.Where(conModels);
                }
            }
            int totalnum = 0;
            if(pagesize==0)
            {
                pagesize = 30;
            }
            var oneobject = basequery.ToPageList(pageindex, pagesize, ref totalnum);
            if (oneobject != null)
            {
                return new QueryResult() { currentIndex = pageindex, DataList = oneobject, PageSize= pagesize , TotalPage = totalnum };
            }
            return new QueryResult();
        }

        public List<T> GetSimpleCode<T>(object simplequery=null) where T : class, new()
        { 
            var oneobject = new List<T>();
            var basequery = Db.Queryable<T>();
            if (simplequery != null)
            {
                List<IConditionalModel> conModels = new List<IConditionalModel>();
                var t = simplequery.GetType();
                foreach (var pi in t.GetProperties())
                {
                    conModels.Add(new ConditionalModel() { FieldName = pi.Name, ConditionalType = ConditionalType.Equal, FieldValue = pi.GetValue(simplequery).ToSafeString() });
                }   ;
                basequery.Where(conModels);
            }
            oneobject = basequery.ToList ();
            if (oneobject == null)
            {
                oneobject = new List<T>();
            }

            //oneobject = Db.Ado.SqlQuery<T>("select * from " + typeof(T).Name);
            return oneobject;
        }

        public bool DeleteSimpleCode<T>(object simplequery) where T : class, new()
        { 
            var reint = Db.Deleteable<T>(simplequery).ExecuteCommand();
            return reint > 0;
        }
        public List<T> GetSimpleCodeTyped<T>(object simplequery) where T : class, new()
        { 
            var oneobject = new List<T>();
            var basequery = Db.Queryable<T>();
            if (simplequery != null)
            {
                if (simplequery != null)
                {
                    List<IConditionalModel> conModels = new List<IConditionalModel>();
                    var t = simplequery.GetType();
                    foreach (var pi in t.GetProperties())
                    {
                        conModels.Add(new ConditionalModel() { FieldName = pi.Name, ConditionalType = ConditionalType.Equal, FieldValue = pi.GetValue(simplequery).ToSafeString() });
                    };
                    basequery.Where(conModels);
                }
            }
            oneobject = basequery.ToList ();
            if (oneobject == null)
            {
                oneobject = new List<T>();
            }
            return oneobject;
        }

        public  List<T> GetSimpleCodeLinq<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        { 
            return Db.Queryable<T>().Where(predicate).ToList();
        }

        public int UpdateSimpleCode<T>(T obj) where T : class, new()
        { 
            return Db.Updateable<T>(obj).ExecuteCommand();
        }
        public int UpdateSimpleCode (object obj) 
        {
            return Db.Updateable (obj).ExecuteCommand();
        }
        public void Dispose()
        {
             
        }
    }

}
