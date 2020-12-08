using System.Threading.Tasks;

namespace Beacon.Client
{
    public interface IIPUpdateService
    {
        public Task UpdateMyIPAsync();
    }
}
