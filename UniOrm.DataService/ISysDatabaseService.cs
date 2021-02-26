 
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace UniOrm.Model.DataService
{
    public interface ISysDatabaseService : IDisposable
    {
        bool IsOpenSessionEveryTime { get; set; }
        SystemACon GetSystemACon();
        List<ComposeEntity> GetConposity(string id, string name = null);
        TypeDefinition GetTypeDefinition(string typeName);
        int InsertCode<T>(T objcode) where T : class, new();
        bool DeleteSimpleCode<T>(object simplequery) where T : class, new();
        List<AConFlowStep> GetAConStateSteps(string id);
        List<T> GetSimpleCode<T>(object simplequery=null) where T : class, new();
        QueryResult GetSimpleCodePage<T>(object simplequery, int pageindex, int pagesize) where T : class, new();
        List<T> GetSimpleCodeLinq<T>(Expression<Func<T, bool>> predicate) where T : class, new();

        int UpdateSimpleCode<T>(T obj) where T : class, new();
        int UpdateSimpleCode(object obj);

        AdminUser GetAdminUser(string username, string password);

        DefaultUser GetDefaultUser(string username, string password);
    }
}
