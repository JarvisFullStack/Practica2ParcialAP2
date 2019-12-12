using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TransaccionesWeb.Herramientas;

namespace TransaccionesWeb.Registros
{
	public partial class rClientes : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			LabelFecha.Text = DateTime.Now.Date.ToString("dd/MM/yy");
			this.textboxId.ReadOnly = true;
			if (!IsPostBack)
			{

				ViewState["data"] = new Clientes();
				int id = Request.QueryString["id"].ToInt();
				if (id > 0)
				{
					BLL.RepositorioBase<Clientes> repositorio = new BLL.RepositorioBase<Clientes>();
					Clientes data = repositorio.Buscar(id);
					if (data == null)
					{
						Utilidades.ShowToastr(this, "Clientes no encontrado!", "Advertencia", "warning");
						return;
					}

					LlenaCampos(data);
					Utilidades.ShowToastr(this, "Clientes Encontrada", "Exito!");
					textboxId.ReadOnly = true;
				}
			}
			else
			{
				Clientes data = (Clientes)ViewState["data"];
			}



		}

		public Clientes LlenaClase()
		{
			Clientes data = new Clientes();
			data.Id_Cliente = textboxId.Text.ToInt();
			data.Nombre = textboxNombre.Text;
			data.Balance = textboxBalance.Text.ToDecimal();
			data.Fecha = LabelFecha.Text.ToDatetime();
			return data;
		}

		public void LlenaCampos(Clientes data)
		{
			LabelFecha.Text = data.Fecha.ToFormatDate();
			textboxNombre.Text = data.Nombre;
			textboxId.Text = data.Id_Cliente.ToString();
			textboxBalance.Text = data.Balance.ToString();
		}

		protected void buttonBusqueda_Click(object sender, EventArgs e)
		{
			int id = textboxId.Text.ToInt();
			if (id == 0)
			{
				Utilidades.ShowToastr(this, "Debes ingresar los datos de busqueda correctamente", "Advertencia", "warning");
				return;
			}

			Clientes data = new BLL.RepositorioBase<Clientes>().Buscar(id);
			if (data == null)
			{
				Utilidades.ShowToastr(this, "No se encontro ninguna data con este id", "Advertencia", "warning");
				return;
			}

			LlenaCampos(data);
			Utilidades.ShowToastr(this, "Clientes Encontrada", "Exito!");
			textboxId.ReadOnly = true;
			return;

		}

		protected void NuevoButton_Click(object sender, EventArgs e)
		{
			Utilidades.ClearControls(formRegistro, new List<Type>() { typeof(TextBox) });
			textboxId.ReadOnly = false;
		}

		protected void GuardarButton_Click(object sender, EventArgs e)
		{
			Clientes data = LlenaClase();

			bool paso = true;
			if (data.Id_Cliente > 0)
			{
				paso = new BLL.RepositorioBase<Clientes>().Modificar(data);
			}
			else
			{
				paso = new BLL.RepositorioBase<Clientes>().Guardar(data);
				Clientes last = new BLL.RepositorioBase<Clientes>().GetList(x => true).LastOrDefault();
			}
			if (!paso)
			{
				Utilidades.ShowToastr(this, "Error al intentar guardar la data!", "Error", "error");
				return;
			}

			Utilidades.ShowToastr(this, "Registro Guardado Correctamete!", "Exito", "success");
			Utilidades.ClearControls(formRegistro, new List<Type>() { typeof(TextBox) });
			return;
		}

		private bool EsValido(Clientes usuario)
		{
			return true;
		}

		protected void EliminarButton_Click(object sender, EventArgs e)
		{
			int id = textboxId.Text.ToInt();
			if (id < 0)
			{
				Utilidades.ShowToastr(this, "Id invalido", "Advertencia", "warning");
				return;
			}
			BLL.RepositorioBase<Clientes> repositorio = new BLL.RepositorioBase<Clientes>();
			if (repositorio.Buscar(id) == null)
			{
				Utilidades.ShowToastr(this, "Registro no encontrado", "Advertencia", "warning");
				return;
			}

			bool paso = repositorio.Eliminar(id);
			if (!paso)
			{
				Utilidades.ShowToastr(this, "Error al intentar eliminar el registro", "Error", "error");
				return;
			}

			Utilidades.ShowToastr(this, "Registro eliminado correctamente!", "exito", "success");
			return;
		}
	}
}