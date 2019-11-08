﻿using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace StarWarsExplorer
{
	public class Program
	{
		[STAThread]
		public static void Main()
		{
			AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;
			App.Main();
		}

		private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			AssemblyName assemblyName = new AssemblyName(args.Name);

			var path = assemblyName.Name + ".dll";
			if (assemblyName.CultureInfo.Equals(CultureInfo.InvariantCulture) == false) path =
				$@"{assemblyName.CultureInfo}\{path}";

			using (Stream stream = executingAssembly.GetManifestResourceStream(path))
			{
				if (stream == null) return null;

				var assemblyRawBytes = new byte[stream.Length];
				stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);
				return Assembly.Load(assemblyRawBytes);
			}
		}
	}
}