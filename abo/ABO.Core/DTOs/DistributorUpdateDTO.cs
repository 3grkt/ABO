using ABO.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Core.DTOs
{
    public class DistributorUpdateDTO
    {
        public DistributorUpdateDTO()
        {
            this.Logs = new List<DistributorLog>();
        }

        public DistributorUpdateDTO(long distNumber, DistributorLog log)
        {
            this.DistNumber = distNumber;
            this.Logs = new List<DistributorLog> { log };
        }

        //public DistributorUpdateDTO()
        //{
        //    this.DistLogIds = new List<int>();
        //    this.UpdateTypes = new List<string>();
        //}

        //public DistributorUpdateDTO(long distNumber, int distLogId, string updateType = null)
        //{
        //    this.DistNumber = DistNumber;
        //    this.DistLogIds = new List<int> { distLogId };
        //    this.UpdateTypes = new List<string>();

        //    if (updateType != null)
        //        this.UpdateTypes.Add(updateType);
        //}

        //public void AddLogAndType(int distLogId, string updateType)
        //{
        //    DistLogIds.Add(distLogId);
        //    UpdateTypes.Add(updateType);
        //}

        public long DistNumber { get; set; }
        //public IList<int> DistLogIds { get; set; }
        //public IList<string> UpdateTypes { get; set; }

        public IList<DistributorLog> Logs { get; set; }
    }
}
