using Microsoft.VisualStudio.Web.CodeGeneration.CommandLine;

namespace Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Store
{
    public class StoreGeneratorModel : CommonCommandLineModel
    {
        [Option(Name = "storeNamespace", ShortName = "store", Description = "Specify the name of the namespace to use for the generated store")]
        public string StoreNamespace { get; set; }

        [Option(Name = "storeName", ShortName = "name", Description = "Name of the store")]
        public string StoreName { get; set; }
        public StoreGeneratorModel()
        {

        }

        public StoreGeneratorModel(StoreGeneratorModel copyFrom) : base(copyFrom)
        {
            StoreNamespace = copyFrom.StoreNamespace;
            StoreName = copyFrom.StoreName;
        }

        public override CommonCommandLineModel Clone()
        {
            return new StoreGeneratorModel(this);
        }
    }
}
