using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.SolverFoundation.Services;
using Microsoft.SolverFoundation.Solvers;
namespace solverTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SolverContext context = SolverContext.GetContext();
            Model model = context.CreateModel();

            Decision x11 = new Decision(Domain.IntegerNonnegative, "x11");
            Decision x12 = new Decision(Domain.IntegerNonnegative, "x12");
            Decision x13 = new Decision(Domain.IntegerNonnegative, "x13");
            Decision x14 = new Decision(Domain.IntegerNonnegative, "x14");
            Decision x15 = new Decision(Domain.IntegerNonnegative, "x15");
            Decision x21 = new Decision(Domain.IntegerNonnegative, "x21");
            Decision x22 = new Decision(Domain.IntegerNonnegative, "x22");
            Decision x23 = new Decision(Domain.IntegerNonnegative, "x23");
            Decision x24 = new Decision(Domain.IntegerNonnegative, "x24");
            Decision x25 = new Decision(Domain.IntegerNonnegative, "x25");
            Decision x31 = new Decision(Domain.IntegerNonnegative, "x31");
            Decision x32 = new Decision(Domain.IntegerNonnegative, "x32");
            Decision x33 = new Decision(Domain.IntegerNonnegative, "x33");
            Decision x34 = new Decision(Domain.IntegerNonnegative, "x34");
            Decision x35 = new Decision(Domain.IntegerNonnegative, "x35");
            Decision x41 = new Decision(Domain.IntegerNonnegative, "x41");
            Decision x42 = new Decision(Domain.IntegerNonnegative, "x42");
            Decision x43 = new Decision(Domain.IntegerNonnegative, "x43");
            Decision x44 = new Decision(Domain.IntegerNonnegative, "x44");
            Decision x45 = new Decision(Domain.IntegerNonnegative, "x45");
            Decision x51 = new Decision(Domain.IntegerNonnegative, "x51");
            Decision x52 = new Decision(Domain.IntegerNonnegative, "x52");
            Decision x53 = new Decision(Domain.IntegerNonnegative, "x53");
            Decision x54 = new Decision(Domain.IntegerNonnegative, "x54");
            Decision x55 = new Decision(Domain.IntegerNonnegative, "x55");

            model.AddDecisions(x11, x12, x13, x14, x15,
                               x21, x22, x23, x24, x25,
                               x31, x32, x33, x34, x35,
                               x41, x42, x43, x44, x45,
                               x51, x52, x53, x54, x55);

            model.AddConstraint("Row1", x11 + x12 + x13 + x14 + x15 >= 70);
            model.AddConstraint("Row2", x21 + x22 + x23 + x24 + x25 >= 80);
            model.AddConstraint("Row3", x31 + x32 + x33 + x34 + x35 >= 90);
            model.AddConstraint("Row4", x41 + x42 + x43 + x44 + x45 >= 85);
            model.AddConstraint("Row5", x51 + x52 + x53 + x54 + x55 >= 90);
            model.AddConstraint("Row6", x11 + x21 + x31 + x41 + x51 >= 110);
            model.AddConstraint("Row7", x12 + x22 + x32 + x42 + x52 >= 120);
            model.AddConstraint("Row8", x13 + x23 + x33 + x43 + x53 >= 130);
            model.AddConstraint("Row9", x14 + x24 + x34 + x44 + x54 >= 120);
            model.AddConstraint("Row10", x15 + x25 + x35 + x45 + x55 >= 100);


            model.AddGoal("Goal", GoalKind.Minimize, x11 + x12 + x13 + x14 + x15 +
                                                     x21 + x22 + x23 + x24 + x25 +
                                                     x31 + x32 + x33 + x34 + x35 +
                                                     x41 + x42 + x43 + x44 + x45 +
                                                     x51 + x52 + x53 + x54 + x55);

            Solution solution = context.Solve(new SimplexDirective());

            Report report = solution.GetReport();

            MessageBox.Show("x11=" + x11.ToString() + ",x12=" + x12.ToString());

            Console.WriteLine("x1: {0}, x2: {1}, x3: {2}", x11, x12, x13, x14, x15,
                                                           x21, x22, x23, x24, x25,
                                                           x31, x32, x33, x34, x35,
                                                           x41, x42, x43, x44, x45,
                                                           x51, x52, x53, x54, x55);
            Console.Write("{0}", report);
            Console.ReadLine();


        }
    }
}
