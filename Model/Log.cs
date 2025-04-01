using SkepsBeholder.Configuration;
using SkepsBeholder.Model.Enum;
using System.Text.Json.Serialization;

namespace SkepsBeholder.Model
{
    public class Log
    {
        public Owner? Owner { get; set; }
        public string? FlowId { get; set; }
        public string? User { get; set; }
        public string? Input { get; set; }
        public List<State>? States { get; set; }
        public List<Action>? InputActions { get; set; }
        public List<Action>? OutputActions { get; set; }
        public List<Action>? AfterStateChangedActions { get; set; }
        public DateTime? Timestamp { get; set; }
        public int? ElapsedMilliseconds { get; set; }
        public string? Error { get; set; }
    }

    public class Owner
    {
        public string? Name { get; set; }
        public string? Domain { get; set; }
    }

    public class ParsedSettings
    {
        public string? StateId { get; set; }
        public string? StateName { get; set; }
        public string? PreviousStateId { get; set; }
        public string? PreviousStateName { get; set; }
        public Extras? Extras { get; set; }
        public string? Category { get; set; }
        public string? Action { get; set; }
    }

    public class Extras
    {
        public string? StateName { get; set; }
        public string? StateId { get; set; }
        public string? MessageId { get; set; }
        public string? PreviousStateId { get; set; }
        public string? PreviousStateName { get; set; }
    }

    public class Action
    {
        public int Order { get; set; }
        [JsonConverter(typeof(ActionTypeConverter))]
        public ActionTypeEnum Type { get; set; }
        public ParsedSettings? ParsedSettings { get; set; }
        public bool? ContinueOnError { get; set; }
        public DateTime? Timestamp { get; set; }
        public int? ElapsedMilliseconds { get; set; }
        public string? Error { get; set; }
    }

    public class Output
    {
        public string? StateId { get; set; }
        public int? ConditionsCount { get; set; }
        public DateTime? Timestamp { get; set; }
        public int ElapsedMilliseconds { get; set; }
    }

    public class ExtensionData
    {
        public string? Name { get; set; }
    }

    public class State
    {
        public string? Id { get; set; }
        public List<Action>? InputActions { get; set; }
        public List<Action>? OutputActions { get; set; }
        public List<Action>? AfterStateChangedActions { get; set; }
        public List<Output>? Outputs { get; set; }
        public ExtensionData? ExtensionData { get; set; }
        public DateTime? Timestamp { get; set; }
        public int ElapsedMilliseconds { get; set; }
        public string? Error { get; set; }
    }
}
