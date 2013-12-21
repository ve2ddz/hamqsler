/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2013 Jim Orcheson
 * 
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 * 
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace hamqsler
{
	/// <summary>
	/// Interaction logic for FormsCardViewContextMenu.xaml
	/// </summary>
	public partial class FormsCardViewContextMenu : ContextMenu
	{
		public static RoutedCommand SelectItemCommand = new RoutedCommand();
		public static RoutedCommand DeselectItemCommand = new RoutedCommand();
		public static RoutedCommand AddImageCommand = new RoutedCommand();
		public static RoutedCommand AddTextItemCommand = new RoutedCommand();
		public static RoutedCommand AddQsosBoxCommand = new RoutedCommand();
		public static RoutedCommand DeleteSelectedItemCommand = new RoutedCommand();
		public static RoutedCommand ClearBackgroundImageCommand = new RoutedCommand();
		public static RoutedCommand ShowQsosBoxAnchorCommand = new RoutedCommand();
		
		private FormsCardView formsView;
		public FormsCardView FormsView
		{
			get {return formsView;}
			set {formsView = value;}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public FormsCardViewContextMenu()
		{
			InitializeComponent();
		}
		
		/// <summary>
		/// Handler for SelectItem CanExecute event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void SelectItemCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = formsView != null && formsView.QslCard != null &&
				formsView.HighlightedCardItem != null;
		}
		
		/// <summary>
		/// Handler for DeselectItem CanExecute event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void DeselectItemCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = formsView != null && formsView.QslCard != null &&
				formsView.QslCard.GetSelectedItem() != null;
		}
		
		/// <summary>
		/// Handler for AddImage CanExecute event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void AddImageCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = formsView != null && formsView.QslCard != null &&
				formsView.QslCard.GetSelectedItem() == null;
		}
		
		/// <summary>
		/// Handler for AddText CanExecute event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void AddTextItemCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = formsView != null && formsView.QslCard != null &&
				formsView.QslCard.GetSelectedItem() == null;
		}

		/// <summary>
		/// Handler for AddQsosBox CanExecute event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void AddQsosBoxCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = formsView != null && formsView.QslCard != null &&
				formsView.QslCard.GetSelectedItem() == null &&
				formsView.QslCard.QsosBox == null;
		}

		/// <summary>
		/// Handler for DeleteSelectedItem CanExecute event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void DeleteSelectedItemCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if(formsView != null && formsView.QslCard != null)
			{
				CardWFItem cardItem = formsView.QslCard.GetSelectedItem();
				e.CanExecute = cardItem != null &&
					cardItem.GetType() != typeof(BackgroundWFImage);
			}
			else
			{
				e.CanExecute = false;
			}
		}

		/// <summary>
		/// Handler for ClearBackgroundImage CanExecute event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void ClearBackgroundImageCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if(formsView != null && formsView.QslCard != null)
			{
				CardWFItem cardItem = formsView.QslCard.GetSelectedItem();
				e.CanExecute = formsView.QslCard.BackgroundImage.ImageFileName != null &&
					formsView.QslCard.BackgroundImage.ImageFileName != string.Empty &&
					(cardItem == null || cardItem == formsView.QslCard.BackgroundImage);
			}
			else
			{
				e.CanExecute = false;
			}
		}
		
		/// <summary>
		/// Handler for ShowQsosBoxAnchor CanExecute event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void ShowQsosBoxAnchorCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = formsView != null && formsView.QslCard != null &&
			   formsView.QslCard.QsosBox != null &&
			   formsView.QslCard.QsosBox.IsSelected;
		}

		/// <summary>
		/// Handler for SelectItem Executed event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">ExecutedRoutedEventArgs object</param>
		private void SelectItemCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			System.Windows.Controls.MenuItem mi = e.OriginalSource as System.Windows.Controls.MenuItem;
			mi.Tag = formsView.HighlightedCardItem;
			((MainWindow)App.Current.MainWindow).OnSelectItem_Clicked(mi, e);
		}
		
		/// <summary>
		/// Handler for DeselectItem Executed event
		/// </summary>
		/// <param name="sender">object generating the event</param>
		/// <param name="e">ExecutedRoutedEventArgs object</param>
		private void DeselectItemCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			// pass processing to MainWindow.OnNone_Clicked
			((MainWindow)App.Current.MainWindow).OnNone_Clicked(sender, e);
		}

		/// <summary>
		/// Handler for AddImage Executed event
		/// </summary>
		/// <param name="sender">object generating the event</param>
		/// <param name="e">not used</param>
		private void AddImageCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			// pass processing to MainWindow.AddImageCommand_Executed
			((MainWindow)App.Current.MainWindow).AddImageCommand_Executed(
				sender, null);
		}

		/// <summary>
		/// Handler for AddTextItem Executed event
		/// </summary>
		/// <param name="sender">object generating the event</param>
		/// <param name="e">not used</param>
		private void AddTextItemCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			// pass processing to MainWindow.AddTextCommand_Executed
			((MainWindow)App.Current.MainWindow).AddTextCommand_Executed(
				sender, null);
		}

		/// <summary>
		/// Handler for AddQsosBox Executed event
		/// </summary>
		/// <param name="sender">object generating the event</param>
		/// <param name="e">not used</param>
		private void AddQsosBoxCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			// pass processing to MainWindow.AddQsosBoxCommand_Executed
			((MainWindow)App.Current.MainWindow).AddQsosBoxCommand_Executed(
				sender, null);
		}

		/// <summary>
		/// Handler for DeleteSelectedItem Executed event
		/// </summary>
		/// <param name="sender">object generating the event</param>
		/// <param name="e">not used</param>
		private void DeleteSelectedItemCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			// pass processing to MainWindow.DeleteItemCommand_Executed
			((MainWindow)App.Current.MainWindow).DeleteItemCommand_Executed(
				sender, null);
		}

		/// <summary>
		/// Handler for ClearBackgroundImage Executed event
		/// </summary>
		/// <param name="sender">object generating the event</param>
		/// <param name="e">not used</param>
		private void ClearBackgroundImageCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			// pass processing to MainWindow.ClearBackgroundCommand_Executed
			((MainWindow)App.Current.MainWindow).ClearBackgroundCommand_Executed(
				sender, null);
		}
	}
}