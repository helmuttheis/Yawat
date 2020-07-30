namespace Yawat.Cli.Data
{
    using System.Threading.Tasks;

    public interface IGenericData
    {
        Task<int> Count();

        Task<long> Insert();

        Task<long> Delete();

        Task Update();
    }
}
