using NetMQ.Sockets;
using System;
using System.Net;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace SLD.Insights
{
	public class InsightsReceiver : IObservable<Insight>
	{
		private readonly Subject<Insight> _insights = new();
		private readonly IPEndPoint _endpoint;
		private SubscriberSocket? _socket;

		private InsightsReceiver(IPEndPoint endpoint)
		{
			_endpoint = endpoint;
		}

		public static InsightsReceiver Connect(IPEndPoint endpoint)
			=> new InsightsReceiver(endpoint);

		public static InsightsReceiver ConnectLocal(int port = Protocol.DefaultPort)
			=> Connect(new IPEndPoint(IPAddress.Loopback, port));

		public IDisposable Subscribe(IObserver<Insight> observer)
		{
			var subscription = _insights.Subscribe(observer);

			if (_socket is null)
			{
				Task.Run(Listen);
			}

			return subscription;
		}

		private void Listen()
		{
			using (_socket = new SubscriberSocket())
			{
				_socket.Options.ReceiveHighWatermark = Protocol.HighWatermark;
				_socket.Connect("tcp://" + _endpoint.ToString());
				_socket.SubscribeToAnyTopic();

				while (true)
				{
					var insight = Protocol.Receive(_socket);

					_insights.OnNext(insight);
				}
			}
		}
	}
}