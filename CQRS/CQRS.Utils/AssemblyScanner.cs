using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CQRS.Utils.Reflection;

namespace CQRS.Utils
{
    /// <summary>
    /// Helpers for assembly scanning operations
    /// </summary>
    public class AssemblyScanner
    {
        /// <summary>
        /// Gets a list with assemblies that can be scanned
        /// </summary>
        /// <returns></returns>
        [DebuggerNonUserCode] //so that exceptions don't jump at the developer debugging their app
        public static IEnumerable<Assembly> GetScannableAssemblies()
        {
            var assemblyFiles = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).GetFiles("*.dll", SearchOption.AllDirectories)
                .Union(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).GetFiles("*.exe", SearchOption.AllDirectories));


            foreach (var assemblyFile in assemblyFiles)
            {
                Assembly assembly;

                try
                {
                    assembly = Assembly.LoadFrom(assemblyFile.FullName);

                    //will throw if assembly cant be loaded
                    assembly.GetTypes();
                }

                catch (Exception)
                {
                    continue;
                }

                yield return assembly;
            }
        }

        public static IEnumerable<Type> ScanAssembliesFor<T>()
        {
            return ScanAssembliesFor(typeof(T));
        }

        public static IEnumerable<Type> ScanAssembliesFor(Type T)
        {
            foreach (var assembly in AssemblyScanner.GetScannableAssemblies())
                foreach (var type in assembly.GetTypes().GetMatchingTypes(T))
                {
                    yield return type;
                }
        }

    }
}
