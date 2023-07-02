﻿using API.Data;
using API.Models;
using API.Contracts;

namespace API.Repositories;

public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(BookingDBContext dBContext) : base(dBContext) { }

    public Employee? GetEmailorPhoneNumber(string data)
    {
        return _context.Set<Employee>().FirstOrDefault(e => e.PhoneNumber == data || e.Email == data);
    }
    public IEnumerable<Employee>? GetsByEmail(string email)
    {
        return _context.Set<Employee>().Where(E => E.Email == email);
    }

    public Employee? CheckEmail(string email)
    {
        return _context.Set<Employee>().FirstOrDefault(e => e.Email == email);
    }
}
