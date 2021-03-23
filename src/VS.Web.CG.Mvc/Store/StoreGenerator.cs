using Microsoft.VisualStudio.Web.CodeGeneration;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.ProjectModel;
using Microsoft.VisualStudio.Web.CodeGeneration.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Store
{
    public class StoreGenerator : StoreGeneratorBase
    {
        public StoreGenerator(
            IProjectContext projectContext,
            IApplicationInfo applicationInfo,
            ICodeGeneratorActionsService codeGeneratorActionsService,
            IServiceProvider serviceProvider,
            ILogger logger)
            : base(projectContext, applicationInfo, codeGeneratorActionsService, serviceProvider, logger)
        {
        }

        public override async Task Generate(StoreGeneratorModel storeGeneratorModel)
        {
            if (!string.IsNullOrEmpty(storeGeneratorModel.StoreName))
            {
                if (!storeGeneratorModel.StoreName.EndsWith(Constants.StoreSuffix, StringComparison.Ordinal))
                {
                    storeGeneratorModel.StoreName = storeGeneratorModel.StoreName + Constants.StoreSuffix;
                }
            }
            else
            {
                throw new ArgumentException(GetRequiredNameError);
            }
            ValidateNameSpaceName(storeGeneratorModel);
            var namespaceName = string.IsNullOrEmpty(storeGeneratorModel.StoreNamespace)
                ? GetDefaultStoreNamespace(storeGeneratorModel.RelativeFolderPath)
                : storeGeneratorModel.StoreNamespace;
            var templateModel = new ClassNameModel(className: storeGeneratorModel.StoreName, namespaceName: namespaceName);

            var outputPath = ValidateAndGetOutputPath(storeGeneratorModel);
            await CodeGeneratorActionsService.AddFileFromTemplateAsync(outputPath, GetTemplateName(storeGeneratorModel), TemplateFolders, templateModel);
            Logger.LogMessage(string.Format(MessageStrings.AddedStore, outputPath.Substring(ApplicationInfo.ApplicationBasePath.Length)));
        }

        protected override string GetTemplateName(StoreGeneratorModel storeGeneratorModel)
        {
            return Constants.StoreTemplate;
        }

        protected virtual string GetRequiredNameError
        {
            get
            {
                return MessageStrings.StoreNameRequired;
            }
        }
    }
}
