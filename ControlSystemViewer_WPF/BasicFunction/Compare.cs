using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFunction
{
	public static class Compare
	{
		public static bool Same(List<bool> a, List<bool> b)
		{
			bool result = false;
			if (a == null && b == null)
				result = true;
			else if (a?.Count == 0 && b?.Count == 0)
				result = true;
			else if (a?.Count == b?.Count)
			{
				for (int i = 0; i < a.Count; i++)
				{
					if (a[i] != b[i]) break;
					if (i == a.Count - 1) result = true;
				}
			}
			return result;
		}
		public static bool Different(List<bool> a, List<bool> b)
		{
			return !Same(a, b);
		}

		public static bool Same(List<string> a, List<string> b)
		{
			bool result = false;
			if (a == null && b == null)
				result = true;
			else if (a?.Count == 0 && b?.Count == 0)
				result = true;
			else if (a?.Count == b?.Count)
			{
				a = a.OrderBy(s => s).ToList();
				b = b.OrderBy(s => s).ToList();
				for (int i = 0; i < a.Count; i++)
				{
					if (a[i] != b[i]) break;
					if (i == a.Count - 1) result = true;
				}
			}
			return result;
		}
		public static bool Different(List<string> a, List<string> b)
		{
			return !Same(a, b);
		}
	}
}

