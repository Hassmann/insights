﻿using System.Windows;
using System.Windows.Media;

namespace SLD.Insights.UI.View
{
	partial class Scope
	{
		void Render(DrawingContext dc)
		{
			dc.DrawLine(new Pen(new SolidColorBrush(Colors.Black), 3), new Point(), new Point(lineLength, lineLength));

		}
	}
}
