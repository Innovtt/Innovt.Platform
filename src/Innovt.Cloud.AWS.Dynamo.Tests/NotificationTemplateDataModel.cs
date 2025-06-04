// Innovt Company
// Author: Michel Borges
// Project: HeyWorld.Notifications.Platform

using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.Table;
using Innovt.Core.Utilities.Mapper;
using Innovt.Notification.Core.Domain;

namespace Innovt.Cloud.AWS.Dynamo.Tests;

[DynamoDBTable("NotificationTemplates")]
public class NotificationTemplateDataModel : ITableMessage
{
    public string Subject { get; set; }

    public string FromName { get; set; }

    public string FromAddress { get; set; }

    public string TemplateUrl { get; set; }

    public string Charset { get; set; }

    public string Body { get; set; }

    public string Builder { get; set; }

    public int Type { get; set; }

    [DynamoDBHashKey("PK")] public string Id { get; set; }


    public static NotificationTemplate ToDomain(NotificationTemplateDataModel dataModel)
    {
        if (dataModel == null) return null;

        var message = SimpleMapper.Map<NotificationTemplateDataModel, NotificationTemplate>(dataModel);
        message.Type = (NotificationMessageType)dataModel.Type;

        return message;
    }

    public static NotificationTemplateDataModel FromDomain(NotificationTemplate notificationMessage)
    {
        if (notificationMessage == null) return null;

        var dataModel = SimpleMapper.Map<NotificationTemplate, NotificationTemplateDataModel>(notificationMessage);
        dataModel.Type = (int)notificationMessage.Type;

        return dataModel;
    }
}