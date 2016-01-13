using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MSSQLServerAuditor.Preprocessor
{
	/// <summary>
	///     Preprocessor Area data item
	/// </summary>
	public class PreprocessorAreaData
	{
		/// <summary>
		///     The ID of Area
		/// </summary>
		public string Id;

		/// <summary>
		///     The name of Area
		/// </summary>
		public string Name;

		/// <summary>
		///     The flag not to use splitter
		/// </summary>
		public bool NoSplitter;

		/// <summary>
		///     Column Width
		/// </summary>
		public float[] Columns;

		/// <summary>
		///     Rows Height
		/// </summary>
		public float[] Rows;

		/// <summary>
		///     Preprocessors
		/// </summary>
		public List<PreprocessorData> Preprocessors;

		/// <summary>
		///     Checks whether this class configured. true, if configured
		/// </summary>
		public bool IsConfigured
		{
			get
			{
				return NoSplitter
					? Preprocessors != null
					: Rows != null && Rows.Length > 0 && Columns != null && Columns.Length > 0 && Preprocessors != null;
			}
		}

		/// <summary>
		///     Preprocessor area with splitter
		/// </summary>
		public PreprocessorAreaData(string id, string name, string columns, string rows)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}

			if (name == null)
			{
				throw new ArgumentNullException("name");
			}

			if (columns == null)
			{
				throw new ArgumentNullException("columns");
			}

			if (string.IsNullOrWhiteSpace(columns))
			{
				throw new ArgumentOutOfRangeException("columns");
			}

			if (rows == null)
			{
				throw new ArgumentNullException("rows");
			}

			if (string.IsNullOrWhiteSpace(rows))
			{
				throw new ArgumentOutOfRangeException("rows");
			}

			this.Id            = id;
			this.Name          = name;
			this.NoSplitter    = false;
			this.Columns       = ParseGridDimension(columns, "columns");
			this.Rows          = ParseGridDimension(rows, "rows");
			this.Preprocessors = new List<PreprocessorData>();
		}

		/// <summary>
		///     Cleanups empty preprocessors and checks whether preprocessors exists
		/// </summary>
		/// <returns>true, when at least one preprocessor with control exists</returns>
		public void CheckPreprocessors()
		{
			if (!this.NoSplitter)
			{
				CheckPreprocessors(this.Columns.Length, this.Rows.Length, this.Preprocessors);
			}
		}

		/// <summary>
		///     Minimal size of any grid dimension (height or width)
		/// </summary>
		private const float GridDimensionEpsilon = 0.0001F;

		/// <summary>
		///     Parses and normalizes grid dimension string.
		/// </summary>
		/// <param name="value">Grid dimension string</param>
		/// <param name="fieldName">Grid dimension field name</param>
		/// <returns>Normalized dimension array</returns>
		public static float[] ParseGridDimension(string value, string fieldName)
		{
			string[] values = value.Split(new[] {';', ':'}, StringSplitOptions.None);

			float[] arr = values.Select(
				c =>
				{
					float f = 0.0F;

					if (!float.TryParse(c, NumberStyles.Float, CultureInfo.InvariantCulture, out f))
					{
						f = 0.0F;
					}
					else
					{
						if (f < 0.0F)
						{
							f = 0.0F;
						}
					}

					return f;
				}
			).ToArray();

			// Fill empty columns. This allows to define columns/rows i.e. "25;" or "30;;" and the system will calc the rest 1x75 or 2x35.
			float sum = arr.Where(c => c > GridDimensionEpsilon).Sum(c => c);

			if (sum < 100.0F - GridDimensionEpsilon)
			{
				int emptyColumnsCount = arr.Count(c => c <= GridDimensionEpsilon);

				if (emptyColumnsCount > 0)
				{
					sum = 100.0F - sum;

					float inc = sum / emptyColumnsCount;

					for (int i = 0, iMax = arr.Length; i < iMax; i++)
					{
						if (arr[i] > GridDimensionEpsilon)
						{
							continue;
						}

						arr[i] = (--emptyColumnsCount == 0) ? sum : inc;

						sum -= inc;
					}
				}
			}

			// Normalize sizes so that sum is exactly 100%
			sum = arr.Sum(c => c);

			if (sum < 100.0F - GridDimensionEpsilon || sum > 100.0F + GridDimensionEpsilon)
			{
				float factor = 100.0F / sum;

				sum = 100.0F - sum;

				int nonEmptyCount = arr.Count(c => c > GridDimensionEpsilon);

				for (int i = 0, iMax = arr.Length; i < iMax; i++)
				{
					float v = arr[i];

					if (v <= GridDimensionEpsilon)
					{
						continue;
					}

					float dec = 0.0f;

					if (--nonEmptyCount > 0)
					{
						arr[i] = v * factor;
						dec = arr[i] - v;
					}
					else
					{
						dec = sum;

						if ((arr[i] += sum) < 0)
						{
							arr[i] = 0;
						}
					}

					sum -= dec;
				}
			}

			return arr;
		}

		/// <summary>
		///     Formats grid dimension string
		/// </summary>
		/// <param name="values">Heights or widths</param>
		/// <returns>Properly formatted dimension string</returns>
		public static string FormatGridDimension(float[] values)
		{
			return String.Join(";", values.Select(v => v.ToString(CultureInfo.InvariantCulture)));
		}

		/// <summary>
		///     Checks preprocessors grid configuration
		/// </summary>
		/// <param name="columnsCount">Columns count</param>
		/// <param name="rowsCount">Rows count</param>
		/// <param name="preprocessors">Preprocessors in the area</param>
		private static void CheckPreprocessors(int columnsCount, int rowsCount, IEnumerable<PreprocessorData> preprocessors)
		{
			BitArray[] grid = new BitArray[rowsCount];

			for (int i = 0, iMax = rowsCount; i < iMax; i++)
			{
				grid[i] = new BitArray(columnsCount, false);
			}

			foreach (PreprocessorData preprocessor in preprocessors)
			{
				int x = preprocessor.Column, y = preprocessor.Row, w = preprocessor.ColSpan, h = preprocessor.RowSpan;

				// Check coordinates and size to be in bounds
				if (x < 1 || x + w - 1 > columnsCount || y < 1 || y + h - 1 > rowsCount)
				{
					throw new ArgumentOutOfRangeException("preprocessors", "Coordinates are incorrect for " + preprocessor.Name + "!");
				}

				for (int yMax = y + h; y < yMax; y++)
				{
					BitArray r = grid[y - 1];

					for (int xMax = x + w; x < xMax; x++)
					{
						// Two or more preprocessors overlapping!
						if (r[x - 1])
						{
							throw new ArgumentOutOfRangeException(
								"preprocessors",
								"Preporcessors overlapping at " + x + ":" + y + "! Check at least " +
								preprocessor.Name + "!"
							);
						}

						r[x - 1] = true;
					}
				}
			}
		}
	}
}
