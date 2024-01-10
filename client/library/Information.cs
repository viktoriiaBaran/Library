using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace library
{
    public partial class Information : Form
    {
        public Information()
        {
            InitializeComponent();
            try
            {
                if (Socket.ws.ReadyState.ToString() == "Open")
                {
                    var data = new Json
                    {
                        func = "getUserInformation",
                        id = Data.Id
                    };

                    Socket.ws.Send(JsonConvert.SerializeObject(data));

                    var str = Socket.Data().Result;
                    if (str != "")
                    {
                        this.label1.Text = str;
                        this.Height = this.label1.Height + 100;
                    }
                }
                else
                {
                    MessageBox.Show("Сервер не відповідає, спробуйте пізніше.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}");
            }
        }
    }
}
