using ABO.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Services.Security
{
    public interface IPermissionService
    {
        bool Authorize(params UserPermission[] permissions);
        bool Authorize(int userId, params UserPermission[] permissions);
    }
}
