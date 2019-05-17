using ABO.Core.Configuration;
using System;
using System.Runtime.CompilerServices;

namespace ABO.Core.Infrastructure
{
    /// <summary>
    /// Provides access to the singleton instance of the Nop engine.
    /// </summary>
    public class EngineContext
    {
        #region Fields
        private static bool _initialized = false;

        #endregion

        #region Utilities

        ///// <summary>
        ///// Creates a factory instance and adds a http application injecting facility.
        ///// </summary>
        ///// <param name="config">Config</param>
        ///// <returns>New engine instance</returns>
        //protected static IEngine CreateEngineInstance()
        //{
        //    return new WebEngine();
        //}

        /// <summary>
        /// Creates a factory instance and adds a http application injecting facility.
        /// </summary>
        /// <param name="config">Config</param>
        /// <returns>New engine instance</returns>
        public static Func<IEngine> CreateEngineInstance = new Func<IEngine>(() =>
            {
                throw new NotImplementedException();
            });

        /// <summary>
        /// Gets IDependencyRegistar instances that would be invoked when IEngine is intialized.
        /// By default, an empty array is used.
        /// </summary>
        public static Func<IDependencyRegistrar[]> GetDependencyRegistrars = () => new IDependencyRegistrar[] { };


        #endregion

        #region Methods

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(bool forceInit = false)
        {
            var engine = EngineContext.Current;
            if (!_initialized || forceInit)
            {
                engine.Initialize(new ABOConfig(), GetDependencyRegistrars());
                _initialized = true;
            }
            return engine;
        }

        /// <summary>
        /// Ends and releases resources in current context.
        /// Mainly used in console application; in web application, autofac automatically manages resource disposal.
        /// </summary>
        public static void End()
        {
            if (Current != null && Current is IDisposable)
            {
                ((IDisposable)Current).Dispose();
            }
        }

        #endregion

        #region Properties

        private static Lazy<IEngine> _lazy = new Lazy<IEngine>(() => CreateEngineInstance());
        /// <summary>
        /// Gets the singleton Nop engine used to access Nop services.
        /// </summary>
        public static IEngine Current
        {
            get
            {
                return _lazy.Value;
            }
        }

        #endregion
    }
}