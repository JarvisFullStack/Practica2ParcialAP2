using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TransaccionesWeb.Herramientas
{
	public static class Utilidades
	{
		public static Int32 ToInt(this String value)
		{
			int retorno = 0;
			int.TryParse(value, out retorno);

			return retorno;
		}

		public static DateTime ToDatetime(this object obj)
		{
			DateTime.TryParse(obj.ToString(), out DateTime value);
			return value;
		}
		static readonly string FECHA_FORMAT = "yyyy-MM-dd";
		public static string ToFormatDate(this DateTime dateTime)
		{
			return dateTime.ToString(FECHA_FORMAT);
		}

		public static decimal ToDecimal(this string valor)
		{
			decimal retorno = 0;
			decimal.TryParse(valor, out retorno);

			return retorno;
		}
		public static void ShowToastr(this Page page, string message, string title, string type = "info")
		{
			page.ClientScript.RegisterStartupScript(page.GetType(), "toastr_message",
				  String.Format("toastr.{0}('{1}', '{2}');", type.ToLower(), message, title), addScriptTags: true);
		}


		public static void CallJsFunction(Page page, Type type, string functionName)
		{
			page.ClientScript.RegisterStartupScript(type, "CallMyFunction", functionName, true);
		}

		public static void ClearControls(Control control, List<Type> controlsToClear)
		{
			foreach (Control c in control.Controls)
			{
				if (controlsToClear.Contains(c.GetType()))
				{
					if (c.GetType() == typeof(TextBox))
					{
						((TextBox)c).Text = string.Empty;
					}

					if (c.GetType() == typeof(DropDownList))
					{
						((DropDownList)c).SelectedIndex = 0;
					}

				}
			}
		}

		public static void LlenarDropDownList<T>(DropDownList dropDown, BLL.IRepository<T> repositorio, Expression<Func<T, bool>> expression, string dataValueField, string dataTextField) where T : class
		{
			dropDown.Items.Clear();
			dropDown.DataSource = repositorio.GetList(expression);
			dropDown.DataTextField = dataTextField;
			dropDown.DataValueField = dataValueField;
			dropDown.DataBind();
		}
			
		public static void LimpiarCookies(Page page)
		{
			page.Request.Cookies.Clear();
			page.Response.Cookies.Clear();
			page.Session.Abandon();
			page.Response.Cookies["EmpresaId"].Value = null;
			page.Response.Cookies["EmpresaNombre"].Value = null;
			page.Response.Cookies["UsuarioId"].Value = null;
			page.Response.Cookies["UsuarioNombre"].Value = null;

		}

		public static void FillGrid<T>(GridView gv, List<T> lista)
		{
			gv.DataSource = null;
			gv.DataBind();
			gv.DataSource = lista;
			gv.DataBind();
		}

		public static string Hash(string input) { var hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(input)); return string.Join("", hash.Select(b => b.ToString("x2")).ToArray()); }

	}
}