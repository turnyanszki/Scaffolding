using Microsoft.VisualStudio.Web.CodeGeneration;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.ProjectModel;
using Microsoft.VisualStudio.Web.CodeGeneration.DotNet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Store
{
    public abstract class StoreGeneratorBase : CommonGeneratorBase
    {
        protected ICodeGeneratorActionsService CodeGeneratorActionsService
        {
            get;
            private set;
        }
        protected IProjectContext ProjectContext
        {
            get;
            private set;
        }
        protected IServiceProvider ServiceProvider
        {
            get;
            private set;
        }
        protected ILogger Logger
        {
            get;
            private set;
        }

        public StoreGeneratorBase(
            IProjectContext projectContext,
            IApplicationInfo applicationInfo,
            ICodeGeneratorActionsService codeGeneratorActionsService,
            IServiceProvider serviceProvider,
            ILogger logger)
            : base(applicationInfo)
        {
            if (projectContext == null)
            {
                throw new ArgumentNullException(nameof(projectContext));
            }

            if (applicationInfo == null)
            {
                throw new ArgumentNullException(nameof(applicationInfo));
            }

            if (codeGeneratorActionsService == null)
            {
                throw new ArgumentNullException(nameof(codeGeneratorActionsService));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            ProjectContext = projectContext;
            CodeGeneratorActionsService = codeGeneratorActionsService;
            ServiceProvider = serviceProvider;
            Logger = logger;
        }

        protected virtual IEnumerable<string> TemplateFolders
        {
            get
            {
                return TemplateFoldersUtilities.GetTemplateFolders(
                    containingProject: Constants.ThisAssemblyName,
                    applicationBasePath: ApplicationInfo.ApplicationBasePath,
                    baseFolders: new[] { "StoreGenerator" },
                    projectContext: ProjectContext);
            }
        }

        protected string GetDefaultStoreNamespace(string relativeFolderPath)
        {
            return NameSpaceUtilities.GetSafeNameSpaceFromPath(relativeFolderPath, ApplicationInfo.ApplicationName);
        }

        protected void ValidateNameSpaceName(StoreGeneratorModel generatorModel)
        {
            if (!string.IsNullOrEmpty(generatorModel.StoreNamespace) &&
                !RoslynUtilities.IsValidNamespace(generatorModel.StoreNamespace))
            {
                throw new InvalidOperationException(string.Format(
                    CultureInfo.CurrentCulture,
                    MessageStrings.InvalidNamespaceName,
                    generatorModel.StoreNamespace));
            }
        }

        protected string ValidateAndGetOutputPath(StoreGeneratorModel generatorModel)
        {
            return ValidateAndGetOutputPath(generatorModel, outputFileName: generatorModel.StoreName + Constants.CodeFileExtension); ;
        }

        private bool IsInArea(string appBasePath, string outputPath)
        {
            return outputPath.StartsWith(Path.Combine(appBasePath, "Areas") + Path.DirectorySeparatorChar);
        }

        protected string GetAreaName(string appBasePath, string outputPath)
        {
            if (IsInArea(appBasePath, outputPath))
            {
                var relativePath = outputPath.Substring(Path.Combine(appBasePath, "Areas").Length);
                return relativePath.Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            }
            return string.Empty;
        }

        public abstract Task Generate(StoreGeneratorModel storeGeneratorModel);
        protected abstract string GetTemplateName(StoreGeneratorModel storeGeneratorModel);
    }
}

