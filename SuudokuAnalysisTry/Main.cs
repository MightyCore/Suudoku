using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Windows.Forms;

namespace SuudokuAnalysisTry
{
    public partial class Main : Form
    {

        string filePath = "";

        public Main()
        {
            InitializeComponent();
            Calc.Map.Initialize();
        }


        /// <summary>
        /// 読込み
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Read_Click(object sender, EventArgs e)
        {
            filePath = OpeFileDaialog();
            try
            {
                if (File.Exists(filePath))
                {

                    Calc.Map.ClearNum();

                    bool IsErr = false;

                    //読込み
                    StreamReader sr = new StreamReader(@"" + filePath, Encoding.GetEncoding("Shift_JIS"));
                    try
                    {
                        int LineNum = 1;
                        while (sr.Peek() != -1)
                        {
                            string[] NumArry = sr.ReadLine().Split(',');
                            SetLineNum(NumArry, LineNum);
                            Calc.Map.SetMap(NumArry, LineNum);
                            LineNum++;
                        }
                    }
                    catch (Exception)
                    {
                        //エラーの時の処理
                        IsErr = true;

                    }
                    finally
                    {
                        sr.Close();

                        //エラーの時の処理
                        if (IsErr)
                        {
                            MessageBox.Show("問題読込みでエラー発生！");
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {

            }


        }



        /// <summary>
        /// 画面への設定
        /// </summary>
        /// <param name="vNum"></param>
        /// <param name="vLine"></param>
        private void SetLineNum(string[] vNum, int vLine)
        {


            switch (vLine)
            {
                case 1:
                    SetupCell(txtNum01, vNum[0]);
                    SetupCell(txtNum02, vNum[1]);
                    SetupCell(txtNum03, vNum[2]);
                    SetupCell(txtNum10, vNum[3]);
                    SetupCell(txtNum11, vNum[4]);
                    SetupCell(txtNum12, vNum[5]);
                    SetupCell(txtNum19, vNum[6]);
                    SetupCell(txtNum20, vNum[7]);
                    SetupCell(txtNum21, vNum[8]);
                    break;
                case 2:
                    SetupCell(txtNum04, vNum[0]);
                    SetupCell(txtNum05, vNum[1]);
                    SetupCell(txtNum06, vNum[2]);
                    SetupCell(txtNum13, vNum[3]);
                    SetupCell(txtNum14, vNum[4]);
                    SetupCell(txtNum15, vNum[5]);
                    SetupCell(txtNum22, vNum[6]);
                    SetupCell(txtNum23, vNum[7]);
                    SetupCell(txtNum24, vNum[8]);
                    break;
                case 3:
                    SetupCell(txtNum07, vNum[0]);
                    SetupCell(txtNum08, vNum[1]);
                    SetupCell(txtNum09, vNum[2]);
                    SetupCell(txtNum16, vNum[3]);
                    SetupCell(txtNum17, vNum[4]);
                    SetupCell(txtNum18, vNum[5]);
                    SetupCell(txtNum25, vNum[6]);
                    SetupCell(txtNum26, vNum[7]);
                    SetupCell(txtNum27, vNum[8]);
                    break;
                case 4:
                    SetupCell(txtNum28, vNum[0]);
                    SetupCell(txtNum29, vNum[1]);
                    SetupCell(txtNum30, vNum[2]);
                    SetupCell(txtNum37, vNum[3]);
                    SetupCell(txtNum38, vNum[4]);
                    SetupCell(txtNum39, vNum[5]);
                    SetupCell(txtNum46, vNum[6]);
                    SetupCell(txtNum47, vNum[7]);
                    SetupCell(txtNum48, vNum[8]);
                    break;
                case 5:
                    SetupCell(txtNum31, vNum[0]);
                    SetupCell(txtNum32, vNum[1]);
                    SetupCell(txtNum33, vNum[2]);
                    SetupCell(txtNum40, vNum[3]);
                    SetupCell(txtNum41, vNum[4]);
                    SetupCell(txtNum42, vNum[5]);
                    SetupCell(txtNum49, vNum[6]);
                    SetupCell(txtNum50, vNum[7]);
                    SetupCell(txtNum51, vNum[8]);
                    break;
                case 6:
                    SetupCell(txtNum34, vNum[0]);
                    SetupCell(txtNum35, vNum[1]);
                    SetupCell(txtNum36, vNum[2]);
                    SetupCell(txtNum43, vNum[3]);
                    SetupCell(txtNum44, vNum[4]);
                    SetupCell(txtNum45, vNum[5]);
                    SetupCell(txtNum52, vNum[6]);
                    SetupCell(txtNum53, vNum[7]);
                    SetupCell(txtNum54, vNum[8]);
                    break;
                case 7:
                    SetupCell(txtNum55, vNum[0]);
                    SetupCell(txtNum56, vNum[1]);
                    SetupCell(txtNum57, vNum[2]);
                    SetupCell(txtNum64, vNum[3]);
                    SetupCell(txtNum65, vNum[4]);
                    SetupCell(txtNum66, vNum[5]);
                    SetupCell(txtNum73, vNum[6]);
                    SetupCell(txtNum74, vNum[7]);
                    SetupCell(txtNum75, vNum[8]);
                    break;
                case 8:
                    SetupCell(txtNum58, vNum[0]);
                    SetupCell(txtNum59, vNum[1]);
                    SetupCell(txtNum60, vNum[2]);
                    SetupCell(txtNum67, vNum[3]);
                    SetupCell(txtNum68, vNum[4]);
                    SetupCell(txtNum69, vNum[5]);
                    SetupCell(txtNum76, vNum[6]);
                    SetupCell(txtNum77, vNum[7]);
                    SetupCell(txtNum78, vNum[8]);
                    break;
                case 9:
                    SetupCell(txtNum61, vNum[0]);
                    SetupCell(txtNum62, vNum[1]);
                    SetupCell(txtNum63, vNum[2]);
                    SetupCell(txtNum70, vNum[3]);
                    SetupCell(txtNum71, vNum[4]);
                    SetupCell(txtNum72, vNum[5]);
                    SetupCell(txtNum79, vNum[6]);
                    SetupCell(txtNum80, vNum[7]);
                    SetupCell(txtNum81, vNum[8]);
                    break;

            }



        }

        /// <summary>
        /// セルカラー設定
        /// </summary>
        /// <param name="vObjText"></param>
        /// <param name="vStrNum"></param>
        private void SetupCell(TextBox vObjText, string vStrNum)
        {


            if ("0" == vStrNum)
            {
                vObjText.BackColor = System.Drawing.Color.LightPink;
            }
            else if (string.IsNullOrEmpty(vStrNum))
            {
                vObjText.BackColor = System.Drawing.Color.White;
            }
            else
            {
                vObjText.BackColor = System.Drawing.Color.White;
            }
            vObjText.Text = vStrNum;


        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Analysis_Click(object sender, EventArgs e)
        {
            var sw = new System.Diagnostics.Stopwatch();

            // 計測開始
            sw.Start();

            try
            {
                //-----------------------------------------
                AnsNum.Items.Clear();
                new Calc.Calc().Exe();
                Enumerable.Range(1, Calc.Map.Ansers.Count).ToList().ForEach(x => AnsNum.Items.Add(x));
                AnsNum.SelectedIndex = 0;
                //-----------------------------------------
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {

            }

            // 計測停止
            sw.Stop();

            // 回答確認
            if (!Calc.Map.CheckAns()) MessageBox.Show("ミスってまっせ。");

            // 結果表示
            TimeSpan ts = sw.Elapsed;
            //string ResultTime = string.Format("{0}:{1}:{2}",ts.Hours,ts.Minutes, ts.Seconds);
            string ResultTime = sw.ElapsedMilliseconds + "ミリ秒";

            label1.Text = "結果：" + ResultTime;
        }

        /// <summary>
        /// クリア
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Clear_Click(object sender, EventArgs e)
        {

            SetupCell(txtNum01, "");
            SetupCell(txtNum02, "");
            SetupCell(txtNum03, "");
            SetupCell(txtNum04, "");
            SetupCell(txtNum05, "");
            SetupCell(txtNum06, "");
            SetupCell(txtNum07, "");
            SetupCell(txtNum08, "");
            SetupCell(txtNum09, "");
            SetupCell(txtNum10, "");
            SetupCell(txtNum11, "");
            SetupCell(txtNum12, "");
            SetupCell(txtNum13, "");
            SetupCell(txtNum14, "");
            SetupCell(txtNum15, "");
            SetupCell(txtNum16, "");
            SetupCell(txtNum17, "");
            SetupCell(txtNum18, "");
            SetupCell(txtNum19, "");
            SetupCell(txtNum20, "");
            SetupCell(txtNum21, "");
            SetupCell(txtNum22, "");
            SetupCell(txtNum23, "");
            SetupCell(txtNum24, "");
            SetupCell(txtNum25, "");
            SetupCell(txtNum26, "");
            SetupCell(txtNum27, "");
            SetupCell(txtNum28, "");
            SetupCell(txtNum29, "");
            SetupCell(txtNum30, "");
            SetupCell(txtNum31, "");
            SetupCell(txtNum32, "");
            SetupCell(txtNum33, "");
            SetupCell(txtNum34, "");
            SetupCell(txtNum35, "");
            SetupCell(txtNum36, "");
            SetupCell(txtNum37, "");
            SetupCell(txtNum38, "");
            SetupCell(txtNum39, "");
            SetupCell(txtNum40, "");
            SetupCell(txtNum41, "");
            SetupCell(txtNum42, "");
            SetupCell(txtNum43, "");
            SetupCell(txtNum44, "");
            SetupCell(txtNum45, "");
            SetupCell(txtNum46, "");
            SetupCell(txtNum47, "");
            SetupCell(txtNum48, "");
            SetupCell(txtNum49, "");
            SetupCell(txtNum50, "");
            SetupCell(txtNum51, "");
            SetupCell(txtNum52, "");
            SetupCell(txtNum53, "");
            SetupCell(txtNum54, "");
            SetupCell(txtNum55, "");
            SetupCell(txtNum56, "");
            SetupCell(txtNum57, "");
            SetupCell(txtNum58, "");
            SetupCell(txtNum59, "");
            SetupCell(txtNum60, "");
            SetupCell(txtNum61, "");
            SetupCell(txtNum62, "");
            SetupCell(txtNum63, "");
            SetupCell(txtNum64, "");
            SetupCell(txtNum65, "");
            SetupCell(txtNum66, "");
            SetupCell(txtNum67, "");
            SetupCell(txtNum68, "");
            SetupCell(txtNum69, "");
            SetupCell(txtNum70, "");
            SetupCell(txtNum71, "");
            SetupCell(txtNum72, "");
            SetupCell(txtNum73, "");
            SetupCell(txtNum74, "");
            SetupCell(txtNum75, "");
            SetupCell(txtNum76, "");
            SetupCell(txtNum77, "");
            SetupCell(txtNum78, "");
            SetupCell(txtNum79, "");
            SetupCell(txtNum80, "");
            SetupCell(txtNum81, "");

            Calc.Map.ClearNum();
        }


        private string OpeFileDaialog()
        {

            //OpenFileDialogクラスのインスタンスを作成
            OpenFileDialog ofd = new OpenFileDialog();

            //はじめのファイル名を指定する
            //はじめに「ファイル名」で表示される文字列を指定する
            ofd.FileName = "";
            //はじめに表示されるフォルダを指定する
            //指定しない（空の文字列）の時は、現在のディレクトリが表示される
            ofd.InitialDirectory = @"C:\";
            //[ファイルの種類]に表示される選択肢を指定する
            //指定しないとすべてのファイルが表示される
            ofd.Filter = "テキストファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
            //[ファイルの種類]ではじめに選択されるものを指定する
            //2番目の「すべてのファイル」が選択されているようにする
            ofd.FilterIndex = 2;
            //タイトルを設定する
            ofd.Title = "開くファイルを選択してください";
            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            ofd.RestoreDirectory = true;
            //存在しないファイルの名前が指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckFileExists = true;
            //存在しないパスが指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckPathExists = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //OKボタンがクリックされたとき、選択されたファイル名を表示する
                return ofd.FileName;
            }


            return "";
        }

        private void btn_End_Click(object sender, EventArgs e)
        {
            //終了
            this.Close();

        }

        /// <summary>
        /// 結果の表示
        /// </summary>
        /// <param name="vNum"></param>
        private void SetAns(int vNum)
        {
            Enumerable.Range(1, 9).ToList().ForEach(i => SetLineNum(Calc.Map.Ansers[vNum - 1].Where(x => x.Row == i).Select(x => x.Num.ToString()).ToArray(), i));
        }

        /// <summary>
        /// 回答番号設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnsNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetAns(int.Parse(AnsNum.Text));
        }

        /// <summary>
        /// 次の回答
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnsNext_Click(object sender, EventArgs e)
        {
            if (AnsNum.Items.Count == AnsNum.SelectedIndex + 1)
            {
                AnsNum.SelectedIndex = 0;
                return;
            }
            AnsNum.SelectedIndex++;
        }

        /// <summary>
        /// 前の回答
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnsBefore_Click(object sender, EventArgs e)
        {
            if (AnsNum.SelectedIndex == 0)
            {
                AnsNum.SelectedIndex = AnsNum.Items.Count - 1;
                return;
            }
            AnsNum.SelectedIndex--;
        }
    }
}
