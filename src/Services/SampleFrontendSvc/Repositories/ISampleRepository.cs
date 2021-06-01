using System.Threading.Tasks;

namespace SampleFrontendSvc.Repositories
{
    /// <summary>
    /// Sample interface to get the data. You can customize your own interface function.
    /// </summary>
    public interface ISampleRepository
    {
        public Task<string> Get(string data);
        public Task<string> Save(string data);
    }
}
