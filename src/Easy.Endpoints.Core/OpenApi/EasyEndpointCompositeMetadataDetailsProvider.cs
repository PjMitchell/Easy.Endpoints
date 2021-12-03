using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace Easy.Endpoints
{
    internal interface IEasyEndpointCompositeMetadataDetailsProvider : ICompositeMetadataDetailsProvider
    {
    }

    internal class EasyEndpointCompositeMetadataDetailsProvider : IEasyEndpointCompositeMetadataDetailsProvider
    {
        private readonly IEnumerable<IMetadataDetailsProvider> providers;

        public EasyEndpointCompositeMetadataDetailsProvider(IEnumerable<IMetadataDetailsProvider> providers)
        {
            this.providers = providers;
        }

        public void CreateBindingMetadata(BindingMetadataProviderContext context)
        {
            foreach (var bindingProvider in providers.OfType<IBindingMetadataProvider>())
                bindingProvider.CreateBindingMetadata(context);
        }

        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            foreach (var bindingProvider in providers.OfType<IDisplayMetadataProvider>())
                bindingProvider.CreateDisplayMetadata(context);
        }

        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            foreach (var bindingProvider in providers.OfType<IValidationMetadataProvider>())
                bindingProvider.CreateValidationMetadata(context);
        }
    }
}
