using System.Runtime.Serialization;

namespace BankingSystemServices.Exceptions;

[Serializable]
public class PropertyValidationException : Exception
{
    private readonly string? _propName;
    private readonly string? _objName;

    public PropertyValidationException(string message) : base(message)
    {
    }
    
    public PropertyValidationException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public PropertyValidationException(string? message, string? validatedPropertiesName, string? validatedObjectName, Exception? innerException)
        : base(message, innerException)
    {
        _propName = validatedPropertiesName;
        _objName = validatedObjectName;
    }
    
    public PropertyValidationException(string message, string? propertyName, string? objectName) : base(message)
    {
        _propName = propertyName;
        _objName = objectName;
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("PropName", _propName, typeof(string));
        info.AddValue("ObjName", _objName, typeof(string));
    }
    
    protected PropertyValidationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        _propName = info.GetString("PropName");
        _objName = info.GetString("ObjName");
    }

    public override string Message
    {
        get
        {
            var mess = base.Message;

            if (!string.IsNullOrEmpty(_objName))
            {
                mess += "\nObject: " + _objName;
            }
            
            if (!string.IsNullOrEmpty(_propName))
            {
                mess += "\nProperty: " + _propName;
            }

            return mess;
        }
    }

    public virtual string? PropName => _propName;
    public virtual string? ObjName => _objName;
}