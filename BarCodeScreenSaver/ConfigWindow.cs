/*
Copyright (c) 2011 Arthur Shagall, Mindflight, Inc.

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BarCodeScreenSaver {
    public partial class ConfigWindow : Form {
        private readonly string[] items = {"hh:mm:ss", "HH:mm:ss"};

        public ConfigWindow() {
            InitializeComponent();
            formatComboBox.Items.AddRange(items);
            string f = global::BarCodeScreenSaver.Properties.Settings.Default.Format;
            formatComboBox.SelectedItem = f;
        }

        private void okButton_Click(object sender, EventArgs e) {
            global::BarCodeScreenSaver.Properties.Settings.Default.Format = items[formatComboBox.SelectedIndex];
            global::BarCodeScreenSaver.Properties.Settings.Default.Save();
            Application.Exit();
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            Application.Exit();
        }
    }
}