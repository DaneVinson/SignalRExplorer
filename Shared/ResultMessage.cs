using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared
{
    public class ResultMessage
    {
        public ResultMessage()
        { }

        public ResultMessage(string clientName, string id = null, bool success = false)
        {
            ClientName = clientName;
            Id = id;
            Success = success;
        }

        public string ClientName { get; set; }
        public string Id { get; set; }
        public bool Success { get; set; }

        public override string ToString()
        {
            return $"Success: {Success}, Id: {Id}, Client: {ClientName}";
        }
    }
}
