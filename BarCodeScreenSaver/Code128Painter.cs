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
using System.Collections;
using System.Drawing;

namespace BarCodeScreenSaver {
    /// <summary>
    /// Class to draw Code128 bar codes on a graphics context.  Should only be accessed from the Windows.Forms event dispatch thread.
    /// </summary>
    public class Code128Painter {
        private enum Code { A, B, C };

        private static readonly string[] printCodes;
        private static readonly Hashtable caTable;
        private static readonly Hashtable cbTable;
        private static readonly Hashtable ccTable;

        private const string startA = "Start Code A";
        private const string startB = "Start Code B";
        private const string startC = "Start Code C";

        private const string codeA = "Code A";
        private const string codeB = "Code B";
        private const string codeC = "Code C";

        private int posX = 0;
        private int posY = 0;
        private int width = 1;
        private int height = 32;

        //private Pen p = new Pen(Color.Black);

        public int X {
            get {
                return posX;
            }
            set {
                posX = value;
            }
        }
        public int Y {
            get {
                return posY;
            }
            set {
                posY = value;
            }
        }
        public int Width {
            get {
                return width;
            }
            set {
                width = value;
            }
        }
        public int Height {
            get {
                return height;
            }
            set {
                height = value;
            }
        }

        static Code128Painter() {
            caTable = new Hashtable();
            cbTable = new Hashtable();
            ccTable = new Hashtable();
            printCodes = new string[] { "11011001100", "11001101100", "11001100110", "10011011000", "10010001100", "10001001100", "10011001000", "10011000100", "10001100100", "11001001000", "11001000100", "11000100100", "10110011100", "10011011100", "10011001110", "10111001100", "10011101100", "10011100110", "11001110010", "11001011100", "11001001110", "11011100100", "11001110100", "11101101110", "11101001100", "11100101100", "11100100110", "11101100100", "11100110100", "11100110010", "11011011000", "11011000110", "11000110110", "10100011000", "10001011000", "10001000110", "10110001000", "10001101000", "10001100010", "11010001000", "11000101000", "11000100010", "10110111000", "10110001110", "10001101110", "10111011000", "10111000110", "10001110110", "11101110110", "11010001110", "11000101110", "11011101000", "11011100010", "11011101110", "11101011000", "11101000110", "11100010110", "11101101000", "11101100010", "11100011010", "11101111010", "11001000010", "11110001010", "10100110000", "10100001100", "10010110000", "10010000110", "10000101100", "10000100110", "10110010000", "10110000100", "10011010000", "10011000010", "10000110100", "10000110010", "11000010010", "11001010000", "11110111010", "11000010100", "10001111010", "10100111100", "10010111100", "10010011110", "10111100100", "10011110100", "10011110010", "11110100100", "11110010100", "11110010010", "11011011110", "11011110110", "11110110110", "10101111000", "10100011110", "10001011110", "10111101000", "10111100010", "11110101000", "11110100010", "10111011110", "10111101110", "11101011110", "11110101110", "11010000100", "11010010000", "11010011100", "1100011101011" };
            string[,] temp1 = { {
				 " ", " ", ""}, {
				 "!", "!", ""}, {
				 "\"", "\"", ""}, {
				 "#", "#", ""}, {
				 "$","$",""}, {
				 "%","%",""}, {
				 "&","&",""}, {
				 '\u02C8'.ToString(), '\u02C8'.ToString(),""}, {
				 "(","(",""}, {
				 ")",")",""}, {
				 "*","*",""}, {
				 "+","+",""}, {
				 "'","'",""}, {
				 "-","-",""}, {
				 ".",".",""}, {
				 "/","/",""}, {
				 "0","0",""}, {
				 "1","1",""}, {
				 "2","2",""}, {
				 "3","3",""}, {
				 "4","4",""}, {
				 "5","5",""}, {
				 "6","6",""}, {
				 "7","7",""}, {
				 "8","8",""}, {
				 "9","9",""}, {
				 ":",":",""}, {
				 ";",";",""}, {
				 "<","<",""}, {
				 "=","=",""}, {
				 ">",">",""}, {
				 "?","?",""}, {
				 "@","@",""}, {
				 "A","A",""}, {
				 "B","B",""}, {
				 "C","C",""}, {
				 "D","D",""}, {
				 "E","E",""}, {
				 "F","F",""}, {
				 "G","G",""}, {
				 "H","H",""}, {
				 "I","I",""}, {
				 "J","J",""}, {
				 "K","K",""}, {
				 "L","L",""}, {
				 "M","M",""}, {
				 "N","N",""}, {
				 "O","O",""}, {
				 "P","P",""}, {
				 "Q","Q",""}, {
				 "R","R",""}, {
				 "S","S",""}, {
				 "T","T",""}, {
				 "U","U",""}, {
				 "V","V",""}, {
				 "W","W",""}, {
				 "X","X",""}, {
				 "Y","Y",""}, {
				 "Z","Z",""}, {
				 "[","[",""}, {
				 "\\","\\",""}, {
				 "]","]",""}, {
				 "^","^",""}, {
				 "_","_",""}, {
				 '\u0000'.ToString(), "`",""}, {
				 '\u0001'.ToString(),"a",""}, {
				 '\u0002'.ToString(),"b",""}, {
				 '\u0003'.ToString(),"c",""}, {
				 '\u0004'.ToString(),"d",""}, {
				 '\u0005'.ToString(),"e",""}, {
				 '\u0006'.ToString(),"f",""}, {
				 '\u0007'.ToString(),"g",""}, {
				 '\u0008'.ToString(),"h",""}, {
				 '\u0009'.ToString(),"i",""}, {
				 '\u000A'.ToString(),"j",""}, {
				 '\u000B'.ToString(),"k",""}, {
				 '\u000C'.ToString(),"l",""}, {
				 '\u000D'.ToString(),"m",""}, {
				 '\u000E'.ToString(),"n",""}, {
				 '\u000F'.ToString(),"o",""}, {
				 '\u0010'.ToString(),"p",""}, {
				 '\u0011'.ToString(),"q",""}, {
				 '\u0012'.ToString(),"r",""}, {
				 '\u0013'.ToString(),"s",""}, {
				 '\u0014'.ToString(),"t",""}, {
				 '\u0015'.ToString(),"u",""}, {
				 '\u0016'.ToString(),"v",""}, {
				 '\u0017'.ToString(),"w",""}, {
				 '\u0018'.ToString(),"x",""}, {
				 '\u0019'.ToString(),"y",""}, {
				 '\u001A'.ToString(),"z",""}, {
				 '\u001B'.ToString(),"{",""}, {
				 '\u001C'.ToString(),"|",""}, {
				 '\u001D'.ToString(),"}",""}, {
				 '\u001E'.ToString(),"~",""}, {
				 '\u001F'.ToString(),'\u007f'.ToString(),""}, {
				 "FNC3","FNC3",""}, {
				 "FNC2","FNC2",""}, {
				 "shift","shift",""}, {
				 "Code C","Code C",""}, {
				 "Code B","FNC4","Code B"}, {
				 "FNC4","Code A","Code A"}, {
				 "FNC1","FNC1","FNC1"}, {
				 startA,startA,startA}, {
				 startB,startB,startB}, {
				 startC,startC,startC}, {
				 "STOP","STOP","STOP"}};

            for (int x = 0; x < 100; x++) {
                if (x < 10) {
                    temp1[x, 2] = "0" + x;
                } else {
                    temp1[x, 2] = "" + x;
                }
            }
            for (int x = 0; x < temp1.GetLength(0); x++) {
                Code128Entity e = new Code128Entity(temp1[x, 0], temp1[x, 1], temp1[x, 2], x);
                caTable.Add(e.CodeA, e);
                cbTable.Add(e.CodeB, e);
                ccTable.Add(e.CodeC, e);
            }

        }

