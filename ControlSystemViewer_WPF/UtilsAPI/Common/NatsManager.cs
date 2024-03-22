using NATS.Client;
using STAN.Client;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace com.mirle.ibg3k0.ohxc.wpf.Common
{
    public class NatsManager
    {
        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        StanConnectionFactory StanConnectionFactory = null;
        IStanConnection conn = null;
        ConcurrentDictionary<string, IStanSubscription> dicSubscription = new ConcurrentDictionary<string, IStanSubscription>();
        Options natsOptions = null;
        ConcurrentDictionary<string, StanSubscriptionOptions> dicSubscription_cOpts = new ConcurrentDictionary<string, StanSubscriptionOptions>();
        ConcurrentDictionary<string, EventHandler<StanMsgHandlerArgs>> dicSubscription_handler = new ConcurrentDictionary<string, EventHandler<StanMsgHandlerArgs>>();
        object connCreatLock = new object();

        StanOptions cOpts = StanOptions.GetDefaultOptions();
        readonly string DefaultNatsURL = "nats://192.168.39.111:4224";
        readonly string[] servers_port = new string[] { "4222", "4223", "4224" };
        string clusterID = null;
        string clientID = null;
        string productID = null;
        IConnection natsConn = null;

        public NatsManager(string product_id, string cluster_id, string client_id,string port)
        {
            try
            {
                productID = product_id;
                clusterID = cluster_id;
                clientID = client_id;

                string nats_server_ip = getHostTableIP("nats.ohxc.mirle.com.tw");
                if (nats_server_ip != null)
                {
                    DefaultNatsURL = $"nats://{nats_server_ip}:" + port;
                }

                string[] srevers_url = new string[servers_port.Length]; 
                for (int i = 0; i < srevers_url.Length; i++)
                {
                    srevers_url[i] = $"nats://{nats_server_ip}:{servers_port[i]}";
                }
             
//#if DEBUG
               
                natsOptions = ConnectionFactory.GetDefaultOptions();
                natsOptions.ReconnectWait = 1000;
                natsOptions.MaxReconnect = Options.ReconnectForever;
                natsOptions.AllowReconnect = true;
                natsOptions.Servers = srevers_url;
                natsOptions.Url = DefaultNatsURL;
                natsOptions.Name = client_id;
                natsOptions.ReconnectedEventHandler += (sender, args) =>
                {
                    try
                    {
                        //ReSubscribeAll();
                        lock (connCreatLock)
                        {
                            if (conn != null)
                            {
                                if(conn.NATSConnection !=null)
                                {
                                    conn.NATSConnection.Close();                                 
                                }
                                conn.Close();
                                conn.Dispose();
                            }
                            conn = getConnection();
                            ResubscribeAll();
                        }
                    }
                    catch(Exception ex)
                    {
                        logger.Warn(ex, "Exception");
                    }
                };

                //natsOptions.PingInterval = 5000;
                natsConn = new ConnectionFactory().CreateConnection(natsOptions);

                cOpts.NatsURL = DefaultNatsURL;
                //cOpts.NatsConn = natsConn;
                clusterID = "test-cluster";
                //#else
                //                natsOptions = ConnectionFactory.GetDefaultOptions();
                //                natsOptions.MaxReconnect = Options.ReconnectForever;
                //                natsOptions.ReconnectWait = 1000;
                //                natsOptions.NoRandomize = true;
                //                natsOptions.Servers = srevers_url;
                //                natsOptions.Name = client_id;
                //                natsOptions.AllowReconnect = true;
                //                natsOptions.Timeout = 1000;
                //                natsOptions.PingInterval = 5000;
                //                natsOptions.Url = DefaultNatsURL;
                //                natsOptions.AsyncErrorEventHandler += (sender, args) =>
                //                {
                //                    logger.Error($"Server:{args.Conn.ConnectedUrl}{Environment.NewLine},Message:{args.Error}{Environment.NewLine},Subject:{args.Subscription.Subject}");
                //                    //Console.WriteLine("Error: ");
                //                    //Console.WriteLine("   Server: " + args.Conn.ConnectedUrl);
                //                    //Console.WriteLine("   Message: " + args.Error);
                //                    //Console.WriteLine("   Subject: " + args.Subscription.Subject);
                //                };

                //                natsOptions.ServerDiscoveredEventHandler += (sender, args) =>
                //                {
                //                    logger.Info($"A new server has joined the cluster:{String.Join(", ", args.Conn.DiscoveredServers)}");
                //                    //Console.WriteLine("A new server has joined the cluster:");
                //                    //Console.WriteLine("    " + String.Join(", ", args.Conn.DiscoveredServers));
                //                };

                //                natsOptions.ClosedEventHandler += (sender, args) =>
                //                {
                //                    logger.Info($"Connection Closed:{Environment.NewLine}Server:{args.Conn.ConnectedUrl}");
                //                    //Console.WriteLine("Connection Closed: ");
                //                    //Console.WriteLine("   Server: " + args.Conn.ConnectedUrl);
                //                };

                //                natsOptions.DisconnectedEventHandler += (sender, args) =>
                //                {
                //                    logger.Info($"Connection Disconnected:{Environment.NewLine}Server:{args.Conn.ConnectedUrl}");
                //                    //Console.WriteLine("Connection Disconnected: ");
                //                    //Console.WriteLine("   Server: " + args.Conn.ConnectedUrl);
                //                };

                //                IConnection natsConn = null;
                //                natsConn = new ConnectionFactory().CreateConnection(natsOptions);
                //                cOpts.NatsConn = natsConn;

                //#endif


                StanConnectionFactory = new StanConnectionFactory();
                // cOpts.NatsURL = DefaultNatsURL;
                conn = getConnection();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private string getHostTableIP(string ip)
        {
            string return_ip = null;
            var remoteipAdr = System.Net.Dns.GetHostAddresses(ip);
            if (remoteipAdr != null && remoteipAdr.Count() > 0)
            {
                return_ip = remoteipAdr[0].ToString();
            }

            return return_ip;
        }

        IStanConnection getConnection()
        {
            IConnection natsConn = null;
            natsConn = new ConnectionFactory().CreateConnection(natsOptions);
            cOpts.NatsConn = natsConn;

            return StanConnectionFactory.CreateConnection(clusterID, clientID, cOpts);
        }

        public string Publish(string subject, byte[] data, EventHandler<StanAckHandlerArgs> handler)
        {
            subject = $"{productID}_{subject}";
            return conn.Publish(subject, data, handler);
        }

        public void Publish(string subject, byte[] data)
        {
            subject = $"{productID}_{subject}";
            conn.Publish(subject, data);
        }

        public void Subscriber(string subject, EventHandler<StanMsgHandlerArgs> handler, bool in_all = false, bool is_last = false, ulong since_seq_no = 0, DateTime? since_duration = null)
        {
            subject = $"{productID}_{subject}";
            StanSubscriptionOptions sOpts = StanSubscriptionOptions.GetDefaultOptions();
            if (in_all)
            {
                sOpts.DeliverAllAvailable();
            }
            else if (is_last)
            {
                sOpts.StartWithLastReceived();
            }
            else if (since_seq_no != 0)
            {
                sOpts.StartAt(since_seq_no);
            }
            else if (since_duration.HasValue)
            {
                sOpts.StartAt(since_duration.Value);
            }
            dicSubscription.GetOrAdd(subject, conn.Subscribe(subject, sOpts, handler));
            dicSubscription_cOpts.GetOrAdd(subject,sOpts);
            dicSubscription_handler.GetOrAdd(subject, handler);
        }

        public void ResubscribeAll ()
        {
            //subject = $"{productID}_{subject}";
            try
            {
                IStanSubscription stanSubscription = null;

                foreach (var subject in dicSubscription.Keys)
                {
                    if (dicSubscription.TryRemove(subject, out stanSubscription))
                    {
                        dicSubscription.GetOrAdd(subject, conn.Subscribe(subject, dicSubscription_cOpts[subject], dicSubscription_handler[subject]));
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                logger.Warn(ex, "Exception");
            }
                    
        }

        public bool Unsubscribe(string subject)
        {
            subject = $"{productID}_{subject}";
            IStanSubscription stanSubscription = null;
            if (dicSubscription.TryRemove(subject, out stanSubscription))
            {
                stanSubscription.Unsubscribe();

                return true;
            }
            else
            {
                return false;
            }
        }

        public void close()
        {
            try
            {
                if (dicSubscription.Count > 0)
                {
                    foreach (var keyPair in dicSubscription)
                    {
                        //keyPair.Value.Unsubscribe();
                        keyPair.Value.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Warn(ex, "Exception");
            }

            conn?.Close();
        }
    }
}
