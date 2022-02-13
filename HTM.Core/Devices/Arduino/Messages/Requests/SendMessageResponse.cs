﻿using HTM.Infrastructure.MessagesBase;

namespace HTM.Core.Devices.Arduino.Messages.Requests;

public class SendMessageResponse : ResponseBase
{
    public Exception? Exception { get; }
    public bool IsError => Exception != null;

    public SendMessageResponse(string? requestId) : base(requestId)
    { }
    
    public SendMessageResponse(string? requestId, Exception exception) : base(requestId)
    {
        Exception = exception;
    }
}