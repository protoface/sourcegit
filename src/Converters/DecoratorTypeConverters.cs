﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace SourceGit.Converters
{
	public static class DecoratorTypeConverters
	{
		public static FuncValueConverter<Models.DecoratorType, IBrush> ToBackground =
			new FuncValueConverter<Models.DecoratorType, IBrush>(v =>
			{
				if (v == Models.DecoratorType.Tag)
					return Models.DecoratorResources.Backgrounds[0];
				return Models.DecoratorResources.Backgrounds[1];
			});

		public static FuncValueConverter<Models.DecoratorType, StreamGeometry> ToIcon =
			new FuncValueConverter<Models.DecoratorType, StreamGeometry>(v =>
			{
				var key = "Icons.Tag";
				switch (v)
				{
					case Models.DecoratorType.CurrentBranchHead:
						key = "Icons.Check";
						break;
					case Models.DecoratorType.RemoteBranchHead:
						key = "Icons.Remote";
						break;
					case Models.DecoratorType.LocalBranchHead:
						key = "Icons.Branch";
						break;
					default:
						break;
				}

				return Application.Current?.FindResource(key) as StreamGeometry;
			});
	}
}
