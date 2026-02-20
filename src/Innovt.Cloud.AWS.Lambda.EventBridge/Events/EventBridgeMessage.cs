using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.EventBridge.Events;

public class EventBridgeMessage
{
  public string Version { get; set; }
  
  public string Account { get; set; }

  public string Region { get; set; }
 
  public object Detail { get; set; }

  [JsonPropertyName("detail-type")]
  public string DetailType { get; set; }

  public string Source { get; set; }
  
  public DateTime Time { get; set; }
  
  public string Id { get; set; }
  
  public List<string> Resources { get; set; }
}