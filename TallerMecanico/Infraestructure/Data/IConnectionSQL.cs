using System.Data;

namespace TallerMecanico.Infraestructure.Data
{
    public interface IConnectionSQL
    {
        IDbConnection GetConnection();
    }
}
