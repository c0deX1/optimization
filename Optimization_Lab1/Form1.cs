using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Optimization_Lab1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private double epsilon;
        private double max;
        private double min;

        private void Button1_Click(object sender, EventArgs e)
        {
            Dictionary<double, double> passive = new Dictionary<double, double>();
            min = Convert.ToDouble(textBox1.Text.Split(' ')[0]);
            max = Convert.ToDouble(textBox1.Text.Split(' ')[1]);
            epsilon = Convert.ToDouble(textBox2.Text);
            FillPassive(passive, epsilon);
            Console.Write(passive);
            Draw(passive);
            PassiveSearch(passive);
            dihomySearch(epsilon, 0.0005);
        }

        private void PassiveSearch(Dictionary<double, double> passive)
        {
            double localMinY;
            double firstX = min;
            double firstY = Function(firstX);
            localMinY = passive[min];
            double localMinX = min;

            foreach (var el in passive)
                if (el.Value < localMinY)
                {
                    localMinX = el.Key;
                    localMinY = el.Value;
                }

            PutSolutionPassive(localMinX, localMinY);
        }

        private void dihomySearch(double epsilon, double delta)
        {
            double error = Math.Abs(max - min);
            double alpha = 0;
            double a = min;
            double af;
            double betta;
            double b = max;
            double bf;

            while (error >= epsilon)
            {
                alpha = (a + b) / 2 - delta;
                betta = (a + b) / 2 + delta;
                af = Function(alpha);
                bf = Function(betta);
                if (af <= bf)
                {
                    b = betta;
                }
                else
                {
                    a = alpha;
                }
                error = b - a;
            }
            PutSolutionDihomy(alpha, Function(alpha));
        }

        private void Draw(Dictionary<double, double> valuePairs)
        {
            foreach (var element in valuePairs)
            {
                chart1.Series[0].Points.AddXY(element.Key, element.Value);
            }
        }

        private void PutSolutionPassive(double x, double y)
        {
            string name = "Решение пассивным поиском - " + Math.Round(x, 4);
            try
            {
                chart1.Series.Add(name);
            }
            catch (ArgumentException) { }
            finally
            {
                chart1.Series[name].Color = Color.Red;
                chart1.Series[name].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                chart1.Series[name].Points.AddXY(x, y);
            }
        }

        private void PutSolutionDihomy(double x, double y)
        {
            string name = "Решение дихомией - " + Math.Round(x, 4);
            try
            {
                chart1.Series.Add(name);
            }
            catch (ArgumentException) { }
            finally
            {
                chart1.Series[name].Color = Color.Green;
                chart1.Series[name].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                chart1.Series[name].Points.AddXY(x, y);
            }
        }

        private double Function(double x)
        {
            return Math.Pow(x, 3) - x + Math.Exp(-x);
        }

        private void FillPassive(Dictionary<double, double> dict, double epsilon)
        {
            int numbers = 1;
            double distance = Math.Abs(max - min);
            double copy = epsilon;
            while (copy < 1)
            {
                numbers *= 10;
                copy *= 10;
            }
            label4.Text = numbers.ToString();
            double step = distance / numbers;

            for (int i = 0; i <= numbers; i++)
            {
                double x = min + i * step;
                double y = Function(x);
                dict.Add(x, y);
            }
        }
    }
}
