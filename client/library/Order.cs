using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace library
{
    public partial class Order : Form
    {
        ListBox list1;

        public Order(ListBox list)
        {
            list1 = list;
            InitializeComponent();
            try
            {
                if (Socket.ws.ReadyState.ToString() == "Open")
                {
                    var data = new Json
                    {
                        func = "getUserInfo",
                        login = Data.Login
                    };
                    Socket.ws.Send(JsonConvert.SerializeObject(data));

                    var str = Socket.Data().Result;
                    if (str != "")
                    {
                        this.textBox1.Text = str;
                        for (var i = 0; i < list.SelectedItems.Count; i++)
                        {
                            this.listBox1.Items.Add(list.SelectedItems[i]);
                        }
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
                string phone = this.textBox2.Text;
                if (phone.Length == 0)
                {
                    MessageBox.Show("Введіть номер телефону!");
                }
                else if (!Regex.IsMatch(phone, @"^[0-9]+$") || (phone.Length != 10 && phone.Length != 12))
                {
                    MessageBox.Show("Некоректний номер телефону!");
                }
                else
                {
                    string book = "";
                    for (var i = 0; i < this.listBox1.Items.Count; i++)
                    {
                        book += this.listBox1.Items[i] + "\n";
                    }
                    if (Socket.ws.ReadyState.ToString() == "Open")
                    {
                        var data = new Json
                        {
                            func = "createOrder",
                            login = Data.Login,
                            book = book.Trim(),
                            phone = phone
                        };
                        Socket.ws.Send(JsonConvert.SerializeObject(data));

                        var str = Socket.Data().Result;
                        if (str != "")
                        {
                            for (int i = 0; i < list1.Items.Count; i++)
                            {
                                list1.SetSelected(i, false);
                            }
                            MessageBox.Show("Замовлення успішно оформлено!\nКлюч - " + str);
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Сервер не відповідає, спробуйте пізніше.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}");
            }
        }
    }
}
