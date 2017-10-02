using System.Linq;
using Qwiq.Exceptions;

namespace Qwiq.Client.Soap
{
    public class QueryFolder : Qwiq.QueryFolder
    {
        public QueryFolder(Microsoft.TeamFoundation.WorkItemTracking.Client.QueryFolder queryFolder)
            : base(
                queryFolder.Id,
                queryFolder.Name,
                new QueryFolderCollection(() =>
                {
                    return queryFolder.OfType<Microsoft.TeamFoundation.WorkItemTracking.Client.QueryFolder>()
                        .Select(qf => ExceptionHandlingDynamicProxyFactory.Create<IQueryFolder>(new QueryFolder(qf)));
                }), new QueryDefinitionCollection(() =>
                {
                    return queryFolder.OfType<Microsoft.TeamFoundation.WorkItemTracking.Client.QueryDefinition>()
                        .Select(qd => ExceptionHandlingDynamicProxyFactory.Create<IQueryDefinition>(new QueryDefinition(qd)));
                }))
        {
        }
    }
}