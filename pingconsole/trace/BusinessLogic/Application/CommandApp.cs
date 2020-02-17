using System;
using lab_traceroute;
using lab_traceroute.network;
using System.Collections.Generic;
namespace BusinessLogic.Application {
    class CommandApp{
        IEnumerable<IcmpResult> results;
        Client client = new Client();
        public void run(string command, string argument){
            switch(command){
                case "--ping":
                    results = client.Ping(argument);
                    break;
                case "--traceroute":
                    results = client.TraceRoute(argument);
                    break;
                default:
                    results = client.Ping(argument);
                    break;
            }
            foreach(IcmpResult result in results){
                if(result.Success){
                    Console.WriteLine("SUCCESFULL {0} from {1} to {2} / {3}", command, result.IPStartPoint, result.IPEndPoint, result.ErrorMessage);
                }
                else{
                    Console.WriteLine("{0} FAILED from {1} to {2} / {3}", command, result.IPStartPoint, result.IPEndPoint, result.ErrorMessage);
                }
            }
        }
    }
}