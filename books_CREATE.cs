using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;

namespace CityCRUD
{
    public partial class books_CREATE : Form
    {
        Database database = new Database();
        public books_CREATE()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            DataTable authors = new DataTable();
            {
                SqlCommand command = new SqlCommand($"select id as id_authors, concat(name,' ', lastname) as authors_name from authors;", database.getConnection());
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(authors);

                comboBox_authors.DataSource = authors;
                comboBox_authors.DisplayMember = "authors_name";
                comboBox_authors.ValueMember = "id_authors";
            }

            DataTable publishing_houses = new DataTable();
            {
                SqlCommand command = new SqlCommand($"select id as id_publishing_houses, title as publishing_houses_title from publishing_houses;", database.getConnection());
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(publishing_houses);

                comboBox_publishing_houses.DataSource = publishing_houses;
                comboBox_publishing_houses.DisplayMember = "publishing_houses_title";
                comboBox_publishing_houses.ValueMember = "id_publishing_houses";
            }

            comboBox_authors.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_publishing_houses.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void books_CREATE_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox_title.Text == "" || textBox_pages.Text == "" || textBox_cost.Text == "")
            {
                MessageBox.Show("1.Введите название книги.\r\n" +
                                "2.Укажите количество страниц.\r\n" +
                                "3.Укажите стоимость книги.\r\n", "Несоответствие форме добавления записи");
            }
            else
            {
                SqlCommand cmd = new SqlCommand($"insert into books (title, pages, id_author, id_publishing_house, cost) values (@title, @pages, @id_author, @id_publishing_house, @cost);", database.getConnection());
                cmd.Parameters.Add("@title", SqlDbType.VarChar).Value = textBox_title.Text;
                cmd.Parameters.Add("@pages", SqlDbType.Int).Value = Int32.Parse(textBox_pages.Text);
                cmd.Parameters.Add("@id_author", SqlDbType.Int).Value = comboBox_authors.SelectedValue;
                cmd.Parameters.Add("@id_publishing_house", SqlDbType.Int).Value = comboBox_publishing_houses.SelectedValue;
                cmd.Parameters.Add("@cost", SqlDbType.Int).Value = Int32.Parse(textBox_cost.Text);


                database.openConnection();
             
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Книга '" + textBox_title.Text + "' успешно добавлена.", "Добавление книги...");

                    database.closeConnection();

                }
                else
                {
                    MessageBox.Show("Произошла ошибка при добавлении книги.", "Ошибка при добавлении...");
                }
            }
        }
    }
}
