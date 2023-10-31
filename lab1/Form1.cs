using ModelingOfSystems1.Core.EmbeddedDistribution;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ModelingOfSystems1.Core;
using ModelingOfSystems1.Core.Generators;
using ModelingOfSystems1.Core.DistributionCalculator;

namespace ModelingOfSystems1
{
    public partial class Form1 : Form
    {
        private EmbeddedGenerator basicGenerator;
        private ExponentialGenerator exponentialGenerator;
        private NormalGenerator normalGenerator;
        private UniformGenerator uniformGenerator;
        public Form1()
        {
            basicGenerator = new EmbeddedGenerator();
            exponentialGenerator = new ExponentialGenerator(basicGenerator);
            normalGenerator = new NormalGenerator(basicGenerator);
            uniformGenerator = new UniformGenerator((0, 1));
            InitializeComponent();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void exponentialDistribution(ExponentialChartInfo chartInfo)
        {
            double l = chartInfo.l;
            int k = chartInfo.k;
            int count = chartInfo.count;

            List<double> distribution = exponentialGenerator.generateDistribution(count, l);

            double min = distribution.Min();
            double max = distribution.Max();

            double u = distribution.Average();

            double s = DistributionUtils.calculateDispersion(distribution);

            double h = (max - min) / k;
            List<Bin> binsN = exponentialGenerator.generateBins(distribution, k, min, max, h);
            List<Bin> binsPDF = exponentialGenerator.generateBinsPDF(distribution, k, min, max, h);
            List<Bin> ruleBinsPDF = exponentialGenerator.generateTheoreticalBinsPDF(binsPDF, l, h);

            double x2 = exponentialGenerator.computeChiSquared(binsN, 1/s);
            double x2cr = exponentialGenerator.getCriticalChiSquared(binsN);

            setUpChart(chartInfo.chart, chartInfo.firstSeriesTitle, chartInfo.secondSeriesTitle, min, max, h); ;

            setValues(binsPDF, chartInfo, chartInfo.firstSeriesTitle);
            setValues(ruleBinsPDF, chartInfo, chartInfo.secondSeriesTitle);
            chartInfo.label_lem.Text = "λ = " + l;
            chartInfo.label_u.Text = "µ̃ = " + u;
            chartInfo.label_si.Text = "σ̃ = " + s;
            chartInfo.label_x2.Text = "X² = " + x2;
            chartInfo.label_x2_cr.Text = "X²кр = " + x2cr;
        }

        private void normalDistribution(NormalChartInfo chartInfo)
        {
            double sigma = chartInfo.si;
            double me = chartInfo.a;
            int k = chartInfo.k;
            int count = chartInfo.count;

            List<double> distribution = normalGenerator.generateDistribution(count, me, sigma);

            double min = distribution.Min();
            double max = distribution.Max();

            double u = distribution.Average();

            double s = DistributionUtils.calculateDispersion(distribution);

            double h = (max - min) / k;

            List<Bin> binsPDF = normalGenerator.generateBinsPDF(distribution, k, min, max, h);
            List<Bin> ruleBinsPDF = normalGenerator.generateTheoreticalBinsPDF(binsPDF, me, sigma);
            List<Bin> binsN = normalGenerator.generateBins(distribution, k, min, max, h);

            double x2 = normalGenerator.computeChiSquared(binsN, me, sigma);
            double x2cr = normalGenerator.getCriticalChiSquared(binsN);

            setUpChart(chartInfo.chart, chartInfo.firstSeriesTitle, chartInfo.secondSeriesTitle, min, max, h); ;

            setValues(binsPDF, chartInfo, chartInfo.firstSeriesTitle);
            setValues(ruleBinsPDF, chartInfo, chartInfo.secondSeriesTitle);
            chartInfo.label_a.Text = "a = " + me;
            chartInfo.label_si_variable.Text = "σ = " + sigma;
            chartInfo.label_u.Text = "µ̃ = " + u;
            chartInfo.label_si.Text = "σ̃ = " + s;
            chartInfo.label_x2.Text = "X² = " + x2;
            chartInfo.label_x2_cr.Text = "X²кр = " + x2cr;
        }

        private void uniformDistribution(UniformChartInfo chartInfo)
        {
            double a = chartInfo.a;
            double c = chartInfo.c;
            int k = chartInfo.k;
            int count = chartInfo.count;

            List<double> distribution = uniformGenerator.generateDistribution(count, a, c);

            double min = distribution.Min();
            double max = distribution.Max();

            double u = distribution.Average();

            double s = DistributionUtils.calculateDispersion(distribution);

            double h = (max - min) / k;

            List<Bin> binsPDF = uniformGenerator.generateBinsPDF(distribution, k, min, max, h);
            List<Bin> ruleBinsPDF = uniformGenerator.generateTheoreticalBinsPDF(binsPDF);
            List<Bin> binsN = uniformGenerator.generateBins(distribution, k, min, max, h);
            double x2 = uniformGenerator.computeChiSquared(binsN);
            double x2cr = uniformGenerator.getCriticalChiSquared(binsN);

            setUpChart(chartInfo.chart, chartInfo.firstSeriesTitle, chartInfo.secondSeriesTitle, min, max, h); ;

            for (int i = 0; i < ruleBinsPDF.Count; i++) {
                System.Console.WriteLine("(" + ruleBinsPDF[i].getRange().Item1 + ", " + ruleBinsPDF[i].getRange().Item2  + ") " + " - " + ruleBinsPDF[i].getSize());
            }

            setValues(binsPDF, chartInfo, chartInfo.firstSeriesTitle);
            setValues(ruleBinsPDF, chartInfo, chartInfo.secondSeriesTitle);
            chartInfo.label_a.Text = "a = " + a;
            chartInfo.label_c.Text = "c = " + c;
            chartInfo.label_u.Text = "µ̃ = " + u;
            chartInfo.label_si.Text = "σ̃ = " + s;
            chartInfo.label_x2.Text = "X² = " + x2;
            chartInfo.label_x2_cr.Text = "X²кр = " + x2cr;
        }


        private void setUpChart(Chart chart, string firstSeriesTitle, string secondSeriesTitle, double min, double max, double step)
        {

            chart.ChartAreas[0].AxisX.Minimum = min;
            chart.ChartAreas[0].AxisX.Maximum = max;
            chart.ChartAreas[0].AxisX.Interval = step;
            chart.ChartAreas[0].AxisX.IntervalOffset = 0;
            chart.ChartAreas[0].AxisX.LabelStyle.Format = "0.00";
            chart.Series[firstSeriesTitle]["PointWidth"] = "1";
            chart.Series[secondSeriesTitle]["PointWidth"] = "1";
        }

        private void setValues(List<Bin> bins, ChartInfo chartInfo, string seriesName)
        {
            List<(double, double)> dataPoints = binsToDataPoints(bins);

            List<double> xValues = dataPoints.Select(tuple => tuple.Item1).ToList();
            List<double> yValues = dataPoints.Select(tuple => tuple.Item2).ToList();

            chartInfo.chart.Series[seriesName].Points.DataBindXY(xValues, yValues);
        }

        private List<(double, double)> binsToDataPoints(List<Bin> bins)
        {
            List<(double, double)> dataPoints = new List<(double, double)>();

            foreach (Bin bin in bins)
            {
                double x = bin.getMiddleNumber();
                double y = bin.getSize();

                dataPoints.Add((x, y));
            }

            return dataPoints.OrderBy(tuple => tuple.Item1).ToList();
        }

        private void loadExponentialSection() {
            const string baseSeriesTitle = "Фактична щільність";

            const string theoreticalSeriesTitle = "Теоретична щільність";

            const int distributionCount = 10_000;

            label_exp_count.Text = "Кількість - " + distributionCount;

            const int barCount = 20;

            double l = 0.5;

            exponentialDistribution(
                new ExponentialChartInfo(
                    chart_exp_1,
                    label_exp_u_1,
                    label_exp_si_1,
                    label_exp_x2_1,
                    label_exp_x2_cr_1,
                    baseSeriesTitle,
                    theoreticalSeriesTitle,
                    barCount,
                    distributionCount,
                    label_exp_lem_1,
                    l
                ));

            l = 2;
            exponentialDistribution(new ExponentialChartInfo(
                    chart_exp_2,
                    label_exp_u_2,
                    label_exp_si_2,
                    label_exp_x2_2,
                    label_exp_x2_cr_2,
                    baseSeriesTitle,
                    theoreticalSeriesTitle,
                    barCount,
                    distributionCount,
                    label_exp_lem_2,
                    l
                ));

            l = 5.25;
            exponentialDistribution(new ExponentialChartInfo(
                    chart_exp_3,
                    label_exp_u_3,
                    label_exp_si_3,
                    label_exp_x2_3,
                    label_exp_x2_cr_3,
                    baseSeriesTitle,
                    theoreticalSeriesTitle,
                    barCount,
                    distributionCount,
                    label_exp_lem_3,
                    l
                ));
        }

        private void loadNormalSection()
        {
            const string baseSeriesTitle = "Фактична щільність";

            const string theoreticalSeriesTitle = "Теоретична щільність";

            const int distributionCount = 10_000;
            label_normal_count.Text = "Кількість - " + distributionCount;

            const int barCount = 20;

            double sigma = 0.5;
            double me = 0.25;
            label_normal_si_header_1.Text = "σ = " + sigma;

            normalDistribution(
                new NormalChartInfo(
                    chart_normal_1,
                    label_normal_u_1,
                    label_normal_si_1,
                    label_normal_x2_1,
                    label_normal_x2_cr_1,
                    baseSeriesTitle,
                    theoreticalSeriesTitle,
                    barCount,
                    distributionCount,
                    label_normal_a_1,
                    sigma,
                    label_normal_si_variable_1,
                    me
                ));

            me = 0.75;
            normalDistribution(
                new NormalChartInfo(
                    chart_normal_2,
                    label_normal_u_2,
                    label_normal_si_2,
                    label_normal_x2_2,
                    label_normal_x2_cr_2,
                    baseSeriesTitle,
                    theoreticalSeriesTitle,
                    barCount,
                    distributionCount,
                    label_normal_a_2,
                    sigma,
                    label_normal_si_variable_2,
                    me
                ));


            sigma = 2.25;
            label_normal_si_header_2.Text = "σ = " + sigma;
            me = 1.75;
            normalDistribution(
                new NormalChartInfo(
                    chart_normal_3,
                    label_normal_u_3,
                    label_normal_si_3,
                    label_normal_x2_3,
                    label_normal_x2_cr_3,
                    baseSeriesTitle,
                    theoreticalSeriesTitle,
                    barCount,
                    distributionCount,
                    label_normal_a_3,
                    sigma,
                    label_normal_si_variable_3,
                    me
                ));

            me = 5.75;
            normalDistribution(
                new NormalChartInfo(
                    chart_normal_4,
                    label_normal_u_4,
                    label_normal_si_4,
                    label_normal_x2_4,
                    label_normal_x2_cr_4,
                    baseSeriesTitle,
                    theoreticalSeriesTitle,
                    barCount,
                    distributionCount,
                    label_normal_a_4,
                    sigma,
                    label_normal_si_variable_4,
                    me
                ));

            sigma = 4.25;
            label_normal_si_header_3.Text = "σ = " + sigma;
            me = 3.5;
            normalDistribution(
                new NormalChartInfo(
                    chart_normal_5,
                    label_normal_u_5,
                    label_normal_si_5,
                    label_normal_x2_5,
                    label_normal_x2_cr_5,
                    baseSeriesTitle,
                    theoreticalSeriesTitle,
                    barCount,
                    distributionCount,
                    label_normal_a_5,
                    sigma,
                    label_normal_si_variable_5,
                    me
                ));

            me = 8.75;
            normalDistribution(
                new NormalChartInfo(
                    chart_normal_6,
                    label_normal_u_6,
                    label_normal_si_6,
                    label_normal_x2_6,
                    label_normal_x2_cr_6,
                    baseSeriesTitle,
                    theoreticalSeriesTitle,
                    barCount,
                    distributionCount,
                    label_normal_a_6,
                    sigma,
                    label_normal_si_variable_6,
                    me
                ));

        }


        private void loadUniformSection()
        {
            const string baseSeriesTitle = "Фактична щільність";

            const string theoreticalSeriesTitle = "Теоретична щільність";

            const int distributionCount = 10_000;
            label_normal_count.Text = "Кількість - " + distributionCount;

            const int barCount = 20;

            double a = Math.Pow(5, 13);
            double c = Math.Pow(2, 31);

            uniformDistribution(new UniformChartInfo(
                    chart_uniform_1,
                    label_uniform_u_1,
                    label_uniform_si_1,
                    label_uniform_x2_1,
                    label_uniform_x2_cr_1,
                    baseSeriesTitle,
                    theoreticalSeriesTitle,
                    barCount,
                    distributionCount,
                    label_uniform_a_1,
                    a,
                    label_uniform_c_1,
                    c
                ));

            a = Math.Pow(5, 10);
            c = Math.Pow(2, 15);
            
            uniformDistribution(new UniformChartInfo(
                    chart_uniform_2,
                    label_uniform_u_2,
                    label_uniform_si_2,
                    label_uniform_x2_2,
                    label_uniform_x2_cr_2,
                    baseSeriesTitle,
                    theoreticalSeriesTitle,
                    barCount,
                    distributionCount,
                    label_uniform_a_2,
                    a,
                    label_uniform_c_2,
                    c
                ));

            a = Math.Pow(5, 9);
            c = Math.Pow(2, 26);

            uniformDistribution(new UniformChartInfo(
                    chart_uniform_3,
                    label_uniform_u_3,
                    label_uniform_si_3,
                    label_uniform_x2_3,
                    label_uniform_x2_cr_3,
                    baseSeriesTitle,
                    theoreticalSeriesTitle,
                    barCount,
                    distributionCount,
                    label_uniform_a_3,
                    a,
                    label_uniform_c_3,
                    c
                ));

            a = Math.Pow(5, 5);
            c = Math.Pow(2, 28);

            uniformDistribution(new UniformChartInfo(
                    chart_uniform_4,
                    label_uniform_u_4,
                    label_uniform_si_4,
                    label_uniform_x2_4,
                    label_uniform_x2_cr_4,
                    baseSeriesTitle,
                    theoreticalSeriesTitle,
                    barCount,
                    distributionCount,
                    label_uniform_a_4,
                    a,
                    label_uniform_c_4,
                    c
                ));
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadExponentialSection();
            loadNormalSection();
            loadUniformSection();
        }

        private class ChartInfo
        {
            public Chart chart;
            public Label label_u;
            public Label label_si;
            public Label label_x2;
            public Label label_x2_cr;
            public string firstSeriesTitle;
            public string secondSeriesTitle;
            public int k;
            public int count;

            public ChartInfo(
                Chart chart_,
                Label label_u_,
                Label label_si_,
                Label label_x2_,
                Label label_x2_cr_,
                string firstSeriesTitle_,
                string secondSeriesTitle_,
                int k_,
                int count_
            )
            {
                chart = chart_;
                label_u = label_u_;
                label_si = label_si_;
                label_x2 = label_x2_;
                label_x2_cr = label_x2_cr_;
                firstSeriesTitle = firstSeriesTitle_;
                secondSeriesTitle = secondSeriesTitle_;
                k = k_;
                count = count_;
            }
        }

        private class ExponentialChartInfo : ChartInfo
        {
            public double l;
            public Label label_lem;

            public ExponentialChartInfo(
                Chart chart_,
                Label label_u_,
                Label label_si_,
                Label label_x2_,
                Label label_x2_cr_,
                string firstSeriesTitle_,
                string secondSeriesTitle_,
                int k_,
                int count_,

                Label label_lem_,
                double l_
            ) : base(
                chart_,
                label_u_,
                label_si_,
                label_x2_,
                label_x2_cr_,
                firstSeriesTitle_,
                secondSeriesTitle_,
                k_,
                count_
            )
            {
                l = l_;
                label_lem = label_lem_;
            }
        }

        private class NormalChartInfo : ChartInfo
        {
            public double a;
            public Label label_a;
            public double si;
            public Label label_si_variable;

            public NormalChartInfo(
                Chart chart_,
                Label label_u_,
                Label label_si_,
                Label label_x2_,
                Label label_x2_cr_,
                string firstSeriesTitle_,
                string secondSeriesTitle_,
                int k_,
                int count_,

                Label label_a_,
                double a_,
                Label label_si_variable_,
                double si_
            ) : base(
                chart_,
                label_u_,
                label_si_,
                label_x2_,
                label_x2_cr_,
                firstSeriesTitle_,
                secondSeriesTitle_,
                k_,
                count_
            )
            {
                label_si_variable = label_si_variable_;
                label_a = label_a_;
                a = a_;
                si = si_;
            }
        }

        private class UniformChartInfo : ChartInfo
        {
            public double a;
            public Label label_a;
            public double c;
            public Label label_c;

            public UniformChartInfo(
                Chart chart_,
                Label label_u_,
                Label label_si_,
                Label label_x2_,
                Label label_x2_cr_,
                string firstSeriesTitle_,
                string secondSeriesTitle_,
                int k_,
                int count_,

                Label label_a_,
                double a_,
                Label label_c_,
                double c_
            ) : base(
                chart_,
                label_u_,
                label_si_,
                label_x2_,
                label_x2_cr_,
                firstSeriesTitle_,
                secondSeriesTitle_,
                k_,
                count_
            )
            {
                label_c = label_c_;
                label_a = label_a_;
                a = a_;
                c = c_;
            }
        }

        private void chart1_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_3(object sender, EventArgs e)
        {

        }

        private void exp_x2_cr_1_Click(object sender, EventArgs e)
        {

        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void chart3_Click(object sender, EventArgs e)
        {

        }

        private void label_normal_si_1_Click(object sender, EventArgs e)
        {

        }

        private void label_normal_u_1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }
    }
}