        public int[] GetValueCodes(string input) {
            int[] values = GenerateValueSequence(input);
            return values;
        }

        public string[] GetPrintCodes(string input) {
            int[] values = GenerateValueSequence(input);
            string[] codes = new string[values.Length];
            for (int x = 0; x < codes.Length; x++) {
                codes[x] = printCodes[values[x]];
            }
            return codes;
        }

        public void DrawBarCode(string input, Graphics g) {
            int[] values = GenerateValueSequence(input);
            foreach (int i in values) {
                DrawRectangle(i, g);
            }
        }

        private void DrawRectangle(int n, Graphics g) {
            char[] cr = printCodes[n].ToCharArray();
            foreach (char z in cr) {
                if (z == '1') {
                    g.FillRectangle(Brushes.White, posX, posY, width, height);
                }
                posX += width;
            }


        }

        private int[] GenerateValueSequence(String input) {
            if (input.Length == 0) return new int[0];
            char[] tempc = input.ToCharArray();
            ArrayList tempv = new ArrayList();
            //Determine start code and first character
            string str = tempc[0].ToString();
            string start;
            if (input.Length == 1) {
                Code128Entity c = LookupEntry(str, Code.A);
                start = startA;
                if (c == null) {
                    c = LookupEntry(str, Code.B);
                    start = startB;
                    if (c == null) {
                        c = LookupEntry(str, Code.C);
                        start = startC;
                    }
                }
                if (c == null) {
                    throw new ArgumentException("The character " + str + " is invalid.", "input");
                }
                if (start == startA) {
                    tempv.Add(caTable[startA]);
                } else if (start == startB) {
                    tempv.Add(caTable[startB]);
                } else if (start == startC) {
                    tempv.Add(caTable[startC]);
                }
                tempv.Add(c);
            } else {
                int startInt = 1;
                start = startA;
                string checkstr;
                Code128Entity c;
                if (CheckC(tempc[0], tempc[1])) {
                    start = startC;
                    startInt++;
                    checkstr = tempc[0].ToString() + tempc[1];
                    c = LookupEntry(checkstr, Code.C);
                } else {
                    checkstr = tempc[0].ToString();
                    start = startA;
                    c = LookupEntry(str, Code.A);
                    if (c == null) {
                        c = LookupEntry(checkstr, Code.B);
                        start = startB;
                        if (c == null) {
                            c = LookupEntry(checkstr, Code.C);
                            start = startC;
                        }
                    }
                    if (c == null) {
                        throw new ArgumentException("The character " + str + " is invalid.", "input");
                    }
                }
                tempv.Add(caTable[start]);
                tempv.Add(c);
                //Need to keep a tab on which code page we're using at any given time.
                string codePage1 = null;
                string codePage2;
                if (start == startA) {
                    codePage1 = codeA;
                    codePage2 = codeA;
                } else if (start == startB) {
                    codePage1 = codeB;
                    codePage2 = codeB;
                } else if (start == startC) {
                    codePage1 = codeC;
                    codePage2 = codeC;
                }
                for (int x = startInt; x < tempc.Length; x++) {
                    c = null; //reset variable
                    //Need to check if this is the last character.					
                    if (x != tempc.Length - 1 && CheckC(tempc[x], tempc[x + 1])) {
                        checkstr = tempc[x].ToString() + tempc[x + 1].ToString();
                        x++; //increment counter since we're using two characters
                    } else {
                        checkstr = tempc[x].ToString();
                    }
                    c = LookupEntry(checkstr, Code.A);
                    if (c == null) {
                        c = LookupEntry(checkstr, Code.B);
                        codePage2 = codeB;
                        if (c == null) {
                            c = LookupEntry(checkstr, Code.C);
                            codePage2 = codeC;
                        }
                    } else {
                        codePage2 = codeA;
                    }
                    if (c == null) {
                        throw new ArgumentException("The character " + str + " is invalid.", "input");
                    }
                    if (codePage1 != null && !codePage1.Equals(codePage2)) {//This means we just switched code pages
                        tempv.Add(CodeSwitch(codePage1, codePage2));
                    }

                    tempv.Add(c);
                    codePage1 = codePage2;
                }

            }

            int[] seq = new int[tempv.Count + 2];
            //Remember to add terminator character
            seq[seq.Length - 1] = 106;
            int counter = ((Code128Entity)tempv[0]).Value;
            for (int x = 0; x < tempv.Count; x++) {
                seq[x] = ((Code128Entity)tempv[x]).Value;
                counter += seq[x] * x;
            }
            seq[seq.Length - 2] = counter % 103;
            return seq;

        }

