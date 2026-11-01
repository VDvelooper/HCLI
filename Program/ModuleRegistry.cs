using ModuleContracts.Abstractions;
using ModuleContracts.Feedback.Error;
using ModuleContracts.Feedback.Warning;

using System.Runtime.Loader;

namespace HCLI.Program
{
    internal class ModuleRegistry
    {
        internal List<AssemblyLoadContext> LoadedContexts { get; private set; } = new();
        /// <summary>
        /// Key: Module's main class, Value: dll file's name
        /// </summary>
        internal Dictionary<Type, string> DetectedModules { get; private set; } = new();
        internal List<ModuleBase> LoadedModules { get; private set; } = new();


        internal List<string> ModuleToLoadAtStart { get; }
        internal string ModuleDllStoragePath { get; } // replace it to an outside %appdata% folder?
        
        internal ModuleRegistry()
        {
            ModuleDllStoragePath = @$"{Runtime.ConfigManager.MainDirectoryPath}\ModuleDlls";

            if (!Path.Exists(ModuleDllStoragePath))
                Directory.CreateDirectory(ModuleDllStoragePath);

            ModuleToLoadAtStart = Runtime.ConfigManager.ConfigData.ModuleToLoadAtStart;
            
            DetectModules();
        }

        internal void DetectModules()
        {
            Type moduleBaseClass = typeof(ModuleBase);

            List<string> possibleDllPaths = System.IO.Directory.GetFiles(ModuleDllStoragePath, "*.dll").ToList();


            foreach (string possibleAbsDllPath in possibleDllPaths)
            {
                try
                {
                    Console.Write($"possibleAbsDllPath=");
                    Console.WriteLine(possibleAbsDllPath);

                    AssemblyLoadContext mdlCtxt = new AssemblyLoadContext(possibleAbsDllPath, true); // moduleContext

                    Type[] moduleDllTypes = mdlCtxt.LoadFromAssemblyPath(possibleAbsDllPath).GetTypes();

                    Type? foundMdlType = moduleDllTypes.Where(type =>
                    !type.IsAbstract &&
                    !type.IsInterface &&
                    type.IsSubclassOf(moduleBaseClass) &&
                    !DetectedModules.ContainsKey(type))
                    .First();

                    if (foundMdlType == null)
                    {
                        mdlCtxt.Unload();
                        continue;
                    }


                    string correctCtxtName = Path.GetFileNameWithoutExtension(possibleAbsDllPath);

                    // mert -> a context neve ne az egész path legyen
                    mdlCtxt.Unload();

                    mdlCtxt = new AssemblyLoadContext(correctCtxtName, true);

                    mdlCtxt.LoadFromAssemblyPath(possibleAbsDllPath);


                    LoadedContexts.Add(mdlCtxt);
                    DetectedModules.Add(foundMdlType, possibleAbsDllPath); // ModuleBase will store the abs path for, whatever. Why not scenario

                }
                catch (Exception ex)
                {
                    Runtime.ContractsForModules.ConsoleViewContract.FatalError($"{ex.Message} (DetectModules())");
                }
            }
        }

        internal ModuleBase? InstanciateModule(Type foundModuleType)
        {
            ModuleBase? moduleObject = Activator.CreateInstance(foundModuleType, Runtime.ContractsForModules, DetectedModules[foundModuleType]) as ModuleBase;

            if (moduleObject == null)
            {
                Runtime.ContractsForModules.ConsoleViewContract.PresentFeedback(new ValueWasNull(null, null, false));
                Runtime.ContractsForModules.ConsoleViewContract.Error($"The instanciation of module {foundModuleType.FullName} is not possible, because it is already loaded.");
                return null;
            }

            if (LoadedModules.Contains(moduleObject))
            {
                Runtime.ContractsForModules.ConsoleViewContract.PresentFeedback(new AlreadyExists(null, null, false));
                Runtime.ContractsForModules.ConsoleViewContract.Warn($"The instanciation of module {foundModuleType.FullName} is not possible, because it is already loaded.");
                return null;
            }
            
            if (moduleObject == null)
            {
                Runtime.ContractsForModules.ConsoleViewContract.PresentFeedback(new NotFound(null, null, false));
                Runtime.ContractsForModules.ConsoleViewContract.FatalError($"The instanciation of module {foundModuleType.FullName} have returned null. (NotFound feedback.)");
                return null;
            }

            LoadedModules.Add(moduleObject);

            return moduleObject;
        }


        internal bool TryGetModuleSubCommand(string command, out ICommand? found)
        {
            found = null;

            foreach (ModuleBase module in LoadedModules)
            {
                if (!module.SubCommands.Where(x => x.Syntax == command).Any()) 
                    continue;

                found = module.SubCommands.Where(x => x.Syntax == command).First();
                return true;
            }

            return false;
        }

        internal bool TryGetBaseCommand(string command, out ICommand? found)
        {
            found = null;

            foreach (ModuleBase module in LoadedModules)
            {
                found = module.BaseCommands.Where(c => c.Syntax == command).FirstOrDefault();

                if (found != null)
                {
                    found = module.BaseCommands.Where(c => c.Syntax == command).First();
                    break;
                }
            }

            return found != null;
        }
    }
}
