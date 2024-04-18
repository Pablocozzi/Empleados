using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;

using System.IO;
using Proyecto_Empleados.pry;

namespace Proyecto_Empleados.pry
{
    internal class clsEmpleados
    {

        private OleDbConnection conexion = new OleDbConnection();
        private OleDbCommand comando = new OleDbCommand();
        private OleDbDataAdapter adaptador = new OleDbDataAdapter();

        private string CadenaConexion = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Empleados.mdb";
        private string Tabla = "Empleados";

        private Int32 idEmp;
        private string nom;

        public Int32 idEmpleado
        {
            get { return idEmp; }
            set { idEmp = value; }
        }
        public string Nombre
        {
            get { return nom; }
            set { nom = value; }
        }

        public void Listar(DataGridView Grilla)
        {
            try
            {
                conexion.ConnectionString = CadenaConexion;
                conexion.Open();
                comando.Connection = conexion;
                comando.CommandType = CommandType.TableDirect;
                comando.CommandText = Tabla;

                OleDbDataReader DR = comando.ExecuteReader();
                Grilla.Rows.Clear();
                if (DR.HasRows)
                {
                    while (DR.Read())
                    {
                        Grilla.Rows.Add(DR.GetInt32(0), DR.GetString(1));
                    }
                }

                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void Agregar()
        {
            try
            {
                conexion.ConnectionString = CadenaConexion;
                conexion.Open();
                comando.Connection = conexion;
                comando.CommandType = CommandType.TableDirect;
                comando.CommandText = Tabla;

                adaptador = new OleDbDataAdapter(comando);
                DataSet DS = new DataSet();
                adaptador.Fill(DS, Tabla);


                DataTable tabla = DS.Tables[Tabla];
                DataRow fila = tabla.NewRow();

                fila["ID"] = idEmp;
                fila["Nombre"] = nom;

                tabla.Rows.Add(fila);
                OleDbCommandBuilder ConciliaCampos = new OleDbCommandBuilder(adaptador); 
                adaptador.Update(DS, Tabla);
                conexion.Close();
                MessageBox.Show("Datos grabados!");
            }
            catch (Exception ex)
            {

                MessageBox.Show("El numero de ID ya pertenece a otro empleado");
            }
        }


        public void GenerarReporte(SaveFileDialog Archivo)
        {
            conexion.ConnectionString = CadenaConexion;
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.TableDirect;
            comando.CommandText = Tabla;
            OleDbDataReader DR = comando.ExecuteReader();

            Archivo.Filter = "Archivo de texto (*.txt) | *.txt ";
            Archivo.FilterIndex = 0;
            Archivo.Title = "Guardar reporte";
            Archivo.RestoreDirectory = true;

                if (Archivo.ShowDialog() == DialogResult.OK)
                {
                    string rutaArchivo = Archivo.FileName;
                    try
                    {
                        using (StreamWriter AD = new StreamWriter (rutaArchivo))
                        {
                            AD.WriteLine("Listado de empleados\n");
                            AD.WriteLine("Codigo     |     Nombre     ");
                            AD.Write("\n");

                            if (DR.HasRows)
                            {
                                while (DR.Read())
                                {
                                    AD.Write(DR.GetInt32(0));
                                    AD.Write("       |     ");
                                    AD.Write(DR.GetString(1));
                                    AD.Write("\n");

                                }
                            }
                        conexion.Close();
                        AD.Close();
                    }
                        MessageBox.Show("Archivo guardado exitosamente en: \n" + rutaArchivo);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al guardar el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    //sfdArchivo.ShowDialog();
                }
         
            
        }   
    }
}
