using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SLD.Insights.UI.View
{
	partial class Scope : UIElement
	{
		static InsightsSource Insights = new InsightsSource(nameof(Scope));

		const int lineLength = 10;

		public Scope()
		{
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo info) => base.OnRenderSizeChanged(info);

		protected override void ArrangeCore(Rect finalRect)
		{
			Insights.Trace($"Arrange: {finalRect}");

			base.ArrangeCore(finalRect);
		}

		protected override Size MeasureCore(Size availableSize)
		{
			Insights.Trace($"Measure: {availableSize}");

			// Take all we have
			return availableSize;
		}

		protected override void OnRender(DrawingContext dc)
		{
			Insights.Trace("Render");

			Render(dc);
		}

		#region Draw





		#endregion
	}
}
