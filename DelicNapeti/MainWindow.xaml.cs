using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DelicNapeti
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        #region Calculation
        
        /// <summary>
        /// 
        /// </summary>
        private void Count()
        {
            float result = -1;

            if (!U1gridWrap.IsEnabled && CheckInput(uTextBox.Text) && CheckInput(r1TextBox.Text) && CheckInput(r2TextBox.Text))
                result = GetU1(float.Parse(ReplaceComma(uTextBox.Text)), float.Parse(ReplaceComma(r1TextBox.Text)), float.Parse(ReplaceComma(r2TextBox.Text)));

            if (!U2gridWrap.IsEnabled && CheckInput(uTextBox.Text) && CheckInput(r1TextBox.Text) && CheckInput(r2TextBox.Text))
                result = GetU2(float.Parse(ReplaceComma(uTextBox.Text)), float.Parse(ReplaceComma(r1TextBox.Text)), float.Parse(ReplaceComma(r2TextBox.Text)));

            if (!R2gridWrap.IsEnabled && CheckInput(uTextBox.Text) && CheckInput(u2TextBox.Text) && CheckInput(r1TextBox.Text))
                result = GetR2(float.Parse(ReplaceComma(uTextBox.Text)), float.Parse(ReplaceComma(u2TextBox.Text)), float.Parse(ReplaceComma(r1TextBox.Text)));

            if (result != 0 && result != -1)
                ShowResult(result);
        }

        private float GetU1(float u, float r1, float r2)
        {
            return (u * (r1 / (r1 + r2)));
        }

        private float GetU2(float u, float r1, float r2)
        {
            return (u * (r2 / (r1 + r2)));
        }

        private float GetR2(float u, float u2, float r1)
        {
            return ((u2 * r1) / (u - u2));
        }

        private void ShowResult(float result)
        {
            resultLabel.Visibility = Visibility.Visible;
            OKbutton.Visibility = Visibility.Visible;
            resultButton.Visibility = Visibility.Collapsed;

            if (!U1gridWrap.IsEnabled)
                resultLabel.Content = "U1 = " + result + " V";

            if (!U2gridWrap.IsEnabled)
                resultLabel.Content = "U2 = " + result + " V";

            if (!R2gridWrap.IsEnabled)
                resultLabel.Content = "R2 = " + result + " Ω";
        }
        #endregion

        #region Check
        /// <summary>
        /// Check imput for empty or zero string, check and fix comma and check positive/negative numbers
        /// </summary>
        /// <param name="s">imput string</param>
        /// <returns>bool - true if string is valid</returns>
        private bool CheckInput(string s)
        {
            bool result = true;

            if (s == "" || s == "0")    // if string is empty or "0", return false
                return false;

            if (!s.All(char.IsDigit))   // if string is not only numeric, return false
            {
                s = s.Replace(".", ",");
                s = s.Replace(",", "");

                if (!s.All(char.IsDigit))   // fix comma
                    return false;
                else result = true;
            }

            if ((s.Length - s.Replace(",", "").Length) > 1) // check multiple comma
                return false;

            if (float.Parse(s, CultureInfo.CurrentCulture) < 0)     // if num is negative, result = false
                result = false;

            return result;
        }

        /// <summary>
        /// Internal fix for multi-cultural comma
        /// </summary>
        /// <param name="s">input string</param>
        /// <returns>string - valid comma in string</returns>
        private string ReplaceComma(string s)
        {
            return s.Replace(".", ",");
        }
        #endregion

        #region Clickable Grids

        // Not implemented yet
        private void Ugrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //
        }

        // Not implemented yet
        private void R1grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //
        }

        /// <summary>
        /// Show textboxes for R2 resistor count
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void R2grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DefaultOpacityVisibility();
            DefaultResultVisibility();

            r2TextBox.Text = "";

            R2gridWrap.IsEnabled = false;
            U1gridWrap.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Show textboxes for U1 voltage count
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void U1grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DefaultOpacityVisibility();
            DefaultResultVisibility();

            u1TextBox.Text = "";

            U1gridWrap.IsEnabled = false;
            U2gridWrap.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Show textboxes for U2 voltage count
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void U2grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DefaultOpacityVisibility();
            DefaultResultVisibility();

            u2TextBox.Text = "";

            U2gridWrap.IsEnabled = false;
            U1gridWrap.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region MouseEnter/Leave Opacity

        /// <summary>
        /// Show border on mouse enter event
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void grid_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid father = sender as Grid;
            father.Opacity = 0.3;
        }

        /// <summary>
        /// Hide border on mouse leave event
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void grid_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid father = sender as Grid;
            father.Opacity = 0;
        }
        #endregion

        #region DefaultStates

        /// <summary>
        /// Enable all grids in controlsWrap
        /// Set visible all grids
        /// Set text to "0" for all textboxes in grids
        /// </summary>
        private void DefaultOpacityVisibility()
        {
            foreach (Grid g in controlsWrap.Children)
            {
                if (!g.IsEnabled)
                    g.IsEnabled = true;
                if (g.Visibility != Visibility.Visible)
                    g.Visibility = Visibility.Visible;

                TextBox tb = g.Children[1] as TextBox;
                tb.Text = "0";
            }
        }

        /// <summary>
        /// Reset content to empty string
        /// Hide result label
        /// Hide OK button
        /// Show resultButton
        /// </summary>
        private void DefaultResultVisibility()
        {
            resultLabel.Content = "";
            resultLabel.Visibility = Visibility.Collapsed;

            OKbutton.Visibility = Visibility.Collapsed;
            resultButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Hide all gridWraps
        /// Reset text value of all gridWraps to "0"
        /// </summary>
        private void DefaultTextBoxVisibility()
        {
            UgridWrap.Visibility = Visibility.Collapsed;
            U1gridWrap.Visibility = Visibility.Collapsed;
            U2gridWrap.Visibility = Visibility.Collapsed;
            R1gridWrap.Visibility = Visibility.Collapsed;
            R2gridWrap.Visibility = Visibility.Collapsed;

            uTextBox.Text = "0";
            u1TextBox.Text = "0";
            u2TextBox.Text = "0";
            r1TextBox.Text = "0";
            r2TextBox.Text = "0";
        }
        #endregion

        #region UI events
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DefaultResultVisibility();

                OKbutton.Visibility = Visibility.Collapsed;
                resultButton.Visibility = Visibility.Visible;
            }
            
            if (e.Key == Key.Enter)
            {
                if (resultLabel.Visibility == Visibility.Visible)
                    DefaultResultVisibility();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Count();
        }

        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Count();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DefaultResultVisibility();
        }

        /// <summary>
        /// Check input character if is numeric or comma
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void uTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox father = sender as TextBox;


            if (!char.IsDigit(e.Text, e.Text.Length - 1) && e.Text != "," && e.Text != ".")
            {
                e.Handled = true;
            }

            if (e.Text == "," && (father.Text.Contains(",") || father.Text.Contains(".")))
                e.Handled = true;

            if (e.Text == "." && (father.Text.Contains(".") || father.Text.Contains(",")))
                e.Handled = true;

            if (father.Text.Length > 10)
                e.Handled = true;

        }
        #endregion
    }
}
