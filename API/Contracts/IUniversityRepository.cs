﻿using API.Models;

namespace API.Contracts;

public interface IUniversityRepository
{
    IEnumerable<University> GetByName(string name);
    ICollection<University> GetAll();
    University? GetByGuid(Guid guid);
    University Create(University university);
    bool Update(University university);
    bool Delete(Guid guid);
}
