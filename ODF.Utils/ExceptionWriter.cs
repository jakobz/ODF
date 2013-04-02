using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ODF.Utils.Exceptions
{
	/// <summary>
	/// Converts an Exception to string. Extracts as much information as possible.
	/// Intended to be used for logging or displaying diagnostic messages.
	/// </summary>
	public class ExceptionWriter
	{
		static string[] excludeMethodNames;
		static HashSet<string> propertiesNamesToExpand;
		// We are storing type name as strings to not involve unnecessary dependencies.
		static HashSet<string> propertyTypesToExpand;
		static HashSet<string> propertyNamespacesToExpand;
		static int indentStep = 1;
		static int maxDepth = 10; // Максимальная глубина вложенности, чтобы избежать рекурсии и не отрендерить слишком много
		static int maxEnumCount = 5; // Максимальное количество элементов перечислений

		static ExceptionWriter()
		{
			excludeMethodNames = new string[] { "Message", "Source", "TargetSite", "Data", "StackTrace", "HelpLink", "InnerException" };
			propertiesNamesToExpand = new HashSet<string>(new[] { "Entries" });
			propertyTypesToExpand = new HashSet<string>(new[] { "System.Data.Entity.Infrastructure.DbEntityEntry", "System.Data.Entity.Infrastructure.DbPropertyValues" });
			propertyNamespacesToExpand = new HashSet<string>(new[] { "GMS.Data.Models", "GMS.Web.Models" });
		}

		/// <summary>
		/// Извлекает информацию из исключения.
		/// </summary>
		/// <param name="e">Исключение.</param>
		/// <returns>Описание исключения в виде строки.</returns>
		public static string GetExceptionInfo(Exception e)
		{
			using (StringWriter writer = new StringWriter())
			{
				WriteExceptionInfo(e, writer);
				return writer.ToString();
			}
		}

		/// <summary>
		/// Извлекает информацию из исключения.
		/// </summary>
		/// <param name="e">Исключение.</param>
		/// <param name="writer">TextWriter для записи информации.</param>
		public static void WriteExceptionInfo(Exception e, TextWriter writer)
		{
			using (IndentedTextWriter sb = new IndentedTextWriter(writer))
			{
				RenderException(e, sb);
			}
		}

		private static void RenderException(Exception e, IndentedTextWriter sb)
		{
			RenderCommonInfo(e, sb);

			RenderExceptionData(e, sb);

			RenderObjectProperties(e, sb);

			RenderStackTrace(e, sb);

			RenderInnerExceptions(e, sb);
		}

		private static void RenderInnerExceptions(Exception e, IndentedTextWriter sb)
		{
			if (e.InnerException != null)
			{
				sb.WriteLine("Inner exception:");
				sb.Indent += indentStep;
				RenderException(e.InnerException, sb);
				sb.Indent -= indentStep;
			}
		}

		private static void RenderStackTrace(Exception e, IndentedTextWriter sb)
		{
			sb.Write("Stack trace:");
			if (e.StackTrace == null)
			{
				sb.WriteLine("[null]");
			}
			else
			{
				sb.WriteLine();
				sb.Indent += indentStep;
				foreach (string line in e.StackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
				{
					sb.WriteLine(line);
				}
				sb.Indent -= indentStep;
			}
		}

		private static void RenderObjectProperties(object e, IndentedTextWriter sb)
		{
			PropertyInfo[] properties = e.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (PropertyInfo property in properties)
			{
				if (IsValidProperty(property))
				{
					object propertyValue = "[Can't get property value]";
					try
					{
						propertyValue = property.GetValue(e, null);
					} catch (Exception)
					{
					}

					sb.Write(property.Name);
					sb.Write(": ");
					RenderProperty(property.Name, propertyValue, sb);
				}
			}
		}

		private static void RenderProperty(string propertyName, object property, IndentedTextWriter sb)
		{
			if (property == null)
			{
				sb.WriteLine("[null]");
			}
			else if (property is String)
			{
				sb.WriteLine(property.ToString());
			}
			else if (property is IEnumerable)
			{
				sb.WriteLine(property.GetType());
				sb.Indent += indentStep;
				var rendered = 0;
				foreach (var val in (IEnumerable)property)
				{
					RenderProperty("", val, sb);
					rendered++;
					if (rendered == maxEnumCount)
					{
						sb.WriteLine("[further elements skipped, Count = " + ((IEnumerable)property).OfType<object>().Count() + "]");
						break;
					}
				}
				sb.Indent -= indentStep;
			}
			else
			{
				sb.WriteLine(property.ToString());
				if ((sb.Indent < indentStep * maxDepth)
					&& (propertiesNamesToExpand.Contains(propertyName)
						|| propertyTypesToExpand.Contains(property.GetType().FullName)
						|| propertyNamespacesToExpand.Contains(property.GetType().Namespace)
						)
					)
				{
					sb.Indent += indentStep;
					RenderObjectProperties(property, sb);
					sb.Indent -= indentStep;
				}
			}
		}

		private static void RenderExceptionData(Exception e, IndentedTextWriter sb)
		{
			if (!(e.Data == null || e.Data.Count == 0))
			{
				sb.WriteLine("Data: ");
				sb.Indent += indentStep;

				foreach (object key in e.Data.Keys)
				{
					sb.Write(key.ToString());
					sb.Write(": ");
					object value = e.Data[key];
					if (value != null)
					{
						sb.WriteLine(value.ToString());
					}
					else
					{
						sb.WriteLine("[null]");
					}
				}

				sb.Indent -= indentStep;
			}
		}

		private static void RenderCommonInfo(Exception e, IndentedTextWriter sb)
		{
			sb.Write("Exception Type: ");
			sb.WriteLine(e.GetType().FullName);

			sb.Write("Message: ");
			foreach (string line in e.Message.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
			{
				sb.WriteLine(line);
			}

			if (!String.IsNullOrEmpty(e.Source))
			{
				sb.Write("Source: ");
				sb.WriteLine(e.Source);
			}
		}

		private static bool IsValidProperty(PropertyInfo property)
		{
			if (!property.CanRead)
			{
				return false;
			}

			if (excludeMethodNames.Contains(property.Name))
			{
				return false;
			}

			return true;
		}
	}
}
