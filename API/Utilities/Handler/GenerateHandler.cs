using API.Contracts;
using API.Repositories;

namespace API.Utilities.Handler;

public class GenerateHandler
{
    public static string GenerateNIK(IEmployeeRepository employeeRepository,string nik)
    {
        var entities = employeeRepository.GetAll();
        if (!entities.Any() || entities is null) return "11111";

        if (nik == "")
        {
            if (int.TryParse(entities.Last().NIK, out int lastNIK))
            {
                return (lastNIK + 1).ToString();
            }
        }
        return nik;
    }
}
