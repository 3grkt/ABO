using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Jobs.Workers
{
    public interface IWorker
    {
        /// <summary>
        /// Runs worker.
        /// </summary>
        void DoWork();
        /// <summary>
        /// Gets flag to indicate if worker can be run.
        /// </summary>
        bool CanWork { get; }
        /// <summary>
        /// Gets running order of worker.
        /// </summary>
        int Order { get; }
    }
}
