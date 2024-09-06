using Ardalis.SharedKernel;

namespace MiniAssetManagement.Core.FolderAggregate.Events;

public class FolderDeletedEvent(int folderId) : DomainEventBase
{
    public int FolderId { get; init; } = folderId;
}