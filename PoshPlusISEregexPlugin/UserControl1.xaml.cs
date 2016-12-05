using System;
using System.Collections.Generic;
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
using Microsoft.PowerShell.Host.ISE;
using System.Text.RegularExpressions;
using System.IO;

namespace PoshPlusISEregexPlugin
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : IAddOnToolHostObject
    {
        public List<datagridRegexMatch> regexSearchMatches = new List<datagridRegexMatch>();

        public UserControl1()
        {
            InitializeComponent();
        }

        public ObjectModelRoot HostObject
        { get; set; }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = null;
            GetAllMatches();
            dataGrid.ItemsSource = regexSearchMatches;
        }

        public void GetAllMatches()
        {
            ISEEditor editor = HostObject.CurrentPowerShellTab.Files.SelectedFile.Editor;

            if (regexSearchMatches  != null) { regexSearchMatches.Clear(); }

            if (textBox.Text != null && editor.Text != null)
            {
                Regex rgx = new Regex(Regex.Escape(textBox.Text));

                using (StringReader reader = new StringReader(editor.Text))
                {
                    
                    string currline = string.Empty;
                    int lineNum = 1;
                    do
                    {
                        currline = reader.ReadLine();
                        if (currline != null)
                        {
                            MatchCollection rgxMatches = rgx.Matches(currline);

                            foreach (Match match in rgxMatches)
                            {
                                datagridRegexMatch currentMatch = new datagridRegexMatch();
                                currentMatch.match = match;
                                currentMatch.line = lineNum;
                                currentMatch.index = match.Index;
                                currentMatch.StringLenght = match.Length;

                                regexSearchMatches.Add(currentMatch);
                            }
                        }
                        lineNum++;
                    } while (currline != null);
                }
            }
        }
    }

    public class datagridRegexMatch
    {
        public Match match { get; set; }
        public int line { get; set; }
        public int index { get; set; }
        public int StringLenght { get; set; }
    }
}
