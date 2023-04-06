using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KSPA_0._01
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                // Відображення тексту у RichTextBox1
                switch (e.Index)
                {
                    case 0:
                        richTextBox1.Text = "Команда IN використовується для присвоєння цілого числового значення " + 
                            "та записується наступним чином:\n" +
                            "IN a = 5 \n" + "Запис має бути таким інакше винекне помилка. а є змінною якій присвоєне " + 
                            " у даному випадку 5";
                        break;
                    case 1:
                        richTextBox1.Text = "Команда DR використовується для присвоєння дробових значення " +
                            "та записується наступним чином:\n" +
                            "DR a = 0.5 \n" + "Запис має бути таким інакше винекне помилка. а є змінною якій присвоєне " +
                            " у даному випадку 0.5";
                        break;
                    case 2:
                        richTextBox1.Text = "Команда ST використовується для присвоєння змінній текстового значення " +
                            "та записується наступним чином:\n" +
                            "ST a = text \n запис має бути таким інакше винекне помилка. a є змінною якій присвоїно текст ";

                        break;
                    case 3:
                        richTextBox1.Text = "Команда UN використовується для присвоєння результатів арифметичних операцій між двома числовими змінними" +
                            "та записується наступним чином:\n" +
                            "UN c = a + b \n" +
                            "UN c = a - b \n" +
                            "UN c = a * b \n" +
                            "UN c = a / b \n" +
                            "запис має бути таким інакше винекне помилка. C є змінною якій присвоєно результат арифметичних операцій міє a та b" +
                            "Вище наведі всі варіації даної команди";
                        break;
                    case 4:
                        richTextBox1.Text = "Команда PR використовується для виводу та записується наступним чином\n" +
                            "PR a \n" +
                            "даний запис виведе значення зміної a";
                        break;
                    case 5:
                        richTextBox1.Text = "Текст для шостого пункту.";
                        break;
                    case 6:
                        richTextBox1.Text = "Текст для сьомого пункту.";
                        break;
                    default:
                        richTextBox1.Text = "";
                        break;
                }
            }
        }
    }
}
