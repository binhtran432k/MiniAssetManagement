using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Folders.Get;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Folders;

public class GetFolderHandlerHandle
{
    private readonly IGetFolderQueryService _service = Substitute.For<IGetFolderQueryService>();
    private GetFolderHandler _handler;

    public GetFolderHandlerHandle() => _handler = new GetFolderHandler(_service);

    [Test]
    public async Task ReturnsFolder()
    {
        // Given
        var folder = FolderFixture.CreateFolderDefaultFromDrive();
        _service.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(folder);

        // When
        var result = await _handler.Handle(
            new GetFolderQuery(FolderFixture.IdDefault, UserFixture.IdDefault),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.Multiple(() =>
        {
            Assert.That(result.Value.Id, Is.EqualTo(folder.Id), nameof(result.Value.Id));
            Assert.That(result.Value.Name, Is.EqualTo(folder.Name), nameof(result.Value.Name));
        });
    }
}
