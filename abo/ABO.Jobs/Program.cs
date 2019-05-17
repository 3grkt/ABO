using ABO.Core.Infrastructure;
using ABO.Jobs.Infrastructure;
using ABO.Jobs.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ABO.Jobs
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize engine context
                EngineContext.CreateEngineInstance = () => new JobEngine(); // engine object used in Web app
                EngineContext.GetDependencyRegistrars = () => new IDependencyRegistrar[]{ // dependency registrar
                    new CoreDependencyRegistrar(),
                    new DependencyRegistrar()
                };
                EngineContext.Initialize();

                // Get registered IWorker types
                var workerType = typeof(IWorker);
                var foundTypes = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => workerType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                    .ToList();

                // Create IWorker instance and add to list
                var workers = new List<IWorker>();
                foreach (var type in foundTypes)
                {
                    //var worker = EngineContext.Current.ContainerManager.ResolveUnregistered(type) as IWorker;
                    var worker = EngineContext.Current.Resolve(type) as IWorker;
                    workers.Add(worker);
                }

                // Sort and execute
                var sortedWorkers = workers.OrderBy(w => w.Order);
                foreach (var worker in sortedWorkers)
                {
                    if (worker.CanWork)
                        worker.DoWork();
                }
            }
            finally
            {
                // End engine context
                EngineContext.End();
            }
        }
    }
}