        private static Code128Entity LookupEntry(string entry, Code code) {
            Code128Entity n = null;

            switch (code) {
                case Code.A:
                    if (caTable.ContainsKey(entry)) {
                        n = (Code128Entity)caTable[entry];
                    }
                    break;
                case Code.B:
                    if (cbTable.ContainsKey(entry)) {
                        n = (Code128Entity)cbTable[entry];
                    }
                    break;
                case Code.C:
                    if (ccTable.ContainsKey(entry)) {
                        n = (Code128Entity)ccTable[entry];
                    }
                    break;
            }
            return n;
        }

        private Code128Entity CodeSwitch(string origCodePage, string termCodePage) {
            if (origCodePage == codeA) {
                return (Code128Entity)caTable[termCodePage];
            } else if (origCodePage == codeB) {
                return (Code128Entity)cbTable[termCodePage];
            } else if (origCodePage == codeC) {
                return (Code128Entity)ccTable[termCodePage];
            } else {
                throw new ArgumentException("Invalid code page specified.");
            }
        }

        private bool CheckC(char a, char b) {
            int x = Convert.ToInt16(a);
            if (x < 48 || x > 57) {
                return false;
            } else {
                int y = Convert.ToInt16(b);
                if (y < 48 || x > 57) {
                    return false;
                } else {
                    return true;
                }
            }
        }

    }

    class Code128Entity {
        private readonly string codeA;
        private readonly string codeB;
        private readonly string codeC;
        private readonly int value;

        public Code128Entity(string a, string b, string c, int v) {
            codeA = a;
            codeB = b;
            codeC = c;
            value = v;
        }

        public string CodeA {
            get { return codeA; }
        }

        public string CodeB {
            get { return codeB; }
        }

        public string CodeC {
            get { return codeC; }
        }

        public int Value {
            get { return value; }
        }
    }
}
