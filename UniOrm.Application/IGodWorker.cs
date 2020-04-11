using System.Threading.Tasks;
using UniOrm.Model; 
namespace UniOrm.Application
{
    public interface IGodWorker
    {
        string WorkerName { get; set; }
        TypeDefinition G(string typeName);
        //DefaultModuleManager ModuleManager { get; set; }
        Task  Run(params object[] parameters);
    }
}
