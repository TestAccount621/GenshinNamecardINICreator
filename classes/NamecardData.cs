using Newtonsoft.Json;
using System.ComponentModel;

namespace GenshinNamecardAutomater.classes
{
    public class NamecardData : INotifyPropertyChanged
    {
        [Newtonsoft.Json.JsonIgnore]
        private string _name = "";
        [Newtonsoft.Json.JsonIgnore]
        private string _mainHash = "";
        [Newtonsoft.Json.JsonIgnore]
        private string _previewHash = "";
        [Newtonsoft.Json.JsonIgnore]
        private string _bannerHash = "";

        [JsonProperty(PropertyName = "namecard_name")]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }
        [JsonProperty(PropertyName = "main_hash")]
        public string MainHash
        {
            get => _mainHash;
            set
            {
                if (_mainHash != value)
                {
                    _mainHash = value;
                    NotifyPropertyChanged(nameof(MainHash));
                }
            }
        }
        [JsonProperty(PropertyName = "preview_hash")]
        public string PreviewHash
        {
            get => _previewHash;
            set
            {
                if (_previewHash != value)
                {
                    _previewHash = value;
                    NotifyPropertyChanged(nameof(PreviewHash));
                }
            }
        }
        [JsonProperty(PropertyName = "top_banner_hash")]
        public string BannerHash
        {
            get => _bannerHash;
            set
            {
                if (_bannerHash != value)
                {
                    _bannerHash = value;
                    NotifyPropertyChanged(nameof(BannerHash));
                }
            }
        }

        public NamecardData(string name, string mainHash, string previewHash, string bannerHash)
        {
            Name = name;
            MainHash = mainHash;
            PreviewHash = previewHash;
            BannerHash = bannerHash;
        }

        public void Update(NamecardData data)
        {
            Name = data.Name;
            MainHash = data.MainHash;
            PreviewHash = data.PreviewHash;
            BannerHash = data.BannerHash;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public override bool Equals(object? obj)
        {
            return obj is NamecardData data &&
                   MainHash == data.MainHash &&
                   PreviewHash == data.PreviewHash &&
                   BannerHash == data.BannerHash;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MainHash, PreviewHash, BannerHash);
        }
    }
}
