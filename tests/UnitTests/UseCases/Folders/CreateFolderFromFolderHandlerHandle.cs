using Ardalis.SharedKernel;
using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Folders.Create;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Folders;

public class CreateFolderFromFolderHandlerHandle
{
    private readonly IRepository<Folder> _repository = Substitute.For<IRepository<Folder>>();
    private readonly CreateFolderFromFolderHandler _handler;

    public CreateFolderFromFolderHandlerHandle() =>
        _handler = new CreateFolderFromFolderHandler(_repository);

    [Test]
    public async Task ReturnsSuccessGivenValidInput()
    {
        // Given
        var drive = FolderFixture.CreateFolderDefaultFromFolder();
        _repository
            .AddAsync(Arg.Any<Folder>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(drive));

        // When
        var result = await _handler.Handle(
            new CreateFolderFromFolderCommand(
                FolderFixture.NameDefault,
                UserFixture.IdDefault,
                FolderFixture.ParentIdDefault
            ),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(result.Value, Is.EqualTo(FolderFixture.IdDefault), nameof(result.Value));
    }
}