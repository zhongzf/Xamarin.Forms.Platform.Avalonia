using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Forms.Extensions;
using Avalonia.Forms.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Avalonia.Forms.Controls
{
    public class NavigationPage : DynamicContentPage, INavigation
    {
		public static readonly StyledProperty<object> CurrentPageProperty = AvaloniaProperty.Register<NavigationPage, object>(nameof(CurrentPage));

		static NavigationPage()
		{
		}

		public TransitioningContentControl ContentControl { get; private set; }

		public ObservableCollection<object> InternalChildren { get; } = new ObservableCollection<object>();
		
		public object CurrentPage
		{
			get { return (object)GetValue(CurrentPageProperty); }
			set { SetValue(CurrentPageProperty, value); }
		}

		public NavigationPage()
		{
		}

		public NavigationPage(object root)
			: this()
		{
			this.Push(root);
		}

		protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
		{
			base.OnTemplateApplied(e);

			ContentControl = e.NameScope.Find<TransitioningContentControl>("PART_Navigation_Content");
		}


		#region INavigation
		public int StackDepth
		{
			get { return InternalChildren.Count; }
		}

		public void InsertPageBefore(object page, object before)
		{
			int index = InternalChildren.IndexOf(before);
			InternalChildren.Insert(index, page);
			ParentWindow?.SynchronizeAppBar();
		}

		public void RemovePage(object page)
		{
			if (InternalChildren.Remove(page))
			{
				if (ContentControl != null)
				{
					ContentControl.Transition = TransitionType.Normal;
				}
				CurrentPage = InternalChildren.Last();
			}

			ParentWindow?.SynchronizeAppBar();
		}

		public void Pop()
		{
			Pop(true);
		}

		public void Pop(bool animated)
		{
			if (StackDepth <= 1)
				return;

			if (InternalChildren.Remove(InternalChildren.Last()))
			{
				if (ContentControl != null)
				{
					ContentControl.Transition = animated ? TransitionType.Right : TransitionType.Normal;
				}
				CurrentPage = InternalChildren.Last();
			}
		}

		public void PopToRoot()
		{
			PopToRoot(true);
		}

		public void PopToRoot(bool animated)
		{
			if (StackDepth <= 1)
				return;

			object[] childrenToRemove = InternalChildren.Skip(1).ToArray();
			foreach (object child in childrenToRemove)
				InternalChildren.Remove(child);

			if (ContentControl != null)
			{
				ContentControl.Transition = animated ? TransitionType.Right : TransitionType.Normal;
			}
			CurrentPage = InternalChildren.Last();
		}

		public void Push(object page)
		{
			Push(page, true);
		}

		public void Push(object page, bool animated)
		{
			InternalChildren.Add(page);
			if (ContentControl != null)
			{
				ContentControl.Transition = animated ? TransitionType.Left : TransitionType.Normal;
			}
			CurrentPage = page;
		}
		
		public void PopModal()
		{
			PopModal(true);
		}

		public void PopModal(bool animated)
		{
			ParentWindow?.PopModal(animated);
		}

		public void PushModal(object page)
		{
			PushModal(page, true);
		}

		public void PushModal(object page, bool animated)
		{
			ParentWindow?.PushModal(page, animated);
		}
		#endregion

		public override string GetTitle()
		{
			if (ContentControl != null && ContentControl.Content is DynamicContentPage page)
			{
				return page.GetTitle();
			}
			return "";
		}

		public override bool GetHasNavigationBar()
		{
			if (ContentControl != null && ContentControl.Content is DynamicContentPage page)
			{
				return page.GetHasNavigationBar();
			}
			return false;
		}

		public override IEnumerable<Control> GetPrimaryTopBarCommands()
		{
			List<Control> frameworkElements = new List<Control>();
			frameworkElements.AddRange(this.PrimaryTopBarCommands);

			if (ContentControl != null && ContentControl.Content is DynamicContentPage page)
			{
				frameworkElements.AddRange(page.GetPrimaryTopBarCommands());
			}

			return frameworkElements;
		}

		public override IEnumerable<Control> GetSecondaryTopBarCommands()
		{
			List<Control> frameworkElements = new List<Control>();
			frameworkElements.AddRange(this.SecondaryTopBarCommands);

			if (ContentControl != null && ContentControl.Content is DynamicContentPage page)
			{
				frameworkElements.AddRange(page.GetSecondaryTopBarCommands());
			}

			return frameworkElements;
		}

		public override IEnumerable<Control> GetPrimaryBottomBarCommands()
		{
			List<Control> frameworkElements = new List<Control>();
			frameworkElements.AddRange(this.PrimaryBottomBarCommands);

			if (ContentControl != null && ContentControl.Content is DynamicContentPage page)
			{
				frameworkElements.AddRange(page.GetPrimaryBottomBarCommands());
			}

			return frameworkElements;
		}

		public override IEnumerable<Control> GetSecondaryBottomBarCommands()
		{
			List<Control> frameworkElements = new List<Control>();
			frameworkElements.AddRange(this.SecondaryBottomBarCommands);

			if (ContentControl != null && ContentControl.Content is DynamicContentPage page)
			{
				frameworkElements.AddRange(page.GetSecondaryBottomBarCommands());
			}

			return frameworkElements;
		}

		public bool GetHasBackButton()
		{
			if (ContentControl != null && ContentControl.Content is DynamicContentPage page)
			{
				return page.HasBackButton && StackDepth > 1;
			}
			return false;
		}

		public string GetBackButtonTitle()
		{
			if (StackDepth > 1)
			{
				return this.InternalChildren[StackDepth - 2].GetPropValue<string>("Title") ?? "Back";
			}
			return "";
		}

		public virtual void OnBackButtonPressed()
		{
			Pop();
		}
	}
}
