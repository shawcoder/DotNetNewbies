namespace AppConfigConfigurationBuilder
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Microsoft.Extensions.Configuration;

	public static class AppConfigConfigurationBinder
	{
		public static T Get<T>
			(this IConfiguration aConfiguration, string aSectionName)
				where T : class
		{
			T vResult = Activator.CreateInstance<T>();
			Type vResultType = typeof(T);
			Type vDictionaryType = typeof(Dictionary<string, string>);
			List<PropertyInfo> vProps =
				vResultType.GetProperties
					(BindingFlags.Instance | BindingFlags.Public).ToList();
			List<PropertyInfo> vDictProps =
			(
				from vProp in vProps
				where vDictionaryType.IsAssignableFrom(vProp.PropertyType)
					&& vProp.CanRead
					&& vProp.CanWrite
				select vProp
			).ToList();
			foreach (PropertyInfo vProp in vDictProps)
			{
				IDictionary<string, string> vContent =
					AppConfigConfigurationProviderBaseHelper.ParseFromFileName(aSectionName);
				vProp.SetValue(vResult, vContent);
			}
			return vResult;
		}

	}
}
