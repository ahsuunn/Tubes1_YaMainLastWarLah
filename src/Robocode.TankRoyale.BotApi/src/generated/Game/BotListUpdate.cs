//----------------------
// <auto-generated>
//     Generated using the NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0) (http://NJsonSchema.org)
// </auto-generated>
//----------------------


namespace Robocode.TankRoyale.Schema.Game
{
    #pragma warning disable // Disable all warnings

    /// <summary>
    /// Bot list update
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.2.0 (Newtonsoft.Json v13.0.0.0)")]
    public class BotListUpdate : Message
    {
        /// <summary>
        /// List of bots
        /// </summary>
        [Newtonsoft.Json.JsonProperty("bots", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<BotInfo> Bots { get; set; } = new System.Collections.ObjectModel.Collection<BotInfo>();


    }
}