using System.Threading.Tasks;

namespace NiceAirplanesRadar
{
    internal interface IDataLoader
    {
        Task<string> Load(string customUrl = null);
    }
}
