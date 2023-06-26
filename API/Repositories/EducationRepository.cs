using API.Data;
using API.Models;
using API.Contracts;

namespace API.Repositories;

public class EducationRepository : GeneralRepository<Education>, IEducationRepository
{
    public EducationRepository(BookingDBContext context) : base(context) { }
}
