using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IUnitOfWork
    {
        // signeture for property
        IDepartmentRepository DepartmentRepository { get; set; }
        IEmployeeRepository EmployeeRepository { get; set; }
        Task<int> CompleteAsync();
    }
}
