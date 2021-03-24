﻿namespace Seva.API.Models.Auth
{
    using Newtonsoft.Json;
    using System;
    public class UserToken
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("expires")]
        public DateTime Expires { get; set; }
    }
}
