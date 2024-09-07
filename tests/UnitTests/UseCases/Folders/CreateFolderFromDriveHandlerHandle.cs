using Ardalis.SharedKernel;
using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Folders.Create;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Folders;

public class CreateFolderFromDriveHandlerHandle
{
    private readonly IRepository<Folder> _repository = Substitute.For<IRepository<Folder>>();
    private readonly CreateFolderFromDriveHandler _handler;

    public CreateFolderFromDriveHandlerHandle() => _handler = new CreateFolderFromDriveHandler(_repository);

    [Test]
    public async Task ReturnsSuccessGivenValidInput()
    {
        // Given
        var drive = FolderFixture.CreateFolderDefaultFromDrive();
        _repository
            .AddAsync(Arg.Any<Folder>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(drive));

        // When
        var result = await _handler.Handle(
            new CreateFolderFromDriveCommand(
                FolderFixture.NameDefault,
                UserFixture.IdDefault,
                FolderFixture.DriveIdDefault
            ),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(result.Value, Is.EqualTo(FolderFixture.IdDefault), nameof(result.Value));
    }
}