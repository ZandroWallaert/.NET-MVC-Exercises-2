using System;
using BusinessLogic.Application;

namespace trace
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length != 2){
                Console.WriteLine("Give command (--ping/--traceroute) and destination (bv. start.howest.be) please");
                return;
            }

            string command = args[0];
            string argument = args[1];

            if(command != "--ping" && command != "--traceroute"){
                Console.WriteLine("This command is not supported");
            }

            CommandApp app = new CommandApp();
            app.run(command, argument);
        }
    }
}
