using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace SuudokuAnalysisTry.Calc
{
    /// <summary>
    /// 表
    /// </summary>
    public static class Map
    {
        #region Field
        /// <summary>
        /// セル一覧
        /// </summary>
        public static List<Cell> Cells = new List<Cell>();

        /// <summary>
        /// 値入力したセル番号を階層別に管理
        /// </summary>
        private static List<List<int>> CellsIndexNumbered = new List<List<int>> { new List<int>() };

        /// <summary>
        /// 不正な値を入力した場合にTrue
        /// </summary>
        private static bool Reset = false;
        #endregion

        #region Class
        /// <summary>
        /// Cell
        /// </summary>
        public partial class Cell
        {
            public int Index { get; private set; }
            public int Row { get; private set; }
            public int Col { get; private set; }
            public int Area { get; private set; }
            public int Num { get; set; }
            public int TempCnt { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="vIndex"></param>
            public Cell(int vIndex)
            {
                Index = vIndex;
                Row = (int)(Math.Floor((decimal)(vIndex / 9)) + 1);
                Col = vIndex - (Row - 1) * 9 + 1;
                Area = (int)(Math.Floor((decimal)(Row - 1) / 3) * 3 + Math.Floor((decimal)((Col - 1) / 3)) + 1);
            }
        }
        #endregion

        #region Property
        /// <summary>
        /// 残りのターゲットを空セルの少ない順に取得
        /// </summary>
        /// <returns></returns>
        public static List<int> Targets
        {
            get
            {
                if (Reset)
                {
                    // 仮設定値の初期化
                    ClearNumTemp();
                    Reset = false;
                }

                return Cells.Where(x => x.Num > 0)
                    .GroupBy(x => x.Num)
                    .ToDictionary(x => x.Key, x => x.ToList())
                    .Select(x => new { x.Key, x.Value.Count })
                    .Where(x => x.Count < 9)
                    .OrderByDescending(x => x.Count)
                    .Select(x => x.Key).ToList();
            }
        }
        #endregion

        #region Initialize
        /// <summary>
        /// 表の配列作成
        /// </summary>
        public static void Initialize()
        {
            Enumerable.Range(0, 9 * 9).ToList().ForEach(i => Cells.Add(new Cell(i)));
        }
        #endregion

        #region Func
        /// <summary>
        /// 値の格納
        /// </summary>
        /// <param name="vNumArray"></param>
        /// <param name="vRow"></param>
        public static void SetMap(string[] vNumArray, int vRow)
        {
            Cells.Where(x => x.Row == vRow).ToList().ForEach(x => x.SetNum(int.Parse(vNumArray[x.Col - 1])));
        }

        /// <summary>
        /// 値の設定
        /// </summary>
        /// <param name="vCell"></param>
        /// <param name="vNum"></param>
        public static void SetNum(this Cell vCell, int vNum)
        {
            var wExists = vCell.Exists();
            vCell.Num = vNum;
            if (vNum == 0) return;
            CellsIndexNumbered[CellsIndexNumbered.Count - 1].Add(vCell.Index);
            if (wExists.Any(x => x == vNum)) Reset = true;
        }

        /// <summary>
        /// 値の仮置き
        /// </summary>
        /// <param name="vCell"></param>
        /// <param name="vNum"></param>
        public static void SetNumTemp(this Cell vCell, int vNum)
        {
            CellsIndexNumbered.Add(new List<int> { vCell.Index });
            vCell.TempCnt++;
            vCell.SetNum(vNum);
        }

        /// <summary>
        /// 仮置きした値の初期化
        /// </summary>
        public static void ClearNumTemp()
        {
            CellsIndexNumbered[CellsIndexNumbered.Count - 1].ForEach(i => Cells[i].Num = 0);
            CellsIndexNumbered.RemoveAt(CellsIndexNumbered.Count - 1);
        }

        /// <summary>
        /// 値の初期化
        /// </summary>
        public static void ClearNum()
        {
            Cells.ForEach(x => {
                x.Num = 0;
                x.TempCnt = 0;
            });
            CellsIndexNumbered.Clear();
            CellsIndexNumbered.Add(new List<int>());
        }

        /// <summary>
        /// 指定番号が存在する行、列、表以外の値が0のセル
        /// </summary>
        /// <param name="vNum"></param>
        /// <returns></returns>
        public static List<Cell> GetZeroCellsWithoutNum(int vNum)
        {
            var wWithouts = Cells.Where(x => x.Num == vNum).ToList();
            return Cells.Where(x => x.Num == 0).Where(x => wWithouts.All(y => y.Row != x.Row && y.Col != x.Col && y.Area != x.Area))?.ToList() ?? new List<Cell>(); ;
        }

        /// <summary>
        /// 空きが1セルの個所があった場合、対象数値で埋める
        /// </summary>
        /// <param name="vCell"></param>
        /// <param name="vNum"></param>
        /// <param name="vCells"></param>
        /// <returns></returns>
        public static bool SetNumWhenTheLastOne(this Cell vCell, int vNum, List<Cell> vCells)
        {
            if (vCells.Count(x => x.Row == vCell.Row) == 1 || vCells.Count(x => x.Col == vCell.Col) == 1 || vCells.Count(x => x.Area == vCell.Area) == 1)
            {
                vCell.SetNum(vNum);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 複製の作成
        /// </summary>
        /// <param name="vTarget"></param>
        /// <returns></returns>
        public static object DeepClone(this object vTarget)
        {
            BinaryFormatter wBinaryFormatter = new BinaryFormatter();
            MemoryStream wMemoryStream = new MemoryStream();

            try
            {
                wBinaryFormatter.Serialize(wMemoryStream, vTarget);
                wMemoryStream.Position = 0;
                return wBinaryFormatter.Deserialize(wMemoryStream);
            }
            finally
            {
                wMemoryStream.Close();
            }
        }

        /// <summary>
        /// セルに当てはまらない数の取得
        /// </summary>
        /// <param name="vCell"></param>
        /// <returns></returns>
        public static List<int> RemainNum(this Cell vCell)
        {
            var wTargets = Targets;
            Func<Func<Cell, bool>, List<int>> wExcepts = x => wTargets.Except(Cells.Where(x).Select(y => y.Num)).ToList();
            return wExcepts(x => x.Row == vCell.Row)
                .Concat(wExcepts(x => x.Col == vCell.Col))
                .Concat(wExcepts(x => x.Area == vCell.Area))
                .Distinct().OrderBy(x => x).ToList();
        }

        /// <summary>
        /// 紐づく行、列、表に存在する値
        /// </summary>
        /// <param name="vCell"></param>
        /// <returns></returns>
        public static List<int> Exists(this Cell vCell) =>
            Cells
            .Where(x => x.Row == vCell.Row || x.Col == vCell.Col || x.Area == vCell.Area)
            .Select(x => x.Num)
            .Distinct().ToList();
        #endregion

        #region Debug
        /// <summary>
        /// 途中経過確認
        /// </summary>
        public static void ShowMap() => MessageBox.Show(string.Join("\r\n", Enumerable.Range(0, 9).ToList().Select(i => string.Join(",", Cells.Where(x => x.Row == i).OrderBy(x => x.Col).Select(x => x.Num)))));

        /// <summary>
        /// 答え合わせ
        /// </summary>
        /// <returns></returns>
        public static bool CheckAns()
        {
            Func<IEnumerable<IGrouping<int, Cell>>, bool> wChecker = vList => vList.All(x => x.ToList().Select(y => y.Num).Distinct().Count() == 9);
            if (!wChecker(Cells.GroupBy(x => x.Row))) return false;
            if (!wChecker(Cells.GroupBy(x => x.Col))) return false;
            if (!wChecker(Cells.GroupBy(x => x.Area))) return false;
            return true;
        }

        /// <summary>
        /// 各階層の深さとルート番号
        /// </summary>
        private static void LogLevel() => Console.WriteLine($"{CellsIndexNumbered.Count} {CellsIndexNumbered[CellsIndexNumbered.Count - 1][0]}");
        #endregion
    }
}
