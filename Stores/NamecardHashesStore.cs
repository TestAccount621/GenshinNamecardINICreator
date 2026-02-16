
using GenshinNamecardINICreator.CreationClasses;
using GenshinNamecardINICreator.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;

namespace GenshinNamecardINICreator.Stores
{
    public class NamecardHashesStore
    {
		public const string NamecardFileName = "namecard_hashes.json";
        public MintPlayer.ObservableCollection.ObservableCollection<NamecardData> Namecards { get; private set; }

		private NamecardData _selectedNamecard;
		public NamecardData SelectedNamecard
		{
			get
			{
				return _selectedNamecard;
			}
			set
			{
				_selectedNamecard = value;		
				SelecetedNamecardChanged?.Invoke();
			}
		}

		public event Action SelecetedNamecardChanged;
		public event Action<NamecardData> NamecardAdded;
		public event Action<NamecardData> NamecardUpdated;
		public event Action<NamecardData> NamecardDeleted;

		public NamecardHashesStore()
		{
			Namecards = [];
			Load();
		}

		public void ChangeNamecard(NamecardData namecard)
		{
			if (Namecards.Contains(namecard))
            {
                SelectedNamecard = namecard;
            }
		}

		public void Add(NamecardData namecard)
        {
            var test = Namecards.Where(x => x.BannerHash == namecard.BannerHash
                && x.MainHash == namecard.MainHash
                && x.PreviewHash == namecard.PreviewHash);
            if (!test.Any())
			{
				Namecards.Add(namecard);
				NamecardAdded?.Invoke(namecard);
				SelectedNamecard = namecard;
			}
		}

		public void UpdateNamecard(NamecardData namecard)
		{
			int index = Namecards.IndexOf(namecard);
			if (index != -1)
			{
				Namecards[index] = namecard;
				NamecardUpdated?.Invoke(namecard);
				SelectedNamecard = namecard;
			}
		}

		public void DeleteNamecard(NamecardData namecard)
		{
			if (Namecards.Contains(namecard))
			{
				Namecards.Remove(namecard);
				NamecardDeleted?.Invoke(namecard);
				SelectedNamecard = null;
			}
		}

		/// <summary>
		/// Loads the namecard_hashes.json file that should be where the .exe is placed.
		/// </summary>
		private void Load()
		{
			if (File.Exists(NamecardFileName))
			{
				try
                {
                    var text = File.ReadAllText(NamecardFileName);
                    var list = JsonConvert.DeserializeObject<List<NamecardData>>(text);
                    if (list != null && list.Count > 0)
                    {
                        Namecards.Clear();
                        Namecards.AddRange(list);
                    }
                }
				catch (Exception e)
				{
					string message = "There was an error when trying to load the Namecard hashes. The 'namecard_hashes.json' file might have been manually formatted incorrectly.";
					MessageBox.Show(message + "\nError Message: " + e.Message);
				}
			}
			else
			{
				string message = "You are missing a 'namecard_hashes.json' file that should have come with this program that contains all of the Namecard hashes up to when this was last updated. You will have to redownload to get it or create your own using the 'Hashes' section of the program.";
                MessageBox.Show(message);
			}
		}

		/// <summary>
		/// This should only run when the program is closing or manually saved maybe? I don't see a need to write to the file every add/edit/delete.
		/// </summary>
		public void Save()
		{
            using (StreamWriter file = File.CreateText(NamecardFileName))
            {
                var serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented
                };
                serializer.Serialize(file, Namecards);
            }
		}
	}
}
