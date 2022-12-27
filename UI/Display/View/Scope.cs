using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SLD.Insights.UI.View
{
	class Scope : UIElement
	{
		const int lineLength = 10;

		protected override void ArrangeCore(Rect finalRect)
		{
			base.ArrangeCore(finalRect);
		}

		protected override Size MeasureCore(Size availableSize)
		{
			return new Size(lineLength, lineLength);
		}

		protected override void OnRender(DrawingContext dc)
		{
			base.OnRender(dc);

			dc.DrawLine(new Pen(new SolidColorBrush(Colors.Black), 3), new Point(), new Point(lineLength, lineLength));
		}
	}
}
