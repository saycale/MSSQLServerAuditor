namespace MSSQLServerAuditor.Preprocessor
{
	/// <summary>
	/// Data item for preprocessor
	/// </summary>
	public class PreprocessorData
	{
		/// <summary>
		/// Name of preprocessor loaded from configuration
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// X coordinate of starting cell, index starts from 1
		/// </summary>
		public int Column { get; set; }

		/// <summary>
		/// Y coordinate of starting cell, index starts from 1
		/// </summary>
		public int Row { get; set; }

		/// <summary>
		/// Width of a control, in numbers of cells
		/// </summary>
		public int ColSpan { get; set; }

		/// <summary>
		/// Height of a control, in numbers of cells
		/// </summary>
		public int RowSpan { get; set; }

		/// <summary>
		/// Control factory
		/// </summary>
		public IContentFactory ContentFactory { get; set; }

		/// <summary>
		/// Vertical align title space
		/// If "null" - dont show title space
		/// </summary>
		public VerticalTextAlign? VerticalTextAlign { get; set; }

		/// <summary>
		/// Text align title space
		/// Default: Left
		/// </summary>
		public TextAlign TextAlign { get; set; }
	}
}
