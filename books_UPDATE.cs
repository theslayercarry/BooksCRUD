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

namespace CityCRUD
{
    public partial class books_UPDATE : Form
    {
        Database database = new Database();


        String title;
        int pages, id_author, id_publishing_house, cost, id_book;

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox_title.Text == "" || textBox_pages.Text == "" || textBox_cost.Text == "")
            {
                MessageBox.Show("1.Введите название книги.\r\n" +
                                "2.Укажите количество страниц.\r\n" +
                                "3.Укажите стоимость книги.\r\n", "Несоответствие форме изменения записи");
            }
            else
            {
                SqlCommand cmd = new SqlCommand($"update books set title = @title, pages = @pages, id_author = @id_author, id_publishing_house = @id_publishing_house, cost = @cost where id = @id_book;", database.getConnection());
                cmd.Parameters.Add("@title", SqlDbType.VarChar).Value = textBox_title.Text;
                cmd.Parameters.Add("@pages", SqlDbType.Int).Value = Int32.Parse(textBox_pages.Text);
                cmd.Parameters.Add("@id_author", SqlDbType.Int).Value = comboBox_authors.SelectedValue;
                cmd.Parameters.Add("@id_publishing_house", SqlDbType.Int).Value = comboBox_publishing_houses.SelectedValue;
                cmd.Parameters.Add("@cost", SqlDbType.Int).Value = Int32.Parse(textBox_cost.Text);
                cmd.Parameters.Add("@id_book", SqlDbType.Int).Value = id_book;


                database.openConnection();

                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Данные успешно изменены.", "Изменение данных...");

                    database.closeConnection();

                }
                else
                {
                    MessageBox.Show("Ошибка при изменении данных.");
                }
            }
        }

        public books_UPDATE()
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

        private void books_UPDATE_Load(object sender, EventArgs e)
        {
            id_book = main.id_book;

            database.openConnection();
            {

                SqlCommand command = new SqlCommand($"select title from books where id = @id_book", database.getConnection());
                command.Parameters.Add("@id_book", SqlDbType.Int).Value = id_book;
                title = command.ExecuteScalar().ToString();

                command = new SqlCommand($"select pages from books where id = @id_book", database.getConnection());
                command.Parameters.Add("@id_book", SqlDbType.Int).Value = id_book;
                pages = Int32.Parse(command.ExecuteScalar().ToString());

                command = new SqlCommand($"select cost from books where id = @id_book", database.getConnection());
                command.Parameters.Add("@id_book", SqlDbType.Int).Value = id_book;
                cost = Int32.Parse(command.ExecuteScalar().ToString());

                command = new SqlCommand($"select id_author from books where id = @id_book", database.getConnection());
                command.Parameters.Add("@id_book", SqlDbType.Int).Value = id_book;
                id_author = Int32.Parse(command.ExecuteScalar().ToString());

                command = new SqlCommand($"select id_publishing_house from books where id = @id_book", database.getConnection());
                command.Parameters.Add("@id_book", SqlDbType.Int).Value = id_book;
                id_publishing_house = Int32.Parse(command.ExecuteScalar().ToString());

            }
            database.closeConnection();

            label_id.Text = id_book.ToString();
            textBox_title.Text = title;
            textBox_pages.Text = pages.ToString();
            textBox_cost.Text = cost.ToString();
            comboBox_authors.SelectedValue = id_author;
            comboBox_publishing_houses.SelectedValue = id_publishing_house;
        }
    }
}
