using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Band.Models
{
    public class SetEmotion
    {
        public required int BandNo { get; set; }
        public required string Type { get; set; }
        public required ContentKey ContentKey { get; set; }
    }
}
