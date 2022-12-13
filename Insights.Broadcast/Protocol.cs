using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.ServiceModel.Channels;
using System.Text;

namespace SLD.Insights
{
	class Protocol
	{
		internal const int DefaultPort = 17777;
		internal const int HighWatermark = 1000;

		internal static Insight Receive(SubscriberSocket socket)
		{
			// Topic
			var topic = socket.ReceiveFrameString();

			// TraceLevel
			var buffer = socket.ReceiveFrameBytes();
			var level = (TraceLevel)buffer[0];

			var insight = new Insight(level);

			// Variables
			insight.TimeStamp = DateTimeOffset.Parse(socket.ReceiveFrameString());
			insight.Source = socket.ReceiveFrameString();
			insight.Text = socket.ReceiveFrameString();

			return insight;
		}

		internal static void Send(PublisherSocket socket, Insight insight)
		{
			// Topic
			socket.SendMoreFrame(string.Empty);

			// TraceLevel
			socket.SendMoreFrame(new byte[] { (byte)insight.Level });

			// Variables
			socket.SendMoreFrame(insight.TimeStamp.ToString("o"));
			socket.SendMoreFrame(insight.Source);
			socket.SendFrame(insight.Text);
		}
	}
}
