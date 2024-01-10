using Newtonsoft.Json;
using System;
using System.Windows.Forms;

namespace library
{
    public partial class Revision : Form
    {
        public Revision()
        {
            InitializeComponent();
            this.listBox1.Items.AddRange(Data.Order.Split('\n'));
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.listBox1.Text))
                {
                    MessageBox.Show("Оберіть замовлення!");
                    return;
                }

                if (Socket.ws.ReadyState.ToString() == "Open")
                {
                    var data = new Json
                    {
                        func = "orderInfo",
                        login = Data.Login,
                        id = this.listBox1.Text.Split('-')[1]
                    };
                    Socket.ws.Send(JsonConvert.SerializeObject(data));

                    var str = Socket.Data().Result;
                    if (str != "")
                    {
                        MessageBox.Show(str);
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.listBox1.Text))
                {
                    MessageBox.Show("Оберіть замовлення!");
                    return;
                }

                if (Socket.ws.ReadyState.ToString() == "Open")
                {
                    var data = new Json
                    {
                        func = "removeOrder",
                        login = Data.Login,
                        id = this.listBox1.Text.Split('-')[1]
                    };
                    Socket.ws.Send(JsonConvert.SerializeObject(data));

                    var str = Socket.Data().Result;
                    if (str != "")
                    {
                        this.listBox1.Items.Remove(this.listBox1.Text);
                        MessageBox.Show(str);
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
