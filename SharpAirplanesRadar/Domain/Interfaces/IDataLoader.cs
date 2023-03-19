using System.Threading.Tasks;

namespace SharpAirplanesRadar
{
    internal interface IDataLoader
    {
        Task<string> Load(string customUrl = null);
    }
}
