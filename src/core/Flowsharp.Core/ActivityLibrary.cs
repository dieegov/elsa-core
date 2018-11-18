using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flowsharp.Models;

namespace Flowsharp
{
    public class ActivityLibrary : IActivityLibrary
    {
        private readonly IEnumerable<IActivityProvider> providers;

        public ActivityLibrary(IEnumerable<IActivityProvider> providers)
        {
            this.providers = providers;
        }
        
        public async Task<IEnumerable<Models.ActivityDescriptor>> GetActivitiesAsync(CancellationToken cancellationToken)
        {
            var tasks = providers.Select(x => x.GetActivitiesAsync(cancellationToken));
            var descriptorsList = await Task.WhenAll(tasks);
            var descriptors = descriptorsList.SelectMany(x => x);

            return descriptors;
        }

        public async Task<Models.ActivityDescriptor> GetActivityByNameAsync(string name, CancellationToken cancellationToken)
        {
            var descriptors = await GetActivitiesAsync(cancellationToken);
            return descriptors.SingleOrDefault(x => x.Name == name);
        }
    }
}