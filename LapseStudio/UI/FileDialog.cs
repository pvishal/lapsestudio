using System;
using System.Collections.Generic;

namespace Timelapse_UI
{
	public abstract class FileDialog : IDisposable
	{
		public FileDialogType DialogType { get; private set; }
		public string Title { get; private set; }
		public string SelectedPath { get; protected set; }
		public string InitialDirectory { get; set; }
		public FileTypeFilter[] FileTypeFilters { get { return filters.ToArray(); } }
		private List<FileTypeFilter> filters = new List<FileTypeFilter>();

		public FileDialog CreateDialog(FileDialogType DialogType, string Title)
		{
			this.DialogType = DialogType;
			this.Title = Title;
			this.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            this.filters = new List<FileTypeFilter>();
			return GetDialog();
		}

		protected abstract FileDialog GetDialog();

		public void AddFileTypeFilter(FileTypeFilter Filter)
		{
			if (Filter == null) throw new ArgumentNullException("Filter must not be null");
			filters.Add(Filter);
		}

		public abstract WindowResponse Show();

		public abstract void Dispose();
	}

	public sealed class FileTypeFilter
	{
		public string FilterName { get; private set; }
		public string[] Filter { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Timelapse_UI.FileTypeFilter"/> class.
		/// </summary>
		/// <param name="FilterName">Displayed file type name (example: Text File)</param>
		/// <param name="Filter">Filter (example: txt)</param>
		public FileTypeFilter(string FilterName, params string[] Filter)
		{
			this.FilterName = FilterName;
			this.Filter = Filter;
		}
	}
}

