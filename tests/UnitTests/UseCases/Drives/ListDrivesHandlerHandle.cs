using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Drives;
using MiniAssetManagement.UseCases.Drives.List;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Drives;

public class ListDrivesHandlerHandle
{
    private readonly IListDrivesQueryService _service = Substitute.For<IListDrivesQueryService>();
    private ListDrivesHandler _handler;

    public ListDrivesHandlerHandle() => _handler = new ListDrivesHandler(_service);

    [Test]
    public async Task ReturnsEmptyWhenEmpty()
    {
        // Given
        _service
            .ListAsync(DriveFixture.OwnerIdDefault, Arg.Any<int?>(), Arg.Any<int?>())
            .Returns([]);

        // When
        var result = await _handler.Handle(
            new ListDrivesQuery(DriveFixture.OwnerIdDefault),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(result.Value, Is.Empty, nameof(result.Value));
    }

    [Test]
    public async Task ReturnsDrivesWhenNotEmpty()
    {
        // Given
        List<DriveDTO> drives = [new(1, "foo"), new(2, "bar")];
        _service
            .ListAsync(DriveFixture.OwnerIdDefault, Arg.Any<int?>(), Arg.Any<int?>())
            .Returns(drives);

        // When
        var result = await _handler.Handle(
            new ListDrivesQuery(DriveFixture.OwnerIdDefault),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(result.Value, Is.EquivalentTo(drives), nameof(result.Value));
    }
}