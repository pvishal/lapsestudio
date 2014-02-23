using System;
using System.Globalization;

namespace HelpAppUI
{
	public enum PP3DataType
	{
		Bool,
		Int,
		Double,
		String,
		VersionNr,
		Curve,
	}

	public class PP3Entry
	{
		public string Topic { get; set; }
		public string SafeTopic { get; set; }
		public string Name { get; set; }
		public PP3DataType Type { get; set; }
		public string Value { get; set; }
		public object MinValue { get; set; }
		public object MaxValue { get; set; }

		public bool MinMaxSuccesful { get; set; }

        private CultureInfo culture = new CultureInfo("en-US");

		public PP3Entry(string Topic, string Name, PP3DataType Type, string Min, string Max)
		{
			this.Topic = Topic;
			this.Name = Name;
			this.Type = Type;
			this.SafeTopic = Topic.Replace("&", "And").Replace(" ", "");

			switch (Type)
			{
				case PP3DataType.Double:
					double tmpD1, tmpD2 = 0;
                    MinMaxSuccesful = double.TryParse(Min, NumberStyles.Any, culture, out tmpD1) && double.TryParse(Max, NumberStyles.Any, culture, out tmpD2);
					MinValue = tmpD1;
					MaxValue = tmpD2;
					break;
				case PP3DataType.Int:
					int tmpI1, tmpI2 = 0;
                    MinMaxSuccesful = int.TryParse(Min, NumberStyles.Any, culture, out tmpI1) && int.TryParse(Max, NumberStyles.Any, culture, out tmpI2);
					MinValue = tmpI1;
					MaxValue = tmpI2;
					break;

				default:
					Min = Max = null;
					MinMaxSuccesful = true;
					break;
			}
		}
	}
}

