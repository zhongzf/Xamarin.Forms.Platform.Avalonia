using AvaloniaForms.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaForms
{
	internal interface IToolbarProvider
	{
		Task<CommandBar> GetCommandBarAsync();
	}
}
