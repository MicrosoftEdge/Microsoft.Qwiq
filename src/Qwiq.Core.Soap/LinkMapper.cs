using System;

using Qwiq.Exceptions;

using Tfs = Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Qwiq.Client.Soap
{
    internal class LinkMapper
    {
        internal Tfs.Link Map(ILink link, Tfs.WorkItem item)
        {
            switch (link.BaseType)
            {
                case BaseLinkType.RelatedLink:
                    var relatedLink = (IRelatedLink)link;
                    var linkTypeEnd = LinkTypeEndMapper.Map(item.Store, relatedLink.LinkTypeEnd);
                    return new Tfs.RelatedLink(linkTypeEnd, relatedLink.RelatedWorkItemId);

                case BaseLinkType.Hyperlink:
                    var hyperlink = (IHyperlink)link;
                    return new Tfs.Hyperlink(hyperlink.Location);

                case BaseLinkType.ExternalLink:
                    var externalLink = (IExternalLink)link;
                    var registeredLinkType = RegisteredLinkTypeMapper.Map(item.Store, externalLink.ArtifactLinkTypeName);
                    return new Tfs.ExternalLink(registeredLinkType, externalLink.LinkedArtifactUri);

                default:
                    throw new ArgumentException("Unknown link type", nameof(link));
            }
        }

        internal ILink Map(Tfs.Link link)
        {
            switch (link.BaseType)
            {
                case Tfs.BaseLinkType.RelatedLink:
                    var relatedLink = (Tfs.RelatedLink)link;
                    return ExceptionHandlingDynamicProxyFactory.Create<IRelatedLink>(new RelatedLink(relatedLink));

                case Tfs.BaseLinkType.Hyperlink:
                    var hyperlink = (Tfs.Hyperlink)link;
                    return ExceptionHandlingDynamicProxyFactory.Create<IHyperlink>(new Hyperlink(hyperlink));

                case Tfs.BaseLinkType.ExternalLink:
                    var externalLink = (Tfs.ExternalLink)link;
                    return ExceptionHandlingDynamicProxyFactory.Create<IExternalLink>(new ExternalLink(externalLink));

                default:
                    throw new ArgumentException("Unknown link type", nameof(link));
            }
        }
    }
}
