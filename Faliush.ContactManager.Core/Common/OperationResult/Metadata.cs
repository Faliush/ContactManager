using Faliush.ContactManager.Core.Common.OperationResult.Base;

namespace Faliush.ContactManager.Core.Common.OperationResult;

[Serializable]
public class Metadata : IMetadataMessage
{
    public string? Message { get; }
    public MetadataTypes Type { get; }
    public object DataObject { get; private set; }

    private readonly OperationResultBase? _source;

    public Metadata() =>
        DataObject = new object();

    public Metadata(OperationResultBase source, string message)
    {
        _source= source;
        Message = message;
    }

    public Metadata(OperationResultBase source, string message, MetadataTypes type) : this(source, message) =>    
        Type = type;


    public void AddData(object data)
    {
        if (_source is null)
            return;

        if(data is Exception exception && _source.Metadata is null)
        {
            _source.Metadata = new Metadata(_source, exception.Message);
            return;
        }

        if (_source.Metadata is not null)
            _source.Metadata.DataObject = data;
    }
}
