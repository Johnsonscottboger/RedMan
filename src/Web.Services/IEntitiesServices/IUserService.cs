using RedMan.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Services.IEntitiesServices
{
    public interface IUserService
    {
        bool AddUser(User user);

    }
}
