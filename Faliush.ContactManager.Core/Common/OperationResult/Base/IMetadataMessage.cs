namespace Faliush.ContactManager.Core.Common.OperationResult.Base;

public interface IMetadataMessage : IHaveData
{
    string Message { get; }
    object DataObject { get; }
}
