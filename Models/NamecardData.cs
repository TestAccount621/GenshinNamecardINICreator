using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinNamecardINICreator.Models
{
    public class NamecardData
    {
        [JsonProperty(PropertyName = "namecard_name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "main_hash")]
        public string MainHash { get; set; }
        [JsonProperty(PropertyName = "preview_hash")]
        public string PreviewHash { get; set; }
        [JsonProperty(PropertyName = "top_banner_hash")]
        public string BannerHash { get; set; }


        public NamecardData()
        {
            Name = "";
            MainHash = "";
            PreviewHash = "";
            BannerHash = "";
        }

        [JsonConstructor]
        public NamecardData(string name, string mainHash, string previewHash, string bannerHash)
        {
            Name = name;
            MainHash = mainHash;
            PreviewHash = previewHash;
            BannerHash = bannerHash;
        }

        public NamecardData(NamecardData namecard)
        {
            Name = namecard.Name;
            MainHash = namecard.MainHash;
            PreviewHash = namecard.PreviewHash;
            BannerHash = namecard.BannerHash;
        }
    }
}
