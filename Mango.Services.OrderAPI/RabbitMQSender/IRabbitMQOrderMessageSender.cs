﻿using Mango.MessageBus;

namespace Mango.Services.OrderAPI.RabbitMQSender;
public interface IRabbitMQOrderMessageSender
{
    void SendMessage(BaseMessage message, string queueName);
}
