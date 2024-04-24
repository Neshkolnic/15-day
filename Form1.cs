using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace _15_day
{
    public partial class Form1 : Form
    {
        // Словарь для хранения заметок по датам
        private Dictionary<DateTime, string> notes = new Dictionary<DateTime, string>();
        // Путь к файлу, в котором будут храниться заметки
        private string notesFilePath = "notes.json";
        // Иконка в трее для отображения уведомлений
        private NotifyIcon notifyIcon;

        // Конструктор формы
        public Form1()
        {
            InitializeComponent();
            // Загрузка заметок из файла при запуске приложения
            LoadNotesFromFile();
            // Обновление текстового поля при запуске приложения
            monthCalendar1.DateSelected += UpdateNoteTextBox;
            UpdateNoteTextBox(null, null);

            // Инициализация иконки в трее
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = SystemIcons.Information;
            notifyIcon.Visible = true;
        }

        // Метод для загрузки заметок из файла
        private void LoadNotesFromFile()
        {
            if (File.Exists(notesFilePath))
            {
                string json = File.ReadAllText(notesFilePath);
                notes = JsonConvert.DeserializeObject<Dictionary<DateTime, string>>(json);
            }
        }

        // Метод для сохранения заметок в файл
        private void SaveNotesToFile()
        {
            string json = JsonConvert.SerializeObject(notes);
            File.WriteAllText(notesFilePath, json);
        }

        // Метод для обновления текстового поля с заметкой при выборе даты в календаре
        private void UpdateNoteTextBox(object sender, DateRangeEventArgs e)
        {
            DateTime selectedDate = monthCalendar1.SelectionStart.Date;
            if (notes.ContainsKey(selectedDate))
            {
                textBox1.Text = notes[selectedDate];
            }
            else
            {
                textBox1.Text = "";
            }
        }

        // Обработчик события нажатия кнопки "Сохранить заметку"
        private void button1_Click(object sender, EventArgs e)
        {
            DateTime selectedDate = monthCalendar1.SelectionStart.Date;
            notes[selectedDate] = textBox1.Text;
            SaveNotesToFile();
            MessageBox.Show("Note saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ShowNotification("Note saved successfully."); // Отправляем уведомление при сохранении заметки
        }

        // Обработчик события нажатия кнопки "Удалить заметку"
        private void button2_Click(object sender, EventArgs e)
        {
            DateTime selectedDate = monthCalendar1.SelectionStart.Date;
            if (notes.ContainsKey(selectedDate))
            {
                notes.Remove(selectedDate);
                SaveNotesToFile();
                MessageBox.Show("Note deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowNotification("Note deleted successfully."); // Отправляем уведомление при удалении заметки
            }
            else
            {
                MessageBox.Show("No note to delete for the selected date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            textBox1.Text = ""; // Очищаем текстовое поле после удаления заметки
        }

        // Метод для отображения всплывающего уведомления
        private void ShowNotification(string message)
        {
            notifyIcon.BalloonTipTitle = "Notification";
            notifyIcon.BalloonTipText = message;
            notifyIcon.ShowBalloonTip(3000); // Отображаем уведомление на 3 секунды
        }

        // Важно освободить ресурсы иконки в трее при закрытии формы

    }
}
