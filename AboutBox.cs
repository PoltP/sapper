using System;
using System.Reflection;
using System.Windows.Forms;

namespace Sapper.WinUI {
	partial class AboutBox : Form {
		#region Assembly Attribute Accessors
		protected static string AssemblyTitle {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				if(attributes.Length > 0) {
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
					if(!String.IsNullOrEmpty(titleAttribute.Title)) {
						return titleAttribute.Title;
					}
				}
				return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}
		protected static string AssemblyVersion {
			get {
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}
		protected static string AssemblyProduct {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				if(attributes.Length == 0) {
					return "";
				}
				return ((AssemblyProductAttribute)attributes[0]).Product;
			}
		}
		protected static string AssemblyCopyright {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if(attributes.Length == 0) {
					return "";
				}
				return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}
		#endregion

		private void linkEMail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			System.Diagnostics.Process.Start(@"mailto:poltavets-pavel@mail.ru");
		}
		public AboutBox() {
			InitializeComponent();
			this.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture, "About \"{0}\"", AssemblyTitle);
			this.lblProductName.Text = AssemblyProduct;
			this.lblVersion.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture, "Version {0}", AssemblyVersion);
			this.lblCopyright.Text = AssemblyCopyright;
		}
	}
}
