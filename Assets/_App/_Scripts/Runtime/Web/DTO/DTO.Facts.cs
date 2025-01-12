using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace _App.Runtime.Web.DTO
{
    public partial class DTOs
    {
        [Serializable]
        public class BreedsListResponse
        {
            [JsonProperty("data")]
            public List<Breed> Data { get; set; }

            [JsonProperty("links")]
            public Links Links { get; set; }
        }
        
        [Serializable]
        public class BreedResponse
        {
            [JsonProperty("data")]
            public Breed Data { get; set; }

            [JsonProperty("links")]
            public Links Links { get; set; }
        }

        [Serializable]
        public class Breed
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("attributes")]
            public BreedAttributes Attributes { get; set; }
        }

        [Serializable]
        public class BreedAttributes
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("min_life")]
            public int MinLife { get; set; }

            [JsonProperty("max_life")]
            public int MaxLife { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("hypoallergenic")]
            public bool Hypoallergenic { get; set; }
        }
        [Serializable]
        public class Links
        {
            [JsonProperty("self")]
            public string Self { get; set; }

            [JsonProperty("current")]
            public string Current { get; set; }

            [JsonProperty("next")]
            public string Next { get; set; }

            [JsonProperty("last")]
            public string Last { get; set; }
        }

        [Serializable]
        public class FactsResponse
        {
            [JsonProperty("data")]
            public List<Fact> Data { get; set; }
        }

        [Serializable]
        public class Fact
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("attributes")]
            public FactAttributes Attributes { get; set; }
        }

        [Serializable]
        public class FactAttributes
        {
            [JsonProperty("body")]
            public string Body { get; set; }
        }
    }
}