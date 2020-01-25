using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Xamarin.Forms.Platform.Avalonia.Controls
{
    public class FormsContentDialog : ContentControl
    {
		public static readonly StyledProperty<object> TitleProperty = AvaloniaProperty.Register<FormsContentDialog, object>(nameof(Title));
		public static readonly StyledProperty<global::Avalonia.Markup.Xaml.Templates.DataTemplate> TitleTemplateProperty = AvaloniaProperty.Register<FormsContentDialog, global::Avalonia.Markup.Xaml.Templates.DataTemplate>(nameof(TitleTemplate));
		public static readonly StyledProperty<bool> FullSizeDesiredProperty = AvaloniaProperty.Register<FormsContentDialog, bool>(nameof(FullSizeDesired));
		public static readonly StyledProperty<bool> IsPrimaryButtonEnabledProperty = AvaloniaProperty.Register<FormsContentDialog, bool>(nameof(IsPrimaryButtonEnabled));
		public static readonly StyledProperty<bool> IsSecondaryButtonEnabledProperty = AvaloniaProperty.Register<FormsContentDialog, bool>(nameof(IsSecondaryButtonEnabled));
		public static readonly StyledProperty<ICommand> PrimaryButtonCommandProperty = AvaloniaProperty.Register<FormsContentDialog, ICommand>(nameof(PrimaryButtonCommand));
		public static readonly StyledProperty<ICommand> SecondaryButtonCommandProperty = AvaloniaProperty.Register<FormsContentDialog, ICommand>(nameof(SecondaryButtonCommand));
		public static readonly StyledProperty<string> PrimaryButtonTextProperty = AvaloniaProperty.Register<FormsContentDialog, string>(nameof(PrimaryButtonText));
		public static readonly StyledProperty<object> PrimaryButtonCommandParameterProperty = AvaloniaProperty.Register<FormsContentDialog, object>(nameof(PrimaryButtonCommandParameter));
		public static readonly StyledProperty<string> SecondaryButtonTextProperty = AvaloniaProperty.Register<FormsContentDialog, string>(nameof(SecondaryButtonText));
		public static readonly StyledProperty<object> SecondaryButtonCommandParameterProperty = AvaloniaProperty.Register<FormsContentDialog, object>(nameof(SecondaryButtonCommandParameter));

		public object Title
		{
			get { return (object)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public global::Avalonia.Markup.Xaml.Templates.DataTemplate TitleTemplate
		{
			get { return (global::Avalonia.Markup.Xaml.Templates.DataTemplate)GetValue(TitleTemplateProperty); }
			set { SetValue(TitleTemplateProperty, value); }
		}

		public bool FullSizeDesired
		{
			get { return (bool)GetValue(FullSizeDesiredProperty); }
			set { SetValue(FullSizeDesiredProperty, value); }
		}

		public bool IsPrimaryButtonEnabled
		{
			get { return (bool)GetValue(IsPrimaryButtonEnabledProperty); }
			set { SetValue(IsPrimaryButtonEnabledProperty, value); }
		}

		public bool IsSecondaryButtonEnabled
		{
			get { return (bool)GetValue(IsSecondaryButtonEnabledProperty); }
			set { SetValue(IsSecondaryButtonEnabledProperty, value); }
		}

		public ICommand PrimaryButtonCommand
		{
			get { return (ICommand)GetValue(PrimaryButtonCommandProperty); }
			set { SetValue(PrimaryButtonCommandProperty, value); }
		}

		public ICommand SecondaryButtonCommand
		{
			get { return (ICommand)GetValue(SecondaryButtonCommandProperty); }
			set { SetValue(SecondaryButtonCommandProperty, value); }
		}

		public string PrimaryButtonText
		{
			get { return (string)GetValue(PrimaryButtonTextProperty); }
			set { SetValue(PrimaryButtonTextProperty, value); }
		}

		public object PrimaryButtonCommandParameter
		{
			get { return (object)GetValue(PrimaryButtonCommandParameterProperty); }
			set { SetValue(PrimaryButtonCommandParameterProperty, value); }
		}

		public string SecondaryButtonText
		{
			get { return (string)GetValue(SecondaryButtonTextProperty); }
			set { SetValue(SecondaryButtonTextProperty, value); }
		}

		public object SecondaryButtonCommandParameter
		{
			get { return (object)GetValue(SecondaryButtonCommandParameterProperty); }
			set { SetValue(SecondaryButtonCommandParameterProperty, value); }
		}

		public event EventHandler<LightContentDialogClosedEventArgs> Closed;
		public event EventHandler<LightContentDialogClosingEventArgs> Closing;
		public event EventHandler<LightContentDialogOpenedEventArgs> Opened;
		public event EventHandler<LightContentDialogButtonClickEventArgs> PrimaryButtonClick;
		public event EventHandler<LightContentDialogButtonClickEventArgs> SecondaryButtonClick;

		public FormsContentDialog()
		{
			//this.DefaultStyleKey = typeof(FormsContentDialog);
			//this.CommandBindings.Add(new CommandBinding(PrimaryButtonRoutedCommand, OnPrimaryButtonRoutedExecuted));
			//this.CommandBindings.Add(new CommandBinding(SecondaryButtonRoutedCommand, this.SecondaryButtonRoutedExecuted));
		}

		//private void OnPrimaryButtonRoutedExecuted(object sender, ExecutedRoutedEventArgs e)
		//{
		//	LightContentDialogButtonClickEventArgs lightContentDialogButtonClickEventArgs = new LightContentDialogButtonClickEventArgs();

		//	PrimaryButtonClick?.Invoke(this, lightContentDialogButtonClickEventArgs);

		//	if (!lightContentDialogButtonClickEventArgs.Cancel)
		//	{
		//		PrimaryButtonCommand?.Execute(PrimaryButtonCommandParameter);
		//		tcs.TrySetResult(LightContentDialogResult.Primary);
		//	}
		//}

		//private void SecondaryButtonRoutedExecuted(object sender, ExecutedRoutedEventArgs e)
		//{
		//	LightContentDialogButtonClickEventArgs lightContentDialogButtonClickEventArgs = new LightContentDialogButtonClickEventArgs();

		//	SecondaryButtonClick?.Invoke(this, lightContentDialogButtonClickEventArgs);

		//	if (!lightContentDialogButtonClickEventArgs.Cancel)
		//	{
		//		SecondaryButtonCommand?.Execute(SecondaryButtonCommandParameter);
		//		tcs.TrySetResult(LightContentDialogResult.Secondary);
		//	}
		//}

		public async Task<LightContentDialogResult> ShowAsync()
		{
			if ((global::Avalonia.Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow is FormsWindow window)
			{
				window.Show();
				LightContentDialogOpenedEventArgs lightContentDialogOpenedEventArgs = new LightContentDialogOpenedEventArgs();
				Opened?.Invoke(this, lightContentDialogOpenedEventArgs);
			}

			LightContentDialogResult contentDialogResult = LightContentDialogResult.None;
			bool exit = false;

			//while (!exit)
			//{
			//	tcs = new TaskCompletionSource<LightContentDialogResult>();
			//	contentDialogResult = await tcs.Task;
			//	exit = InternalHide(contentDialogResult);
			//}

			return contentDialogResult;
		}

		private bool InternalHide(LightContentDialogResult contentDialogResult)
		{
			LightContentDialogClosingEventArgs lightContentDialogClosingEventArgs = new LightContentDialogClosingEventArgs(contentDialogResult);
			Closing?.Invoke(this, lightContentDialogClosingEventArgs);

			if (!lightContentDialogClosingEventArgs.Cancel && (global::Avalonia.Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow is FormsWindow window)
			{
				window.Hide();
				LightContentDialogClosedEventArgs lightContentDialogClosedEventArgs = new LightContentDialogClosedEventArgs(contentDialogResult);
				Closed?.Invoke(this, lightContentDialogClosedEventArgs);
				return true;
			}
			return false;
		}

		public void Hide()
		{
			InternalHide(LightContentDialogResult.None);
		}
	}

	internal interface ILightContentDialog
	{
		void Hide();

		Task<LightContentDialogResult> ShowAsync();

		bool FullSizeDesired { get; set; }
		bool IsPrimaryButtonEnabled { get; set; }
		bool IsSecondaryButtonEnabled { get; set; }
		ICommand PrimaryButtonCommand { get; set; }
		object PrimaryButtonCommandParameter { get; set; }
		string PrimaryButtonText { get; set; }
		ICommand SecondaryButtonCommand { get; set; }
		object SecondaryButtonCommandParameter { get; set; }
		string SecondaryButtonText { get; set; }
		object Title { get; set; }
		global::Avalonia.Markup.Xaml.Templates.DataTemplate TitleTemplate { get; set; }

		event EventHandler<LightContentDialogClosedEventArgs> Closed;
		event EventHandler<LightContentDialogClosingEventArgs> Closing;
		event EventHandler<LightContentDialogOpenedEventArgs> Opened;
		event EventHandler<LightContentDialogButtonClickEventArgs> PrimaryButtonClick;
		event EventHandler<LightContentDialogButtonClickEventArgs> SecondaryButtonClick;
	}

	public enum LightContentDialogResult
	{
		None = 0,
		Primary = 1,
		Secondary = 2
	}

	public sealed class LightContentDialogClosedEventArgs
	{
		public LightContentDialogResult Result { get; }

		public LightContentDialogClosedEventArgs(LightContentDialogResult result)
		{
			Result = result;
		}
	}

	public sealed class LightContentDialogClosingEventArgs
	{
		public bool Cancel { get; set; }
		public LightContentDialogResult Result { get; }

		public LightContentDialogClosingEventArgs(LightContentDialogResult result)
		{
			Result = result;
		}
	}

	public sealed class LightContentDialogOpenedEventArgs
	{
	}

	public sealed class LightContentDialogButtonClickEventArgs
	{
		public bool Cancel { get; set; }
	}
}
