﻿using CityCRUD.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CityCRUD
{
    public partial class main : Form
    {
        Database database = new Database();

        public static int id_book;
        String check;

        public main()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void main_Load(object sender, EventArgs e)
        {
            CreateColumns();
            refresh();

            // тестирование видов загрузок ORM
            test_ORM();
        }

        private void refresh()  // Обновление значений datagridview, снятие выделения + обновление переменной для проверки выбранной ячейки ( = null)
        {
            RefreshDataGrid_books(dataGridView1);
            dataGridView1.ClearSelection();
            check = null;
        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id_book", "id");
            dataGridView1.Columns.Add("title", "Название книги");
            dataGridView1.Columns.Add("author", "Автор");
            dataGridView1.Columns.Add("pages", "Количество страниц");
            dataGridView1.Columns.Add("publishing_house", "Издатель");
            dataGridView1.Columns.Add("cost", "Стоимость");
            dataGridView1.Columns[0].Width = 80;
            dataGridView1.Columns[1].Width = 235;
            dataGridView1.Columns[2].Width = 235;
            dataGridView1.Columns[3].Width = 200;
            dataGridView1.Columns[4].Width = 220;
            dataGridView1.Columns[5].Width = 135;

            this.dataGridView1.Columns[2].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleLeft;

            this.dataGridView1.Columns[3].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;

            this.dataGridView1.Columns[5].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }

        private void ReadSingleRow_books(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetInt32(3), record.GetString(4), record.GetInt32(5));

        }

        private void RefreshDataGrid_books(DataGridView dwg)
        {
            dwg.Rows.Clear();

            string queryString = $"select books.id, books.title as 'Название книги', concat(authors.name,' ', authors.lastname) as 'Автор', pages as 'Количество страниц', publishing_houses.title as 'Издатель', cost as 'Стоимость' from books join authors on authors.id = books.id_author  join publishing_houses on publishing_houses.id = books.id_publishing_house;";

            SqlCommand command = new SqlCommand(queryString, database.getConnection());
            database.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow_books(dwg, reader);
            }
            reader.Close();
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRow = e.RowIndex;
            String i;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                i = row.Cells[0].Value.ToString();
                check = i;
                id_book = Convert.ToInt32(i);
            }
        }


        private void button_delete_Click(object sender, EventArgs e)
        {
            if (check != null) // проверка того, выбрана ли у нас запись
            {
                try
                {
                    string queryString = $"delete from books where id = @id_book";

                    SqlCommand command = new SqlCommand(queryString, database.getConnection());
                    command.Parameters.Add("@id_book", SqlDbType.Int).Value = id_book;


                    database.openConnection();

                    if (command.ExecuteNonQuery() == 1)
                    {

                        MessageBox.Show("Запись успешно удалена.", "Удаление записи...");

                        refresh();
                        database.closeConnection();
                    }
                }
                catch (SqlException ex)
                {

                    // Проверяем код ошибки
                    if (ex.Number == 547) // Код 547 указывает на нарушение ссылочной целостности
                    {
                        MessageBox.Show("Невозможно удалить запись, так как она связана с дочерними таблицами.", "Ошибка при удалении...");
                    }
                    else
                    {
                        // Обрабатываем другие SQL-исключения
                        MessageBox.Show($"Произошла ошибка при удалении: {ex.Message}", "Ошибка");
                    }
                }
                
            }
            else
                MessageBox.Show("Ни одна запись не выбрана.");
        }

        private void button_update_Click(object sender, EventArgs e)
        {
            if (check != null)
            {
                books_UPDATE frm1 = new books_UPDATE();
                this.Hide();
                frm1.ShowDialog();

                refresh();

                this.Show();
            }
            else
                MessageBox.Show("Ни одна запись не выбрана.");
        }

        private void button_create_Click(object sender, EventArgs e)
        {
            books_CREATE frm1 = new books_CREATE();
            this.Hide();
            frm1.ShowDialog();

            refresh();

            this.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void test_ORM()
        {
            /*// ВИДЫ ЗАГРУЗОК ORM И ВРЕМЯ ИХ ВЫПОЛНЕНИЯ: //
            // 1) Жадная загрузка (Eager Loading)
            var stopwatchEager = Stopwatch.StartNew();
            using (var context = new books_db_klychevContext())
            {
                var authorsWithBooks = context.Authors
                                              .Include(a => a.Books)
                                              .ToList();

                foreach (var author in authorsWithBooks)
                {
                    Console.WriteLine($"Автор: {author.Name} {author.Lastname}");
                    foreach (var book in author.Books)
                    {
                        Console.WriteLine($"- Книга: \"{book.Title}\"");
                    }
                }
            }
            stopwatchEager.Stop();
            Console.WriteLine($"Жадная загрузка: {stopwatchEager.ElapsedMilliseconds} ms\n\n");


            // 2) Явная загрузка (Explicit Loading)
            var stopwatchExplicit = Stopwatch.StartNew();
            using (var context = new books_db_klychevContext())
            {
                var author = context.Authors.FirstOrDefault(a => a.Id == 1);

                if (author != null)
                {
                    context.Entry(author)
                           .Collection(a => a.Books)
                           .Load();

                    Console.WriteLine($"Автор: {author.Name} {author.Lastname}");
                    foreach (var book in author.Books)
                    {
                        Console.WriteLine($"- Книга: \"{book.Title}\"");
                    }
                }
            }
            stopwatchExplicit.Stop();
            Console.WriteLine($"Явная загрузка: {stopwatchExplicit.ElapsedMilliseconds} ms\n\n");


            // 3) Ленивая загрузка (Lazy Loading)
            var stopwatchLazy = Stopwatch.StartNew();
            using (var context = new books_db_klychevContext())
            {
                var author = context.Authors.FirstOrDefault(a => a.Id == 1);

                if (author != null)
                {
                    Console.WriteLine($"Автор: {author.Name} {author.Lastname}");
                    foreach (var book in author.Books)
                    {
                        Console.WriteLine($"- Книга: \"{book.Title}\"");
                    }
                }
            }
            stopwatchLazy.Stop();
            Console.WriteLine($"Ленивая загрузка: {stopwatchLazy.ElapsedMilliseconds} ms\n\n");



            // Использование JOIN и LEFT JOIN: //
            // 1) JOIN
            using (var context = new books_db_klychevContext())
            {
                var booksWithAuthors = from book in context.Books
                                       join author in context.Authors
                                       on book.IdAuthor equals author.Id
                                       select new
                                       {
                                           BookTitle = book.Title,
                                           AuthorName = $"{author.Name} {author.Lastname}"
                                       };

                foreach (var item in booksWithAuthors)
                {
                    Console.WriteLine($"Книга: {item.BookTitle}, Автор: {item.AuthorName}");
                }
            }

            // 2) Left JOIN
            Console.WriteLine($"\nLeft JOIN:");
            using (var context = new books_db_klychevContext())
            {
                var booksWithAuthors = from book in context.Books
                                       join author in context.Authors
                                       on book.IdAuthor equals author.Id into authorGroup
                                       from author in authorGroup.DefaultIfEmpty()
                                       select new
                                       {
                                           BookTitle = book.Title,
                                           AuthorName = author != null ? $"{author.Name} {author.Lastname}" : "Неизвестный автор"
                                       };

                foreach (var item in booksWithAuthors)
                {
                    Console.WriteLine($"Книга: {item.BookTitle}, Автор: {item.AuthorName}");
                }
            }*/

            // 1) Прямой SQL - запрос для получения всех авторов с их книгами
            var stopwatchSql = Stopwatch.StartNew();

            string queryString = $"select concat(name,' ',lastname) as author_name, books.title from authors\r\n  join books on books.id_author = authors.id;";
            database.openConnection();

            using (SqlCommand command = new SqlCommand(queryString, database.getConnection()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        string authorName = reader.GetString(0);
                        string bookTitle = reader.GetString(1);

                        Console.WriteLine($"Автор: {authorName}, Книга - \"{bookTitle}\"");
                    }
                    reader.Close();
                }
            }

            stopwatchSql.Stop();
            Console.WriteLine($"Прямой SQL-запрос: {stopwatchSql.ElapsedMilliseconds} ms\n\n");


            // 2) Прямой SQL - запрос для получения конкретного автора с его книгами
            stopwatchSql = Stopwatch.StartNew();

            queryString = $"select concat(name,' ',lastname) as author_name, books.title from authors\r\n  join books on books.id_author = authors.id where authors.id = @authors_id;";
            database.openConnection();

            using (SqlCommand command = new SqlCommand(queryString, database.getConnection()))
            {
                command.Parameters.AddWithValue("@authors_id", 10); // нужный ID автора

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        string authorName = reader.GetString(0);
                        string bookTitle = reader.GetString(1);

                        Console.WriteLine($"Автор: {authorName}, Книга - \"{bookTitle}\"");
                    }
                    reader.Close();
                }
            }

            stopwatchSql.Stop();
            Console.WriteLine($"Прямой SQL-запрос (один автор): {stopwatchSql.ElapsedMilliseconds} ms\n\n");

        }
    }
}
