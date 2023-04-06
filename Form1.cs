using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KSPA_0._01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void compileButton_Click(object sender, EventArgs e)
        {
            // Отримати текст програми з RichTextBox1
            string programText = richTextBox1.Text;
            // Створити об'єкт для збереження результатів
            StringBuilder resultsBuilder = new StringBuilder();
            // Створити об'єкт класу команд та передати в нього результати
            Commands commands = new Commands(resultsBuilder);
            // Розділити програму на окремі рядки
            string[] lines = programText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            // Виконати кожен рядок програми
            foreach (string line in lines)
            {
                // Викликати функцію Execute з рядком програми
                commands.Execute(line);
            }
            richTextBox2.Text = "";
            // Відобразити результати в RichTextBox2
            richTextBox2.AppendText(resultsBuilder.ToString());
            richTextBox2.AppendText(commands.ERROR);
        }
        private void translateButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            saveFileDialog1.Title = "Save file";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Зберігаємо вміст RichTextBox в файл
                System.IO.File.WriteAllText(saveFileDialog1.FileName, richTextBox1.Text);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;
                if (Path.GetExtension(filePath).Equals(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    richTextBox1.Text = File.ReadAllText(filePath);
                }
                else
                {
                    MessageBox.Show("Please select a .txt file.");
                }
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Form2  form = new Form2();
            form.Show();
        }
    }
    public class Commands
    {
        private Dictionary<string, dynamic> variables = new Dictionary<string, dynamic>();
        public StringBuilder resultsBuilder;
        private CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US"); // для того, щоб роздільником десяткової частини була крапка
        public string ERROR ="";
        public Commands(StringBuilder resultsBuilder)
        {
            this.resultsBuilder = resultsBuilder;
        }

        public void ParseCommand(string command)
        {
            // Код для розбору команди та виконання потрібної дії
        }

        public void Execute(string commandsText)
        {
            var lines = commandsText.Split('\n', '\r').Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l));
            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                if (parts.Length < 2)
                {
                    ERROR = $"Неправильний запис команди {line}";
                    //throw new ArgumentException($"Invalid command: {line}");
                }
                switch (parts[0])
                {
                    case "IN":
                        try
                        {
                            if (parts.Length >= 4)
                            {
                                variables[parts[1]] = Convert.ToDouble(parts[3]);
                            }
                            else
                            {
                                throw new ArgumentException($"Invalid arguments for command: {parts[0]}");
                                ERROR = $"Неправильний запис команди: {parts[0]}";
                            }
                        }
                        catch
                        {
                            ERROR = $"Неправильний запис команди: {parts[0]}";
                        }
                        break;
                    case "ST":
                        try
                        {
                            if (parts.Length >= 4)
                            {
                                variables[parts[1]] = parts[3].Trim('"');
                            }
                            else
                            {
                                throw new ArgumentException($"Invalid arguments for command: {parts[0]}");
                                ERROR = $"Неправильний запис команди: {parts[0]}";
                            }
                        }
                        catch
                        {
                            ERROR = $"Неправильний запис команди: {parts[0]}";
                        }
                        break;
                    case "DR":
                        try
                        {
                            if (parts.Length >= 4)
                            {
                                variables[parts[1]] = Convert.ToDouble(parts[3], cultureInfo);
                            }
                            else
                            {
                                throw new ArgumentException($"Invalid arguments for command: {parts[0]}");
                                ERROR = $"Неправильний запис команди: {parts[0]}";
                            }
                        }
                        catch
                        {
                            ERROR = $"Неправильний запис команди: {parts[0]}";
                        }
                        break;
                    case "PR":
                        try
                        {
                            if (parts.Length >= 2)
                            {
                                var result = variables[parts[1]].ToString();
                                resultsBuilder.AppendLine(result);
                            }
                            else
                            {
                                throw new ArgumentException($"Invalid arguments for command: {parts[0]}");
                                ERROR = $"Неправильний запис команди: {parts[0]}";
                            }
                        }
                        catch
                        {
                            ERROR = $"Неправильний запис команди: {parts[0]}";
                        }
                        break;
                    case "UN":
                        try
                        {
                            if (parts.Length >= 6 && parts[2] == "=")
                            {
                                dynamic operand1;
                                dynamic operand2;
                                if (!variables.TryGetValue(parts[3], out operand1) || !variables.TryGetValue(parts[5], out operand2))
                                {
                                    throw new ArgumentException("Variable not found");
                                    ERROR = "Змінна не знайдена";
                                }
                                switch (parts[4])
                                {
                                    case "+":
                                        variables[parts[1]] = operand1 + operand2;
                                        break;
                                    case "-":
                                        variables[parts[1]] = operand1 - operand2;
                                        break;
                                    case "*":
                                        variables[parts[1]] = operand1 * operand2;
                                        break;
                                    case "/":
                                        variables[parts[1]] = operand1 / operand2;
                                        break;
                                    default:
                                        throw new ArgumentException($"Invalid operator: {parts[4]}");
                                        ERROR = $"Неправильний запис команди: {parts[4]}";
                                }
                            }
                            else
                            {
                                throw new ArgumentException($"Invalid arguments for command: {parts[0]}");
                            }
                        }
                        catch
                        {
                            ERROR = $"Неправильний запис команди: {parts[4]}";
                        }
                        break;
                   // default:
                      //  throw new ArgumentException($"Invalid command: {parts[0]}");
                }
            }
        }

        private void AddResult(string result)
        {
            resultsBuilder.AppendLine(result);
        }

        private dynamic Evaluate(string operation, dynamic operand1, dynamic operand2)
        {
            switch (operation)
            {
                case "+":
                    return operand1 + operand2;
                case "-":
                    return operand1 - operand2;
                case "*":
                    return operand1 * operand2;
                case "/":
                    return operand1 / operand2;
                case "%":
                    return operand1 % operand2;
                case "^":
                    return Math.Pow(operand1, operand2);
                default:
                    throw new ArgumentException($"Invalid operation: {operation}");
                    ERROR = $"Неправильний запис команди: {operation}";
            }
        }
        private dynamic ProcessVariable(string variable)
        {
            if (variables.ContainsKey(variable))
            {
                return variables[variable];
            }
            else if (double.TryParse(variable, NumberStyles.Any, cultureInfo, out double value))
            {
                return value;
            }
            else
            {
                ERROR = $"Невідома змінна або невірне значення: {variable}";
                return null;
            }
        }
    }
}
