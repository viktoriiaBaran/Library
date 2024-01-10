using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace library
{
    public partial class Authorization : Form
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string login = this.textBox1.Text;
                string password = this.textBox2.Text;

                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                {
                    throw new Exception("Заповніть всі поля для входу.");
                }

                if (Socket.ws.ReadyState.ToString() != "Open") Socket.ws.Connect();

                if (Socket.ws.ReadyState.ToString() != "Open")
                {
                    throw new Exception("Сервер не відповідає, спробуйте пізніше.");
                }

                var data = new Json
                {
                    func = "login",
                    login = login,
                    password = password
                };
                Socket.ws.Send(JsonConvert.SerializeObject(data));

                var str = Socket.Data().Result;
                if (str == "missing_login")
                {
                    throw new Exception("Користувач з даним логіном відсутній!");
                }
                else if (str == "wrong_password")
                {
                    throw new Exception("Пароль неправильний!");
                }
                else if (str == "user_logined")
                {
                    throw new Exception("Даний користувач вже ввійшов в систему!");
                }

                Data.Login = this.textBox1.Text;
                Data.Name = str;
                Main main = new Main();
                main.FormClosed += new FormClosedEventHandler(delegate { Close(); });
                main.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Registration registration = new Registration();
            registration.FormClosed += new FormClosedEventHandler(delegate { Close(); });
            registration.Show();
            this.Hide();
        }
    }
}
