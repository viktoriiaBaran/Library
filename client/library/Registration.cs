using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace library
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string name = textBox5.Text;
                string surname = textBox4.Text;
                string login = textBox1.Text;
                string password_1 = textBox2.Text;
                string password_2 = textBox3.Text;

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) || string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password_1) || string.IsNullOrWhiteSpace(password_2))
                {
                    MessageBox.Show("Заповніть всі поля.");
                }
                else if (!Regex.IsMatch(name, @"^[А-ЯЁа-яё\s]+$"))
                {
                    MessageBox.Show("Поле ім'я повинно містити тільки кирилицю.");
                }
                else if (!Regex.IsMatch(surname, @"^[А-ЯЁа-яё\s]+$"))
                {
                    MessageBox.Show("Поле прізвище повинно містити тільки кирилицю.");
                }
                else if (login.Length < 6)
                {
                    MessageBox.Show("Логін повинен містити мінімум 6 символів!");
                }
                else if (!Regex.IsMatch(login, @"^[0-9a-zA-Z]+$"))
                {
                    MessageBox.Show("Логін повинен містити тільки цифри та латинські букви!");
                }
                else if (password_1.Length < 6)
                {
                    MessageBox.Show("Пароль повинен містити мінімум 6 символів!");
                }
                else if (!Regex.IsMatch(password_1, @"^[0-9a-zA-Z]+$"))
                {
                    MessageBox.Show("Пароль повинен містити тільки цифри та латинські букви!");
                }
                else if (password_1 != password_2)
                {
                    MessageBox.Show("Паролі повинні співпадати!");
                }
                else
                {
                    if (Socket.ws.ReadyState.ToString() != "Open") Socket.ws.Connect();

                    if (Socket.ws.ReadyState.ToString() == "Open")
                    {
                        var data = new Json
                        {
                            func = "registration",
                            name = name,
                            surname = surname,
                            login = login,
                            password = password_1
                        };
                        Socket.ws.Send(JsonConvert.SerializeObject(data));

                        var str = Socket.Data().Result;
                        MessageBox.Show(str);

                        if (str != "Користувач з даним логіном вже існує.")
                        {
                            Authorization authorization = new Authorization();
                            authorization.FormClosed += new FormClosedEventHandler(delegate { Close(); });
                            authorization.Show();
                            this.Hide();
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

        private void button1_Click(object sender, EventArgs e)
        {
            Authorization authorization = new Authorization();
            authorization.FormClosed += new FormClosedEventHandler(delegate { Close(); });
            authorization.Show();
            this.Hide();
        }
    }
}
