using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace library
{
    class Socket
    {
        //public static WebSocket ws = new WebSocket("ws://library-nazirko.herokuapp.com");
        public static WebSocket ws = new WebSocket("ws://localhost:3000");

        public static Task<string> Data()
        {
            var tcs = new TaskCompletionSource<string>();
            ws.OnMessage += (sender, e) => tcs.TrySetResult(e.Data);
            return tcs.Task;
        }
    }
}
