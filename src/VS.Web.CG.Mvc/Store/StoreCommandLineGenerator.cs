using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Microsoft.VisualStudio.Web.CodeGeneration.CommandLine;
using System;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Store
{
    [Alias("store")]
    public class StoreCommandLineGenerator : ICodeGenerator
    {
        private readonly IServiceProvider _serviceProvider;

        public StoreCommandLineGenerator(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            _serviceProvider = serviceProvider;
        }

        public async Task GenerateCode(StoreGeneratorModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            StoreGeneratorBase generator = null;

            if (string.IsNullOrEmpty(model.ModelClass))
            {
                    generator = GetGenerator<StoreGenerator>(); //This need to handle the WebAPI Empty as well...
            }
            //else
            //{
            //    generator = GetGenerator<ControllerWithContextGenerator>();
            //}

            if (generator != null)
            {
                await generator.Generate(model);
            }
        }

        private StoreGeneratorBase GetGenerator<TChild>() where TChild : StoreGeneratorBase
        {
            return (StoreGeneratorBase)ActivatorUtilities.CreateInstance<TChild>(_serviceProvider);
        }
    }
}

