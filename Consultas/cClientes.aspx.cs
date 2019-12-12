using BLL;
using Entidades;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TransaccionesWeb.Herramientas;

namespace TransaccionesWeb.Consultas
{
	public partial class cClientes : System.Web.UI.Page
	{
		List<Clientes> lista = new List<Clientes>();
		protected void Page_Load(object sender, EventArgs e)
		{
			if(!IsPostBack)
			{
				this.TextBoxFechaFin.Text = DateTime.Now.ToFormatDate();
				this.TextBoxFechaInicio.Text = DateTime.Now.ToFormatDate();
			}
			
		}		

		protected void filtrarFechaCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if(filtrarFechaCheckBox.Checked)
			{
				TextBoxFechaInicio.Enabled = true;
				TextBoxFechaFin.Enabled = true;
			} else
			{
				TextBoxFechaInicio.Enabled = false;
				TextBoxFechaFin.Enabled = false;
			}
		}

		protected void BotonBuscar_Click(object sender, EventArgs e)
		{

			SetDataSource(BuscarDatos());
		}

		public void SetDataSource(List<Clientes> listaClientes)
		{
			gridviewClientes.DataSource = null;
			gridviewClientes.DataSource = listaClientes;
			lista = listaClientes;
			gridviewClientes.DataBind();
		}

		public List<Clientes> BuscarDatos()
		{
			int filtroIndex = filtro.SelectedIndex;
			string criterio = criterioTextBox.Text;
			DateTime fechaInicio = TextBoxFechaInicio.Text.ToDatetime();
			DateTime fechaFin = TextBoxFechaFin.Text.ToDatetime();

			Expression<Func<Clientes, bool>> expression = x => true;

			if (filtrarFechaCheckBox.Checked)
			{
				switch (filtroIndex)
				{
					case 0:
						expression = x => true && x.Fecha >= fechaInicio && x.Fecha <= fechaInicio;
						break;
					case 1:
						expression = x => x.Id_Cliente == criterio.ToInt() && x.Fecha >= fechaInicio && x.Fecha <= fechaInicio;
						break;
					case 2:
						expression = x => x.Nombre == criterio && x.Fecha >= fechaInicio && x.Fecha <= fechaInicio;
						break;
				}
			}
			else
			{
				switch (filtroIndex)
				{
					case 0:
						expression = x => true;
						break;
					case 1:
						int id = criterio.ToInt();
						expression = x => x.Id_Cliente == id;
						break;
					case 2:
						expression = x => x.Nombre.Contains(criterio);
						break;
				}
			}

			List<Clientes> listaClientes = new RepositorioBase<Clientes>().GetList(expression);
			return listaClientes;

		}

		protected void BotonImprimir_Click(object sender, EventArgs e)
		{
			ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);

			ReportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

			ReportViewer.LocalReport.ReportPath = Server.MapPath(@"~\Reportes\ReporteClientes.rdlc");

			ReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSetClientes", BuscarDatos()));

			ReportViewer.LocalReport.Refresh();

		}
	}
}