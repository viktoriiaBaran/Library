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
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            try
            {
                if (Socket.ws.ReadyState.ToString() == "Open")
                {
                    var data = new Json
                    {
                        func = "getBooks",
                    };
                    Socket.ws.Send(JsonConvert.SerializeObject(data));

                    var str = Socket.Data().Result;
                    if (str != "")
                    {
                        this.listBox1.Items.AddRange(str.Split('\n'));
                    }
                    this.label3.Text = Data.Name;
                }
                else
                {
                    MessageBox.Show("Сервер не відповідає, спробуйте пізніше.");
                }
            }
            catch (Exception ex)
            {
                // Опрацьовуємо винятки тут. Ви можете вивести повідомлення про помилку або виконати інші дії за потреби.
                MessageBox.Show($"Сталася помилка: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBox1.Text == "")
                {
                    MessageBox.Show("Оберіть хоча б одну книгу!");
                }
                else
                {
                    Order order = new Order(this.listBox1);
                    order.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (Socket.ws.ReadyState.ToString() == "Open")
                {
                    var data = new Json
                    {
                        func = "viewOrders",
                        login = Data.Login
                    };
                    Socket.ws.Send(JsonConvert.SerializeObject(data));

                    var str = Socket.Data().Result;
                    if (str != "")
                    {
                        Data.Order = str;
                        Revision revision = new Revision();
                        revision.Show();
                    }
                    else
                    {
                        MessageBox.Show("Наразі немає наявних замовлень!");
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

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.textBox1.Text.Length == 0)
                {
                    MessageBox.Show("Поле для пошуку не може бути пустим.");
                }
                else
                {
                    if (Socket.ws.ReadyState.ToString() == "Open")
                    {
                        var data = new Json
                        {
                            func = "searchBooks",
                            search = this.textBox1.Text
                        };
                        Socket.ws.Send(JsonConvert.SerializeObject(data));

                        var str = Socket.Data().Result;
                        if (str == "")
                        {
                            MessageBox.Show("Пошук не дав результатів.");
                        }
                        else
                        {
                            this.listBox1.Items.Clear();
                            this.listBox1.Items.AddRange(str.Split('\n'));
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.textBox1.Text == "")
                {
                    if (Socket.ws.ReadyState.ToString() == "Open")
                    {
                        var data = new Json
                        {
                            func = "getBooks",
                        };
                        Socket.ws.Send(JsonConvert.SerializeObject(data));

                        var str = Socket.Data().Result;
                        if (str != "")
                        {
                            this.listBox1.Items.Clear();
                            this.listBox1.Items.AddRange(str.Split('\n'));
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

        int i;
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var box = this.checkedListBox1;
                if (box.CheckedItems.Count != 0)
                {
                    if (box.SelectedIndex != i) box.SetItemChecked(i, false);
                    i = box.SelectedIndex;
                    if (Socket.ws.ReadyState.ToString() == "Open")
                    {
                        var data = new Json
                        {
                            func = "searchBooksSection",
                            search = this.checkedListBox1.Text
                        };
                        Socket.ws.Send(JsonConvert.SerializeObject(data));

                        var str = Socket.Data().Result;
                        if (str == "")
                        {
                            MessageBox.Show("Пошук не дав результатів.");
                        }
                        else
                        {
                            this.listBox1.Items.Clear();
                            this.listBox1.Items.AddRange(str.Split('\n'));
                        }
                    }
                    else
                    {
                        MessageBox.Show("Сервер не відповідає, спробуйте пізніше.");
                    }
                }
                else
                {
                    i = box.SelectedIndex;
                    if (Socket.ws.ReadyState.ToString() == "Open")
                    {
                        var data = new Json
                        {
                            func = "getBooks",
                        };
                        Socket.ws.Send(JsonConvert.SerializeObject(data));

                        var str = Socket.Data().Result;
                        if (str != "")
                        {
                            this.listBox1.Items.Clear();
                            this.listBox1.Items.AddRange(str.Split('\n'));
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

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBox1.Text == "")
                {
                    MessageBox.Show("Оберіть книгу!");
                }
                else if (this.listBox1.SelectedItems.Count >= 2)
                {
                    MessageBox.Show("Оберіть тільки одну книгу!");
                }
                else
                {
                    Data.Id = this.listBox1.Text.Split(':')[0];
                    Information information = new Information();
                    information.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult logout = MessageBox.Show("Ви впевнені, що хочете вийти?", "Підтвердження", MessageBoxButtons.YesNo);
                if (logout == DialogResult.Yes)
                {
                    if (Socket.ws.ReadyState.ToString() == "Open") Socket.ws.Close();
                    Authorization authorization = new Authorization();
                    authorization.FormClosed += new FormClosedEventHandler(delegate { Close(); });
                    authorization.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}");
            }
        }
    }
}
