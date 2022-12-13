using NetMQ.Sockets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SLD.Insights
{
	public class InsightsEmitter : IObserver<Insight>
	{

		ConcurrentQueue<Insight> _outbox = new();
		ManualResetEventSlim _available = new();
		private IPEndPoint _endpoint;

		public InsightsEmitter(IPEndPoint endpoint)
		{
			_endpoint = endpoint;

			Task.Run(Emit);
		}

		private void Emit()
		{
			using (var socket = new PublisherSocket())
			{
				socket.Options.SendHighWatermark = Protocol.HighWatermark;
				socket.Bind("tcp://" + _endpoint.ToString());

				while (true)
				{
					_available.Wait();
					_available.Reset();

					while (_outbox.TryDequeue(out var insight))
					{
						Protocol.Send(socket, insight);
					}
				}
			}
		}

		public static InsightsEmitter Create(IPEndPoint endpoint)
			=> new InsightsEmitter(endpoint);

		public static InsightsEmitter CreateLocal(int port = Protocol.DefaultPort)
			=> Create(new IPEndPoint(IPAddress.Loopback, port));

		public void OnCompleted() => throw new NotImplementedException();
		public void OnError(Exception error) => throw new NotImplementedException();
		public void OnNext(Insight value)
		{
			_outbox.Enqueue(value);
			_available.Set();
		}
	}
}
