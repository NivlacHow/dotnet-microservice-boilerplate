using System;

namespace Sandline.RMQLibs.Entities
{
    public class Event : BaseEvent
    {
        /// <summary>
        /// Service Name
        /// </summary>
        public string Publisher { get; set; }
        /// <summary>
        /// Service message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Attached data
        /// </summary>
        public object Data { get; set; }
    }


}
