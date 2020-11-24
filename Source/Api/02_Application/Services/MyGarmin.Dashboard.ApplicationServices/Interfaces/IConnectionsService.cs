using MyGarmin.Dashboard.ApplicationServices.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices.Interfaces
{
    public interface IConnectionsService
    {
        Task<Tuple<int, List<Connection>>> GetAllConnections();
    }
}
