using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Proyecto_Empleados.pry
{
    public partial class frmVentanaPrincipal : Form
    {
        public frmVentanaPrincipal()
        {
            InitializeComponent();
        }

        clsEmpleados empleado = new clsEmpleados();
        private void btnListarEmpleados_Click(object sender, EventArgs e)
        {
            empleado.Listar( dgvEmpleados );
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            clsEmpleados empleado = new clsEmpleados();
            empleado.Nombre = txtNombre.Text;
            empleado.idEmpleado = Convert.ToInt32 (txtId.Text);


            if (txtId.Text.Length > 3)
            {
                empleado.Agregar();
                txtId.Text = "";
                txtNombre.Text = "";
            }
            else
            {
                MessageBox.Show("El codigo de ID debe tener 4 caracteres");
            }

            
        }

        private void frmVentanaPrincipal_Load(object sender, EventArgs e)
        {
            empleado.Listar(dgvEmpleados);
            txtId.MaxLength = 4;

        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {

            empleado.GenerarReporte(sfdArchivo);
            dgvEmpleados.Rows.Clear();

        }
    }
}
