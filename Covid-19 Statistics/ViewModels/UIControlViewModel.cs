using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covid_19_Statistics.ViewModels
{
    public class UIControlViewModel
    {
        public UIControlViewModel(string uniqueId, string parentId, string title, string subtitle, string imagePath, bool isUsingSysIcon, string description)
        {
            this.UniqueId = uniqueId;
            this.ParentId = parentId;
            this.Title = title;
            this.Subtitle = subtitle;
            this.Description = description;
            this.ImagePath = imagePath;
            this.IsUsingSysIcon = isUsingSysIcon;
        }
        public UIControlViewModel(string uniqueId, string parentId, string title, string subtitle, string imagePath, bool isUsingSysIcon, string description
            , int newConfirmed)
        {
            this.UniqueId = uniqueId;
            this.ParentId = parentId;
            this.Title = title;
            this.Subtitle = subtitle;
            this.Description = description;
            this.ImagePath = imagePath;
            this.NewConfirmed = newConfirmed;
            this.IsUsingSysIcon = isUsingSysIcon;
        }
        public string ParentId { get; private set; }
        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string Subtitle { get; private set; }
        public string Description { get; private set; }
        public string ImagePath { get; private set; }
        public bool IsUsingSysIcon { get; private set; }
        public int NewConfirmed { get; private set; }

    }

    public class UIControlGroupViewModel
    {
        public UIControlGroupViewModel(string uniqueId, string title, string subtitle, string imagePath, bool isUsingSysIcon, string description)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Subtitle = subtitle;
            this.Description = description;
            this.ImagePath = imagePath;
            this.IsUsingSysIcon = isUsingSysIcon;
            this.Items = new ObservableCollection<UIControlViewModel>();
        }

        public UIControlGroupViewModel(string uniqueId, string title, string subtitle, string imagePath, bool isUsingSysIcon, string description, int newConfirmed)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Subtitle = subtitle;
            this.Description = description;
            this.ImagePath = imagePath;
            this.IsUsingSysIcon = isUsingSysIcon;
            this.NewConfirmed = newConfirmed;
            this.Items = new ObservableCollection<UIControlViewModel>();
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string Subtitle { get; private set; }
        public string Description { get; private set; }
        public string ImagePath { get; private set; }
        public bool IsUsingSysIcon { get; private set; }
        public int NewConfirmed { get; private set; }
        public ObservableCollection<UIControlViewModel> Items { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

}
