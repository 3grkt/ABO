using ABO.Core.Data;
using ABO.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Services
{
    public abstract class ServiceBase
    {
        protected readonly IRepository<DataLog> _dataLogRepository;

        protected ServiceBase() { }

        protected ServiceBase(IRepository<DataLog> dataLogRepository)
        {
            _dataLogRepository = dataLogRepository;
        }

        protected void CreateLogRecord(string tableName, string fieldName, object pk, object oldValue, object newValue, string logUser, string logInfo = null, DateTime? logDate = null)
        {
            _dataLogRepository.Insert(new DataLog()
            {
                TableName = tableName,
                FieldName = fieldName,
                PK = Convert.ToString(pk),
                OldValue = Convert.ToString(oldValue),
                NewValue = Convert.ToString(newValue),
                LogUser = logUser,
                LogInfo = logInfo,
                LogDate = logDate ?? DateTime.Now
            });
        }
    }
}
