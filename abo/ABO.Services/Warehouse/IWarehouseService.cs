using ABO.Core;
using ABO.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Services.WareHouse
{
    public interface IWarehouseService
    {
        Dictionary<string, string> GetAllWarehouses();
    }
}
